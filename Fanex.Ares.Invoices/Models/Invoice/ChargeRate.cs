namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;

    /// <summary>
    /// Charge Rate.
    /// </summary>
    public class ChargeRate
    {
        public byte ProductID { get; set; }

        public decimal DistributorRate { get; set; }

        public decimal PartnerRate { get; set; }

        public decimal ONEworksRate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
