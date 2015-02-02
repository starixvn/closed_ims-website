namespace Fanex.Ares.Invoice.Service.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Ares.Core.Models;
    using Fanex.Ares.Invoice.Criteria.Invoice;
    using Fanex.Ares.Invoice.Models.Invoice;

    /// <summary>
    /// Invoice Service Interface.
    /// </summary>
    public interface IInvoiceService
    {
        IEnumerable<Party> GetOperatorList(OperatorSelectionCriteria criteria);

        IEnumerable<Licensee> GetLicenseesList(LicenseeByPartySelectionCriteria criteria);

        IEnumerable<InvoiceType> GetInvoieType(InvoiceTypesCriteria criteria);

        int GetInvoiceNo(InvoiceNumberCriteria criteria);

        InvoiceModel GetInvoice(NewInvoiceItemsInsertionCriteria criteria);

        Result SaveInvoice(InvoiceModel invoiceModel);

        IEnumerable<ChargeRate> GetChargeRate(ProductChargeRateCriteria criteria);

        IEnumerable<SelectListItem> GetBaseCurrencyInvoiceFilter(BaseCurrencySelectionCriteria criteria);

        bool IsExistInvoiceNo(ExistedInvoiceNumberCheckingCriteria criteria);

        IEnumerable<DateRange> GetDateRange(DateTime fromDate, DateTime toDate);
    }
}
