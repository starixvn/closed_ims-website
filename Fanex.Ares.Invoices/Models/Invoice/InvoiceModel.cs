// -----------------------------------------------------------------------
// <copyright file="InvoiceModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Ares.Invoice.Criteria.Invoice;
    using Fanex.Ares.Core.Models;
    using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class InvoiceModel
    {
        public Licensee Licensee { get; set; }

        public Operator Operator { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime DueDate { get; set; }

        public int Currency { get; set; }
        public string CurrencyName { get; set; }

        public InvoiceType InvoiceType { get; set; }
        public string Product { get; set; }
        public int InvoiceNo { get; set; }
        public string InvoiceName { get; set; }
        public List<Statement> StatementList { get; set; } // List statement

        public string Note { get; set; }

        public List<string> HTMLReportList { get; set; }

        public List<InvoiceReport> InvoiceReportList { get; set; }

        public InvoiceModel()
        {
            this.StatementList = new List<Statement>();
            this.InvoiceReportList = new List<InvoiceReport>();
            this.HTMLReportList = new List<string>();
        }
    }
    [Serializable]
    public class Statement
    {
        public List<StatementDetail> StatementDetailList { get; set; }

        public int StatementID { get; set; }

        public double TotalAmount { get; set; }

        public Statement()
        {
            this.StatementDetailList = new List<StatementDetail>();
        }
    }

    [Serializable]
    public class StatementDetail
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public string Reference { get; set; }

        public double Amount { get; set; }
    }

    [Serializable]
    public class Operator
    {
        public string PartyName { get; set; }

        public int PartyID { get; set; }

        public int PartyType { get; set; }

        public string Company {get;set;}

        public string Description { get; set; }

        public string PartyFullName { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public string BankName { get; set; }

        public string Benificiary { get; set; }

        public string IBAN { get; set; }

        public string SwiftCode { get; set; }

        public string CorrespondentBank { get; set; }

        public string AttnName { get; set; }

        public string To1 { get; set; }

        public string To2 { get; set; }

        public string Logo { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }
    }

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

    [Serializable]
    public class InvoiceReport {
        public string Name { get; set; }

        public LicenseeSettlementModel LicenseeModel { get; set; }

    }

}
