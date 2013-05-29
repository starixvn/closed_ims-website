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
