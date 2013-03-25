namespace Fanex.TicketControl.Gaming
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/Gaming/VoidNumberGameTicket").Include(
                "~/Scripts/_Libs/TimePicker/jquery-ui-timepicker-addon.js",
                "~/Scripts/_Libs/Utils/stdlib.js",
                "~/Scripts/Common/Message.Helper.js",
                "~/Scripts/Common/Message.Validator.js",
                "~/Areas/Gaming/Scripts/VoidNumberGameTicket/VoidNumberGameTicket.js",
                "~/Areas/Gaming/Scripts/VoidNumberGameTicket/VoidNumberGameTicketManager.js",
                "~/Areas/Gaming/Scripts/VoidNumberGameTicket/VoidNumberGame.Helper.js",
                "~/Areas/Gaming/Scripts/VoidNumberGameTicket/VoidNumberGame.Validator.js"
            ));

            bundles.Add(new ScriptBundle("~/areas/Gaming/RecoverNumberGameTicket").Include(
               "~/Scripts/_Libs/Utils/stdlib.js",
               "~/Areas/Gaming/Scripts/RecoverNumberGameTicket/RecoverNumberGameTicket.js"
           ));
        }
    }
}