namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Statement Class.
    /// </summary>
    [Serializable]
    public class Statement
    {
        public Statement()
        {
            this.StatementDetailList = new List<StatementDetail>();
        }

        public List<StatementDetail> StatementDetailList { get; set; }

        public int StatementID { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int No { get; set; }
    }
}
