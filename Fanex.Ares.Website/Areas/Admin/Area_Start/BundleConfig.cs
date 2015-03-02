namespace Fanex.Ares.Website.Areas.Admin
{
    using System.Web.Optimization;

    /// <summary>
    /// Bundle Config.
    /// </summary>
    public static class BundleConfig
    {
        /// <summary>
        /// Register Bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/admin/ticketlisting").Include(
                "~/Scripts/_Libs/TimePicker/jquery-ui-timepicker-addon.js",
                "~/Scripts/_Libs/Utils/stdlib.js",
                "~/Areas/Misc/Scripts/TicketListing.js",
                "~/Areas/Misc/Scripts/TicketListingManager.js"));

            bundles.Add(new ScriptBundle("~/areas/admin/BatchMessage").Include(
                "~/Scripts/_Libs/Utils/stdlib.js",
                "~/Scripts/Common/Message.Helper.js",
                "~/Scripts/Common/Message.Validator.js",
                "~/Areas/Misc/Scripts/BatchMessage/BatchMessageHelper.js",
                "~/Areas/Misc/Scripts/BatchMessage/BatchMessageValidateData.js",
                "~/Areas/Misc/Scripts/BatchMessage/BatchMessage.js"));
        }
    }
}