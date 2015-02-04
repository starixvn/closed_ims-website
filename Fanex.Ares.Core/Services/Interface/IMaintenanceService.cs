namespace Fanex.Ares.Core.Services.Interface
{
    using Fanex.Ares.Core.Models;
    using Sunplus.UMControllerLibrary;

    /// <summary>
    /// IMaintenance Service.
    /// </summary>
    public interface IMaintenanceService
    {
        #region Properties

        UMBusiness UnderMaintenanceService { get; }

        #endregion

        #region Methods

        MaintenanceInfo GetMaintenanceInfo();

        #endregion
    }
}
