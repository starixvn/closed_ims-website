// -----------------------------------------------------------------------
// <copyright file="ILicenseeSettlementService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Report.Services.Interface
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
        /// <summary>
        /// Get All Licensees.
        /// </summary>
        /// <returns>Return result.</returns>
        IEnumerable<SelectListItem> GetAllLicensees();

        /// <summary>
        /// Get All Licensees.
        /// </summary>
        /// <returns>Return result.</returns>
        IEnumerable<SelectListItemEx> GetAllLicenseesEx();

        /// <summary>
        /// Get Products By Licensee.
        /// </summary>
        /// <param name="criteria">Licensee product criteria.</param>
        /// <returns>Return result.</returns>
        IEnumerable<SelectListItem> GetProductsByLicensee(IMS_LicenseeProductSelCriteria criteria);

        /// <summary>
        /// Get Currency By Licensee.
        /// </summary>
        /// <param name="criteria">Licensee currency criteria.</param>
        /// <returns>Return result.</returns>
        IEnumerable<SelectListItem> GetCurrencyByLicensee(IMS_LicenseeCurrencySelCriteria criteria);

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
        LicenseeSettlementModel GenerateReport(IMS_Rpt_LicenseeSettlementCriteria criteria);

        /// <summary>
        /// Get File Name.
        /// </summary>
        /// <returns>Return result.</returns>
        string GetFileName();
    }
}