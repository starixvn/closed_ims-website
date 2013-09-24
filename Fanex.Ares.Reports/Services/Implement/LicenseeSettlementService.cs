// -----------------------------------------------------------------------
// <copyright file="LicenseeSettlementService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Report.Services.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Ares.Core.Criteria;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.Ares.Core.Utils;
    using Fanex.Ares.Report.Criteria.LicenseeSettlement;
    using Fanex.Ares.Report.ViewModels;
    using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
    using Fanex.Common.Extensions;

    /// <summary>
    /// Licensee Settlement Service.
    /// </summary>
    public class LicenseeSettlementService : ServiceBase, ILicenseeSettlementService
    {
        #region ctor
        public LicenseeSettlementService()
        {
        }
        #endregion

        /// <summary>
        /// Get File Name.
        /// </summary>
        /// <returns>Return file name.</returns>
        public string GetFileName()
        {
            LicenseeSettlementModel model = null;
            if (SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport) == null)
            {
                return string.Empty;
            }

            model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport);
            
            return string.Format(
                "{0}_{1}_Settlement_From_{2}_To_{3}",
                model.LicenseeInfor.LicenseeName,
                model.LicenseeInfor.ProductName,
                model.LicenseeInfor.FromDate.ToAresDayMonthYear(),
                model.LicenseeInfor.ToDate.ToAresDayMonthYear());
        }

        #region Generate Report
       
        /// <summary>
        /// Generate Report.
        /// </summary>
        /// <param name="criteria">Licensee criteria.</param>
        /// <returns>Return result.</returns>
        public LicenseeSettlementModel GenerateReport(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            //// Licensee Information
            var listLicensee = this.GetAllLicenseesEx();
            var listProduct = this.GetProductsByLicensee(new IMS_LicenseeProductSelCriteria() { LicenseeId = criteria.LicenseeId });
            var listCurrency = this.GetCurrencyByLicensee(new IMS_LicenseeCurrencySelCriteria() { LicenseeId = criteria.LicenseeId });
            var listBaseCurrency = this.GetBaseCurrency(new IMS_BaseCurrencySel());
            var model = new LicenseeSettlementModel();
            var licenseeInfoCriteria = new IMS_LicenseeBillingInfoSelCriteria();
            var exchangeRateCriteria = new IMS_ExchangeRateBillingSelCriteria();
            if (criteria.InvoiceReport)
            {
                licenseeInfoCriteria = new IMS_Invoice_LicenseeBillingInfoSelCriteria();
                exchangeRateCriteria = new IMS_Invoice_ExchangeRateBillingSelCriteria();
            }

            try
            {
                licenseeInfoCriteria.LicenseeId = criteria.LicenseeId;
                licenseeInfoCriteria.ProductId = criteria.Product;
                licenseeInfoCriteria.PartyId = criteria.PartyId;
                model.LicenseeInfor = this.GetLicenseeInformation(licenseeInfoCriteria);
                model.LicenseeInfor.LicenseeID = criteria.LicenseeId;
                var licenseeItem = listLicensee.Where(m => m.Item.Value == criteria.LicenseeId.ToString()).FirstOrDefault<SelectListItemEx>();
                criteria.LicenseeName = Convert.ToString(licenseeItem.Extension["LicenseeName"]);
                model.LicenseeInfor.LicenseeName = licenseeItem.Item.Text;
                model.LicenseeInfor.ProductName = criteria.ProductName;
                model.LicenseeInfor.SettlementDate = criteria.ToDate;
                model.Report = this.GetLicenseeReport(criteria);
                //// Exchange Rate                
                model.ExchangeList = new List<ViewModels.ExchangeRate>();
                var exchangeRatecurrencies = string.Empty;
                exchangeRatecurrencies = string.Join(",", model.Report.EntityList.Select(m => m.Currency).Distinct().ToArray());
                exchangeRateCriteria.Currency = exchangeRatecurrencies;
                exchangeRateCriteria.ExchangeDate = criteria.ToDate;
                exchangeRateCriteria.BaseCurrencyId = criteria.BaseCurrency;
                exchangeRateCriteria.BaseCurrencyName = listBaseCurrency.Where(m => m.Value == criteria.BaseCurrency.ToString()).FirstOrDefault<SelectListItem>().Text;
                var exchangeList = this.GetLicenseeExchangeRate(exchangeRateCriteria).ToList();
                model.ExchangeList = exchangeList;
                model.LicenseeInfor.BaseCurrencyName = exchangeRateCriteria.BaseCurrencyName;
                SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport, model);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return model;
        }
        #endregion

        /// <summary>
        /// Get Currency by licensee.
        /// </summary>
        /// <param name="criteria">Licensee criteria.</param>
        /// <returns>Return result.</returns>
        public IEnumerable<SelectListItem> GetCurrencyByLicensee(IMS_LicenseeCurrencySelCriteria criteria)
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

                listProducts = BaseRepository.Fetch<SelectListItem>(criteria, mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listProducts;
        }

        /// <summary>
        /// Get Products By Licensee.
        /// </summary>
        /// <param name="criteria">Licensee criteria.</param>
        /// <returns>Return result.</returns>
        public IEnumerable<SelectListItem> GetProductsByLicensee(IMS_LicenseeProductSelCriteria criteria)
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

                listProducts = BaseRepository.Fetch<SelectListItem>(criteria, mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listProducts;
        }

        /// <summary>
        /// Get all licensees.
        /// </summary>
        /// <returns>Return result.</returns>
        public IEnumerable<SelectListItem> GetAllLicensees()
        {
            IList<SelectListItem> listLicensees = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    return new SelectListItem
                    {
                        Value = Convert.ToString(r["LicenseeID"]),
                        Text = Convert.ToString(r["BrandName"])
                    };
                };

                listLicensees = BaseRepository.Fetch<SelectListItem>(new IMS_LicenseeSelCriteria(), mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listLicensees;
        }

        /// <summary>
        /// Get all licensee extent.
        /// </summary>
        /// <returns>Return result.</returns>
        public IEnumerable<SelectListItemEx> GetAllLicenseesEx()
        {
            IList<SelectListItemEx> listLicensees = new List<SelectListItemEx>();
            try
            {
                Func<IDataReader, SelectListItemEx> mappingFunc = r =>
                {
                    return new SelectListItemEx
                    {
                        Item = new SelectListItem()
                        {
                            Value = Convert.ToString(r["LicenseeID"]),
                            Text = Convert.ToString(r["BrandName"])
                        },
                        Extension = new Dictionary<string, object>() 
                        { 
                            { "LicenseeName", Convert.ToString(r["LicenseeName"]) } 
                        }
                    };
                };

                listLicensees = BaseRepository.Fetch<SelectListItemEx>(new IMS_LicenseeSelCriteria(), mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listLicensees;
        }

        /// <summary>
        /// Get Licensee Information.
        /// </summary>
        /// <param name="criteria">Licensee criteria.</param>
        /// <returns>Return licensee info.</returns>
        private LicenseeInformation GetLicenseeInformation(IMS_LicenseeBillingInfoSelCriteria criteria)
        {
            var listLicenseeInfo = DynamicRepository.Fetch<LicenseeInformation>(criteria).FirstOrDefault<LicenseeInformation>();
            if (listLicenseeInfo == null)
            {
                listLicenseeInfo = new LicenseeInformation();
            }                

            return listLicenseeInfo;
        }

        /// <summary>
        /// Get Licensee Report.
        /// </summary>
        /// <param name="criteria">Licensee criteria.</param>
        /// <returns>Return result.</returns>
        private Fanex.Ares.Report.ViewModels.LicenseeSettlement.Report GetLicenseeReport(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            var report = new Fanex.Ares.Report.ViewModels.LicenseeSettlement.Report();
            Func<IDataReader, ReportEntity> mappingFunc;
            mappingFunc = r =>
            {
                var res = new ReportEntity
                {
                    Betcount = Convert.ToInt32(r["Betcount"].ToString()),
                    Company = Convert.ToDouble(r["Company"].ToString()),
                    Currency = Convert.ToInt32(r["Currency"].ToString()),
                    CurrencyName = r["CurrencyName"].ToString(),
                    LicenseeComm = Convert.ToDouble(r["LicenseeComm"].ToString()),
                    LicenseeWL = Convert.ToDouble(r["LicenseeWL"].ToString()),
                    MemberComm = Convert.ToDouble(r["MemberComm"].ToString()),
                    MemberWinloss = Convert.ToDouble(r["MemberWinloss"].ToString()),
                    Turnover = Convert.ToDouble(r["Turnover"].ToString()),
                    WinlostDate = DateTime.Parse(r["WinLostMonth"].ToString()),
                    BaseCurrencyExchangeRate = Convert.ToDouble(r[criteria.BaseCurrencyName].ToString())
                };

                res.TurnoverBase = res.Turnover * res.BaseCurrencyExchangeRate;
                res.CompanyBaseCurrency = res.Company * res.BaseCurrencyExchangeRate;
                res.LicenseeCommBase = res.LicenseeComm * res.BaseCurrencyExchangeRate;
                res.LicenseeWLBase = res.LicenseeWL * res.BaseCurrencyExchangeRate;
                res.MemberWinlossBase = res.MemberWinloss * res.BaseCurrencyExchangeRate;
                res.MemberTotal = res.MemberWinloss + res.MemberComm;
                res.LicenseeTotal = res.LicenseeWL + res.LicenseeComm;

                report.GrandTotal.Betcount += res.Betcount;
                report.GrandTotal.Company += res.Company;
                report.GrandTotal.CompanyBaseCurrency += res.CompanyBaseCurrency;
                report.GrandTotal.Currency += res.Currency;
                report.GrandTotal.LicenseeComm += res.LicenseeComm;
                report.GrandTotal.LicenseeCommBase += res.LicenseeCommBase;
                report.GrandTotal.LicenseeWL += res.LicenseeWL;
                report.GrandTotal.LicenseeWLBase += res.LicenseeWLBase;
                report.GrandTotal.MemberComm += res.MemberComm;
                report.GrandTotal.MemberWinloss += res.MemberWinloss;
                report.GrandTotal.MemberWinlossBase += res.MemberWinlossBase;
                report.GrandTotal.Turnover += res.Turnover;
                report.GrandTotal.TurnoverBase += res.TurnoverBase;
                report.GrandTotal.BaseCurrencyExchangeRate += res.BaseCurrencyExchangeRate;

                return res;
            };

            report.EntityList = BaseRepository.Fetch<ReportEntity>(criteria, mappingFunc).ToList();
            return report;
        }

        /// <summary>
        /// Get Licensee Exchange Rate.
        /// </summary>
        /// <param name="criteria">Billing currency.</param>
        /// <returns>Return result.</returns>
        private IEnumerable<ExchangeRate> GetLicenseeExchangeRate(IMS_ExchangeRateBillingSelCriteria criteria)
        {
            var listExchangeRate = new List<ExchangeRate>();
            try
            {
                Func<IDataReader, ExchangeRate> mappingFunc = r =>
                {
                    var res = new ExchangeRate
                    {
                        Currency = Convert.ToInt32(r["ExchangeId"].ToString()),
                        CurrencyName = r["Code"].ToString(),
                        BasePerUnit = Convert.ToDouble(r[criteria.BaseCurrencyName].ToString())
                    };

                    res.UnitsPerBase = 1 / res.BasePerUnit;

                    return res;
                };

                listExchangeRate = BaseRepository.Fetch<ExchangeRate>(criteria, mappingFunc).ToList();
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listExchangeRate;
        }
    }
}