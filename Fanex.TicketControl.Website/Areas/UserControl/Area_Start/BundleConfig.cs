using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Fanex.TicketControl.Website.Areas.UserControl.Area_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/UserControl").Include(
                "~/Scripts/Common/Message.Helper.js",
                "~/Scripts/Common/Message.Validator.js"
            ));
        }
    }
}