// --------------------------------------------------------------------------
// <copyright file="PartyService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.05.28">Created</change>
// </history>

namespace Fanex.Ares.Admin.Services.Party.Implement
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Ares.Admin.Models.Party;
    using Fanex.Ares.Admin.Repositories.Impl;
    using Fanex.Ares.Admin.Repositories.Interface;
    using Fanex.Ares.Admin.Services.Party.Interface;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.NAP.Framework.Data;

    public class PartyService : ServiceBase, IPartyService
    {
        IAdminRepository _adminRepository;

        public PartyService()
        {
            _adminRepository = new AdminRepository();
        }

        public IEnumerable<PartyModel> GetAllParties(ICriteria criteria)
        {
            var listParties = new List<PartyModel>();
            try
            {
                Func<IDataReader, PartyModel> mappingFunc = r =>
                {
                    var res = new PartyModel
                    {
                        PartyID = Convert.ToInt32(r["PartyID"]),
                        PartyName = Convert.ToString(r["PartyName"]),
                        PartyType = Convert.ToInt32(r["PartyType"]),
                        Description = Convert.ToString(r["Description"]),
                        PartyFullName = Convert.ToString(r["PartyFullName"]),
                        AccountNumber = Convert.ToString(r["AccountNumber"]),
                        AccountName = Convert.ToString(r["AccountName"]),
                        BankName = Convert.ToString(r["BankName"]),
                        Benificiary = Convert.ToString(r["Benificiary"]),
                        IBAN = Convert.ToString(r["IBAN"]),
                        SwiftCode = Convert.ToString(r["SwiftCode"]),
                        CorrespondentBank = Convert.ToString(r["CorrespondentBank"]),
                        AttnName = Convert.ToString(r["AttnName"]),
                        To1 = Convert.ToString(r["To1"]),
                        To2 = Convert.ToString(r["To2"]),
                        Logo = Convert.ToString(r["Logo"])
                    };

                    return res;
                };

                listParties = _repository.Fetch<PartyModel>(criteria, mappingFunc).ToList();
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listParties;
        }
    }
}
