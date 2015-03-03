namespace Fanex.Ares.Website
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

            CommonBundle(bundles);

            BundleTable.EnableOptimizations = true;
        }

        /// <summary>
        /// Adds the Common.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void CommonBundle(BundleCollection bundles)
        {
            var commonScriptsBundle = new Bundle("~/bundles/CommonJs").Include(
                "~/Scripts/_Libs/jQuery/jquery-1.8.2.js",
                "~/Scripts/_Libs/jQuery/jquery.form.js",
                "~/Scripts/_Libs/jQuery/jquery-ui-1.9.1.js",
                "~/Scripts/_Libs/jQuery/jquery.jqprint-0.3.js",
                "~/Scripts/_Libs/jQuery/jquery.formattyping.js",
                "~/Scripts/_Libs/Bootstrap/bootstrap-alert.js",
                "~/Scripts/_Libs/Bootstrap/bootstrap-tooltip.js",
                "~/Scripts/_Libs/BlockUI/jquery.blockUI.1.33.js",
                "~/Scripts/_Libs/FormatLibs/NumberFormat/numeral.js",
                "~/Scripts/_Libs/Validation/jquery.validationEngine-en.js",
                "~/Scripts/_Libs/Validation/jquery.validationEngine.js",
                "~/Scripts/_Libs/Msgbox/msgbox-0.0.1.js",
                "~/Scripts/ReCheckIn/ReCheckIn.js");

            commonScriptsBundle.Transforms.Add(_scripsTransformer);

            var commonStylesBundle = new Bundle("~/bundles/CommonCss")
                .Include("~/Content/jquery-ui/jquery-ui-1.9.1.custom.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Validation/validationEngine.jquery.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/base/style.less", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Msgbox/msgbox-0.0.1.css", new CssRewriteUrlTransformWrapper());

            commonStylesBundle.Transforms.Add(_cssTransformer);

            bundles.Add(commonStylesBundle);
            bundles.Add(commonScriptsBundle);
        }
    }
}