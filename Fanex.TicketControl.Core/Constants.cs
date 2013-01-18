// --------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2012.11.12">Created</change>
// </history>

using System.Diagnostics.CodeAnalysis;

namespace Fanex.TicketControl.Core
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public static class Constants
    {
        public const string SHORT_DATE_FORMAT = "MMM dd, yyyy";
        public const string MONTH_YEAR_DATE_FORMAT = "MMM yyyy";
        public const string SHORT_DATE_FORMAT_WITH_SLASH = "MM/dd/yyyy";
        public const string LONG_DATE_FORMAT_WITH_SLASH = "MM/dd/yyyy hh:mm:ss tt";
        public const string MONTH_YEAR_DATE_FORMAT_WITH_SLASH = "MM/yyyy";
        public const string LONG_DATE_FORMAT = "dd MMM, yyyy hh:mm:ss tt";
        public const string CURRENT_LOG_IN_USER = "Fanex.AccountingReports.Core.Contants.AppSession.CurrentLogInUser";
        public const string CURRENT_USER_MENU = "Fanex.AccountingReports.Core.Contants.AppSession.CurrentUserMenu";
        public const string CURRENT_USER_ROLE_ACCESS = "Fanex.AccountingReports.Core.Contants.AppSession.CurrentUserRoleAccess";
        public const string CURRENT_WORKING_ON_MENU = "Fanex.AccountingReports.Core.Contants.AppSession.CurrentWorkingOnMenu";
        public const string IS_TRACKED_MENU = "Fanex.AccountingReports.Core.Contants.AppSession.IsTrackedMenu";
        public const string CURRENT_ACTION_NAME = "Fanex.AccountingReports.Core.Contants.AppSession.CurrentActionName";
        public const string TWO_DECIMAL_DIGIT_FORMAT = ".00";
        public const string FOUR_DECIMAL_DIGIT_FORMAT = ".0000";
        public const string MOST_VISITED_PAGE_CACHE_KEY = "Fanex.AccountingReports.Core.Contants.AppSession.IntervalGetMostVisitedPage";
        public static readonly object MOST_VISITED_PAGE_LOCK_OBJECT = new object();
        public const string SP_PARAMS_SEPERATOR = ",";
        public const string VIEW_IP_VN = "ViewIPVN";
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public static class BetTypeContants
    {
        public const int MIX_PARLAY = 9;
        public const int SYSTEM_PARLAY = 29;
    }

    public enum CurrencyContants
    {
        None = 0,
        MYR = 2,
        EUR = 6,
        USD = 3
    }
}
