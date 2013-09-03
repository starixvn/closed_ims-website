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
    using System.Linq;
    using System.Text;
    using DNAide.Core;
    using Fanex.Ares.Core.Models;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.Ares.Core.Web;
    using Fanex.Ares.Invoice.Criteria.Invoice;
    using Fanex.Ares.Invoice.Models.Invoice;
    using Fanex.Ares.Invoice.Service.Interface;
    using Fanex.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class InvoiceService : ServiceBase, IInvoiceService
    {
        /// <summary>
        ///  Get operator list
        /// </summary>
        /// <param name="criteria">IMS_OperatorSelCriteria criteria</param>
        /// <returns> Operator list</returns>
        public List<Operator> GetOperatorList(IMS_OperatorCriteria criteria)
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
                        Company = Convert.ToString(r["Company"]),
                        CorrespondentAccNo = Convert.ToString(r["CorrespondentAccNo"]),
                        CorrespondentSwiftCode = Convert.ToString(r["CorrespondentSwiftCode"]),
                        AttnName = Convert.ToString(r["AttnName"]),
                        To1 = Convert.ToString(r["To1"]),
                        To2 = Convert.ToString(r["To2"]),
                        Logo = Convert.ToString(r["Logo"]),
                        IsHide = Convert.ToBoolean(r["IsHide"])
                    };

                    return res;
                };

                operators = _repository.Fetch<Operator>(criteria, mappingFunc).Where(p => p.IsHide == false).ToList();
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return operators;
        }

        /// <summary>
        /// Select lisensee list.
        /// </summary>
        /// <param name="criteria">IMS_LicenseeSelByPartyCriteria criteria</param>
        /// <returns>Licensee List</returns>
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

        /// <summary>
        /// Get invoice type.
        /// </summary>
        /// <param name="criteria">IMS_InvoieTypesSelCriteria criteria</param>
        /// <returns>InvoiceType List.</returns>
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

                dateRangeList = this.GetDateRange(criteria.FromDate, criteria.ToDate);

                invoice.StatementList = new List<Statement>();
                var statementRawData = GetListData<dynamic>(criteria);
                 
                if (statementRawData != null)
                {
                    var statementDetailList = new List<StatementDetail>(); 
                    foreach (var row in statementRawData)
                    {
                        StatementDetail statementDetail = new StatementDetail()
                        {
                            ID = 0,
                            Description = row.Description,
                            Reference = row.Reference,
                            Amount = (double)row.TotalAmount,
                            AdjustItem = false,
                            InvoiceMonth = row.InvoiceMonth,
                            DiscountItem = row.DiscountItem,
                            ProductType = row.ItemType
                        };

                        this.InsertToStatement(ref statementDetailList, statementDetail);
                    }

                    var statement = new Statement();
                    statement.StatementDetailList = new List<StatementDetail>();
                    var oldMonth = string.Empty;
                    foreach (var row in statementDetailList)
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
                            Amount = row.Amount,
                            AdjustItem = false,
                            InvoiceMonth = row.InvoiceMonth,
                            DiscountItem = row.DiscountItem,
                            ProductType = row.ProductType
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
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return invoice;
        }

        public Result SaveInvoice(InvoiceModel invoiceModel)
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
                invoiceXmlFormat.Append("   <TotalAmount>" + Math.Round(invoiceModel.TotalAmount, 2) + "</TotalAmount>");
                invoiceXmlFormat.Append("   <InvoiceItems>");
                foreach (Statement statement in invoiceModel.StatementList)
                {
                    foreach (StatementDetail statementDetail in statement.StatementDetailList)
                    {
                        invoiceXmlFormat.Append("<InvoiceItem>");
                        invoiceXmlFormat.Append("<InvoiceItemID>" + statementDetail.ID + "</InvoiceItemID>");
                        invoiceXmlFormat.Append("<Description>" + (statementDetail.Description != null ? statementDetail.Description.Replace("&", "amp") : string.Empty)  + "</Description>");
                        invoiceXmlFormat.Append("   <Reference>" + statementDetail.Reference + "</Reference>");
                        invoiceXmlFormat.Append("   <Amount>" + statementDetail.Amount + "</Amount>");
                        invoiceXmlFormat.Append("   <AdjustItem>" + statementDetail.AdjustItem + "</AdjustItem>");
                        invoiceXmlFormat.Append("   <InvoiceMonth>" + statementDetail.InvoiceMonth + "</InvoiceMonth>");
                        invoiceXmlFormat.Append("   <DiscountItem>" + statementDetail.DiscountItem + "</DiscountItem>");
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

        public List<DateRange> GetDateRange(DateTime fromDate, DateTime toDate)
        {
            List<DateRange> dateRangeList = new List<DateRange>();

            int numericMonth = toDate.Month - fromDate.Month + 1;
            if (numericMonth == 1)
            {
                dateRangeList.Add(new DateRange
                {
                    StartDate = fromDate,
                    EndDate = toDate
                });
            }
            else
            {
                for (int i = 0; i < numericMonth; i++)
                {
                    DateRange dateRange = new DateRange();
                    DateTime startDateTmp = new DateTime();
                    DateTime endDateTmp = new DateTime();
                    if (i == 0)
                    {
                        startDateTmp = fromDate;
                        DateTime firstOfNextMonth = new DateTime(fromDate.Year, fromDate.Month, 1).AddMonths(1);
                        endDateTmp = firstOfNextMonth.AddDays(-1);
                    }
                    else if (i == numericMonth - 1)
                    {
                        startDateTmp = new DateTime(fromDate.Year, toDate.Month, 1);
                        endDateTmp = toDate;
                    }
                    else
                    {
                        startDateTmp = new DateTime(fromDate.Year, fromDate.Month + i, 1);
                        DateTime firstOfNextMonth = new DateTime(fromDate.Year, fromDate.Month + i, 1).AddMonths(1);
                        endDateTmp = firstOfNextMonth.AddDays(-1);
                    }

                    dateRange.StartDate = startDateTmp;
                    dateRange.EndDate = endDateTmp;
                    dateRangeList.Add(dateRange);
                }
            }

            return dateRangeList;
        }

        public List<ChargeRate> GetChargeRate(IMS_GetChargeRateCriteria criteria)
        {
            List<ChargeRate> chargeRateList = new List<ChargeRate>();

            try
            {
                var rawData = GetListData<dynamic>(criteria);
                if (rawData != null)
                {
                    foreach (var row in rawData)
                    {
                        chargeRateList.Add(new ChargeRate
                        {
                            ProductID = (int)row.ProductID,
                            DistributorRate = Converter.To<decimal>(row.DistributorRate),
                            PartnerRate = Converter.To<decimal>(row.PartnerRate),
                            ONEworksRate = Converter.To<decimal>(row.ONEworksRate),
                            StartDate = (DateTime)row.StartDate,
                            EndDate = (DateTime)row.EndDate
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }

            return chargeRateList;
        }

        private void InsertToStatement(ref List<StatementDetail> statementDetailList, StatementDetail statementDetail)
        {
            if (statementDetailList != null && statementDetail != null)
            {
                ////If this statement detail is not exist in statement, add statement detail to statement 
                var isExistedStatementDetail = statementDetailList.Find(item => item.InvoiceMonth.Equals(statementDetail.InvoiceMonth) && item.DiscountItem.Equals(statementDetail.DiscountItem)) != null;

                if (!isExistedStatementDetail)
                {
                    statementDetail.StatementProductDetail.Add(statementDetail);
                    statementDetailList.Add(statementDetail);
                }
                else
                {
                    statementDetailList = statementDetailList.Select(item =>
                    {
                        if (item.InvoiceMonth.Equals(statementDetail.InvoiceMonth) && item.DiscountItem.Equals(statementDetail.DiscountItem))
                        {
                            item.StatementProductDetail.Add(statementDetail);
                            item.Amount = item.Amount + statementDetail.Amount;
                            return item;
                        }
                        else
                        {
                            return item;
                        }
                    }).ToList();
                }
            }
        }
    }
}
