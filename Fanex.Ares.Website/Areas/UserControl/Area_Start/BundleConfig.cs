namespace Fanex.Areas.Website.Areas.UserControl
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
            bundles.Add(new ScriptBundle("~/areas/UserControl").Include(
                "~/Scripts/Common/Message.Helper.js",
                "~/Scripts/Common/Message.Validator.js"));
        }
    }
}