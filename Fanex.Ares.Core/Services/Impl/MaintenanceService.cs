namespace Fanex.Ares.Core.Services.Impl
{
    using Fanex.Ares.Core.DataDomain;
    using Fanex.Ares.Core.Models;
    using Fanex.Ares.Core.Services.Interface;
    using Sunplus.UMControllerLibrary;

    /// <summary>
    /// Maintenance Service.
    /// </summary>
    public class MaintenanceService : IMaintenanceService
    {
        #region Fields

        private UMBusiness _underMaintenanceService;

        #endregion

        #region Constructors

        public MaintenanceService()
        {
            _underMaintenanceService = new UMBusiness();
        }

        public MaintenanceService(UMBusiness underMaintenanceService)
        {
            _underMaintenanceService = underMaintenanceService;
        }

        #endregion

        #region Properties

        public UMBusiness UnderMaintenanceService
        {
            get { return _underMaintenanceService; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get Maintenance Information.
        /// </summary>
        /// <returns>Maintenance Information.</returns>
        public MaintenanceInfo GetMaintenanceInfo()
        {
            var maintenanceInfo = new MaintenanceInfo { IsDownTime = false };
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
            }

            return maintenanceInfo;
        }

        #endregion
    }
}
