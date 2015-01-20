namespace Fanex.Ares.Report.Export.Excel
{
    using System.Globalization;
    using Fanex.Ares.Core.DataDomain;
    using Fanex.Ares.Core.Export;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Core.Resources;
    using Fanex.Ares.Core.Utils;
    using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Export Licensee Settlement Report for Sports book.
    /// </summary>
    public class BetAggregator : ExcelBase
    {
        #region Rows

        private const int TitleRow = 0;
        private const int ReportDateRow = 5;
        private const int LicenseeRow = 6;
        private const int TermsRow = 7;

        private const int FirstHeaderRow = 9;
        private const int SecondHeaderRow = 10;
        private const int OddContentRow = 11;
        private const int EvenContentRow = 12;
        private const int GrandTotalRow = 13;
        private const int NoteRow = 14;
        private const int NoInfoRow = 15;

        #endregion

        #region Information and Header Columns

        private const int ReportTitleCol = 0;
        private const int ReportDateCol = 3;
        private const int ReportLicenseeCol = 2;
        private const int ProductCol = 3;
        private const int FirstBillingCol = 7;
        private const int TermsCol = 7;

        private const int ForexTimeCol = 13;

        #endregion

        #region Report Columns

        private const int DateCol = 1;
        private const int CurrencyCol = 2;
        private const int TurnoverCol = 3;
        private const int TurnoverBaseCol = 4;
        private const int TotalTransCol = 5;
        private const int MemberWinLossCol = 6;
        private const int MemberWinLossBaseCol = 7;
        private const int BACommCol = 8;
        private const int BACommBaseCol = 9;
        private const int AmountDueToBACol = 10;
        private const int AmountDueToBABaseCol = 11;
        private const int CompanyBaseCurrencyCol = 12;
        private const int CurrencyPerUnitCol = 13;
        private const int UnitPerCurrencyCol = 14;

        #endregion

        #region Fields

        private readonly LicenseeSettlementModel _model;

        #endregion

        #region Constructors

        public BetAggregator()
        {
        }

        public BetAggregator(string template, LicenseeSettlementModel model)
        {
            TemplatePath = template;
            _model = model;
        }

        #endregion

        #region Assign Data

        /// <summary>
        /// Assign Header.
        /// </summary>
        /// <param name="sheet">Sheet parameter.</param>
        public override void AssignHeader(ISheet sheet)
        {
            for (int index = OddContentRow; index <= NoInfoRow; index++)
            {
                sheet.CreateRow(index);
            }

            var row = sheet.CopyRow(TemplateSheet, TitleRow, TitleRow);

            row.Cells[ReportTitleCol].SetCellValue(ReportTitle);

            AssignReportDate(sheet);

            AssignReportInformation(sheet);
        }

        /// <summary>
        /// Assign Sheet Data.
        /// </summary>
        /// <param name="sheet">Sheet parameter.</param>
        public override void AssignSheetData(ISheet sheet)
        {
            var rowIndex = OddContentRow;

            AssignHeaderTable(sheet);
            AssignContentTable(sheet, ref rowIndex);
            AssignGrandTotal(sheet, ref rowIndex);

            if (!_model.LicenseeInfor.BaseCurrencyName.Equals(BaseCurrencyName.CNY))
            {
                AssignNote(sheet, ref rowIndex);
            }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Assign Note.
        /// </summary>
        /// <param name="sheet">Sheet Template.</param>
        /// <param name="rowIndex">Row Index.</param>
        private void AssignNote(ISheet sheet, ref int rowIndex)
        {
            var row = sheet.CopyRow(TemplateSheet, NoteRow, rowIndex++);
            var firstNote = string.Format(CultureInfo.CurrentCulture, Labels.IDR_VND_Amount_In_IDR, _model.LicenseeInfor.BaseCurrencyName);
            var secondNote = string.Format(CultureInfo.CurrentCulture, Labels.FOREX_Info, _model.LicenseeInfor.BaseCurrencyName);

            row = sheet.CopyRow(TemplateSheet, NoteRow, rowIndex++);
            row.Height = 500;
            row.Cells[DateCol].SetCellValue(firstNote);
            sheet.MergeCells(rowIndex - 1, DateCol, rowIndex - 1, CompanyBaseCurrencyCol);

            row = sheet.CopyRow(TemplateSheet, NoteRow, rowIndex++);
            row.Height = 500;
            row.Cells[DateCol].SetCellValue(secondNote);
            sheet.MergeCells(rowIndex - 1, DateCol, rowIndex - 1, CompanyBaseCurrencyCol);
        }

        /// <summary>
        /// Assign Grand Total.
        /// </summary>
        /// <param name="sheet">Sheet Template.</param>
        /// <param name="rowIndex">Row Index.</param>
        private void AssignGrandTotal(ISheet sheet, ref int rowIndex)
        {
            var row = sheet.CopyRow(TemplateSheet, GrandTotalRow, rowIndex++);

            row.Height = 500;
            row.Cells[DateCol].SetCellValue(_model.Report.GrandTotal.TurnoverBase);
            sheet.MergeCells(rowIndex - 1, DateCol, rowIndex - 1, TurnoverBaseCol);

            row.Cells[TotalTransCol].SetCellValue(_model.Report.GrandTotal.MemberWinlossBase);
            sheet.MergeCells(rowIndex - 1, TotalTransCol, rowIndex - 1, MemberWinLossBaseCol);

            row.Cells[BACommCol].SetCellValue(_model.Report.GrandTotal.LicenseeWLBase);
            sheet.MergeCells(rowIndex - 1, BACommCol, rowIndex - 1, BACommBaseCol);

            row.Cells[AmountDueToBACol].SetCellValue(_model.Report.GrandTotal.CompanyBaseCurrency);
            sheet.MergeCells(rowIndex - 1, AmountDueToBACol, rowIndex - 1, AmountDueToBABaseCol);
        }

        /// <summary>
        /// Assign Content Table.
        /// </summary>
        /// <param name="sheet">Sheet Template.</param>
        /// <param name="rowIndex">Row Index.</param>
        private void AssignContentTable(ISheet sheet, ref int rowIndex)
        {
            for (var index = 0; index < _model.Report.EntityList.Count; index++)
            {
                var report = _model.Report.EntityList[index];
                var exchangeRate = _model.ExchangeList[index];
                var contentRow = index % 2 == 0 ? OddContentRow : EvenContentRow;

                var row = sheet.CopyRow(TemplateSheet, contentRow, rowIndex++);

                row.Height = 400;
                row.Cells[DateCol].SetCellValue(report.WinlostDate.ToAresDayMonthYear());
                row.Cells[CurrencyCol].SetCellValue(report.CurrencyName);
                row.Cells[TurnoverCol].SetCellValue(report.Turnover);
                row.Cells[TurnoverBaseCol].SetCellValue(report.TurnoverBase);
                row.Cells[TotalTransCol].SetCellValue(report.Betcount);
                row.Cells[MemberWinLossCol].SetCellValue(report.MemberWinloss);
                row.Cells[MemberWinLossBaseCol].SetCellValue(report.MemberWinlossBase);
                row.Cells[BACommCol].SetCellValue(report.LicenseeWL);
                row.Cells[BACommBaseCol].SetCellValue(report.LicenseeWLBase);
                row.Cells[AmountDueToBACol].SetCellValue(report.Company);
                row.Cells[AmountDueToBABaseCol].SetCellValue(report.CompanyBaseCurrency);
                row.Cells[CompanyBaseCurrencyCol].SetCellValue(exchangeRate.CurrencyName);
                row.Cells[CurrencyPerUnitCol].SetCellValue(exchangeRate.BasePerUnit);
                row.Cells[UnitPerCurrencyCol].SetCellValue(exchangeRate.UnitsPerBase);
            }
        }

        /// <summary>
        /// Assign Header Table.
        /// </summary>
        /// <param name="sheet">Sheet Template.</param>
        private void AssignHeaderTable(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, FirstHeaderRow, FirstHeaderRow);
            var forexTime = "Forex@ " + Utilities.GetForexTime(_model.LicenseeInfor.SettlementDate);
            var turnoverText = string.Format(CultureInfo.CurrentCulture, "Turnover ({0})", _model.LicenseeInfor.BaseCurrencyName);
            var customerWinLossText = string.Format(CultureInfo.CurrentCulture, "   Customer    Win Loss ({0})", _model.LicenseeInfor.BaseCurrencyName);
            var betAggegatorText = string.Format(CultureInfo.CurrentCulture, "BA Comm ({0})", _model.LicenseeInfor.BaseCurrencyName);

            row.Cells[TurnoverBaseCol].SetCellValue(turnoverText);
            row.Cells[MemberWinLossBaseCol].SetCellValue(customerWinLossText);
            row.Cells[BACommBaseCol].SetCellValue(betAggegatorText);
            row.Cells[ForexTimeCol].SetCellValue(forexTime);

            row = sheet.CopyRow(TemplateSheet, SecondHeaderRow, SecondHeaderRow);

            row.Cells[CurrencyPerUnitCol].SetCellValue(_model.LicenseeInfor.BaseCurrencyName + "/1 Unit");
            row.Cells[UnitPerCurrencyCol].SetCellValue("Units/1 " + _model.LicenseeInfor.BaseCurrencyName);
        }

        /// <summary>
        /// Assign Report Information.
        /// </summary>
        /// <param name="sheet">Sheet parameter.</param>
        private void AssignReportInformation(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, LicenseeRow, LicenseeRow);

            row.Cells[ReportLicenseeCol].SetCellValue(_model.LicenseeInfor.LicenseeName);
            row.Cells[ProductCol].SetCellValue(_model.LicenseeInfor.ProductName.ToUpper());
            row.Cells[FirstBillingCol].SetCellValue(_model.LicenseeInfor.FirstBilling.ToAresMonthYear());

            row = sheet.CopyRow(TemplateSheet, TermsRow, TermsRow);
            row.Cells[TermsCol].SetCellValue(_model.LicenseeInfor.HeadOfTerms);
        }

        /// <summary>
        /// Assign Report Date.
        /// </summary>
        /// <param name="sheet">Sheet parameter.</param>
        private void AssignReportDate(ISheet sheet)
        {
            var reportDate = string.Format(
                CultureInfo.CurrentCulture,
                "From {0} to {1}",
                _model.LicenseeInfor.FromDate.ToAresDayMonthYear(),
                _model.LicenseeInfor.ToDate.ToAresDayMonthYear());

            var row = sheet.CopyRow(TemplateSheet, ReportDateRow, ReportDateRow);

            row.Cells[ReportDateCol].SetCellValue(reportDate);
        }

        #endregion
    }
}
