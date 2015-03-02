namespace Fanex.Ares.Website.Areas.UserLogin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Ares.Core.Web;
    using Fanex.Ares.Core.Web.Controllers;
    using Fanex.Common;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// SSO controller.
    /// </summary>
    public class SSOController : BaseController
    {
        #region members
        private readonly int _timeExipration = 30;
        private IUserService _userService;
        private IMenuItemService _menuItemService;
        private ICurrentSession _currentSession;
        #endregion members

        #region ctors

        public SSOController()
            : this(null, null, null)
        {
        }

        public SSOController(
            IUserService userService,
            IMenuItemService menuItemService,
            ICurrentSession currentSession)
        {
            _userService = userService ?? new UserService();
            _menuItemService = menuItemService ?? new MenuItemService();
            _currentSession = currentSession ?? new CurrentSession();
        }

        #endregion ctors

        /// <summary>
        /// The index page.
        /// </summary>
        /// <param name="userId">Login user id.</param>
        /// <param name="onuserId">On user id.</param>
        /// <param name="time">Log in time.</param>
        /// <param name="redirectUrl">Redirect URL.</param>
        /// <param name="newTab">Is New Tab.</param>
        /// <returns>Return result.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string userId, string onuserId, string time, Uri redirectUrl = null, bool newTab = true)
        {
            Session.Clear();

            string currentTime = this._userService.UserSSOGetTime();
            string forwardLink = redirectUrl == null ? FormsAuthentication.DefaultUrl : redirectUrl.ToString();
            TimeSpan expiration = new TimeSpan();

            userId = Cryptography.CfsDeCrypt(userId.Replace(" ", "+"));
            onuserId = Cryptography.CfsDeCrypt(onuserId.Replace(" ", "+"));
            time = Cryptography.CfsDeCrypt(time.Replace(" ", "+"));
            expiration = Convert.ToDateTime(currentTime) - Convert.ToDateTime(time);

            if (expiration.TotalSeconds <= this._timeExipration)
            {
                // do SSO check-in
                var criteria = new UserSSOCheckInUserCriteria
                {
                    UserId = Convert.ToInt32(userId),
                    SiteId = SiteId,
                    CheckOnUserId = Convert.ToInt32(onuserId),
                    OnUserId = 0,
                    IP = CurrentContext.IP,
                    NetSessionId = Session.SessionID,
                    LoginDate = DateTime.Now,
                    BrowserName = CurrentContext.BrowserName,
                    CountryCode = CurrentContext.CountryCode,
                    Domain = CurrentContext.DomainName,
                    Platform = CurrentContext.Platform
                };

                this._userService.UserSSOCheckIn(ref criteria);

                if (criteria.OnUserId != 0)
                {
                    var user = this._userService.GetById(criteria.UserId) as User;

                    // saves user session & auth info
                    this._currentSession.UserId = user.Id;
                    this._currentSession.Username = user.Username;
                    this._currentSession.OnUserId = criteria.OnUserId;
                    this._currentSession.SessionId = Session.SessionID.ToString();
                    this._currentSession.Level = this._userService.GetUserSiteLevel(user.Id, this.SiteId);

                    if (this.SiteId != 0)
                    {
                        this._currentSession.MenuItemList = this._menuItemService.GetByUserAndSiteIncludeGroup(
                                                                                user.Id,
                                                                                SiteId,
                                                                                this.Environment,
                                                                                true) as IList<MenuItem>;
                        AppSession.CurrentUserMenu = _currentSession.MenuItemList;
                    }

                    this.CreateAuthInfo(user);
                    AppSession.IsShowMenu = newTab;
                    return this.Redirect(forwardLink);
                }
            }

            return this.Redirect(FormsAuthentication.LoginUrl);
        }

        #region utilities
        /// <summary>
        /// Create Authenticate Info.
        /// </summary>
        /// <param name="user">User info.</param>
        private void CreateAuthInfo(User user)
        {
            var currentDomain = CurrentContext.GetCurrentDomain();

            var authInfo = new AuthInfo();

            authInfo.UserId = user.Id;
            authInfo.OnUserId = this._currentSession.OnUserId;

            var authTicket = new FormsAuthenticationTicket(
                 1,
                user.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20000),
                false,
                authInfo.ToXml(),
                currentDomain.Path);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = currentDomain.Path,
                Name = FormsAuthentication.FormsCookieName
            };

            Response.Cookies.Add(authCookie);
        }

        #endregion utilities
    }
}