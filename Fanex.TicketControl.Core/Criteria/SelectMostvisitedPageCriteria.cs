// --------------------------------------------------------------------------
// <copyright file="SelectMostvisitedPageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

using System.Diagnostics.CodeAnalysis;
using Fanex.NAP.Core.Configuration;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Configurations;
using Fanex.TicketControl.Core.Web;

namespace Fanex.TicketControl.Core.Criteria
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class SelectMostvisitedPageCriteria : CriteriaBase
    {
        /// <summary>
        /// Gets max number of total page when get most visited pages list
        /// </summary>
        public static int MaxPages
        {
            get
            {
                return AppSettings.TopRateMostVisitedPages;
            }
        }

        public string Environment
        {
            get
            {
                return NapSettings.Settings.Environment;
            }
        }

        public int UserId
        {
            get
            {
                if (AppSession.CurrentUser != null)
                {
                    return AppSession.CurrentUser.Id;
                }

                return 0;
            }
        }

        public int SiteId
        {
            get
            {
                return NapSettings.Settings.SiteId;
            }
        }

        public override string GetSettingKey()
        {
            return "Home_SelectMostvisitedPage";
        }

        /// <summary>
        /// Check data's constrains
        /// </summary>
        /// <returns>True: valid; False: invalid</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}
