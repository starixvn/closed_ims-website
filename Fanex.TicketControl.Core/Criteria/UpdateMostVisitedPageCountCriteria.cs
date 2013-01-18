// --------------------------------------------------------------------------
// <copyright file="UpdateMostVisitedPageCountCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

using System.Diagnostics.CodeAnalysis;
using Fanex.NAP.Framework.Data;

namespace Fanex.TicketControl.Core.Criteria
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class UpdateMostVisitedPageCountCriteria : CriteriaBase
    {
        /// <summary>
        /// Gets or sets customer id
        /// </summary>
        public int CustId { get; set; }

        public string MenuId { get; set; }

        public string Count { get; set; }

        public override string GetSettingKey()
        {
            return "Home_UpdateMostvisitedPageCount";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
