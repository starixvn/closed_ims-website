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
    public class BAExcelService : ServiceBase, IBAExcelService
    {
        #region Export Excel
        public void ExportReportToExcel(string exchange, string product)
        {
            #region local varialbles

            var model = new LicenseeSettlementModel();
            model = (LicenseeSettlementModel)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementReport);
            int month = (int)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementMonth);
            int year = (int)SessionExtensions.GetSession(System.Web.HttpContext.Current.Session, CacheServiceKey.SessionKeyLicenseeSettlementYear);
            var reportName = model.LicenseeInfor.LicenseeName + "_" + model.LicenseeInfor.ProductName + "_" + "Settlement_For_" + month + "-" + year;
            var excelFile = new ExcelFile();
            var ewReport = excelFile.Worksheets.Add(string.Format("LicenseeSettlementReport"));
            var rowIndex = 0;
            #endregion

            if (model != null && model.Report.EntityList.Count > 0)
            {

                //Billing info
                ExportReportToExcelAssignBillingInfo(model.LicenseeInfor, ref rowIndex, ref ewReport, product);
                rowIndex += 2;
                //report & exchange rate
                ExportReportToExcelAssignDetailAndExchangeRate(model.ExchangeList, model.Report, model.LicenseeInfor.LicenseeName, model.LicenseeInfor.SettlementDate,
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

        #region private functions
        private void ExportReportToExcelAssignBillingInfo(LicenseeInformation licenseeInfo,
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
            ewReport.Cells[rowIndex, 6].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 6].Value = licenseeInfo.FirstBilling.ToString("MMM-yy");
            ewReport.Cells[rowIndex += 1, 5].Value = string.Format("{0}:", Labels.Head_of_Terms);
            ewReport.Cells[rowIndex, 6].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 6].Value = licenseeInfo.HeadOfTerms;
        }


        private void ExportReportToExcelAssignDetailAndExchangeRate(List<ExchangeRate> exchangeRateInfo,
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
            ewReport.Columns[0].Width = 255 * 10;   // Date
            ewReport.Columns[1].Width = 255 * 9;    // Currency
            ewReport.Columns[2].Width = 255 * 15;   // Turnover
            ewReport.Columns[3].Width = 255 * 15;   // Turnover(EUR)
            ewReport.Columns[4].Width = 255 * 12;   // Total Trans
            ewReport.Columns[5].Width = 255 * 15;   // Customer Win Loss
            ewReport.Columns[6].Width = 255 * 15;   // Customer Win Loss(EUR)
            ewReport.Columns[7].Width = 255 * 10;   // BA Comm
            ewReport.Columns[8].Width = 255 * 10;   // BA Comm (EUR)
            ewReport.Columns[9].Width = 255 * 12;   // BA
            ewReport.Columns[10].Width = 255 * 12;  // BA Equi EUR
            ewReport.Columns[11].Width = 255 * 15;  // Forex Currency Name
            ewReport.Columns[12].Width = 255 * 12;  // EUR / 1 Unit
            ewReport.Columns[13].Width = 255 * 12;  // Units / 1 EUR
            #endregion

            #region win/loss & exchange rate
            #region header - winloss
            // Date
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = Labels.Date;

            // Currency
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = Labels.Currency;

            // Turnover
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = Labels.Turnover;

            // Turnover(EUR)
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = Labels.Turnover + " (" + exchangeTo + ")";

            // Total Trans
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = Labels.TotalTrans;

            // Customer Win Loss
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = Labels.CustomerWinLoss;

            // Customer Win Loss (EUR)
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = Labels.CustomerWinLoss + " (" + exchangeTo + ")";

            // BA Comm
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = Labels.BAComm;

            // BA Comm (EUR)
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex += 1, rowIndex + 1, colIndex).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex, rowIndex + 1, colIndex).Value = Labels.BAComm + " (" + exchangeTo + ")";

            // Amount Due (to/from)
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex + 1, rowIndex, colIndex + 2).Merged = true;
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex + 1, rowIndex, colIndex + 2).Style.FormatTitleCell(headerColor);
            ewReport.Cells.GetSubrangeAbsolute(rowIndex, colIndex + 1, rowIndex, colIndex + 2).Value = Labels.AmountDue;

            // BA
            ewReport.Cells[rowIndex + 1, colIndex += 1].Style.FormatTitleCell(headerColor);
            ewReport.Cells[rowIndex + 1, colIndex].Value = Labels.BA;

            // BA Equi EUR
            ewReport.Cells[rowIndex + 1, colIndex += 1].Style.FormatTitleCell(headerColor);
            ewReport.Cells[rowIndex + 1, colIndex].Value = Labels.BAEqui + " " + exchangeTo;

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
                // Date
                ewReport.Cells[rowIndex, colIndex].Style = normalStyle;
                ewReport.Cells[rowIndex, colIndex].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                ewReport.Cells[rowIndex, colIndex].Value = detail.WinlostDate.ToString("dd-MMM-yy");

                // Currency
                ewReport.Cells[rowIndex, colIndex += 1].Style = normalStyle;
                ewReport.Cells[rowIndex, colIndex].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                ewReport.Cells[rowIndex, colIndex].Value = detail.CurrencyName;

                // Turnover
                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                ewReport.Cells[rowIndex, colIndex].Value = detail.Turnover;

                // Turnover(EUR)
                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                ewReport.Cells[rowIndex, colIndex].Value = detail.TurnoverBase;

                // Total Trans
                ewReport.Cells[rowIndex, colIndex += 1].Style = numberStyle;
                ewReport.Cells[rowIndex, colIndex].Value = detail.Betcount;

                // Customer Win Loss
                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                ewReport.Cells[rowIndex, colIndex].Value = detail.MemberWinloss;

                // Customer Win Loss(EUR)
                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                if (detail.MemberWinlossBase != 0)
                {
                    ewReport.Cells[rowIndex, colIndex].Value = detail.MemberWinlossBase;
                }
                else
                {
                    ewReport.Cells[rowIndex, colIndex].Value = "-";
                }

                // BA Comm
                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                ewReport.Cells[rowIndex, colIndex].Value = detail.LicenseeWL;

                // BA Comm (EUR)
                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                if (detail.LicenseeWLBase != 0)
                {
                    ewReport.Cells[rowIndex, colIndex].Value = detail.LicenseeWLBase;
                }
                else
                {
                    ewReport.Cells[rowIndex, colIndex].Value = "-";
                }

                // BA
                ewReport.Cells[rowIndex, colIndex += 1].Style = decimalStyle;
                ewReport.Cells[rowIndex, colIndex].Value = detail.Company;

                // BA Equi EUR
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
            // Turnover (EUR)
            ewReport.Cells[rowIndex, 3].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 3].Style.NumberFormat = "#,##0.00_);[RED](#,##0.00)";
            ewReport.Cells[rowIndex, 3].Value = reportDetails.GrandTotal.TurnoverBase;

            // Customer Win Loss (EUR)
            ewReport.Cells[rowIndex, 6].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 6].Style.NumberFormat = "#,##0.00_);[RED](#,##0.00)";
            ewReport.Cells[rowIndex, 6].Value = reportDetails.GrandTotal.MemberWinlossBase;

            // BA Comm (EUR)
            ewReport.Cells[rowIndex, 8].Style.Font.Weight = ExcelFont.BoldWeight;
            ewReport.Cells[rowIndex, 8].Style.NumberFormat = "#,##0.00_);[RED](#,##0.00)";
            ewReport.Cells[rowIndex, 8].Value = reportDetails.GrandTotal.LicenseeWLBase;

            // BA Equi EUR
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