namespace Fanex.Ares.Website.Areas.Report
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
            LicenseeSettlementBundle(bundles);
        }

        /// <summary>
        /// Licensee Settlement Bundle.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void LicenseeSettlementBundle(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/LicenseeSettlementJs")
                .Include("~/Scripts/_Libs/MultipleSelect/jquery.multiselect.js")
                .Include("~/Scripts/_Libs/DatePicker/jquery.nexDatePicker.js")
                .Include("~/Scripts/_Libs/MonthPicker/monPickr-0.0.3.js")
                .Include("~/Scripts/_Libs/tooltip/jquery.tipsy.js")
                .Include("~/Areas/Report/Scripts/LicenseeSettlement.js"));

            bundles.Add(new StyleBundle("~/bundles/LicenseeSettlementCss")
                .Include("~/Content/Libs/MultipleSelect/jquery.multiselect.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Libs/DatePicker/jquery.nexDatePicker.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Libs/MonthPicker/monPickr-0.0.3.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Libs/MonthPicker/monPickr-0.0.3-custom.less", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Libs/Tooltip/tipsy.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Areas/Report/Content/LicenseeSettlement.css", new CssRewriteUrlTransformWrapper()));
        }
    }
}