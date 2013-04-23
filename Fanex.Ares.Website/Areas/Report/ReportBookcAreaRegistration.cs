using System.Web.Mvc;
using System.Web.Optimization;
using Fanex.Ares.Website.Areas.Report.Area_Start;

namespace Fanex.Ares.Website.Areas.Report
{
    public class ReportBookcAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Report";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(this.AreaName, context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}