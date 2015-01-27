namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Invoice Model.
    /// </summary>
    [Serializable]
    public class InvoiceModel
    {
        public InvoiceModel()
        {
            this.DateRangeList = new List<DateRange>();
            this.StatementList = new List<Statement>();
            this.HTMLReportList = new List<string>();
            this.IsSignature = true;
        }

        public int InvoiceID { get; set; }

        public Licensee Licensee { get; set; }

        public Operator Operator { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string DueDate { get; set; }

        public int Currency { get; set; }

        public string CurrencyName { get; set; }

        public InvoiceType InvoiceType { get; set; }

        public string Product { get; set; }

        public string ProductsName { get; set; }

        public int InvoiceNo { get; set; }

        public string InvoiceName { get; set; }

        public List<Statement> StatementList { get; set; } // List statement

        public List<DateRange> DateRangeList { get; set; }

        public List<string> HTMLReportList { get; set; }

        public double TotalAmount { get; set; }

        public string TotalAmountFomarted { get; set; }

        public bool IsSignature { get; set; }

        public string Note { get; set; }
    }
}
