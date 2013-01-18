// --------------------------------------------------------------------------
// <copyright file="UserControlAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ethan.Nguyen" date="2012.10.18">Created</change>
// </history>

using System.Web.Mvc;

namespace Fanex.TicketControl.Website.Areas.UserControl
{
    public class UserControlAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UserControl";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "UserControl_default",
                "UserControl/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional });
        }
    }
}
