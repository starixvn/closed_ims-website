// -----------------------------------------------------------------------
// <copyright file="IInvoiceService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Ares.Invoice.Models.Invoice;
    using Fanex.Ares.Invoice.Criteria.Invoice;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IInvoiceService
    {
        List<Operator> GetOperatorList(IMS_InvoiceTypeCriteria criteria);
    }
}
