namespace Fanex.TicketControl.Gaming
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    public class MiscAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Gaming";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(this.AreaName, context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}