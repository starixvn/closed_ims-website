namespace Fanex.TicketControl.Misc
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/areas/misc/ticketlisting").Include(
                "~/Areas/Misc/Scripts/TicketListing.js",
                "~/Areas/Misc/Scripts/TicketListingManager.js"
            ));
        }
    }
}