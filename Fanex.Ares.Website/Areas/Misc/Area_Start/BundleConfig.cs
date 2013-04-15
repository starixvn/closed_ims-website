using System.Web.Optimization;

namespace Fanex.Ares.Website.Areas.Misc.Area_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/misc/ticketlisting").Include(
                "~/Scripts/_Libs/TimePicker/jquery-ui-timepicker-addon.js",
                "~/Scripts/_Libs/Utils/stdlib.js",
                "~/Areas/Misc/Scripts/TicketListing.js",
                "~/Areas/Misc/Scripts/TicketListingManager.js"
            ));

            bundles.Add(new ScriptBundle("~/areas/misc/BatchMessage").Include(
                "~/Scripts/_Libs/Utils/stdlib.js",
                "~/Scripts/Common/Message.Helper.js",
                "~/Scripts/Common/Message.Validator.js",
                "~/Areas/Misc/Scripts/BatchMessage/BatchMessageHelper.js",
                "~/Areas/Misc/Scripts/BatchMessage/BatchMessageValidateData.js",
                "~/Areas/Misc/Scripts/BatchMessage/BatchMessage.js"                
            ));
        }
    }
}