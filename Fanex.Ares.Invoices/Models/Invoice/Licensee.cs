namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;

    /// <summary>
    /// Licensee Class.
    /// </summary>
    [Serializable]
    public class Licensee
    {
        public string LicenseeName { get; set; }

        public int LicenseeID { get; set; }

        public string BrandName { get; set; }

        public string CompanyName { get; set; }

        public bool IsLabel { get; set; }

        public bool IsActive { get; set; }

        public string AttnName { get; set; }

        public string To1 { get; set; }

        public string To2 { get; set; }
    }
}
