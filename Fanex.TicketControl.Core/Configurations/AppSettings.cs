// --------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Steven.Nguyen" date="2012.10.12">Created</change>
// </history>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using Fanex.Common;

namespace Fanex.TicketControl.Core.Configurations
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public static class AppSettings
    {
        private static List<string> _excludedControllers;

        public static List<string> ExcludedControllers
        {
            get
            {
                return _excludedControllers;
            }
        }

        private static List<string> _specialIncludedViews;

        public static List<string> SpecialIncludedViews
        {
            get
            {
                return _specialIncludedViews;
            }
        }

        private static List<string> _multiAgentLicensses;

        public static List<string> MultiAgentLicensses
        {
            get
            {
                return _multiAgentLicensses;
            }
        }

        private static List<string> _virtualSportBetTypes;

        public static List<string> VirtualSportBetTypes
        {
            get
            {
                return _virtualSportBetTypes;
            }
        }

        private static DateTime _winLossByLicensee_OldTableBaseTime;

        public static DateTime WinLossByLicensee_OldTableBaseTime
        {
            get
            {
                return _winLossByLicensee_OldTableBaseTime;
            }
        }

        static AppSettings()
        {
            LbServiceUrl = ConfigurationManager.AppSettings["LBUrl"];
            IsUMTime = ConfigurationManager.AppSettings["IsUM"] == "1";
            IsLocalTest = ConfigurationManager.AppSettings["IsLocalTest"] == "1";
            _excludedControllers = GetExcludedControllers();
            _specialIncludedViews = GetSpecialIncludedViews();
            _multiAgentLicensses = GetMultiAgentLicensses();
            _winLossByLicensee_OldTableBaseTime = Converter.ToDateTime(
                ConfigurationManager.AppSettings["WinLossByLicensee_OldTableBaseTime"]);
            IntervalGetMostVisitedPage = Converter.ToInt(ConfigurationManager.AppSettings["IntervalGetMostVisitedPage"]);
            GetSportBookBetTypes();
            IPVNReplace = Converter.ToString2(ConfigurationManager.AppSettings["ipvnreplace"]);
            _topRateMostVisitedPages = Converter.ToInt(ConfigurationManager.AppSettings["MostVisitedPages_MaxPages"]);
        }

        public static string LbServiceUrl { get; set; }

        public static bool IsUMTime { get; set; }

        public static bool IsLocalTest { get; set; }

        public static int MaxPagesOfMostVisitedPage
        {
            get
            {
                return Converter.ToInt(ConfigurationManager.AppSettings["MostVisitedPages_MaxPages"]);
            }
        }

        private static List<string> GetExcludedControllers()
        {
            List<string> excludedControllers =
                new List<string>(ConfigurationManager
                .AppSettings["PageTracker_ExcludedControllers"]
                .ToString()
                .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            return excludedControllers;
        }

        private static List<string> GetSpecialIncludedViews()
        {
            List<string> specialIncludedViews =
                new List<string>(ConfigurationManager
                .AppSettings["PageTracker_SpecialIncludedViews"]
                .ToString()
                .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            return specialIncludedViews;
        }

        private static List<string> GetMultiAgentLicensses()
        {
            List<string> multiAgentLicensses =
                new List<string>(ConfigurationManager
                .AppSettings["MultiAgentLicensses"]
                .ToString()
                .Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
            return multiAgentLicensses;
        }

        public static int IntervalGetMostVisitedPage { get; private set; }

        public static bool IsDisableMostvisitedPage
        {
            get
            {
                return Converter.ToBool(ConfigurationManager.AppSettings["IsDisableMostvisitedPage"]);
            }
        }

        public static List<int> SportBookBetTypes { get; private set; }

        private static void GetSportBookBetTypes()
        {
            SportBookBetTypes = new List<int>();
            string[] betTypes = ConfigurationManager
                .AppSettings["SportBookBetType"]
                .ToString()
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string betType in betTypes)
            {
                SportBookBetTypes.Add(Converter.ToInt(betType));
            }
        }

        public static string IPVNReplace
        {
            get;
            private set;
        }

        private static int _topRateMostVisitedPages;

        public static int TopRateMostVisitedPages
        {
            get
            {
                return _topRateMostVisitedPages;
            }
        }
    }
}