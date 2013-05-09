using System.Web.Mvc;
using System.Web.Optimization;
using Fanex.Ares.Website.Areas.Invoice.Area_Start;

namespace Fanex.Ares.Website.Areas.Invoice
{
    public class InvoiceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Invoice";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(this.AreaName, context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}