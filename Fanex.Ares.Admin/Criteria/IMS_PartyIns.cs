// --------------------------------------------------------------------------
// <copyright file="IMS_PartyInfoSelCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.05.30">Created</change>
// </history>

namespace Fanex.Ares.Admin.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class IMS_PartyIns : CriteriaBase
    {
        public string PartyName { get; set; }
        public int PartyType { get; set; }
        public string Description { get; set; }
        public string PartyFullName { get; set; }
        public string Company { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string BankName { get; set; }
        public string Benificiary { get; set; }
        public string BenificiaryBank { get; set; }
        public string IBAN { get; set; }
        public string SwiftCode { get; set; }
        public string CorrespondentBank { get; set; }
        public string CorrespondentAccNo { get; set; }
        public string CorrespondentSwiftCode { get; set; }
        public string AttnName { get; set; }
        public string To1 { get; set; }
        public string To2 { get; set; }
        public string Logo { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "IMS_PartyIns";
        }

        public override bool IsValid() { return true; }
    }
}


