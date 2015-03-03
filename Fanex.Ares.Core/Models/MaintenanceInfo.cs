namespace Fanex.Ares.Core.Models
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Maintenance Info.
    /// </summary>
    public class MaintenanceInfo
    {
        public MaintenanceInfo()
        {
            IsDownTime = false;
            UmTimeFrom = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            UmTimeTo = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        public bool IsDownTime { get; set; }

        public string UmTimeFrom { get; set; }

        public string UmTimeTo { get; set; }
    }
}