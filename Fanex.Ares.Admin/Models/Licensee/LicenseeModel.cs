// -----------------------------------------------------------------------
// <copyright file="LicenseeModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Admin.Models.Licensee
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LicenseeModel
    {
        public int LicenseeID { get; set; }

        public string LicenseeName { get; set; }

        public string BrandName { get; set; }

        public string CompanyName { get; set; }

        public bool IsLabel { get; set; }

        public bool IsActive { get; set; }

        public bool IsOneWorkActive{ get; set; }

        public string AttnName { get; set; }

        public string To1 { get; set; }

        public string To2 { get; set; }
    }
}