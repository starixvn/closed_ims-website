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

namespace Fanex.Ares.Report.Services.Interface
{
    public class LicenseeSettlementService : ServiceBase, ILicenseeSettlementService
    {
        #region ctor
        public LicenseeSettlementService()
        {
        }
        #endregion

        public string GetFileName()
        {
            LicenseeSettlementModel model = null;
            if (SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport) == null)
            {
                return string.Empty;
            }

            model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport);
            return model.LicenseeInfor.LicenseeName + "_" + model.LicenseeInfor.ProductName + "_" + "Settlement_From_" + model.LicenseeInfor.FromDate.ToAresDayMonthYear() + "_To_" + model.LicenseeInfor.ToDate.ToAresDayMonthYear();
        }

        #region Generate Report
        private LicenseeInformation GetLicenseeInformation(IMS_LicenseeBillingInfoSelCriteria criteria)
        {
            var listLicenseeInfo = base.DynamicRepository.Fetch<LicenseeInformation>(criteria).FirstOrDefault<LicenseeInformation>();

            return listLicenseeInfo;
        }

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

            report.EntityList = _repository.Fetch<ReportEntity>(criteria, mappingFunc).ToList();
            return report;
        }

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

                listExchangeRate = _repository.Fetch<ExchangeRate>(criteria, mappingFunc).ToList();
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listExchangeRate;
        }

        public LicenseeSettlementModel GenerateReport(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            // Licensee Information
            var listLicensee = this.GetAllLicenseesEx();
            var listProduct = this.GetProductsByLicensee(new IMS_LicenseeProductSelCriteria() { LicenseeId = criteria.LicenseeId });
            var listCurrency = this.GetCurrencyByLicensee(new IMS_LicenseeCurrencySelCriteria() { LicenseeId = criteria.LicenseeId });
            var listBaseCurrency = this.GetBaseCurrency(new IMS_BaseCurrencySel());
            var model = new LicenseeSettlementModel();
            var licenseeInfoCriteria = new IMS_LicenseeBillingInfoSelCriteria();

            try
            {
                licenseeInfoCriteria.LicenseeId = criteria.LicenseeId;
                licenseeInfoCriteria.ProductId = criteria.Product;
                model.LicenseeInfor = this.GetLicenseeInformation(licenseeInfoCriteria);
                model.LicenseeInfor.LicenseeID = criteria.LicenseeId;
                var licenseeItem = listLicensee.Where(m => m.item.Value == criteria.LicenseeId.ToString()).FirstOrDefault<SelectListItemEx>();
                criteria.LicenseeName = Convert.ToString(licenseeItem.extension["LicenseeName"]);
                model.LicenseeInfor.LicenseeName = licenseeItem.item.Text;
                model.LicenseeInfor.ProductName = listProduct.Where(m => m.Value == criteria.Product.ToString()).FirstOrDefault<SelectListItem>().Text;
                model.LicenseeInfor.SettlementDate = criteria.ToDate;

                model.Report = this.GetLicenseeReport(criteria);

                // Exchange Rate
                var exchangeRateCriteria = new IMS_ExchangeRateBillingSelCriteria();
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
    }
}