namespace Fanex.Ares.Website
{
    using System.Web.Configuration;
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
            CommonBundle(bundles);
            InvoicePreviewCSS(bundles);
            LicenseeSettlementBundle(bundles);

            BundleTable.EnableOptimizations = WebConfigurationManager.AppSettings["EnableMinifyJsCss"] == "true";
        }

        /// <summary>
        /// Adds the Common.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void CommonBundle(BundleCollection bundles)
        {
            var scriptsBundle = new ScriptBundle("~/bundles/CommonJs").Include(
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

            var stylesBundle = new StyleBundle("~/bundles/CommonCss")
                .Include("~/Content/jquery-ui/jquery-ui-1.9.1.custom.min.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Validation/validationEngine.jquery.css", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/base/style.less", new CssRewriteUrlTransformWrapper())
                .Include("~/Content/Msgbox/msgbox-0.0.1.css", new CssRewriteUrlTransformWrapper());

            bundles.Add(stylesBundle);
            bundles.Add(scriptsBundle);
        }

        private static void InvoicePreviewCSS(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/Invoice/InvoicePreviewCss")
                .Include("~/Content/Invoice/InvoicePreview.css")
                .Include("~/Areas/Report/Content/LicenseeSettlement.css"));
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