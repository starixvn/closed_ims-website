using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;
using ExcelUtil.Spreadsheet;
using Fanex.Ares.Core.Repository.Impl;
using Fanex.Ares.Core.Services.Impl;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;
using Fanex.Ares.Report.Criteria;
using System.Linq;
using Fanex.Caching;
using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
using Fanex.Ares.Report.ViewModels;
using System.Drawing;
using Fanex.Ares.Core.Extensions;
using Resource;
using Fanex.Common.Extensions;
using Fanex.Ares.Core.Utils;

namespace Fanex.Ares.Report.Services.Interface
{
    public class SportsbookExcelService : ServiceBase, ISportsbookExcelService
    {
        #region Export Excel
        public void ExportReportToExcel(string exchange, string product)
        {
            try
            {
                #region local varialbles

                var model = new LicenseeSettlementModel();
                model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session,CacheServiceKey.SessionKeyLicenseeSettlementReport);
                DateTime fromdate = (DateTime)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementFromDate);
                DateTime todate = (DateTime)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementToDate);
                var reportName = model.LicenseeInfor.LicenseeName + "_" + model.LicenseeInfor.ProductName + "_" + "Settlement_From_" + fromdate.ToString("dd-MMM-yy") + "_To_" + todate.ToString("dd-MMM-yy");
                var excelFile = new ExcelFile();
                var ewReport = excelFile.Worksheets.Add(string.Format("LicenseeSettlementReport"));
                var rowIndex = 0;
                #endregion

                if (model != null && model.Report.EntityList.Count > 0)
                {

                    //Billing info
                    ExportSportsBookReportToExcelAssignBillingInfo(model.LicenseeInfor, ref rowIndex, ref ewReport, product);
                    rowIndex += 2;
                    //report & exchange rate
                    ExportSportsBookReportToExcelAssignDetailAndExchangeRate(model.ExchangeList, model.Report, model.LicenseeInfor.LicenseeName, model.LicenseeInfor.SettlementDate,
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
        private void ExportSportsBookReportToExcelAssignBillingInfo(LicenseeInformation licenseeInfo,
            ref int rowIndex, ref ExcelWorksheet ewReport, string product)
        {
            ewReport.Cells[rowIndex, 0].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 0].Style.Font.Size = 230;
            ewReport.Rows[0].Height = 330;
            ewReport.Cells[rowIndex, 0].Value = Labels.INTERNAL_WORKSHEET_FOR_SETTLEMENT;
            ewReport.Cells[rowIndex += 1, 0].Value = Labels.Finance_Department;
            ewReport.Cells[rowIndex += 2, 0].Value = Labels.SETTLEMENT_REPORT;
            ewReport.Cells[rowIndex, 3].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 3].Value = licenseeInfo.SettlementDate.ToString("dd-MMM-yy");
            ewReport.Cells[rowIndex += 1, 0].Value = string.Format("{0}:", Labels.Licensee.ToUpper());
            ewReport.Cells[rowIndex, 2].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 2].Style.Font.Color = Color.FromArgb(0, 64, 255);
            ewReport.Cells[rowIndex, 2].Value = licenseeInfo.LicenseeName;
            ewReport.Cells[rowIndex, 3].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 3].Style.Font.Color = Color.FromArgb(194, 21, 21);
            ewReport.Cells[rowIndex, 3].Value = product.ToUpper();
            ewReport.Cells[rowIndex, 5].Value = string.Format("{0}:", Labels.FBilling);
            ewReport.Cells[rowIndex, 7].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 7].Value = licenseeInfo.FirstBilling.ToString("MMM-yy");
            ewReport.Cells[rowIndex += 1, 5].Value = string.Format("{0}:", Labels.Basis_of_Billing);
            ewReport.Cells[rowIndex, 7].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 7].Value = licenseeInfo.BasisOfBilling;
            ewReport.Cells[rowIndex += 2, 5].Value = string.Format("{0}:", Labels.Minimum_Fees);
            ewReport.Cells[rowIndex, 7].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 7].Value = licenseeInfo.MinimumFees;
            ewReport.Cells[rowIndex += 1, 5].Value = string.Format("{0}:", Labels.Head_of_Terms);
            ewReport.Cells[rowIndex, 7].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 7].Value = licenseeInfo.HeadOfTerms;
        }


        private void ExportSportsBookReportToExcelAssignDetailAndExchangeRate(List<ExchangeRate> exchangeRateInfo,
            Fanex.Ares.Report.ViewModels.LicenseeSettlement.Report reportDetails, string licenseeName, DateTime settlementDate,
            ref int rowIndex, ref ExcelWorksheet ewReport, string exchangeTo)
        {
            //(228, 242, 253)
            var headerColor = ExcelExtensions.HEADER_TITLE_COLOR;
            var normalStyle = ExcelExtensions.GetCellTextStyle(ExcelExtensions.BACKGROUND_COLOR, false);
            var numberStyle = ExcelExtensions.GetCellNumberStyle("", ExcelExtensions.BACKGROUND_COLOR, false, false, false);
            var decimalStyle = ExcelExtensions.GetCellNumberStyle(".00", ExcelExtensions.BACKGROUND_COLOR, false, false, true);

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

            #region win/loss & exchange rate
            #region header - winloss
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

            #region header - exchange rate
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 2, rowIndex, colIndex + 1).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex, colIndex + 1).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex, colIndex + 1).Value = string.Format("FOREX @ {0}",
                settlementDate.ToString("dd-MMM-yy"));

            ewReport.Cells[rowIndex += 1, colIndex].Style.FormatTitleCell(headerColor);
            ewReport.Cells[rowIndex, colIndex].Value = string.Format("{0}/1 Unit", exchangeTo);
            ewReport.Cells[rowIndex, colIndex + 1].Style.FormatTitleCell(headerColor);
            ewReport.Cells[rowIndex, colIndex + 1].Value = string.Format("Units/1 {0}", exchangeTo);
            #endregion

            rowIndex += 1;
            #region detail

            var itemIndex = 0;
            foreach (ReportEntity detail in reportDetails.EntityList)
            {
                colIndex = 0;
                #region set background color
                if (itemIndex % 2 > 0)
                {

                    normalStyle.FillPattern.SetSolid(ExcelExtensions.ODD_ROW_COLOR);
                    numberStyle.FillPattern.SetSolid(ExcelExtensions.ODD_ROW_COLOR);
                    decimalStyle.FillPattern.SetSolid(ExcelExtensions.ODD_ROW_COLOR);
                }
                else
                {
                    normalStyle.FillPattern.SetSolid(ExcelExtensions.EVEN_ROW_COLOR);
                    numberStyle.FillPattern.SetSolid(ExcelExtensions.EVEN_ROW_COLOR);
                    decimalStyle.FillPattern.SetSolid(ExcelExtensions.EVEN_ROW_COLOR);
                }
                #endregion

                #region winloss report
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
                ewReport.Cells[rowIndex, colIndex].Value = detail.CompanyBaseCurrency;
                #endregion

                #region exchange rate
                colIndex += 1;
                if (itemIndex < exchangeRateInfo.Count() && exchangeRateInfo[itemIndex] != null)
                {
                    var exchangeRate = exchangeRateInfo[itemIndex];
                    ewReport.Cells[rowIndex, colIndex].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    ewReport.Cells[rowIndex, colIndex].Value = exchangeRate.CurrencyName;
                    ewReport.Cells[rowIndex, colIndex + 1].Style = normalStyle;
                    ewReport.Cells[rowIndex, colIndex + 1].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    ewReport.Cells[rowIndex, colIndex + 1].Value = NumberExtensions.Format(exchangeRate.BasePerUnit, 5);

                    ewReport.Cells[rowIndex, colIndex + 2].Style = normalStyle;
                    ewReport.Cells[rowIndex, colIndex + 2].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    ewReport.Cells[rowIndex, colIndex + 2].Value = NumberExtensions.Format(exchangeRate.UnitsPerBase, 5);
                }
                #endregion

                rowIndex += 1;
                itemIndex++;
            }
            #region grand total
            colIndex -= 1;
            ewReport.Cells[rowIndex, colIndex].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, colIndex].Style.NumberFormat = "#,##0.00_);[RED](#,##0.00)";
            ewReport.Cells[rowIndex, colIndex].Value = reportDetails.GrandTotal.CompanyBaseCurrency;
            #endregion
            #endregion
            #endregion

            #region note *
            if (!exchangeTo.ToLower().Equals("cny"))
            {
                ewReport.Cells.GetSubrangeAbsolute(rowIndex += 2, 0, rowIndex, colIndex).Merged = true;
                ewReport.Cells.GetSubrangeAbsolute(rowIndex, 0, rowIndex, colIndex).SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Medium);
                ewReport.Cells.GetSubrangeAbsolute(rowIndex, 0, rowIndex, colIndex).Value = string.Format(Labels.IDR_VND_Amount_In_IDR, exchangeTo);
                ewReport.Rows[rowIndex].Height = 300;
                ewReport.Cells[rowIndex += 1, 0].Value = string.Format(Labels.FOREX_Info, exchangeTo);
            }

            #endregion
        }
        #endregion
        #endregion
    }
}