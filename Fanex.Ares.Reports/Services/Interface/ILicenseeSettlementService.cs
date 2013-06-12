using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.Ares.Core.Criteria;
using Fanex.Ares.Core.Extensions;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;
using Fanex.Ares.Report.ViewModels.LicenseeSettlement;

namespace Fanex.Ares.Report.Services.Interface
{
    public interface ILicenseeSettlementService
    {
        IEnumerable<SelectListItem> GetAllLicensees();
        IEnumerable<SelectListItemEx> GetAllLicenseesEx();
        IEnumerable<SelectListItem> GetProductsByLicensee(IMS_LicenseeProductSelCriteria criteria);
        IEnumerable<SelectListItem> GetCurrencyByLicensee(IMS_LicenseeCurrencySelCriteria criteria);
        IEnumerable<SelectListItem> GetBaseCurrency(IMS_BaseCurrencySel criteria);
        LicenseeSettlementModel GenerateReport(IMS_Rpt_LicenseeSettlementCriteria criteria);
        string GetFileName();
    }
}