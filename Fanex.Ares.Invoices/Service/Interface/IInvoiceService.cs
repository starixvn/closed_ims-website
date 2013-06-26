// -----------------------------------------------------------------------
// <copyright file="IInvoiceService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.Ares.Invoice.Criteria.Invoice;
    using Fanex.Ares.Invoice.Models.Invoice;
    using Fanex.Ares.Invoice.Service.Imp;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IInvoiceService
    {
        List<Operator> GetOperatorList(IMS_OperatorSelCriteria criteria);

        List<Licensee> GeLicenseesList(IMS_LicenseeSelByPartyCriteria criteria);

        List<InvoiceType> GetInvoieType(IMS_InvoieTypesSelCriteria criteria);

        int GetInvoieNo(IMS_InvoiceNoSelCriteria criteria);

        InvoiceModel GetInvoice(IMS_NewInvoiceItemsSelCriteria criteria);

        Core.Models.Result SaveInvoice(InvoiceModel invoiceModel);

        List<ChargeRate> GetChargeRate(IMS_GetChargeRateCriteria criteria);
    }
}
