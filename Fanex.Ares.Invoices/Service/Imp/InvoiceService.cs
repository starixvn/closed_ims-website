
// -----------------------------------------------------------------------
// <copyright file="InvoiceService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Service.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Ares.Invoice.Service.Interface;
    using Fanex.Ares.Invoice.Models.Invoice;
    using Fanex.Ares.Invoice.Criteria.Invoice;
    using System.Data;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.Common;
using Fanex.Ares.Core.Criteria;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InvoiceService : ServiceBase,IInvoiceService
    {
        public List<Operator> GetOperatorList(IMS_OperatorSelCriteria criteria)
        {
            List<Operator> operatorProducts = new List<Operator>();
            try
            {
                Func<IDataReader, Operator> mappingFunc = r =>
                {
                    return new Operator
                    {
                        PartyID = Converter.To<int>(r["PartyID"]),
                        PartyName = Converter.To<string>(r["PartyName"]),
                        PartyType = Converter.To<int>(r["PartyType"]),
                        Description = Converter.To<string>(r["Description"]),
                        Company = Converter.To<string>(r["Company"]),
                        AccountNumber = Converter.To<string>(r["AccountNumber"]),
                        AccountName = Converter.To<string>(r["AccountName"]),
                        BankName = Converter.To<string>(r["BankName"]),
                        Benificiary = Converter.To<string>(r["Benificiary"]),
                        IBAN = Converter.To<string>(r["IBAN"]),
                        SwiftCode = Converter.To<string>(r["SwiftCode"]),
                        CorrespondentBank = Converter.To<string>(r["CorrespondentBank"]),
                        AttnName = Converter.To<string>(r["AttnName"]),
                        To1 = Converter.To<string>(r["To1"]),
                        To2 = Converter.To<string>(r["To2"]),
                        Logo = Converter.To<string>(r["Logo"]),
                        Phone = Converter.To<string>(r["Phone"]),
                        MobilePhone = Converter.To<string>(r["MobilePhone"]),
                        Fax = Converter.To<string>(r["Fax"]),
                        Email = Converter.To<string>(r["Email"])
                    };
                };

                //operatorProducts = _repository.Fetch<Operator>(criteria, mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return operatorProducts;
        }
    }
}
