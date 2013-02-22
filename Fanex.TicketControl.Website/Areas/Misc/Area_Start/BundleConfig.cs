namespace Fanex.TicketControl.Misc
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/misc/ticketlisting").Include(
                "~/Scripts/_Libs/TimePicker/jquery-ui-timepicker-addon.js",
                "~/Scripts/_Libs/Utils/stdlib.js",
                "~/Areas/Misc/Scripts/Core.js",
                "~/Areas/Misc/Scripts/TicketListing.js",
                "~/Areas/Misc/Scripts/TicketListingManager.js"
            ));
        }
    }
}