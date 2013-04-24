using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;

namespace Fanex.Ares.Report.Services.Interface
{
    public interface ILicenseeSettlementService
    {
        IList<SelectListItem> GetAllLicensees();
        IList<SelectListItem> GetProductsByLicensee(IMS_LicenseeProductSelCriteria criteria);
        IList<SelectListItem> GetCurrencyByLicensee(IMS_LicenseeCurrencySelCriteria criteria);
        void ExportSettlementReportToExcel(string exchange, string product);
    }
}