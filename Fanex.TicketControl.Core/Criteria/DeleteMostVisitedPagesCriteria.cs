// --------------------------------------------------------------------------
// <copyright file="DeleteMostVisitedPagesCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

using System.Diagnostics.CodeAnalysis;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Web;

namespace Fanex.TicketControl.Core.Criteria
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class DeleteMostVisitedPagesCriteria : CriteriaBase
    {
        public int CustId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        public int MenuId { get; set; }

        public override string GetSettingKey()
        {
            return "Home_DeleteMostvisitedPage";
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
