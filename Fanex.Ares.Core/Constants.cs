// --------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Core
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The constants.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Constants")]
    public static class Constants
    {
        public const string _SHORT_DATE_FORMAT = "MMM dd, yyyy";
        public const string _MONTH_YEAR_DATE_FORMAT = "MMM yyyy";
        public const string _SHORT_DATE_FORMAT_WITH_SLASH = "MM/dd/yyyy";
        public const string _LONG_DATE_FORMAT_WITH_SLASH = "MM/dd/yyyy hh:mm:ss tt";
        public const string _MONTH_YEAR_DATE_FORMAT_WITH_SLASH = "MM/yyyy";
        public const string _LONG_DATE_FORMAT = "dd MMM, yyyy hh:mm:ss tt";
        public const string _CURRENT_LOG_IN_USER = "Fanex.Ares.Core.Contants.AppSession.CurrentLogInUser";
        public const string _CURRENT_USER_MENU = "Fanex.Ares.Core.Contants.AppSession.CurrentUserMenu";
        public const string _CURRENT_USER_ROLE_ACCESS = "Fanex.Ares.Core.Contants.AppSession.CurrentUserRoleAccess";
        public const string _CURRENT_WORKING_ON_MENU = "Fanex.Ares.Core.Contants.AppSession.CurrentWorkingOnMenu";
        public const string _CURRENT_ACTION_NAME = "Fanex.Ares.Core.Contants.AppSession.CurrentActionName";
        public const string _TWO_DECIMAL_DIGIT_FORMAT = ".00";
        public const string _FOUR_DECIMAL_DIGIT_FORMAT = ".0000";
        public const string _SP_PARAMS_SEPERATOR = ",";
        public const string _VIEW_IP_VN = "ViewIPVN";
        public const string _ARES_DATE_FORMAT = "dd-MMM-yy";
        public const string _ARES_MONTH_YEAR_FORMAT = "MMM-yy";
        public const string _IS_SHOW_MENU = "Fanex.Ares.Core.Contants.AppSession.IsShowMenu";
    }
}