namespace Fanex.Ares.Core.Services.Interfaces
{
    using System.Diagnostics.CodeAnalysis;
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

        #endregion Properties

        #region Methods

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed")]
        MaintenanceInfo GetMaintenanceInfo();

        #endregion Methods
    }
}