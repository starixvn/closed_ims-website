using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;
using Fanex.Ares.Report.ViewModels;

namespace Fanex.Ares.Report.Services.Interface
{
    public interface ILicenseeSettlementService
    {
        IEnumerable<SelectListItem> GetAllLicensees();
        IEnumerable<SelectListItem> GetProductsByLicensee(IMS_LicenseeProductSelCriteria criteria);
        IEnumerable<SelectListItem> GetCurrencyByLicensee(IMS_LicenseeCurrencySelCriteria criteria);
        LicenseeInformation GetLicenseeInformation(IMS_LicenseeBillingInfoSelCriteria criteria);
        Fanex.Ares.Report.ViewModels.LicenseeSettlement.Report GetLicenseeReport(IMS_Rpt_LicenseeSettlementCriteria criteria);
        IEnumerable<ExchangeRate> GetLicenseeExchangeRate(IMS_ExchangeRateBillingSelCriteria criteria);
    }
}