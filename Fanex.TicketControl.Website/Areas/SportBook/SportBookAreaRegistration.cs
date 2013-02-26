namespace Fanex.TicketControl.SportBook
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    public class SportBookcAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SportBook";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(this.AreaName, context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}