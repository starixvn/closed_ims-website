namespace Fanex.Ares.Core.Services.Interfaces
{
    using Fanex.Ares.Core.Models;
    using Fanex.Caching;
    using Sunplus.UMControllerLibrary;

    /// <summary>
    /// IMaintenance Service.
    /// </summary>
    public interface IMaintenanceService
    {
        #region Properties

        ICache Cache { get; }

        UMBusiness UnderMaintenanceService { get; }

        #endregion

        #region Methods

        MaintenanceInfo GetMaintenanceInfo();

        #endregion
    }
}
