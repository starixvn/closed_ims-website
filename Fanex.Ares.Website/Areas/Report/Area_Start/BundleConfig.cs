namespace Fanex.Ares.Website.Areas.Report
{
    using System.Web.Optimization;
    using BundleTransformer.Core.Resolvers;
    using BundleTransformer.Core.Transformers;

    /// <summary>
    /// Bundle Config.
    /// </summary>
    public static class BundleConfig
    {
        private static StyleTransformer _cssTransformer = new StyleTransformer();
        private static ScriptTransformer _scripsTransformer = new ScriptTransformer();

        /// <summary>
        /// Register Bundles.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            _cssTransformer.CombineFilesBeforeMinification = true;
            _scripsTransformer.CombineFilesBeforeMinification = true;
            BundleResolver.Current = new CustomBundleResolver();

            LicenseeSettlementBundle(bundles);
        }

        /// <summary>
        /// Licensee Settlement Bundle.
        /// </summary>
        /// <param name="bundles">Bundle Collection.</param>
        private static void LicenseeSettlementBundle(BundleCollection bundles)
        {
            var scriptsBundle = new Bundle("~/bundles/LicenseeSettlementJs")
                .Include("~/Scripts/_Libs/MultipleSelect/jquery.multiselect.js")
                .Include("~/Scripts/_Libs/DatePicker/jquery.nexDatePicker.js")
                .Include("~/Scripts/_Libs/MonthPicker/monPickr-0.0.3.js")
                .Include("~/Scripts/_Libs/tooltip/jquery.tipsy.js")
                .Include("~/Areas/Report/Scripts/LicenseeSettlement.js");

            var stylesBundle = new Bundle("~/bundles/LicenseeSettlementCss")
                .Include("~/Content/Libs/MultipleSelect/jquery.multiselect.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Libs/DatePicker/jquery.nexDatePicker.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Libs/MonthPicker/monPickr-0.0.3.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Libs/MonthPicker/monPickr-0.0.3-custom.less", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Libs/Tooltip/tipsy.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Areas/Report/Content/LicenseeSettlement.css", new CssRewriteUrlTransformWrapper());

            stylesBundle.Transforms.Add(_cssTransformer);
            scriptsBundle.Transforms.Add(_scripsTransformer);

            bundles.Add(stylesBundle);
            bundles.Add(scriptsBundle);
        }
    }
}