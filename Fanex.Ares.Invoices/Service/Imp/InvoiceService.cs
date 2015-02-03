namespace Fanex.Ares.Invoice.Service.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using DNAide.Core;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Core.Models;
    using Fanex.Ares.Core.Repository.Impl;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.Ares.Core.Web;
    using Fanex.Ares.Invoice.Criteria.Invoice;
    using Fanex.Ares.Invoice.Models.Invoice;
    using Fanex.Ares.Invoice.Service.Interfaces;
    using Fanex.Artist.Services;
    using Fanex.Common;

    /// <summary>
    /// Invoice Service.
    /// </summary>
    public class InvoiceService : ServiceBase, IInvoiceService
    {
        public IEnumerable<Party> GetOperatorList(OperatorSelectionCriteria criteria)
        {
            return GetEntites<Party>(criteria).Where(p => p.IsHide == false).ToList();
        }

        public IEnumerable<Licensee> GetLicenseesList(LicenseeByPartySelectionCriteria criteria)
        {
            return GetEntites<Licensee>(criteria);
        }

        /// <summary>
        /// Get invoice type.
        /// </summary>
        /// <param name="criteria">Invoice Types Criteria.</param>
        /// <returns>InvoiceType List.</returns>
        public IEnumerable<InvoiceType> GetInvoieType(InvoiceTypesCriteria criteria)
        {
            var invoiceTypeList = new List<InvoiceType>();
            var rawData = GetEntites<dynamic>(criteria);

            if (rawData != null)
            {
                foreach (var row in rawData)
                {
                    var id = (int)row.invoiceTypeID;
                    var isSameInvoiceType = false;

                    foreach (var invoiceType in invoiceTypeList)
                    {
                        if (id == invoiceType.InvoiceTypeID)
                        {
                            invoiceType.ProductList.Add(new Product
                            {
                                ProductID = (int)row.productID,
                                ProductName = row.productName,
                            });

                            isSameInvoiceType = true;
                        }
                    }

                    if (!isSameInvoiceType)
                    {
                        var productList = new List<Product>();
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

            return invoiceTypeList;
        }

        public int GetInvoiceNo(InvoiceNumberCriteria criteria)
        {
            int? invoiceNumber = GetEntity<int>(criteria);

            return invoiceNumber ?? 1;
        }

        /// <summary>
        /// Get invoice detail.
        /// </summary>
        /// <param name="criteria">IMS_NewInvoiceItemsCriteria criteria.</param>
        /// <returns>Invoice model.</returns>
        public InvoiceModel GetInvoice(NewInvoiceItemsInsertionCriteria criteria)
        {
            InvoiceModel invoice = null;
            var dateRangeList = GetDateRange(criteria.FromDate, criteria.ToDate).ToList();
            var statementRawData = GetEntites<dynamic>(criteria);

            if (statementRawData != null)
            {
                var statementDetaitlList = new List<StatementDetail>();

                foreach (var row in statementRawData)
                {
                    var statementProductDetail = new StatementDetail()
                    {
                        ID = 0,
                        Description = row.Description,
                        Reference = row.Reference,
                        Amount = (double)row.TotalAmount,
                        AmountFormated = ((double)row.TotalAmount).Format(2),
                        AdjustItem = (int)row.ItemType,
                        InvoiceMonth = row.InvoiceMonth,
                        DiscountItem = row.DiscountItem,
                        ProductType = row.ItemType
                    };

                    InsertToStatementProductDetailList(ref statementDetaitlList, statementProductDetail);
                }

                var invoiceMonthList = statementDetaitlList.Select(item => item.InvoiceMonth).Distinct().ToList();

                invoice = GroupingInvoiceDetailByMonth(invoiceMonthList, statementDetaitlList, dateRangeList);
            }

            return invoice;
        }

        /// <summary>
        /// Save invoice.
        /// </summary>
        /// <param name="invoiceModel">Invoice model.</param>
        /// <returns>Return result.</returns>
        public Result SaveInvoice(InvoiceModel invoiceModel)
        {
            var result = new Result();

            try
            {
                double totalAmount = 0;
                var invoiceXmlFormat = ConvertInvoiceDataToXMLFormat(invoiceModel, ref totalAmount);
                var criteria = new InvoiceInsertionCriteria();

                criteria.AdminID = AppSession.CurrentUser.Id;
                criteria.XMLInvoice = invoiceXmlFormat.ToString();

                var hashResult = BaseRepository.ExecuteCommand(criteria);

                result.ErrorCode = Convert.ToInt32(hashResult["errcode"]);
                result.ErrorMessage = result.ErrorCode == 0 ? hashResult["invoiceid"].ToString() : hashResult["errmsg"].ToString();
            }
            catch (SqlException exception)
            {
                result.ErrorCode = 100;
                result.ErrorMessage = exception.Message;

                LoggingHandler.Log(exception.Message);
                throw new ServiceException(exception.Message);
            }

            return result;
        }

        public bool IsExistInvoiceNo(ExistedInvoiceNumberCheckingCriteria criteria)
        {
            var hashResult = BaseRepository.ExecuteCommand(criteria);

            return Converter.ToBool(hashResult["isexisted"]);
        }

        /// <summary>
        /// Divide range date to array of date period.
        /// </summary>
        /// <param name="fromDate">The Date begin.</param>
        /// <param name="toDate">The Date end.</param>
        /// <returns>Return list date period.</returns>
        public IEnumerable<DateRange> GetDateRange(DateTime fromDate, DateTime toDate)
        {
            var dateRangeList = new List<DateRange>();

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
                    var dateRange = new DateRange();
                    var startDateTmp = new DateTime();
                    var endDateTmp = new DateTime();
                    if (i == 0)
                    {
                        startDateTmp = fromDate;
                        var firstOfNextMonth = new DateTime(fromDate.Year, fromDate.Month, 1).AddMonths(1);
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
                        var firstOfNextMonth = new DateTime(fromDate.Year, fromDate.Month + i, 1).AddMonths(1);
                        endDateTmp = firstOfNextMonth.AddDays(-1);
                    }

                    dateRange.StartDate = startDateTmp;
                    dateRange.EndDate = endDateTmp;
                    dateRangeList.Add(dateRange);
                }
            }

            return dateRangeList;
        }

        public IEnumerable<ChargeRate> GetChargeRates(ProductChargeRateCriteria criteria)
        {
            return GetEntites<ChargeRate>(criteria).ToList();
        }

        /// <summary>
        /// Get base currency invoice filter.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable {Select List Item}.</returns>
        public IEnumerable<SelectListItem> GetBaseCurrencyInvoiceFilter(BaseCurrencySelectionCriteria criteria)
        {
            var listProducts = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    return new SelectListItem
                    {
                        Value = Convert.ToString(r["currency"]),
                        Text = Convert.ToString(r["currencyname"])
                    };
                };

                listProducts = BaseRepository.Fetch<SelectListItem>(criteria, mappingFunc).ToList();
            }
            catch (SqlException exception)
            {
                LoggingHandler.Log(exception.Message);
                throw new ServiceException(exception.Message, criteria.GetSettingKey(), criteria.GetSpParams());
            }

            return listProducts;
        }

        /// <summary>
        /// Insert statement to list.
        /// </summary>
        /// <param name="statementDetailList">List is exist.</param>
        /// <param name="statementProductDetail">Statement detail to insert.</param>
        private void InsertToStatementProductDetailList(ref List<StatementDetail> statementDetailList, StatementDetail statementProductDetail)
        {
            if (statementDetailList != null && statementProductDetail != null)
            {
                ////If this statement detail is not exist in statement, add statement detail to statement 
                var isExistedStatementDetail = statementDetailList.Find(
                    item => item.InvoiceMonth.Equals(statementProductDetail.InvoiceMonth) && item.DiscountItem.Equals(statementProductDetail.DiscountItem)) != null;

                if (!isExistedStatementDetail)
                {
                    var productDetail = new StatementDetail();
                    productDetail.AdjustItem = statementProductDetail.AdjustItem;
                    productDetail.Amount = statementProductDetail.Amount;
                    productDetail.AmountFormated = statementProductDetail.AmountFormated;
                    if (statementProductDetail.ProductType == 1)
                    {
                        productDetail.Description = statementProductDetail.Description;
                    }

                    productDetail.DiscountItem = statementProductDetail.DiscountItem;
                    productDetail.ID = statementProductDetail.ID;
                    productDetail.InvoiceMonth = statementProductDetail.InvoiceMonth;
                    productDetail.ProductType = statementProductDetail.ProductType;
                    productDetail.Reference = statementProductDetail.Reference;

                    statementProductDetail.StatementProductDetail.Add(productDetail);
                    statementDetailList.Add(statementProductDetail);
                }
                else
                {
                    statementDetailList = statementDetailList.Select(item =>
                    {
                        if (item.InvoiceMonth.Equals(statementProductDetail.InvoiceMonth) && item.DiscountItem.Equals(statementProductDetail.DiscountItem))
                        {
                            item.StatementProductDetail.Add(statementProductDetail);
                            item.Amount = item.Amount + statementProductDetail.Amount;
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

        /// <summary>
        /// Converts the invoice data to XML format.
        /// </summary>
        /// <param name="invoiceModel">The invoice model.</param>
        /// <param name="totalAmount">The total amount.</param>
        /// <returns>String Builder.</returns>
        private StringBuilder ConvertInvoiceDataToXMLFormat(InvoiceModel invoiceModel, ref double totalAmount)
        {
            var invoiceXmlFormat = new StringBuilder();
            invoiceXmlFormat.Append("<Invoice>");
            invoiceXmlFormat.Append("   <InvoiceID>" + invoiceModel.InvoiceID + "</InvoiceID>");
            invoiceXmlFormat.Append("   <InvoiceNo>" + invoiceModel.InvoiceNo + "</InvoiceNo>");
            invoiceXmlFormat.Append("   <InvoiceName>" + invoiceModel.InvoiceName + "</InvoiceName>");
            invoiceXmlFormat.Append("   <PartyID>" + invoiceModel.Operator.PartyID + "</PartyID>");
            invoiceXmlFormat.Append("   <PartyName>" + invoiceModel.Operator.PartyFullName + "</PartyName>");
            invoiceXmlFormat.Append("   <LicenseeID>" + invoiceModel.Licensee.LicenseeID + "</LicenseeID>");
            invoiceXmlFormat.Append("   <InvoiceType>" + invoiceModel.InvoiceType.InvoiceTypeID + "</InvoiceType>");
            invoiceXmlFormat.Append("   <Products>" + invoiceModel.Product + "</Products>");
            invoiceXmlFormat.Append("   <Currency>" + invoiceModel.Currency + "</Currency>");
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
                    invoiceXmlFormat.Append("<Description>" + (statementDetail.Description != null ?
                        statementDetail.Description.Replace("&", "amp") :
                        string.Empty) + "</Description>");
                    invoiceXmlFormat.Append("   <Reference>" + statementDetail.Reference + "</Reference>");
                    invoiceXmlFormat.Append("   <Amount>" + statementDetail.Amount + "</Amount>");
                    invoiceXmlFormat.Append("   <ItemType>" + statementDetail.AdjustItem + "</ItemType>");
                    invoiceXmlFormat.Append("   <InvoiceMonth>" + statementDetail.InvoiceMonth + "</InvoiceMonth>");
                    invoiceXmlFormat.Append("   <DiscountItem>" + statementDetail.DiscountItem + "</DiscountItem>");
                    invoiceXmlFormat.Append("</InvoiceItem>");
                    totalAmount += statementDetail.Amount;
                }
            }

            invoiceXmlFormat.Append("   </InvoiceItems>");
            invoiceXmlFormat.Append("</Invoice>");

            return invoiceXmlFormat;
        }

        /// <summary>
        /// Groupings the invoice detail by month.
        /// </summary>
        /// <param name="invoiceMonthList">The invoice month list.</param>
        /// <param name="statementDetaitlList">The statement detail list.</param>
        /// <param name="dateRangeList">The date range list.</param>
        /// <returns>Invoice Model.</returns>
        private InvoiceModel GroupingInvoiceDetailByMonth(
            List<string> invoiceMonthList,
            List<StatementDetail> statementDetaitlList,
            List<DateRange> dateRangeList)
        {
            var invoice = new InvoiceModel();
            invoice.StatementList = new List<Statement>();

            foreach (string month in invoiceMonthList)
            {
                var statement = new Statement();
                statement.StatementDetailList = statementDetaitlList.Where(item => item.InvoiceMonth == month).ToList();
                foreach (var range in dateRangeList)
                {
                    if (CastAide.AsDateTime("1 " + month).Month == range.StartDate.Month)
                    {
                        statement.FromDate = Converter.To<string>(range.StartDate);
                        statement.ToDate = Converter.To<string>(range.EndDate);
                    }
                }

                invoice.StatementList.Add(statement);
            }

            return invoice;
        }
    }
}
