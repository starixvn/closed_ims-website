namespace Fanex.Ares.Report.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Ares.Core.Criteria;
    using Fanex.Ares.Core.DataDomain;
    using Fanex.Ares.Core.Export;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Core.Models;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.Ares.Core.Utils;
    using Fanex.Ares.Report.Criteria.LicenseeSettlement;
    using Fanex.Ares.Report.Entities;
    using Fanex.Ares.Report.Export.Excel;
    using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
    using Fanex.Artist.Services;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// Licensee Settlement Service.
    /// </summary>
    public class LicenseeSettlementService : ServiceBase, ILicenseeSettlementService
    {
        #region Constructor

        public LicenseeSettlementService()
        {
        }

        #endregion

        #region Properties

        public IEnumerable<SelectListItem> AllLicensees
        {
            get { return GetAllLicensees(); }
        }

        public IEnumerable<SelectListItemExtension> AllLicenseesExension
        {
            get { return GetAllLicenseesExension(); }
        }

        public string FileName
        {
            get { return GetFileName(); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generate Report.
        /// </summary>
        /// <param name="criteria">Report Licensee Settlement Criteria.</param>
        /// <returns>Return result.</returns>
        public LicenseeSettlementModel GenerateReport(ReportLicenseeSettlementCriteria criteria)
        {
            var listLicensee = GetAllLicenseesExension();
            var model = new LicenseeSettlementModel();
            var licenseeInfoCriteria = new LicenseeBillingInfoSelCriteria();

            licenseeInfoCriteria.LicenseeId = criteria.LicenseeId;
            licenseeInfoCriteria.ProductId = criteria.Product;
            licenseeInfoCriteria.PartyId = criteria.PartyId;
            model.LicenseeInfor = GetLicenseeInformation(licenseeInfoCriteria);
            model.LicenseeInfor.LicenseeID = criteria.LicenseeId;

            var licenseeItem = listLicensee.Where(m => m.Item.Value == criteria.LicenseeId.ToString()).FirstOrDefault<SelectListItemExtension>();

            criteria.LicenseeName = Convert.ToString(licenseeItem.Extension["LicenseeName"]);
            model.LicenseeInfor.LicenseeName = licenseeItem.Item.Text;
            model.LicenseeInfor.ProductName = criteria.ProductName;
            model.LicenseeInfor.SettlementDate = criteria.ToDate;
            model.ForexTime = Utilities.GetForexTime(criteria.ToDate);

            GetLicenseeReport(model, criteria);

            var exchangeRateCriteria = GetExchangeRateCriteria(criteria.ToDate, criteria.BaseCurrency, model.ReportList);
            var exchangeList = GetLicenseeExchangeRate(exchangeRateCriteria).ToList();

            model.ExchangeList.AddRange(exchangeList);
            model.LicenseeInfor.BaseCurrencyName = exchangeRateCriteria.BaseCurrencyName;

            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKeys.SessionKeyLicenseeSettlementReport, model);

            return model;
        }

        /// <summary>
        /// Get Currency by licensee.
        /// </summary>
        /// <param name="criteria">Licensee Currency Select Criteria.</param>
        /// <returns>Return result.</returns>
        public IEnumerable<SelectListItem> GetCurrencyByLicensee(LicenseeCurrencySelCriteria criteria)
        {
            var listProducts = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    return new SelectListItem
                    {
                        Value = Converter.ToString2(r["currency"]),
                        Text = Converter.ToString2(r["currencyname"])
                    };
                };

                listProducts = BaseRepository.Fetch<SelectListItem>(criteria, mappingFunc).ToList();
            }
            catch (SqlException exception)
            {
                LoggingHandler.Log(exception.Message);
                throw new ServiceException(exception.Message, criteria.GetSettingKey(), criteria.GetSpParams());
            }

            return listProducts;
        }

        /// <summary>
        /// Get Products By Licensee.
        /// </summary>
        /// <param name="criteria">Licensee criteria.</param>
        /// <returns>Return result.</returns>
        public IEnumerable<SelectListItem> GetProductsByLicensee(LicenseeProductSelCriteria criteria)
        {
            var listProducts = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    return new SelectListItem
                    {
                        Value = Converter.ToString2(r["productid"]),
                        Text = Converter.ToString2(r["productname"])
                    };
                };

                listProducts = BaseRepository.Fetch<SelectListItem>(criteria, mappingFunc).ToList();
            }
            catch (SqlException exception)
            {
                LoggingHandler.Log(exception.Message);
                throw new ServiceException(exception.Message, criteria.GetSettingKey(), criteria.GetSpParams());
            }

            return listProducts;
        }

        /// <summary>
        /// Export Excel.
        /// </summary>
        /// <param name="model">Licensee Settlement Model.</param>
        /// <param name="productId">Product Identify.</param>
        /// <param name="excelTemplateRootPath">Excel Template Root Path.</param>
        /// <returns>NPOI Result.</returns>
        public NPOIResult ExportExcel(LicenseeSettlementModel model, int productId, string excelTemplateRootPath)
        {
            var templatePath = GetExcelTemplatePath(productId, excelTemplateRootPath);
            ExcelBase excel = null;

            switch (productId)
            {
                case Products.Sportsbook:
                case Products.NumberGame:
                    excel = new Sportsbook(templatePath, model);
                    break;
                case Products.BA:
                    excel = new BetAggregator(templatePath, model);
                    break;
                default:
                    excel = new Sportsbook(templatePath, model);
                    break;
            }

            excel.ReportTitle = GetReportTitle(model.LicenseeInfor.LicenseeName, model.LicenseeInfor.FromDate, model.LicenseeInfor.ToDate);
            var fileName = excel.ReportTitle.Replace(' ', '.');
            excel.Export();

            return new NPOIResult((HSSFWorkbook)excel.Workbook, fileName);
        }

        public string GetReportTitle(string licenseeName, DateTime fromDate, DateTime toDate)
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "Settlement Report for {0} from {1} to {2}",
                licenseeName,
                fromDate.ToAresDayMonthYear(),
                toDate.ToAresDayMonthYear());
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get Excel Template Path.
        /// </summary>
        /// <param name="productId">Product Identify.</param>
        /// <param name="excelTemplateRootPath">Excel Template Root Path.</param>
        /// <returns>Template Path.</returns>
        private static string GetExcelTemplatePath(int productId, string excelTemplateRootPath)
        {
            var templatePath = string.Empty;

            switch (productId)
            {
                case Products.Sportsbook:
                case Products.NumberGame:
                    templatePath = TemplatePath.LicenseeBillingForSportsBook;
                    break;
                case Products.BA:
                    templatePath = TemplatePath.LicenseeBillingForBA;
                    break;
                default:
                    templatePath = TemplatePath.LicenseeBillingForSportsBook;
                    break;
            }

            return Path.Combine(excelTemplateRootPath, templatePath);
        }

        /// <summary>
        /// Get Licensee Grand Total.
        /// </summary>
        /// <param name="reportList">Report List.</param>
        /// <returns>Licensee Report.</returns>
        private static LicenseeReport GetLicenseeGrandTotal(List<LicenseeReport> reportList)
        {
            return new LicenseeReport
            {
                Betcount = reportList.Sum(x => x.Betcount),
                Company = reportList.Sum(x => x.Company),
                CompanyBaseCurrency = reportList.Sum(x => x.CompanyBaseCurrency),
                Currency = reportList.Sum(x => x.Currency),
                LicenseeComm = reportList.Sum(x => x.LicenseeComm),
                LicenseeCommBase = reportList.Sum(x => x.LicenseeCommBase),
                LicenseeWL = reportList.Sum(x => x.LicenseeWL),
                LicenseeWLBase = reportList.Sum(x => x.LicenseeWLBase),
                MemberComm = reportList.Sum(x => x.MemberComm),
                MemberWinloss = reportList.Sum(x => x.MemberWinloss),
                MemberWinlossBase = reportList.Sum(x => x.MemberWinlossBase),
                Turnover = reportList.Sum(x => x.Turnover),
                TurnoverBase = reportList.Sum(x => x.TurnoverBase),
                BaseCurrencyExchangeRate = reportList.Sum(x => x.BaseCurrencyExchangeRate)
            };
        }

        /// <summary>
        /// Get Exchange Rate Criteria.
        /// </summary>
        /// <param name="toDate">Exchange Date.</param>
        /// <param name="baseCurrencyId">Base Currency.</param>
        /// <param name="reportList">Report List.</param>
        /// <returns>Exchange Rate Billing Criteria.</returns>
        private ExchangeRateBillingSelCriteria GetExchangeRateCriteria(DateTime toDate, int baseCurrencyId, List<LicenseeReport> reportList)
        {
            var listBaseCurrency = GetBaseCurrency(new BaseCurrencyCriteria());
            var exchangeRatecurrencies = string.Empty;
            var baseCurrencyName = listBaseCurrency.Where(m => m.Value == baseCurrencyId.ToString()).FirstOrDefault<SelectListItem>().Text;

            exchangeRatecurrencies = string.Join(",", reportList.Select(m => m.Currency).Distinct().ToArray());

            return new ExchangeRateBillingSelCriteria
            {
                Currency = exchangeRatecurrencies,
                ExchangeDate = toDate,
                BaseCurrencyId = baseCurrencyId,
                BaseCurrencyName = baseCurrencyName
            };
        }

        /// <summary>
        /// Get all licensees.
        /// </summary>
        /// <returns>Return result.</returns>
        private IEnumerable<SelectListItem> GetAllLicensees()
        {
            var listLicensees = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    return new SelectListItem
                    {
                        Value = Converter.ToString2(r["LicenseeID"]),
                        Text = Converter.ToString2(r["BrandName"])
                    };
                };

                listLicensees = BaseRepository.Fetch<SelectListItem>(new LicenseeSelCriteria(), mappingFunc).ToList();
            }
            catch (SqlException exception)
            {
                LoggingHandler.Log(exception.Message);
                throw new ServiceException(exception.Message);
            }

            return listLicensees;
        }

        /// <summary>
        /// Get all licensee extension.
        /// </summary>
        /// <returns>Return result.</returns>
        private IEnumerable<SelectListItemExtension> GetAllLicenseesExension()
        {
            var listLicensees = new List<SelectListItemExtension>();
            try
            {
                Func<IDataReader, SelectListItemExtension> mappingFunc = r =>
                {
                    return new SelectListItemExtension
                    {
                        Item = new SelectListItem()
                        {
                            Value = Converter.ToString2(r["LicenseeID"]),
                            Text = Converter.ToString2(r["BrandName"])
                        },
                        Extension = new Dictionary<string, object>() 
                        { 
                            { "LicenseeName", Convert.ToString(r["LicenseeName"]) } 
                        }
                    };
                };

                listLicensees = BaseRepository.Fetch<SelectListItemExtension>(new LicenseeSelCriteria(), mappingFunc).ToList();
            }
            catch (SqlException exception)
            {
                LoggingHandler.Log(exception.Message);
                throw new ServiceException(exception.Message);
            }

            return listLicensees;
        }

        private LicenseeInformation GetLicenseeInformation(LicenseeBillingInfoSelCriteria criteria)
        {
            var licenseeInfo = GetEntity<LicenseeInformation>(criteria);

            return licenseeInfo ?? new LicenseeInformation();
        }

        /// <summary>
        /// Get Licensee Report.
        /// </summary>
        /// <param name="model">Licensee Settlement Model.</param>
        /// <param name="criteria">Licensee criteria.</param>
        private void GetLicenseeReport(LicenseeSettlementModel model, ReportLicenseeSettlementCriteria criteria)
        {
            Func<IDataReader, LicenseeReport> mappingFunc;
            mappingFunc = r =>
            {
                var res = new LicenseeReport
                {
                    Betcount = Converter.To<int>(r["Betcount"]),
                    Company = Converter.To<double>(r["Company"].ToString()),
                    Currency = Converter.To<int>(r["Currency"].ToString()),
                    CurrencyName = Converter.ToString2(r["CurrencyName"]),
                    LicenseeComm = Converter.To<double>(r["LicenseeComm"].ToString()),
                    LicenseeWL = Converter.To<double>(r["LicenseeWL"].ToString()),
                    MemberComm = Converter.To<double>(r["MemberComm"].ToString()),
                    MemberWinloss = Converter.To<double>(r["MemberWinloss"].ToString()),
                    Turnover = Converter.To<double>(r["Turnover"].ToString()),
                    WinlostDate = DateTime.Parse(r["WinLostMonth"].ToString()),
                    BaseCurrencyExchangeRate = Converter.To<double>(r[criteria.BaseCurrencyName].ToString())
                };

                res.TurnoverBase = res.Turnover * res.BaseCurrencyExchangeRate;
                res.CompanyBaseCurrency = res.Company * res.BaseCurrencyExchangeRate;
                res.LicenseeCommBase = res.LicenseeComm * res.BaseCurrencyExchangeRate;
                res.LicenseeWLBase = res.LicenseeWL * res.BaseCurrencyExchangeRate;
                res.MemberWinlossBase = res.MemberWinloss * res.BaseCurrencyExchangeRate;
                res.MemberTotal = res.MemberWinloss + res.MemberComm;
                res.LicenseeTotal = res.LicenseeWL + res.LicenseeComm;

                return res;
            };

            var reportList = BaseRepository.Fetch<LicenseeReport>(criteria, mappingFunc).ToList();

            model.GrandTotal = GetLicenseeGrandTotal(reportList);
            model.ReportList.AddRange(reportList);
        }

        /// <summary>
        /// Get Licensee Exchange Rate.
        /// </summary>
        /// <param name="criteria">Billing currency.</param>
        /// <returns>Return result.</returns>
        private IEnumerable<ExchangeRate> GetLicenseeExchangeRate(ExchangeRateBillingSelCriteria criteria)
        {
            var listExchangeRate = new List<ExchangeRate>();
            try
            {
                Func<IDataReader, ExchangeRate> mappingFunc = r =>
                {
                    var res = new ExchangeRate
                    {
                        Currency = Converter.To<int>(r["ExchangeId"]),
                        CurrencyName = Converter.ToString2(r["Code"]),
                        BasePerUnit = Converter.To<double>(r[criteria.BaseCurrencyName].ToString())
                    };

                    res.UnitsPerBase = res.BasePerUnit == 0 ? 0 : 1 / res.BasePerUnit;

                    return res;
                };

                listExchangeRate = BaseRepository.Fetch<ExchangeRate>(criteria, mappingFunc).ToList();
            }
            catch (SqlException exception)
            {
                LoggingHandler.Log(exception.Message);
                throw new ServiceException(exception.Message, criteria.GetSettingKey(), criteria.GetSpParams());
            }

            return listExchangeRate;
        }

        /// <summary>
        /// Get File Name.
        /// </summary>
        /// <returns>Return file name.</returns>
        private string GetFileName()
        {
            if (SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKeys.SessionKeyLicenseeSettlementReport) == null)
            {
                return string.Empty;
            }

            var model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKeys.SessionKeyLicenseeSettlementReport);

            return string.Format(
                "{0}_{1}_Settlement_From_{2}_To_{3}",
                model.LicenseeInfor.LicenseeName,
                model.LicenseeInfor.ProductName,
                model.LicenseeInfor.FromDate.ToAresDayMonthYear(),
                model.LicenseeInfor.ToDate.ToAresDayMonthYear());
        }

        #endregion
    }
}