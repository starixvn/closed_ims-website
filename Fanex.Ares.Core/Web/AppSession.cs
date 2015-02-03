namespace Fanex.Ares.Core.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Ares.Core.DataDomain;
    using Fanex.NAP.Core.Model;

    /// <summary>
    /// App Session.
    /// </summary>
    public static class AppSession
    {
        /// <summary>
        /// Gets or sets current login user.
        /// </summary>
        /// <value>Current User.</value>
        public static User CurrentUser
        {
            get
            {
                return SessionManager.Get<User>(Constants.CurrentLogInUser);
            }

            set
            {
                SessionManager.Set<User>(Constants.CurrentLogInUser, value);
            }
        }

        /// <summary>
        /// Gets or sets menu of current login user.
        /// </summary>
        /// <value>Current User Menu.</value>
        public static IList<MenuItem> CurrentUserMenu
        {
            get
            {
                return SessionManager.Get<IList<MenuItem>>(Constants.CurrentUserMenu);
            }

            set
            {
                SessionManager.Set<IList<MenuItem>>(Constants.CurrentUserMenu, value);
            }
        }

        public static Dictionary<string, int> RoleAccess
        {
            get
            {
                try
                {
                    return SessionManager.Get<Dictionary<string, int>>(Constants.CurrentUserRoleAccess);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            set
            {
                SessionManager.Set<Dictionary<string, int>>(Constants.CurrentUserRoleAccess, value);
            }
        }

        /// <summary>
        /// Gets or sets current working menu url.
        /// </summary>
        /// <value>Current Working O nMenu.</value>
        public static MenuItem CurrentWorkingOnMenu
        {
            get
            {
                return SessionManager.Get<MenuItem>(Constants.CurrentWorkingOnMenu);
            }

            set
            {
                SessionManager.Set<MenuItem>(Constants.CurrentWorkingOnMenu, value);
            }
        }

        /// <summary>
        /// Gets or sets IsTrackedMenu: determine current working on menu is,
        /// tracked for page tracking or not.
        /// </summary>
        /// <value>Current Action Name.</value>
        public static string CurrentActionName
        {
            get
            {
                return SessionManager.Get<string>(Constants.CurrentActionName);
            }

            set
            {
                SessionManager.Set<string>(Constants.CurrentActionName, value);
            }
        }

        public static bool ViewIPVN
        {
            get
            {
                return SessionManager.Get<bool>(Constants.ViewIpVN);
            }

            set
            {
                SessionManager.Set<bool>(Constants.ViewIpVN, value);
            }
        }

        public static bool IsShowMenu
        {
            get
            {
                return SessionManager.Get<bool>(Constants.IsShowMenu);
            }

            set
            {
                SessionManager.Set<bool>(Constants.IsShowMenu, value);
            }
        }
    }
}