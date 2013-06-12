
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
    using DNAide.Core;

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
                        BenificiaryBank = Convert.ToString(r["BenificiaryBank"]),
                        IBAN = Convert.ToString(r["IBAN"]),
                        SwiftCode = Convert.ToString(r["SwiftCode"]),
                        CorrespondentBank = Convert.ToString(r["CorrespondentBank"]),
                        CorrespondentAccNo = Convert.ToString(r["CorrespondentAccNo"]),
                        CorrespondentSwiftCode = Convert.ToString(r["CorrespondentSwiftCode"]),
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
                List<DateRange> dateRangeList = new List<DateRange>();

                dateRangeList = GetDateRange(criteria.FromDate, criteria.ToDate);

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
                            Amount = (double)row.TotalAmount,
                            AdjustItem = false,
                            InvoiceMonth = row.InvoiceMonth,
                            DiscountItem = row.DiscountItem
                            
                        });
                        oldMonth = invoiceMonth;
                        foreach (DateRange range in dateRangeList)
                        {
                            if (CastAide.AsDateTime("1 " + oldMonth).Month == range.StartDate.Month)
                            {
                                statement.FromDate = Converter.To<string>(range.StartDate);
                                statement.ToDate = Converter.To<string>(range.EndDate);
                            }
                        }
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
                invoiceXmlFormat.Append("   <InvoiceID>" + invoiceModel.InvoiceID + "</InvoiceID>");
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
                invoiceXmlFormat.Append("   <Note>" + invoiceModel.Note + "</Note>");
                invoiceXmlFormat.Append("   <InvoiceItems>");
                foreach (Statement statement in invoiceModel.StatementList)
                {
                    foreach (StatementDetail statementDetail in statement.StatementDetailList)
                    {
                        invoiceXmlFormat.Append("<InvoiceItem>");
                        invoiceXmlFormat.Append("<InvoiceItemID>" + statementDetail.ID + "</InvoiceItemID>");
                        invoiceXmlFormat.Append("<Description>" + statementDetail.Description + "</Description>");
                        invoiceXmlFormat.Append("   <Reference>" + statementDetail.Reference + "</Reference>");
                        invoiceXmlFormat.Append("   <Amount>" + statementDetail.Amount + "</Amount>");
                        invoiceXmlFormat.Append("   <AdjustItem>" + statementDetail.AdjustItem + "</AdjustItem>");
                        invoiceXmlFormat.Append("   <InvoiceMonth>" + statementDetail.InvoiceMonth + "</InvoiceMonth>");
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
                if (result.ErrorCode == 0)
                {
                    result.ErrorMessage = hashResult["invoiceid"].ToString();
                }
                else
                {
                    result.ErrorMessage = hashResult["errmsg"].ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                result.ErrorCode = 100;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public bool IsExistInvoiceNo(IMS_CheckExistedInvoiceNo criteria)
        {
            var hashResult = _repository.ExecuteCommand(criteria);
            return Converter.ToBool(hashResult["isexisted"]);
        }
        public List<DateRange> GetDateRange(DateTime FromDate, DateTime ToDate)
        {
            List<DateRange> dateRangeList = new List<DateRange>();

            int numericMonth = ToDate.Month - FromDate.Month + 1;

            for (int i = 0; i < numericMonth; i++)
            {
                DateRange dateRange = new DateRange();
                DateTime StartDateTmp = new DateTime();
                DateTime EndDateTmp = new DateTime();
                if (i == 0)
                {
                    StartDateTmp = FromDate;
                    DateTime firstOfNextMonth = new DateTime(FromDate.Year, FromDate.Month, 1).AddMonths(1);
                    //DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);
                    EndDateTmp = firstOfNextMonth.AddDays(-1);
                }
                else if (i == numericMonth-1)
                {
                    StartDateTmp = new DateTime(FromDate.Year, ToDate.Month, 1);
                    //DateTime firstOfNextMonth = new DateTime(FromDate.Year + i - 1, FromDate.Month + i - 1, 1).AddMonths(1);
                    //DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);
                    EndDateTmp = ToDate;
                }
                else
                {
                    StartDateTmp = new DateTime(FromDate.Year, FromDate.Month + i, 1);
                    DateTime firstOfNextMonth = new DateTime(FromDate.Year, FromDate.Month + i, 1).AddMonths(1);
                    //DateTime lastOfThisMonth = firstOfNextMonth.AddDays(-1);
                    EndDateTmp = firstOfNextMonth.AddDays(-1);
                }

                dateRange.StartDate = StartDateTmp;
                dateRange.EndDate = EndDateTmp;
                dateRangeList.Add(dateRange);
            }

            return dateRangeList;
        }
    }
}
