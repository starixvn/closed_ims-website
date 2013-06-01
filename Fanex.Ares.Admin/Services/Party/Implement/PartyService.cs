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
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Ares.Admin.Models.Party;
    using Fanex.Ares.Admin.Repositories.Impl;
    using Fanex.Ares.Admin.Repositories.Interface;
    using Fanex.Ares.Admin.Services.Party.Interface;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.NAP.Framework.Data;
    using Fanex.Ares.Admin.Criteria;

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

        public IEnumerable<SelectListItem> GetAllPartyTypes(ICriteria criteria)
        {
            var listPartyTypes = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    var res = new SelectListItem
                    {
                       Value = r["PartyTypeID"].ToString(),
                       Text = r["PartyTypename"].ToString()
                    };

                    return res;
                };

                listPartyTypes = _repository.Fetch<SelectListItem>(criteria, mappingFunc).ToList();
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listPartyTypes;
        }

        public IEnumerable<SelectListItem> GetAvailableOrSelectedLicensees(ICriteria criteria)
        {
            var listAvailableLicensees = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    var brandName = r["BrandName"].ToString1();
                    var licenseeName = r["LicenseeName"].ToString1();
                    var res = new SelectListItem
                    {
                        Value = r["LicenseeID"].ToString1(),
                        Text = brandName != "" ? brandName : licenseeName
                    };

                    return res;
                };

                listAvailableLicensees = _repository.Fetch<SelectListItem>(criteria, mappingFunc).ToList();
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listAvailableLicensees;
        }

        /// <summary>
        /// Using for Insert, Update, Delete
        /// </summary>
        /// <param name="criteria">Criteria</param>
        /// <returns>ErrCode, ErrMsg</returns>
        public Hashtable ExecuteCommand(ICriteria criteria)
        {
            Hashtable rs = new Hashtable();
            try
            {
                rs = _repository.ExecuteCommand(criteria);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return rs;
        }
    }
}
