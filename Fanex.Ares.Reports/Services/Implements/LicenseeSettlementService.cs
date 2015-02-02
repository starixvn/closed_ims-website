namespace Fanex.Ares.Report.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Ares.Core.Criteria;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.Ares.Core.Utils;
    using Fanex.Ares.Report.Criteria.LicenseeSettlement;
    using Fanex.Ares.Report.Entities;
    using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
    using Fanex.Artist.Services;
    using Fanex.Common;
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

        #region Properties

        public IEnumerable<SelectListItem> AllLicensees
        {
            get { return GetAllLicensees(); }
        }

        public IEnumerable<SelectListItemEx> AllLicenseesExension
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
            //// Licensee Information
            var listLicensee = this.GetAllLicenseesExension();
            var listBaseCurrency = this.GetBaseCurrency(new IMS_BaseCurrencySel());
            var model = new LicenseeSettlementModel();
            var licenseeInfoCriteria = new LicenseeBillingInfoSelCriteria();
            var exchangeRateCriteria = new ExchangeRateBillingSelCriteria();
            if (criteria.InvoiceReport)
            {
                licenseeInfoCriteria = new InvoiceLicenseeBillingInfoSelCriteria();
                exchangeRateCriteria = new InvoiceExchangeRateBillingSelCriteria();
            }

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

            GetLicenseeReport(model, criteria);
            //// Exchange Rate                
            var exchangeRatecurrencies = string.Empty;
            exchangeRatecurrencies = string.Join(",", model.ReportList.Select(m => m.Currency).Distinct().ToArray());
            exchangeRateCriteria.Currency = exchangeRatecurrencies;
            exchangeRateCriteria.ExchangeDate = criteria.ToDate;
            exchangeRateCriteria.BaseCurrencyId = criteria.BaseCurrency;
            exchangeRateCriteria.BaseCurrencyName = listBaseCurrency.Where(m => m.Value == criteria.BaseCurrency.ToString()).FirstOrDefault<SelectListItem>().Text;
            var exchangeList = this.GetLicenseeExchangeRate(exchangeRateCriteria).ToList();
            model.ExchangeList.AddRange(exchangeList);
            model.LicenseeInfor.BaseCurrencyName = exchangeRateCriteria.BaseCurrencyName;
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport, model);

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

        #endregion

        #region Utilities

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
        private IEnumerable<SelectListItemEx> GetAllLicenseesExension()
        {
            var listLicensees = new List<SelectListItemEx>();
            try
            {
                Func<IDataReader, SelectListItemEx> mappingFunc = r =>
                {
                    return new SelectListItemEx
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

                listLicensees = BaseRepository.Fetch<SelectListItemEx>(new LicenseeSelCriteria(), mappingFunc).ToList();
            }
            catch (SqlException exception)
            {
                LoggingHandler.Log(exception.Message);
                throw new ServiceException(exception.Message);
            }

            return listLicensees;
        }

        /// <summary>
        /// Get Licensee Information.
        /// </summary>
        /// <param name="criteria">Licensee criteria.</param>
        /// <returns>Return licensee info.</returns>
        private LicenseeInformation GetLicenseeInformation(LicenseeBillingInfoSelCriteria criteria)
        {
            var listLicenseeInfo = new LicenseeInformation();

            try
            {
                listLicenseeInfo = DynamicRepository.Fetch<LicenseeInformation>(criteria).FirstOrDefault<LicenseeInformation>();
            }
            catch (SqlException exception)
            {
                LoggingHandler.Log(exception.Message);
                throw new ServiceException(exception.Message, criteria.GetSettingKey(), criteria.GetSpParams());
            }

            return listLicenseeInfo ?? new LicenseeInformation();
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

                model.GrandTotal.Betcount += res.Betcount;
                model.GrandTotal.Company += res.Company;
                model.GrandTotal.CompanyBaseCurrency += res.CompanyBaseCurrency;
                model.GrandTotal.Currency += res.Currency;
                model.GrandTotal.LicenseeComm += res.LicenseeComm;
                model.GrandTotal.LicenseeCommBase += res.LicenseeCommBase;
                model.GrandTotal.LicenseeWL += res.LicenseeWL;
                model.GrandTotal.LicenseeWLBase += res.LicenseeWLBase;
                model.GrandTotal.MemberComm += res.MemberComm;
                model.GrandTotal.MemberWinloss += res.MemberWinloss;
                model.GrandTotal.MemberWinlossBase += res.MemberWinlossBase;
                model.GrandTotal.Turnover += res.Turnover;
                model.GrandTotal.TurnoverBase += res.TurnoverBase;
                model.GrandTotal.BaseCurrencyExchangeRate += res.BaseCurrencyExchangeRate;

                return res;
            };

            var reportList = BaseRepository.Fetch<LicenseeReport>(criteria, mappingFunc).ToList();

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

                    res.UnitsPerBase = 1 / res.BasePerUnit;

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

        #endregion
    }
}