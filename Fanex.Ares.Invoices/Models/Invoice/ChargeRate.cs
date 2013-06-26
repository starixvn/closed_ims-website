// -----------------------------------------------------------------------
// <copyright file="ChargeRate.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ChargeRate
    {
        public int ProductID { get; set; }

        public decimal DistributorRate { get; set; }

        public decimal PartnerRate { get; set; }

        public decimal ONEworksRate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
    
}
