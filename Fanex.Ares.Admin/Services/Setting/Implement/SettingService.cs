// --------------------------------------------------------------------------
// <copyright file="SettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.06.03">Created</change>
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
    using Fanex.Ares.Admin.Models.Setting;

    public class SettingService : ServiceBase, ISettingService
    {
        IAdminRepository _adminRepository;

        public SettingService()
        {
            _adminRepository = new AdminRepository();
        }

        public IEnumerable<SettingModel> GetAllSettings(ICriteria criteria)
        {
            var listSettings = new List<SettingModel>();
            try
            {
                Func<IDataReader, SettingModel> mappingFunc = r =>
                {
                    var res = new SettingModel
                    {
                        Currency = Convert.ToInt32(r["Currency"]),
                        FirstBillingDate = Convert.ToDateTime(r["FirstBillingDate"] == System.DBNull.Value ? DateTime.MinValue : r["FirstBillingDate"]),
                        HeadOfTerms = Convert.ToDateTime(r["HeadOfTerms"] == System.DBNull.Value ? DateTime.MinValue : r["HeadOfTerms"]),
                        MinimumFee = Convert.ToInt32(r["MinimumFee"] == System.DBNull.Value ? 0 : r["MinimumFee"]),
                        LicenseeID = Convert.ToInt32(r["LicenseeID"]),
                        PartyID = Convert.ToInt32(r["PartyID"]),
                        ProductID = Convert.ToInt32(r["ProductID"]),
                        WaivedDate = Convert.ToDateTime(r["WaivedDate"] == System.DBNull.Value ? DateTime.MinValue : r["WaivedDate"]),
                        WaivedMonth = Convert.ToInt32(r["WaivedMonth"] == System.DBNull.Value ? 0 : r["WaivedMonth"])
                    };

                    return res;
                };

                listSettings = _repository.Fetch<SettingModel>(criteria, mappingFunc).ToList();
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listSettings;
        }

        public IEnumerable<LicenseeProductChargeModel> GetAllLicenseeProductCharge(ICriteria criteria)
        {
            var listObjects = new List<LicenseeProductChargeModel>();
            try
            {
                Func<IDataReader, LicenseeProductChargeModel> mappingFunc = r =>
                {
                    var res = new LicenseeProductChargeModel
                    {
                        BillingCurrency = Convert.ToInt32(r["BillingCurrency"]),
                        ChargeType = Convert.ToInt32(r["ChargeType"]),
                        ONEWorksRate = Convert.ToDouble(r["ONEWorksRate"]),
                        PartnerRate = Convert.ToDouble(r["PartnerRate"]),
                        DistributorRate = Convert.ToDouble(r["DistributorRate"]),
                        StartDate = Convert.ToDateTime(r["StartDate"] == System.DBNull.Value ? DateTime.MinValue : r["StartDate"]),
                        EndDate = Convert.ToDateTime(r["EndDate"] == System.DBNull.Value ? DateTime.MinValue : r["EndDate"]),
                        LicenseeID = Convert.ToInt32(r["LicenseeID"]),
                        PartyID = Convert.ToInt32(r["PartyID"]),
                        ProductID = Convert.ToInt32(r["ProductID"])
                    };

                    return res;
                };

                listObjects = _repository.Fetch<LicenseeProductChargeModel>(criteria, mappingFunc).ToList();
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listObjects;
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
