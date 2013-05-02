using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;
using Fanex.Ares.Report.ViewModels;
using Fanex.Ares.Report.ViewModels.SportsBook;

namespace Fanex.Ares.Report.Services.Interface
{
    public interface ISportsBookService
    {
        IEnumerable<SelectListItem> GetAllLicensees();
        IEnumerable<SelectListItem> GetProductsByLicensee(IMS_LicenseeProductSelCriteria criteria);
        IEnumerable<SelectListItem> GetCurrencyByLicensee(IMS_LicenseeCurrencySelCriteria criteria);
        IEnumerable<SelectListItem> GetBaseCurrency(IMS_BaseCurrencySel criteria);
        LicenseeInformation GetLicenseeInformation(IMS_LicenseeBillingInfoSelCriteria criteria);
        void ExportSportsBookReportToExcel(string exchange, string product);
        Fanex.Ares.Report.ViewModels.SportsBook.Report GetLicenseeReport(IMS_Rpt_LicenseeSettlementCriteria criteria);
        IEnumerable<ExchangeRate> GetLicenseeExchangeRate(IMS_ExchangeRateBillingSelCriteria criteria);
        SportsBookModel GenerateReport(IMS_Rpt_LicenseeSettlementCriteria criteria);
    }
}