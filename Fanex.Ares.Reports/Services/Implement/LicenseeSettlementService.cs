using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using ExcelUtil.Spreadsheet;
using Fanex.Ares.Core.Repository.Impl;
using Fanex.Ares.Core.Services.Impl;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;
using System.Linq;
using Fanex.Caching;
using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
using Fanex.Ares.Report.ViewModels;
using System.Drawing;
using Fanex.Ares.Core.Extensions;
using Resource;
using Fanex.Common.Extensions;

namespace Fanex.Ares.Report.Services.Interface
{
    public class LicenseeSettlementService : ServiceBase, ILicenseeSettlementService
    {
        private readonly BaseRepository _repository;

        /// <summary>
        /// Cache key for All Licensees
        /// </summary>
        private const string CacheKeyAllLicensee = "Fanex.Ares.Report.Services.AllLicensees";
        private const string SessionKeyLicenseeSettlementReport = "Fanex.Ares.Report.Services.LicenseeSettlementReport";

        public LicenseeSettlementService()
        {
            _repository = new BaseRepository();
        }


        public IEnumerable<SelectListItem> GetAllLicensees()
        {
            IList<SelectListItem> listLicensees = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    return new SelectListItem
                    {
                        Value = Convert.ToString(r["LicenseeID"]),
                        Text = Convert.ToString(r["LicenseeName"])
                    };
                };

                listLicensees = _repository.Fetch<SelectListItem>(new IMS_LicenseeSelCriteria(), mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return listLicensees;
        }


        public IEnumerable<SelectListItem> GetProductsByLicensee(IMS_LicenseeProductSelCriteria criteria)
        {
            IList<SelectListItem> listProducts = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    return new SelectListItem
                    {
                        Value = Convert.ToString(r["productid"]),
                        Text = Convert.ToString(r["productname"])
                    };
                };

                listProducts = _repository.Fetch<SelectListItem>(criteria, mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return listProducts;
        }

        public IEnumerable<SelectListItem> GetCurrencyByLicensee(IMS_LicenseeCurrencySelCriteria criteria)
        {
            IList<SelectListItem> listProducts = new List<SelectListItem>();
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

                listProducts = _repository.Fetch<SelectListItem>(criteria, mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listProducts;
        }

        public IEnumerable<SelectListItem> GetBaseCurrency(IMS_BaseCurrencySel criteria)
        {
            IList<SelectListItem> listProducts = new List<SelectListItem>();
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

                listProducts = _repository.Fetch<SelectListItem>(criteria, mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listProducts;
        }

        public LicenseeInformation GetLicenseeInformation(IMS_LicenseeBillingInfoSelCriteria criteria)
        {
            var listLicenseeInfo = base.DynamicRepository.Fetch<LicenseeInformation>(criteria).FirstOrDefault<LicenseeInformation>();
            
            return listLicenseeInfo;
        }

        public Fanex.Ares.Report.ViewModels.LicenseeSettlement.Report GetLicenseeReport(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            var report = new Fanex.Ares.Report.ViewModels.LicenseeSettlement.Report();
            try
            {
                Func<IDataReader, ReportEntity> mappingFunc = r =>
                {
                    var baseColName = string.Empty;
                    if (criteria.BaseCurrency == 3) // USD = 3
                    {
                        baseColName = "CompanyBaseUSD";
                    }
                    else // EUR = 6
                    {
                        baseColName = "CompanyBaseEUR";
                    }

                    var res = new ReportEntity
                    {
                        Betcount = Convert.ToInt32(r["Betcount"].ToString()),
                        Company = Convert.ToDouble(r["Company"].ToString()),
                        CompanyBaseCurrency = Convert.ToDouble(r[baseColName].ToString()),
                        Currency = Convert.ToInt32(r["Currency"].ToString()),
                        CurrencyName = r["CurrencyName"].ToString(),
                        LicenseeComm = Convert.ToDouble(r["LicenseeComm"].ToString()),
                        LicenseeWL = Convert.ToDouble(r["LicenseeWL"].ToString()),
                        MemberComm = Convert.ToDouble(r["MemberComm"].ToString()),
                        MemberWinloss = Convert.ToDouble(r["MemberWinloss"].ToString()),
                        Turnover = Convert.ToDouble(r["Turnover"].ToString()),
                        WinlostDate = DateTime.Parse(r["WinLostMonth"].ToString())
                    };
                    return res;
                };

                report.EntityList = _repository.Fetch<ReportEntity>(criteria, mappingFunc).ToList();
                report.GrandTotal = 0;
                foreach (ReportEntity item in report.EntityList)
                {
                    report.GrandTotal += item.CompanyBaseCurrency;
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return report;
        }

        public IEnumerable<ExchangeRate> GetLicenseeExchangeRate(IMS_ExchangeRateBillingSelCriteria criteria)
        {
            var listExchangeRate = new List<ExchangeRate>();
            try
            {
                Func<IDataReader, ExchangeRate> mappingFunc = r =>
                {
                    var res = new ExchangeRate
                    {
                        Currency = Convert.ToInt32(r["ExchangeId"].ToString()),
                        CurrencyName = r["Code"].ToString(),
                        BasePerUnit = Convert.ToDouble(r[criteria.BaseCurrencyName].ToString())
                    };

                    res.UnitsPerBase = 1 / res.BasePerUnit;

                    return res;
                };

                listExchangeRate = _repository.Fetch<ExchangeRate>(criteria, mappingFunc).ToList();
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listExchangeRate;
        }

        public LicenseeSettlementModel GenerateReport(IMS_Rpt_LicenseeSettlementCriteria criteria)
        {
            // Licensee Information
            var listLicensee = this.GetAllLicensees();
            var listProduct = this.GetProductsByLicensee(new IMS_LicenseeProductSelCriteria() { LicenseeId = criteria.LicenseeId });
            var listCurrency = this.GetCurrencyByLicensee(new IMS_LicenseeCurrencySelCriteria() { LicenseeId = criteria.LicenseeId });
            var model = new LicenseeSettlementModel();
            var licenseeInfoCriteria = new IMS_LicenseeBillingInfoSelCriteria();
            licenseeInfoCriteria.LicenseeId = criteria.LicenseeId;
            licenseeInfoCriteria.ProductId = criteria.Product;
            model.LicenseeInfor = this.GetLicenseeInformation(licenseeInfoCriteria);
            model.LicenseeInfor.LicenseeID = criteria.LicenseeId;
            model.LicenseeInfor.LicenseeName = listLicensee.Where(m => m.Value == criteria.LicenseeId.ToString()).FirstOrDefault<SelectListItem>().Text;
            model.LicenseeInfor.ProductName = listProduct.Where(m => m.Value == criteria.Product.ToString()).FirstOrDefault<SelectListItem>().Text;
            //model.LicenseeInfor.SettlementDate = DateTime.Now;
            model.LicenseeInfor.SettlementDate = criteria.ToDate;

            // Licensee Report
            var reportCriteria = new IMS_Rpt_LicenseeSettlementCriteria();
            reportCriteria.LicenseeId = criteria.LicenseeId;
            reportCriteria.LicenseeName = model.LicenseeInfor.LicenseeName;
            reportCriteria.Product = criteria.Product;
            reportCriteria.Currency = criteria.Currency;
            reportCriteria.BaseCurrency = criteria.BaseCurrency;
            reportCriteria.FromDate = criteria.FromDate;
            reportCriteria.ToDate = criteria.ToDate;

            model.Report = this.GetLicenseeReport(reportCriteria);

            // Exchange Rate
            var exchangeRateCriteria = new IMS_ExchangeRateBillingSelCriteria();
            model.ExchangeList = new List<ViewModels.ExchangeRate>();

            var exchangeRatecurrencies = string.Empty;
            foreach (ReportEntity item in model.Report.EntityList)
            {
                // Calculate membertotal = memberwinloss + membercomm
                item.MemberTotal = item.MemberWinloss + item.MemberComm;

                // Calculate licenseetotal = licenseewinloss + licenseecomm
                item.LicenseeTotal = item.LicenseeWL + item.LicenseeComm;

                if (criteria.Currency == 0) // All Currency
                {
                    if (!exchangeRatecurrencies.Contains(item.Currency.ToString()))
                    {
                        exchangeRatecurrencies += item.Currency.ToString() + ",";
                    }
                }
                else // Single currency
                {
                    exchangeRatecurrencies = item.Currency.ToString();
                }
            }

            exchangeRatecurrencies.Trim(',');

            exchangeRateCriteria.Currency = exchangeRatecurrencies;
            exchangeRateCriteria.ExchangeDate = criteria.ToDate;
            exchangeRateCriteria.BaseCurrencyId = criteria.BaseCurrency;
            exchangeRateCriteria.BaseCurrencyName = listCurrency.Where(m => m.Value == criteria.BaseCurrency.ToString()).FirstOrDefault<SelectListItem>().Text;

            var exchangeList = this.GetLicenseeExchangeRate(exchangeRateCriteria).ToList();

            model.ExchangeList = (from Item1 in model.Report.EntityList
                                  join Item2 in exchangeList
                                  on Item1.CurrencyName equals Item2.CurrencyName
                                  select Item2)
                                  .ToList();

            model.LicenseeInfor.CurrencyBilling = exchangeRateCriteria.BaseCurrencyName;
            SessionExtensions.AddSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeSettlementReport, model); 
            return model;
        }

            
        public void ExportSettlementReportToExcel(string exchange, string product)
        {
            try
            {
                #region local varialbles
                var reportName = "LicenseeSettlementReport";
                var model = new LicenseeSettlementModel();
                model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, SessionKeyLicenseeSettlementReport);
                var excelFile = new ExcelFile();
                var ewReport = excelFile.Worksheets.Add(string.Format("LicenseeSettlementReport"));
                var rowIndex = 0;
                #endregion

                if (model != null && model.Report.EntityList.Count > 0)
                {

                    //Billing info
                    ExportSettlementReportToExcelAssignBillingInfo(model.LicenseeInfor, ref rowIndex, ref ewReport, product);
                    rowIndex+=2;
                    //report & exchange rate
                    ExportSettlementReportToExcelAssignDetailAndExchangeRate(model.ExchangeList, model.Report, model.LicenseeInfor.LicenseeName, model.LicenseeInfor.SettlementDate,
                        ref rowIndex, ref ewReport, exchange);
                }

                #region response file
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
                #endregion
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
        }

        #region private functions
        private void ExportSettlementReportToExcelAssignBillingInfo(LicenseeInformation licenseeInfo, 
            ref int rowIndex, ref ExcelWorksheet ewReport, string product) {
                ewReport.Cells[rowIndex, 0].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 0].Style.Font.Size = 230;
            ewReport.Rows[0].Height = 330;
            ewReport.Cells[rowIndex, 0].Value = Labels.INTERNAL_WORKSHEET_FOR_SETTLEMENT;
            ewReport.Cells[rowIndex+=1, 0].Value = Labels.Finance_Department;
            ewReport.Cells[rowIndex+=2, 0].Value = Labels.SETTLEMENT_REPORT;
            ewReport.Cells[rowIndex, 3].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 3].Value = licenseeInfo.SettlementDate.ToString("dd-MMM-yy");
            ewReport.Cells[rowIndex += 1, 0].Value = string.Format("{0}:", Labels.Licensee.ToUpper());
            ewReport.Cells[rowIndex, 2].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 2].Value = licenseeInfo.LicenseeName;
            ewReport.Cells[rowIndex, 3].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 3].Value = product.ToUpper();
            ewReport.Cells[rowIndex, 5].Value = string.Format("{0}:", Labels.FBilling);
            ewReport.Cells[rowIndex, 7].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 7].Value = licenseeInfo.FirstBilling.ToString("dd-MMM-yy");
            ewReport.Cells[rowIndex += 1, 5].Value = string.Format("{0}:", Labels.Basis_of_Billing);
            ewReport.Cells[rowIndex, 7].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 7].Value = licenseeInfo.BasisOfBilling;
            ewReport.Cells[rowIndex += 2, 5].Value = string.Format("{0}:", Labels.Minimum_Fees);
            ewReport.Cells[rowIndex, 7].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 7].Value = licenseeInfo.MinimumFees;
            ewReport.Cells[rowIndex += 1, 5].Value = string.Format("{0}:", Labels.Head_of_Terms);
            ewReport.Cells[rowIndex, 7].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 7].Value = licenseeInfo.HeadOfTerms.ToString("dd-MMM-yy");
        }


        private void ExportSettlementReportToExcelAssignDetailAndExchangeRate(IEnumerable<ExchangeRate> exchangeRateInfo, 
            Fanex.Ares.Report.ViewModels.LicenseeSettlement.Report reportDetails, string licenseeName, DateTime settlementDate,
            ref int rowIndex, ref ExcelWorksheet ewReport, string exchangeTo)
        {
            //cellStyle.FillPattern.SetSolid
            
            var headerColor = ExcelExtensions.HEADER_TITLE_COLOR;
            var normalStyle = ExcelExtensions.GetCellTextStyle(ExcelExtensions.BACKGROUND_COLOR, false);
            var numberStyle = ExcelExtensions.GetCellNumberStyle("", ExcelExtensions.BACKGROUND_COLOR, false, false, false);
            var decimalStyle = ExcelExtensions.GetCellNumberStyle(".00", ExcelExtensions.BACKGROUND_COLOR, false, false, true);
            var startRow = rowIndex;
            var colIndex = 0;

            #region set width of each col
            ewReport.Columns[0].Width = 255 * 10;
            ewReport.Columns[1].Width = 255 * 9;
            ewReport.Columns[2].Width = 255 * 15;
            ewReport.Columns[3].Width = 255 * 10;
            ewReport.Columns[4].Width = 255 * 15;
            ewReport.Columns[5].Width = 255 * 8;
            ewReport.Columns[6].Width = 255 * 15;
            ewReport.Columns[7].Width = 255 * 15;
            ewReport.Columns[8].Width = 255 * 8;
            ewReport.Columns[9].Width = 255 * 15;
            ewReport.Columns[10].Width = 255 * 15;
            ewReport.Columns[11].Width = 255 * 15;
            ewReport.Columns[12].Width = 255 * 8;
            #endregion
            #region win/loss
            #region header
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = Labels.Date;

            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = Labels.Currency;

            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = Labels.Turnover;

            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = Labels.TotalTrans;

            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex, colIndex + 2).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex, colIndex + 2).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex, colIndex + 2).Value = Labels.Member;

            ewReport.Cells[rowIndex + 1, colIndex].Style.FormatTitleCell(headerColor);
            ewReport.Cells[rowIndex + 1, colIndex].Value = Labels.WinLoss;

            ewReport.Cells[rowIndex + 1, colIndex += 1].Style.FormatTitleCell(headerColor);
            ewReport.Cells[rowIndex + 1, colIndex].Value = Labels.Comm;

            ewReport.Cells[rowIndex + 1, colIndex += 1].Style.FormatTitleCell(headerColor);
            ewReport.Cells[rowIndex + 1, colIndex].Value = Labels.Total;

            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex, colIndex + 2).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex, colIndex + 2).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex, colIndex + 2).Value = licenseeName;

            ewReport.Cells[rowIndex + 1, colIndex].Style.FormatTitleCell(headerColor);
            ewReport.Cells[rowIndex + 1, colIndex].Value = Labels.WinLoss;

            ewReport.Cells[rowIndex + 1, colIndex += 1].Style.FormatTitleCell(headerColor);
            ewReport.Cells[rowIndex + 1, colIndex].Value = Labels.Comm;

            ewReport.Cells[rowIndex + 1, colIndex += 1].Style.FormatTitleCell(headerColor);
            ewReport.Cells[rowIndex + 1, colIndex].Value = Labels.Total;

            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = Labels.Company;

            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = exchangeTo;
            #endregion
            rowIndex += 2;

            #region detail
            foreach (ReportEntity detail in reportDetails.EntityList)
            {
                colIndex = 0;

                ewReport.Cells[rowIndex, colIndex].Style = normalStyle;
                ewReport.Cells[rowIndex, colIndex].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                ewReport.Cells[rowIndex, colIndex].Value = detail.WinlostDate.ToString("dd-MMM-yy");
                ewReport.Cells[rowIndex, colIndex += 1].Style = normalStyle;
                ewReport.Cells[rowIndex, colIndex].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                ewReport.Cells[rowIndex, colIndex].Value = detail.CurrencyName;
                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                ewReport.Cells[rowIndex, colIndex].Value = detail.Turnover;
                ewReport.Cells[rowIndex, colIndex += 1].Style = numberStyle;
                ewReport.Cells[rowIndex, colIndex].Value = detail.Betcount;
                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                ewReport.Cells[rowIndex, colIndex].Value = detail.MemberWinloss;
                
                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                if (detail.MemberComm != 0)
                {
                    ewReport.Cells[rowIndex, colIndex].Value = detail.MemberComm;
                }
                else
                {
                    ewReport.Cells[rowIndex, colIndex].Value = "-";
                }

                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                ewReport.Cells[rowIndex, colIndex].Value = detail.MemberTotal;
                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                ewReport.Cells[rowIndex, colIndex].Value = detail.LicenseeWL;

                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                if (detail.LicenseeComm != 0)
                {
                    ewReport.Cells[rowIndex, colIndex].Value = detail.LicenseeComm;
                }
                else
                {
                    ewReport.Cells[rowIndex, colIndex].Value = "-";
                }

                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                ewReport.Cells[rowIndex, colIndex].Value = detail.LicenseeTotal;
                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                ewReport.Cells[rowIndex, colIndex].Value = detail.Company;
                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                ewReport.Cells[rowIndex, colIndex].Value = detail.Company;
                rowIndex += 1;
            }
            ewReport.Cells[rowIndex, colIndex].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, colIndex].Style.NumberFormat = "#,##0.00_);[RED](#,##0.00)";
            ewReport.Cells[rowIndex, colIndex].Value = reportDetails.GrandTotal;
            #endregion
            
            #endregion

            #region exchange rate
            #region header
            ewReport.Cells.GetSubrangeAbsolute(startRow, colIndex += 2, startRow, colIndex + 1).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(startRow, colIndex, startRow, colIndex + 1).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(startRow, colIndex, startRow, colIndex + 1).Value = string.Format("FOREX @ {0}",
                settlementDate.ToString("dd-MMM-yy"));

            ewReport.Cells[startRow += 1, colIndex].Style.FormatTitleCell(headerColor);
            ewReport.Cells[startRow, colIndex].Value = string.Format("{0}/1 Unit", exchangeTo);
            ewReport.Cells[startRow, colIndex + 1].Style.FormatTitleCell(headerColor);
            ewReport.Cells[startRow, colIndex + 1].Value = string.Format("Units/1 {0}", exchangeTo);
            #endregion

            colIndex -= 1;
            foreach (ExchangeRate exchangeRate in exchangeRateInfo)
            {
                startRow+=1;
                ewReport.Cells[startRow, colIndex].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                ewReport.Cells[startRow, colIndex].Value = exchangeRate.CurrencyName;
                ewReport.Cells[startRow, colIndex+1].Style = normalStyle;
                ewReport.Cells[startRow, colIndex + 1].Value = exchangeRate.BasePerUnit;
                ewReport.Cells[startRow, colIndex+2].Style = normalStyle;
                ewReport.Cells[startRow, colIndex + 2].Value = exchangeRate.UnitsPerBase;
            }
            #endregion

            #region note            
            ewReport.Cells.GetSubrangeAbsolute(rowIndex += 2, 0, rowIndex, colIndex -1).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, 0, rowIndex, colIndex - 1).SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Medium);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, 0, rowIndex, colIndex - 1).Value = string.Format(Labels.IDR_VND_Amount_In_IDR, exchangeTo);
            ewReport.Rows[rowIndex].Height = 300;
            ewReport.Cells[rowIndex += 1, 0].Value = string.Format(Labels.FOREX_Info, exchangeTo);
            #endregion
        }
        #endregion
    }
    
}