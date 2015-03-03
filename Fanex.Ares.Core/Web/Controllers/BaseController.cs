namespace Fanex.Ares.Core.Web.Controllers
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// Base Controller.
    /// </summary>
    public class BaseController : Controller
    {
        #region Fields

        private readonly string _environment = NapSettings.Settings.Environment;

        private readonly int _siteId = NapSettings.Settings.SiteId;

        private readonly string _hubUrl = NapSettings.Settings.HubUrl;

        private ICurrentContext _currentContext;

        private ICurrentSession _currentSession;

        private IServiceContext _serviceContext;

        private bool _issiteLogin = NapSettings.Settings.EnableDirectLogin;

        #endregion Fields

        #region Constructors

        public BaseController()
        {
            _currentContext = new CurrentContext();
            ViewBag.Session = _currentSession = new CurrentSession();

            _serviceContext = new ServiceContext
            {
                UserId = CurrentSession.UserId,
                SiteId = _siteId,
                Ip = CurrentContext.IP
            };
        }

        #endregion Constructors

        #region Properties

        public bool IsSiteLogin
        {
            get
            {
                return this._issiteLogin;
            }
        }

        protected string ExcelTemplateRootPath
        {
            get { return Server.MapPath(ConfigurationManager.AppSettings["ExcelTemplatePath"]); }
        }

        protected string CurrentDomain
        {
            get { return Server.MapPath(string.Empty); }
        }

        protected ICurrentContext CurrentContext
        {
            get { return _currentContext; }
        }

        protected ICurrentSession CurrentSession
        {
            get { return _currentSession; }
        }

        protected IServiceContext ServiceContext
        {
            get { return _serviceContext; }
        }

        protected string Environment
        {
            get { return _environment; }
        }

        protected int SiteId
        {
            get { return _siteId; }
        }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed")]
        protected string HubUrl
        {
            get { return _hubUrl; }
        }

        #endregion Properties

        #region Methods

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AppSession.CurrentActionName = filterContext.ActionDescriptor.ActionName;
            base.OnActionExecuting(filterContext);
        }

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

        #endregion Methods
    }
}