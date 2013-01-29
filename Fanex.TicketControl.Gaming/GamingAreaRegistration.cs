// --------------------------------------------------------------------------
// <copyright file="LicenseeAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ethan.Nguyen" date="2012.10.18">Created</change>
// </history>

namespace Fanex.TicketControl.Gaming
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class GamingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Gaming";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Gaming_default",
                "Gaming/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { 
                    "Fanex.TicketControl.Gaming.Controllers",
                    "Fanex.TicketControl.Gaming.Controllers.LiveCasino",
                    "Fanex.TicketControl.Gaming.Controllers.NumberGame",
                    "Fanex.TicketControl.Gaming.Controllers.VirtualSports"
                });
        }
    }
}
