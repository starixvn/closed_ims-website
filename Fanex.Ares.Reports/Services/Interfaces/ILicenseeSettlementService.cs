namespace Fanex.Ares.Report.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Ares.Core.Criteria;
    using Fanex.Ares.Core.Export;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Report.Criteria.LicenseeSettlement;
    using Fanex.Ares.Report.ViewModels.LicenseeSettlement;

    /// <summary>
    /// Licensee Settlement Service.
    /// </summary>
    public interface ILicenseeSettlementService
    {
        IEnumerable<SelectListItem> AllLicensees { get; }

        IEnumerable<SelectListItemEx> AllLicenseesExension { get; }

        string FileName { get; }

        IEnumerable<SelectListItem> GetProductsByLicensee(LicenseeProductSelCriteria criteria);

        IEnumerable<SelectListItem> GetCurrencyByLicensee(LicenseeCurrencySelCriteria criteria);

        IEnumerable<SelectListItem> GetBaseCurrency(BaseCurrencyCriteria criteria);

        LicenseeSettlementModel GenerateReport(ReportLicenseeSettlementCriteria criteria);

        NPOIResult ExportExcel(LicenseeSettlementModel model, int productId, string excelTemplateRootPath);

        string GetReportTitle(string licenseeName, DateTime fromDate, DateTime toDate);
    }
}