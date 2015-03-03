namespace Fanex.Ares.Core.Web.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.Ares.Core.Models;
    using Fanex.NAP.Core.Model;

    /// <summary>
    /// Authenticate Action.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class AuthenticateAction : ActionFilterAttribute
    {
        #region Fields

        private AuthenticationType _authorizeType;

        private string _mainAction;

        #endregion Fields

        #region Constructors

        public AuthenticateAction(AuthenticationType authorizeType, string mainAction)
        {
            _authorizeType = authorizeType;
            _mainAction = mainAction;
        }

        #endregion Constructors

        #region Properties

        public AuthenticationType AuthorizeType
        {
            get { return _authorizeType; }
        }

        public string MainAction
        {
            get { return _mainAction; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// On Action Executing.
        /// </summary>
        /// <param name="filterContext">Filter Context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controlName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            var pathActionName = controlName + "/" + _mainAction;

            var actionPrivilege = CheckPrivilege(pathActionName);

            switch (_authorizeType)
            {
                case AuthenticationType.Read:
                    if (actionPrivilege < 1)
                    {
                        ActionDenied(filterContext);
                        return;
                    }

                    break;

                case AuthenticationType.Host:
                    if (actionPrivilege < 2)
                    {
                        var error = new Result();
                        error.ErrorCode = 500;
                        error.ErrorMessage = Resources.Errors._500;
                        filterContext.Result = new JsonResult { Data = error };
                        return;
                    }

                    break;
            }

            base.OnActionExecuting(filterContext);
        }

        #endregion Methods

        #region Utilities

        /// <summary>
        /// Action Denied.
        /// </summary>
        /// <param name="filterContext">Filter Context.</param>
        private static void ActionDenied(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                area = string.Empty,
                controller = "Common",
                action = "AccessDenied"
            }));
        }

        /// <summary>
        /// Check Privilege.
        /// </summary>
        /// <param name="fullActionPath">Full Action Path.</param>
        /// <returns>Return result.</returns>
        private int CheckPrivilege(string fullActionPath)
        {
            var privilege = 0;

            if (string.IsNullOrEmpty(fullActionPath))
            {
                return 0;
            }

            var useMenu = AppSession.CurrentUserMenu;

            if (useMenu == null || useMenu.Count == 0)
            {
                return 0;
            }

            var result = useMenu.Where(menuItem => menuItem.Url.Trim().ToLower().Contains(fullActionPath.Trim().ToLower()));

            var s = result as List<MenuItem> ?? result.ToList();
            if (s.Any())
            {
                var firstOrDefault = s.FirstOrDefault();
                if (firstOrDefault != null)
                {
                    privilege = firstOrDefault.Policy;
                }
            }

            return privilege;
        }

        #endregion Utilities
    }
}