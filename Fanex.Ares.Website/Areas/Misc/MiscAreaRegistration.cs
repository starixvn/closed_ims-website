using System.Web.Mvc;
using System.Web.Optimization;
using Fanex.Ares.Website.Areas.Misc.Area_Start;

namespace Fanex.Ares.Website.Areas.Misc
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Misc";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(this.AreaName, context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}