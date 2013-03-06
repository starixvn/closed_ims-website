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
                "~/Areas/SportBook/Scripts/VoidSingleTicket.js",
                "~/Areas/SportBook/Scripts/VoidSingleTicketHelper.js",
                "~/Areas/SportBook/Scripts/VoidSingleTicketValidateData.js",
                "~/Areas/SportBook/Scripts/VoidSingleTicketManager.js"
            ));
        }
    }
}