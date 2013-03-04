namespace Fanex.TicketControl.SportBook
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/SportBook/DeleteSingleTicket").Include(
                "~/Scripts/_Libs/TimePicker/jquery-ui-timepicker-addon.js",
                "~/Scripts/_Libs/Utils/stdlib.js",
                "~/Scripts/_Libs/FormatLibs/NumberFormat/numeral.js",
                "~/Areas/SportBook/Scripts/DeleteSingleTicket.js",
                "~/Areas/SportBook/Scripts/DeleteSingleTicketManager.js"
            ));
        }
    }
}