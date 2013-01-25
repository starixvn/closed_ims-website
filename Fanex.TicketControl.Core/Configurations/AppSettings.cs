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
        public static bool IsLocalTest { get; private set; }

        static AppSettings()
        {
            IsLocalTest = ConfigurationManager.AppSettings["IsLocalTest"] == "1";
        }
    }
}