// -----------------------------------------------------------------------
// <copyright file="InvoiceCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Criteria.Invoice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IMS_InvoiceCriteria
    {
        public int OperatorID { get; set; }

        public string OperatorName { get; set; }

        public int LicenseeID { get; set; }

        public string LicenseeName { get; set; }

        public string ExchangeName { get; set; }

        public int InvoiceNo { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime DueDate { get; set; }

        //public List<DateRangeCriteria> DateRangeList { get; set; }

        //public List<ProductCriteria> ProductList { get; set; }

    }

    public class DateRangeCriteria
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }

    public class ProductCriteria
    {
        public string ProductName { get; set; }

        public int ProductID { get; set; }
    }
}
