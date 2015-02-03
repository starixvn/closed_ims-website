namespace Fanex.Ares.Core.Web.Controllers
{
    using System.Configuration;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// Base Controller.
    /// </summary>
    public class BaseController : Controller
    {
        protected readonly ICurrentContext CurrentContext;

        protected readonly ICurrentSession CurrentSession;

        protected readonly IServiceContext ServiceContext;

        protected readonly string Environment = NapSettings.Settings.Environment;

        protected readonly int SiteId = NapSettings.Settings.SiteId;

        protected readonly string HubUrl = "http://10.18.200.209:8009/nap-home-local2";

        private ViewFlags _viewFlags;

        private int _issiteLogin = NapSettings.Settings.IsSiteLogin;

        public BaseController()
        {
            this.CurrentContext = new CurrentContext();
            ViewBag.Session = this.CurrentSession = new CurrentSession();
            this.ServiceContext = new ServiceContext
            {
                UserId = this.CurrentSession.UserId,
                SiteId = this.SiteId,
                Ip = this.CurrentContext.IP
            };

            ViewFlags = new ViewFlags
            {
                IsLive = this.Environment.Equals(Constant.LIVE),
                HasEditRight = false
            };
        }

        /// <summary>
        /// Gets ... Indicates whether the current site allows users login directly.
        /// If "0", users must login through NAP Hub.
        /// </summary>
        /// <value>Is Site Login.</value>
        public int IsSiteLogin
        {
            get
            {
                return this._issiteLogin;
            }
        }

        protected ViewFlags ViewFlags
        {
            get
            {
                return ViewBag.ViewFlags;
            }

            set
            {
                ViewBag.ViewFlags = this._viewFlags = value;
            }
        }

        protected string ExcelTemplateRootPath
        {
            get { return Server.MapPath(ConfigurationManager.AppSettings["ExcelTemplatePath"]); }
        }

        /// <summary>
        /// Execute before each request, if Downtime then go to Downtime page,
        /// Or not Authenticate will go to Login Page.
        /// </summary>
        /// <param name="filterContext">Filter Context.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AppSession.CurrentActionName = filterContext.ActionDescriptor.ActionName;
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// On Result Executing.
        /// </summary>
        /// <param name="filterContext">Filter Context.</param>
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }

        /// <summary>
        /// Override function.
        /// </summary>
        /// <param name="data">Object data.</param>
        /// <param name="contentType">Content type.</param>
        /// <param name="contentEncoding">Content Encoding.</param>
        /// <param name="behavior">The behavior.</param>
        /// <returns>Return result.</returns>
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }
    }
}
