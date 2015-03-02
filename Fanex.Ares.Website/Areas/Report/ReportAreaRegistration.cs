namespace Fanex.Ares.Website.Areas.Report
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// Report Area Registration.
    /// </summary>
    public class ReportAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Report";
            }
        }

        /// <summary>
        /// Register Area.
        /// </summary>
        /// <param name="context">Area Registration Context.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(this.AreaName, context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}