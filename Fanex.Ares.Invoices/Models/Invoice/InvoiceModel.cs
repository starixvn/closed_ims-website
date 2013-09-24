// -----------------------------------------------------------------------
// <copyright file="InvoiceModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Ares.Invoice.Criteria.Invoice;
    using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
    
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Run stylecop")]
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
