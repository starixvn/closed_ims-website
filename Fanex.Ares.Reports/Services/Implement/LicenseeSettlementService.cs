using Fanex.Ares.Core.Models;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.ViewModels.SingleTicket;
using System.Collections.Generic;
using Fanex.Ares.Core.Services.Impl;
using Fanex.Ares.Core.Repository.Impl;
using System;
using System.Data;
using System.Web.Mvc;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;
using Fanex.Ares.Report.ViewModels;
using System.Linq;
using Fanex.Caching;
using Fanex.Ares.Report.ViewModels.LicenseeSettlement;

namespace Fanex.Ares.Report.Services.Interface
{
    public class LicenseeSettlementService : ServiceBase, ILicenseeSettlementService
    {
        private readonly BaseRepository _repository;

        /// <summary>
        /// Cache key for All Licensees
        /// </summary>
        private const string CacheKeyAllLicensee = "Fanex.Ares.Report.Services.AllLicensees";

        public LicenseeSettlementService()
        {
            _repository = new BaseRepository();
        }


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

        public LicenseeInformation GetLicenseeInformation(IMS_LicenseeBillingInfoSelCriteria criteria)
        {
            var listLicenseeInfo = base.DynamicRepository.Fetch<LicenseeInformation>(criteria).FirstOrDefault<LicenseeInformation>();
            return listLicenseeInfo;
        }

        public Fanex.Ares.Report.ViewModels.LicenseeSettlement.Report GetLicenseeReport(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            var report = new Fanex.Ares.Report.ViewModels.LicenseeSettlement.Report();

            try
            {
                Func<IDataReader, ReportEntity> mappingFunc = r =>
                {
                    var res = new ReportEntity
                    {
                        Betcount = Convert.ToInt32(r["Betcount"].ToString()),
                        Company = Convert.ToDouble(r["Company"].ToString()),
                        CompanyBaseCurrency = Convert.ToDouble(r["CompanyBaseCurrency"].ToString()),
                        Currency = Convert.ToInt32(r["Currency"].ToString()),
                        CurrencyName = r["CurrencyName"].ToString(),
                        LicenseeComm = Convert.ToDouble(r["LicenseeComm"].ToString()),
                        LicenseeWL = Convert.ToDouble(r["LicenseeWL"].ToString()),
                        MemberComm = Convert.ToDouble(r["MemberComm"].ToString()),
                        MemberWinloss = Convert.ToDouble(r["MemberWinloss"].ToString()),
                        Turnover = Convert.ToDouble(r["Turnover"].ToString()),
                        WinlostDate = DateTime.Parse(r["WinlostDate"].ToString())
                    };
                    return res;
                };

                report.EntityList = _repository.Fetch<ReportEntity>(criteria, mappingFunc).ToList();
                report.GrandTotal = 0;
                foreach (ReportEntity item in report.EntityList)
                {
                    report.GrandTotal += item.CompanyBaseCurrency;
                }

            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

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
                        Currency = Convert.ToInt32(r["Currency"].ToString()),
                        CurrencyName = r["CurrencyName"].ToString(),
                        BasePerUnit = Convert.ToDouble(r["BasePerUnit"].ToString()),
                        UnitsPerBase = Convert.ToDouble(r["UnitsPerBase"].ToString())
                    };
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

    }
}