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
    using Fanex.Ares.Admin.Models.Licensee;
    using Fanex.Ares.Admin.Models.Party;
    using Fanex.Ares.Admin.Repositories.Impl;
    using Fanex.Ares.Admin.Repositories.Interface;
    using Fanex.Ares.Admin.Services.Party.Interface;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.NAP.Framework.Data;

    public class LicenseeService : ServiceBase, ILicenseeService
    {
        IAdminRepository _adminRepository;

        public LicenseeService()
        {
            _adminRepository = new AdminRepository();
        }

        public IEnumerable<LicenseeModel> GetAllLicensees(ICriteria criteria)
        {
            var listLicensees = new List<LicenseeModel>();
            try
            {
                Func<IDataReader, LicenseeModel> mappingFunc = r =>
                {
                    var res = new LicenseeModel
                    {
                        LicenseeID = Convert.ToInt32(r["LicenseeID"]),
                        LicenseeName = r["LicenseeName"].ToString1(),
                        BrandName = r["BrandName"].ToString1(),
                        CompanyName = r["CompanyName"].ToString1(),
                        IsLabel = Convert.ToBoolean(r["IsLabel"]),
                        IsActive = Convert.ToBoolean(r["IsActive"]),
                        IsOneWorkActive = Convert.ToBoolean(r["IsOneWorkActive"]),
                        AttnName = r["AttnName"].ToString1(),
                        To1 = r["To1"].ToString1(),
                        To2 = r["To2"].ToString1()
                    };

                    return res;
                };

                listLicensees = _repository.Fetch<LicenseeModel>(criteria, mappingFunc).ToList();
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listLicensees;
        }

        public IEnumerable<SelectListItem> GetAvailableOrSelectedParties(ICriteria criteria)
        {
            var listAvailableLicensees = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    var res = new SelectListItem
                    {
                        Value = r["PartyID"].ToString1(),
                        Text = r["PartyName"].ToString1()
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
