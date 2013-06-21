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
    using Fanex.Ares.Admin.Models.Setting;
    using Fanex.Ares.Admin.Repositories.Impl;
    using Fanex.Ares.Admin.Repositories.Interface;
    using Fanex.Ares.Admin.Services.Party.Interface;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.NAP.Framework.Data;
    using System.Web.Mvc;

    public class SettingService : ServiceBase, ISettingService
    {

        public SettingService()
        {
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
                        LicenseeName = Convert.ToString(r["BrandName"] != System.DBNull.Value ? r["BrandName"] : r["LicenseeName"]),
                        PartyID = Convert.ToInt32(r["PartyID"]),
                        PartyName = Convert.ToString(r["PartyName"]),
                        ProductID = Convert.ToInt32(r["ProductID"]),
                        ProductName = Convert.ToString(r["ProductName"]),
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
                        ProductID = Convert.ToInt32(r["ProductID"]),
                        Description = Convert.ToString(r["Description"] == System.DBNull.Value ? string.Empty : r["Description"])
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

        public IEnumerable<LicenseeProductDiscountModel> GetAllLicenseeProductDiscount(ICriteria criteria)
        {
            var listObjects = new List<LicenseeProductDiscountModel>();
            try
            {
                Func<IDataReader, LicenseeProductDiscountModel> mappingFunc = r =>
                {
                    var res = new LicenseeProductDiscountModel
                    {
                        ID = Convert.ToInt32(r["ID"]),
                        LicenseeID = Convert.ToInt32(r["LicenseeID"]),
                        ProductID = Convert.ToInt32(r["ProductID"]),
                        Currency = Convert.ToInt32(r["Currency"]),
                        FromTotalAmount = Convert.ToDouble(r["FromTotalAmount"]),
                        ToTotalAmount = Convert.ToDouble(r["ToTotalAmount"]),
                        DiscountRate = Convert.ToDouble(r["DiscountRate"]),
                        PartyID = Convert.ToInt32(r["PartyID"]),
                    };

                    return res;
                };

                listObjects = _repository.Fetch<LicenseeProductDiscountModel>(criteria, mappingFunc).ToList();
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


        public IEnumerable<System.Web.Mvc.SelectListItem> GetAllCurrenciesByLicensee(ICriteria criteria)
        {
            IList<SelectListItem> listProducts = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    return new SelectListItem
                    {
                        Value = Convert.ToString(r["currency"]),
                        Text = Convert.ToString(r["currencyname"])
                    };
                };

                listProducts = _repository.Fetch<SelectListItem>(criteria, mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listProducts;
        }
        
        public IEnumerable<SelectListItem> GetProductsByLicenseeParty(ICriteria criteria)
        {
            IList<SelectListItem> listProducts = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    return new SelectListItem
                    {
                        Value = Convert.ToString(r["productid"]),
                        Text = Convert.ToString(r["productname"])
                    };
                };

                listProducts = _repository.Fetch<SelectListItem>(criteria, mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return listProducts;
        }
    }
}
