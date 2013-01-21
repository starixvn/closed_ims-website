// --------------------------------------------------------------------------
// <copyright file="LicenseeAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ethan.Nguyen" date="2012.10.18">Created</change>
// </history>

using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace Fanex.TicketControl.MixParlay
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class LicenseeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MixParlay";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MixParlay_default",
                "MixParlay/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Fanex.TicketControl.MixParlay.Controllers" });
        }
    }
}
