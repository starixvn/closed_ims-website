namespace Fanex.Ares.Report.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Ares.Core.Criteria;
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

        /// <summary>
        /// Get Products By Licensee.
        /// </summary>
        /// <param name="criteria">Licensee product criteria.</param>
        /// <returns>Return result.</returns>
        IEnumerable<SelectListItem> GetProductsByLicensee(LicenseeProductSelCriteria criteria);

        /// <summary>
        /// Get Currency By Licensee.
        /// </summary>
        /// <param name="criteria">Licensee currency criteria.</param>
        /// <returns>Return result.</returns>
        IEnumerable<SelectListItem> GetCurrencyByLicensee(LicenseeCurrencySelCriteria criteria);

        /// <summary>
        /// Get Base Currency.
        /// </summary>
        /// <param name="criteria">Base currency criteria.</param>
        /// <returns>Return result.</returns>
        IEnumerable<SelectListItem> GetBaseCurrency(IMS_BaseCurrencySel criteria);

        /// <summary>
        /// Generate Report.
        /// </summary>
        /// <param name="criteria">Licensee settlement criteria.</param>
        /// <returns>Return result.</returns>
        LicenseeSettlementModel GenerateReport(ReportLicenseeSettlementCriteria criteria);
    }
}