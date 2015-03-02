namespace Fanex.Ares.Website.Areas.Invoice
{
    using System.Web.Mvc;
    using System.Web.Optimization;

    /// <summary>
    /// Invoice Area Registration.
    /// </summary>
    public class InvoiceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Invoice";
            }
        }

        /// <summary>
        /// Register Area.
        /// </summary>
        /// <param name="context">AreaRegistration Context.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            RouteConfig.RegisterRoutes(this.AreaName, context);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}