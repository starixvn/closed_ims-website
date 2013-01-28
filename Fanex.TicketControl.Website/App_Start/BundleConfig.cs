using System.Web;
using System.Web.Optimization;

namespace Fanex.TicketControl.Website
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Common Scripts every page will use
            bundles.Add(new StyleBundle("~/Scripts/_Libs/Tooltip/style").Include(
            "~/Scripts/_Libs/Tooltip/tipsy.css"
            ));
            bundles.Add(new StyleBundle("~/Themes/base/jquery/style").Include(
            "~/Themes/base/jquery/jquery-ui-1.9.1.custom.css"
            ));
            bundles.Add(new StyleBundle("~/Themes/style").Include(
            "~/Themes/reset.css"
            ));
            bundles.Add(new StyleBundle("~/Scripts/_Libs/Pager/style").Include(
            "~/Scripts/_Libs/Pager/pagination.css"
            ));
            bundles.Add(new ScriptBundle("~/Scripts-bundle/jquery").Include(
                       "~/Scripts/_Libs/jQuery/jquery-*",
                       "~/Scripts/_Libs/BlockUI/jquery.blockUI.1.33.js",
                       "~/Scripts/_Libs/FormatLibs/NumberFormat/numeral.js",
                       "~/Scripts/_Libs/FanexGrid/Fanex.Grid.js",
                       "~/Scripts/_Libs/Pager/jpager.js",
                       "~/Scripts/_Libs/Tooltip/jquery.tipsy.js",
                       "~/Scripts/NapAccReports.js"
                        ));
            #endregion

            #region Multiselect Plugin
            bundles.Add(new StyleBundle("~/Scripts/_Libs/MultipleSelect/style").Include(
            "~/Scripts/_Libs/MultipleSelect/jquery.multiselect.css"
            ));
            bundles.Add(new ScriptBundle("~/Scripts/_Libs/multiselect").Include(
                      "~/Scripts/_Libs/MultipleSelect/jquery.multiselect.js"));

            #endregion

            #region Nex Date Picker
            bundles.Add(new StyleBundle("~/Scripts/_Libs/DatePicker/style").Include(
             "~/Scripts/_Libs/DatePicker/jquery.nexDatePicker.css"
             ));
            bundles.Add(new ScriptBundle("~/Scripts/_Libs/nexDatePicker").Include(
                      "~/Scripts/_Libs/DatePicker/jquery.nexDatePicker.js"));

            #endregion

            #region Client Template Render
            bundles.Add(new ScriptBundle("~/Scripts/_Libs/template").Include(
                     "~/Scripts/_Libs/TemplateRender/doT/doT.js"));

            #endregion

            #region Month Picker
            bundles.Add(new StyleBundle("~/Scripts/_Libs/MonthPicker/style").Include(
            "~/Scripts/_Libs/MonthPicker/monPickr-0.0.3.css"
            ));
            bundles.Add(new ScriptBundle("~/Scripts/_Libs/monPickr").Include(
                      "~/Scripts/_Libs/MonthPicker/monPickr-0.0.3.js"));
            #endregion

            #region Knockout
            bundles.Add(new ScriptBundle("~/scripts-bundle/knockout").Include(
                        "~/Scripts/_Libs/knockout*"));
            #endregion
        }
    }
}