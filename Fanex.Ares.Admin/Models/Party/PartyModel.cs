// -----------------------------------------------------------------------
// <copyright file="PartyModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Admin.Models.Party
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PartyModel
    {
        public string PartyName { get; set; }

        public int PartyType { get; set; }

        public string Description { get; set; }

        public string PartyFullName { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string BankName { get; set; }

        public string Benificiary { get; set; }

        public string IBAN { get; set; }

        public string SwiftCode { get; set; }

        public string CorrespondentBank { get; set; }

        public string AttnName { get; set; }

        public string To1 { get; set; }

        public string To2 { get; set; }

        //public PartyModel()
        //{
        //    PartyName = "Boping";
        //    PartyType = 1;
        //    Description = "Description";
        //    PartyFullName = " Boping Full";
        //    AccountName = "sa";
        //    AccountNumber = "12345";
        //    BankName = "ACB";
        //    Benificiary = "";
        //    IBAN = "4325";
        //    SwiftCode = "45674";
        //    CorrespondentBank = "CorrespondentBank";
        //    AttnName = "AttnName";
        //    To1 = "New York";
        //    To2 = "Ho chi minh";
        //}
    }
}
