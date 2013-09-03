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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Invoice service
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IInvoiceService
    {
        List<Operator> GetOperatorList(IMS_OperatorCriteria criteria);

        List<Licensee> GeLicenseesList(IMS_LicenseeByPartyCriteria criteria);

        List<InvoiceType> GetInvoieType(IMS_InvoiceTypesCriteria criteria);

        int GetInvoiceNo(IMS_InvoiceNoCriteria criteria);

        InvoiceModel GetInvoice(IMS_NewInvoiceItemsCriteria criteria);

        Core.Models.Result SaveInvoice(InvoiceModel invoiceModel);

        List<ChargeRate> GetChargeRate(IMS_GetChargeRateCriteria criteria);
    }
}
