namespace Fanex.Ares.Core.Services.Impl
{
    using System;
    using Fanex.Ares.Core.DataDomain;
    using Fanex.Ares.Core.Models;
    using Fanex.Ares.Core.Services.Interfaces;
    using Fanex.Caching;
    using Sunplus.UMControllerLibrary;

    /// <summary>
    /// Maintenance Service.
    /// </summary>
    public class MaintenanceService : IMaintenanceService
    {
        #region Fields

        private const int CacheTimeoutMinute = 60;

        private ICache _cache;

        private UMBusiness _underMaintenanceService;

        #endregion Fields

        #region Constructors

        public MaintenanceService()
        {
            _underMaintenanceService = new UMBusiness();
            _cache = CacheService.CreateInstance(CacheType.MemoryCache);
        }

        public MaintenanceService(ICache cache, UMBusiness underMaintenanceService)
        {
            _underMaintenanceService = underMaintenanceService;
            _cache = cache;
        }

        #endregion Constructors

        #region Properties

        public UMBusiness UnderMaintenanceService
        {
            get { return _underMaintenanceService; }
        }

        public ICache Cache
        {
            get { return _cache; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get Maintenance Information.
        /// </summary>
        /// <returns>Maintenance Information.</returns>
        public MaintenanceInfo GetMaintenanceInfo()
        {
            var maintenanceInfo = Cache.Get<MaintenanceInfo>(CacheServiceKeys.MaintenanceInformation);

            if (maintenanceInfo == null)
            {
                maintenanceInfo = new MaintenanceInfo { IsDownTime = false };
                var maintenanceHost = Constants.MaintenanceHost;

                if (!string.IsNullOrEmpty(maintenanceHost))
                {
                    var fromDate = maintenanceInfo.UmTimeFrom;
                    var toDate = maintenanceInfo.UmTimeTo;

                    maintenanceInfo.IsDownTime = _underMaintenanceService.isUnderMaintainence(Sites.AccountSite, maintenanceHost, ref fromDate, ref toDate);

                    if (maintenanceInfo.IsDownTime)
                    {
                        maintenanceInfo.UmTimeFrom = fromDate;
                        maintenanceInfo.UmTimeTo = toDate;
                    }

                    Cache.Set(CacheServiceKeys.MaintenanceInformation, maintenanceInfo, DateTime.Now.AddMinutes(CacheTimeoutMinute));
                }
            }

            return maintenanceInfo;
        }

        #endregion Methods
    }
}