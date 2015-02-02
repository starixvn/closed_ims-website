namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;

    /// <summary>
    /// Party Class.
    /// </summary>
    [Serializable]
    public class Party
    {
        public string PartyName { get; set; }

        public byte PartyID { get; set; }

        public byte PartyType { get; set; }

        public string Company { get; set; }

        public string Description { get; set; }

        public string PartyFullName { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public string BankName { get; set; }

        public string Benificiary { get; set; }

        public string BenificiaryBank { get; set; }

        public string Iban { get; set; }

        public string SwiftCode { get; set; }

        public string CorrespondentBank { get; set; }

        public string CorrespondentAccNo { get; set; }

        public string CorrespondentSwiftCode { get; set; }

        public string AttnName { get; set; }

        public string To1 { get; set; }

        public string To2 { get; set; }

        public string Logo { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public bool IsHide { get; set; }
    }
}
