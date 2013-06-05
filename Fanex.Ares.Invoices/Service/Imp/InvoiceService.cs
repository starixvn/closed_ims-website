
// -----------------------------------------------------------------------
// <copyright file="InvoiceService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Service.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.Ares.Invoice.Models.Invoice;
    using Fanex.Ares.Invoice.Service.Interface;
    using Fanex.Common;
    using Fanex.Ares.Invoice.Criteria.Invoice;
    using System.Linq;
using System.Text;
    using System.Collections;
    using Fanex.Ares.Core.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InvoiceService : ServiceBase,IInvoiceService
    {
        public List<Operator> GetOperatorList(IMS_OperatorSelCriteria criteria)
        {
            List<Operator> operators = new List<Operator>();
            try
            {
                Func<IDataReader, Operator> mappingFunc = r =>
                {
                    var res = new Operator
                    {
                        PartyID = Convert.ToInt32(r["PartyID"]),
                        PartyName = Convert.ToString(r["PartyName"]),
                        PartyType = Convert.ToInt32(r["PartyType"]),
                        Description = Convert.ToString(r["Description"]),
                        PartyFullName = Convert.ToString(r["PartyFullName"]),
                        AccountNumber = Convert.ToString(r["AccountNumber"]),
                        AccountName = Convert.ToString(r["AccountName"]),
                        BankName = Convert.ToString(r["BankName"]),
                        Benificiary = Convert.ToString(r["Benificiary"]),
                        IBAN = Convert.ToString(r["IBAN"]),
                        SwiftCode = Convert.ToString(r["SwiftCode"]),
                        CorrespondentBank = Convert.ToString(r["CorrespondentBank"]),
                        AttnName = Convert.ToString(r["AttnName"]),
                        To1 = Convert.ToString(r["To1"]),
                        To2 = Convert.ToString(r["To2"]),
                        Logo = Convert.ToString(r["Logo"])
                    };

                    return res;
                };

                operators = _repository.Fetch<Operator>(criteria, mappingFunc).ToList();             
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return operators;
        }

        public List<Licensee> GeLicenseesList(IMS_LicenseeSelByPartyCriteria criteria)
        {
            var licensees = new List<Licensee>();
            try
            {
                Func<IDataReader, Licensee> mappingFunc = r =>
                {
                    var res = new Licensee
                    {
                        LicenseeID = Convert.ToInt32(r["LicenseeID"]),
                        LicenseeName = r["LicenseeName"].ToString(),
                        BrandName = Convert.ToString(r["BrandName"]),
                        CompanyName = Convert.ToString(r["CompanyName"]),
                        AttnName = r["AttnName"].ToString(),
                        To1 = Convert.ToString(r["To1"]),
                        To2 = Convert.ToString(r["To2"])
                    };

                    return res;
                };

                licensees = _repository.Fetch<Licensee>(criteria, mappingFunc).ToList();
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return licensees;
        }

        public List<InvoiceType> GetInvoieType(IMS_InvoieTypesSelCriteria criteria)
        {
            List<InvoiceType> invoiceTypeList = new List<InvoiceType>();

            try
            {
                var rawData = GetListData<dynamic>(criteria);
                if (rawData != null)
                {
                    foreach (var row in rawData)
                    {
                        int id = (int)row.invoiceTypeID;
                        bool isExist = false;

                        foreach (InvoiceType invoiceType in invoiceTypeList)
                        {
                            if (id == invoiceType.InvoiceTypeID)
                            {
                                invoiceType.ProductList.Add(new Product
                                {
                                    ProductID = (int)row.productID,
                                    ProductName = row.productName,
                                });
                                isExist = true;
                            }
                        }

                        if (!isExist)
                        {
                            List<Product> productList = new List<Product>();
                            productList.Add(new Product
                            {
                                ProductID = (int)row.productID,
                                ProductName = row.productName,
                            });
                            invoiceTypeList.Add(new InvoiceType
                            {
                                InvoiceTypeID = (int)row.invoiceTypeID,
                                InvoiceTypeName = row.invoiceTypeName,
                                ProductList = productList
                            });
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return invoiceTypeList;
        }

        public int GetInvoieNo(IMS_InvoiceNoSelCriteria criteria)
        {
            int invoiceNo = 0;

            try
            {
                var rawData = GetListData<dynamic>(criteria);
                if (rawData != null)
                {
                    foreach (var row in rawData)
                    {
                        invoiceNo = (int)row.NewInvoiceNo;
                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return invoiceNo;
        }

        public InvoiceModel GetInvoice(IMS_NewInvoiceItemsSelCriteria criteria)
        {
            var invoice = new InvoiceModel();
            try
            {   
                #region mapping invoice items
                invoice.StatementList = new List<Statement>();
                var statementRawData = GetListData<dynamic>(criteria);

                if (statementRawData != null)
                {
                    var statement = new Statement();
                    statement.StatementDetailList = new List<StatementDetail>();
                    var oldMonth = string.Empty;
                    foreach (var row in statementRawData)
                    {
                        string invoiceMonth = (string)row.InvoiceMonth;
                        if (!string.IsNullOrEmpty(oldMonth) && !oldMonth.Equals(invoiceMonth))
                        {
                            invoice.StatementList.Add(statement);
                            statement = new Statement();
                            statement.StatementDetailList = new List<StatementDetail>();
                        }                        
                        statement.StatementDetailList.Add(new StatementDetail()
                        {
                            ID = 0,
                            Description = row.Description,
                            Reference = row.Reference,
                            Amount = (double)row.TotalAmount
                        });
                        oldMonth = invoiceMonth;
                    }

                    if (statement != null && statement.StatementDetailList.Count > 0)
                    {
                        invoice.StatementList.Add(statement);
                    }
                }
                #endregion                
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return invoice;
        }

        public Core.Models.Result SaveInvoice(InvoiceModel invoiceModel)
        {
            var result = new Core.Models.Result();

            try
            {
                double totalAmount = 0;
                var invoiceXmlFormat = new StringBuilder();
                
                invoiceXmlFormat.Append("<Invoice>");
                invoiceXmlFormat.Append("   <InvoiceNo>" + invoiceModel.InvoiceNo + "</InvoiceNo>");
                invoiceXmlFormat.Append("   <InvoiceName>" + invoiceModel.InvoiceName + "</InvoiceName>");
                invoiceXmlFormat.Append("   <PartyID>" + invoiceModel.Operator.PartyID + "</PartyID>");
                invoiceXmlFormat.Append("   <PartyName>" + invoiceModel.Operator.PartyFullName + "</PartyName>");
                invoiceXmlFormat.Append("   <LicenseeID>" + invoiceModel.Licensee.LicenseeID + "</LicenseeID>");
                invoiceXmlFormat.Append("   <LicenseeName>" + invoiceModel.Licensee.LicenseeName + "</LicenseeName>");
                invoiceXmlFormat.Append("   <InvoiceType>" + invoiceModel.InvoiceType.InvoiceTypeID + "</InvoiceType>");
                invoiceXmlFormat.Append("   <InvoiceTypeName>" + invoiceModel.InvoiceType.InvoiceTypeName + "</InvoiceTypeName>");
                invoiceXmlFormat.Append("   <Products>" + invoiceModel.Product + "</Products>");
                invoiceXmlFormat.Append("   <Currency>" + invoiceModel.Currency + "</Currency>");
                invoiceXmlFormat.Append("   <CurrencyName>" + invoiceModel.CurrencyName + "</CurrencyName>");
                invoiceXmlFormat.Append("   <FromDate>" + invoiceModel.FromDate + "</FromDate>");
                invoiceXmlFormat.Append("   <ToDate>" + invoiceModel.ToDate + "</ToDate>");
                invoiceXmlFormat.Append("   <DueDate>" + invoiceModel.DueDate + "</DueDate>");
                invoiceXmlFormat.Append("   <InvoiceItems>");
                foreach (Statement statement in invoiceModel.StatementList)
                {
                    foreach (StatementDetail statementDetail in statement.StatementDetailList)
                    {
                        invoiceXmlFormat.Append("<InvoiceItem>");
                        invoiceXmlFormat.Append("<Description>" + statementDetail.Description + "</Description>");
                        invoiceXmlFormat.Append("   <Reference>" + statementDetail.Reference + "</Reference>");
                        invoiceXmlFormat.Append("   <Amount>" + statementDetail.Amount + "</Amount>");
                        invoiceXmlFormat.Append("</InvoiceItem>");
                        totalAmount += statementDetail.Amount;
                    }
                }
                invoiceXmlFormat.Append("   </InvoiceItems>");
                invoiceXmlFormat.Append("</Invoice>");

                var criteria = new IMS_InvoiceInsCriteria();
                criteria.AdminID = AppSession.CurrentUser.Id;
                criteria.XMLInvoice = invoiceXmlFormat.ToString();

                var hashResult = _repository.ExecuteCommand(criteria);
                result.ErrorCode = Convert.ToInt32(hashResult["errcode"]);
                result.ErrorMessage = hashResult["errmsg"].ToString();
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                result.ErrorCode = 100;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}
