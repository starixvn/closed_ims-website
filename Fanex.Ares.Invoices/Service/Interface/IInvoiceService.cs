namespace Fanex.Ares.Invoice.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Fanex.Ares.Core.Models;
    using Fanex.Ares.Invoice.Criteria.Invoice;
    using Fanex.Ares.Invoice.Models.Invoice;

    /// <summary>
    /// Invoice Service Interface.
    /// </summary>
    public interface IInvoiceService
    {
        IEnumerable<Operator> GetOperatorList(IMS_OperatorCriteria criteria);

        IEnumerable<Licensee> GeLicenseesList(IMS_LicenseeByPartyCriteria criteria);

        IEnumerable<InvoiceType> GetInvoieType(IMS_InvoiceTypesCriteria criteria);

        int GetInvoiceNo(IMS_InvoiceNoCriteria criteria);

        InvoiceModel GetInvoice(IMS_NewInvoiceItemsCriteria criteria);

        Result SaveInvoice(InvoiceModel invoiceModel);

        IEnumerable<ChargeRate> GetChargeRate(IMS_GetChargeRateCriteria criteria);

        IEnumerable<SelectListItem> GetBaseCurrencyInvoiceFilter(IMS_GenInv_BaseCurrencySel criteria);

        bool IsExistInvoiceNo(IMS_CheckExistedInvoiceNo criteria);

        IEnumerable<DateRange> GetDateRange(DateTime fromDate, DateTime toDate);
    }
}
