using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using ExcelUtil.Spreadsheet;
using Fanex.Ares.Core.Repository.Impl;
using Fanex.Ares.Core.Services.Impl;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;
using Fanex.Ares.Report.Criteria;
using System.Linq;
using Fanex.Caching;
using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
using Fanex.Ares.Report.ViewModels;
using System.Drawing;
using Fanex.Ares.Core.Extensions;
using Resource;
using Fanex.Common.Extensions;

namespace Fanex.Ares.Report.Services.Interface
{
    public class LicenseeSettlementService : ServiceBase, ILicenseeSettlementService
    {
        #region Local Variables
        private readonly BaseRepository _repository;
        private readonly ISportsbookExcelService _sportsbookExcelService;
        private readonly IBAExcelService _BAExcelService;
        #endregion

        #region Constants
        /// <summary>
        /// Cache key for All Licensees
        /// </summary>
        private const string CacheKeyAllLicensee = "Fanex.Ares.Report.Services.AllLicensees";
        private const string SessionKeyLicenseeSettlementReport = "Fanex.Ares.Report.Services.LicenseeSettlementReport";
        private const string SessionKeyLicenseeSettlementMonth = "Fanex.Ares.Report.Services.Month";
        private const string SessionKeyLicenseeSettlementYear = "Fanex.Ares.Report.Services.Year";
        #endregion

        #region ctor
        public LicenseeSettlementService()
        {
            _repository = new BaseRepository();
            _sportsbookExcelService = new SportsbookExcelService();
            _BAExcelService = new BAExcelService();
        }
        #endregion

        #region Get data for combobox
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
                        Text = Convert.ToString(r["LicenseeName"])
                    };
                };

                listLicensees = _repository.Fetch<SelectListItem>(new IMS_LicenseeSelCriteria(), mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return listLicensees;
        }


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

                listProducts = _repository.Fetch<SelectListItem>(criteria, mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return listProducts;
        }

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

                listProducts = _repository.Fetch<SelectListItem>(criteria, mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listProducts;
        }

        public IEnumerable<SelectListItem> GetBaseCurrency(IMS_BaseCurrencySel criteria)
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
        #endregion

        #region Generate Report
        public LicenseeInformation GetLicenseeInformation(IMS_LicenseeBillingInfoSelCriteria criteria)
        {
            var listLicenseeInfo = base.DynamicRepository.Fetch<LicenseeInformation>(criteria).FirstOrDefault<LicenseeInformation>();

            return listLicenseeInfo;
        }

        public Fanex.Ares.Report.ViewModels.LicenseeSettlement.Report GetLicenseeReport(IMS_Rpt_LicenseeSettlementCriteria criteria)
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

        public IEnumerable<ExchangeRate> GetLicenseeExchangeRate(IMS_ExchangeRateBillingSelCriteria criteria)
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
            var listLicensee = this.GetAllLicensees();
            var listProduct = this.GetProductsByLicensee(new IMS_LicenseeProductSelCriteria() { LicenseeId = criteria.LicenseeId });
            var listCurrency = this.GetCurrencyByLicensee(new IMS_LicenseeCurrencySelCriteria() { LicenseeId = criteria.LicenseeId });
            var listBaseCurrency = this.GetBaseCurrency(new IMS_BaseCurrencySel());
            var model = new LicenseeSettlementModel();
            var licenseeInfoCriteria = new IMS_LicenseeBillingInfoSelCriteria();
            licenseeInfoCriteria.LicenseeId = criteria.LicenseeId;
            licenseeInfoCriteria.ProductId = criteria.Product;
            model.LicenseeInfor = this.GetLicenseeInformation(licenseeInfoCriteria);
            model.LicenseeInfor.LicenseeID = criteria.LicenseeId;
            criteria.LicenseeName = model.LicenseeInfor.LicenseeName = listLicensee.Where(m => m.Value == criteria.LicenseeId.ToString()).FirstOrDefault<SelectListItem>().Text;
            model.LicenseeInfor.ProductName = listProduct.Where(m => m.Value == criteria.Product.ToString()).FirstOrDefault<SelectListItem>().Text;
            //model.LicenseeInfor.SettlementDate = DateTime.Now;
            model.LicenseeInfor.SettlementDate = criteria.ToDate;

            // Licensee Report
            //var reportCriteria = new IMS_Rpt_LicenseeSettlementCriteria();
            //reportCriteria.LicenseeId = criteria.LicenseeId;
            //reportCriteria.LicenseeName = model.LicenseeInfor.LicenseeName;
            //reportCriteria.Product = criteria.Product;
            //reportCriteria.Currency = criteria.Currency;
            //reportCriteria.BaseCurrency = criteria.BaseCurrency;
            //reportCriteria.FromDate = criteria.FromDate;
            //reportCriteria.ToDate = criteria.ToDate;

            model.Report = this.GetLicenseeReport(criteria);

            // Exchange Rate
            var exchangeRateCriteria = new IMS_ExchangeRateBillingSelCriteria();
            model.ExchangeList = new List<ViewModels.ExchangeRate>();

            var exchangeRatecurrencies = string.Empty;
            foreach (ReportEntity item in model.Report.EntityList)
            {
                // Calculate membertotal = memberwinloss + membercomm
                item.MemberTotal = item.MemberWinloss + item.MemberComm;

                // Calculate licenseetotal = licenseewinloss + licenseecomm
                item.LicenseeTotal = item.LicenseeWL + item.LicenseeComm;

                if (criteria.Currency == 0) // All Currency
                {
                    if (!exchangeRatecurrencies.Contains(item.Currency.ToString()))
                    {
                        exchangeRatecurrencies += item.Currency.ToString() + ",";
                    }
                }
                else // Single currency
                {
                    exchangeRatecurrencies = item.Currency.ToString();
                }
            }

            exchangeRatecurrencies.Trim(',');

            exchangeRateCriteria.Currency = exchangeRatecurrencies;
            exchangeRateCriteria.ExchangeDate = criteria.ToDate;
            exchangeRateCriteria.BaseCurrencyId = criteria.BaseCurrency;
            exchangeRateCriteria.BaseCurrencyName = listBaseCurrency.Where(m => m.Value == criteria.BaseCurrency.ToString()).FirstOrDefault<SelectListItem>().Text;

            var exchangeList = this.GetLicenseeExchangeRate(exchangeRateCriteria).ToList();

            model.ExchangeList = exchangeList;
           
            model.LicenseeInfor.BaseCurrencyName = exchangeRateCriteria.BaseCurrencyName;
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeSettlementReport, model);
            return model;
        }
        #endregion

        #region Export Excel
        public void ExportReportToExcel(string exchange, string product)
        {
            if (product == "Sportsbook")
            {
                _sportsbookExcelService.ExportReportToExcel(exchange, product);
            }
            else if (product == "BA")
            {
                _BAExcelService.ExportReportToExcel(exchange, product);
            }
            else
            {
                _sportsbookExcelService.ExportReportToExcel(exchange, product);
            }
        }
        #endregion
    }
}