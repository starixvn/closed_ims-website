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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InvoiceModel
    {
        public List<Statement> StatementList { get; set; }


        public double TotalAmount { get; set; }

        public InvoiceModel()
        {
            this.StatementList = new List<Statement>();
        }

    }

    public class Statement
    {
        public int ID { get; set; }

        public string Description { get; set; }

        public string Reference { get; set; }

        public double Amount { get; set; }
    }


    public class Licensee
    {
        public string LicenseeName { get; set; }

        public int LicenseeID { get; set; }
    }

    public class Operator
    {
        public string OperatorName { get; set; }

        public int OperatorID { get; set; }

    }
}
