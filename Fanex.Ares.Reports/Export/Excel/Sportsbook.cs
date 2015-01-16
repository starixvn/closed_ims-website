namespace Fanex.Ares.Report.Export.Excel
{
    using System.Globalization;
    using Fanex.Ares.Core.Export;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Report.ViewModels.LicenseeSettlement;
    using NPOI.SS.UserModel;
    using Fanex.Ares.Core.Utils;

    /// <summary>
    /// Export Licensee Settlement Report for Sports book.
    /// </summary>
    public class Sportsbook : ExcelBase
    {
        #region Rows

        private const int TitleRow = 0;
        private const int ReportDateRow = 5;
        private const int LicenseeRow = 6;
        private const int BasisBillingRow = 7;
        private const int MinimumFeeRow = 9;
        private const int TermsRow = 10;

        private const int FirstHeaderRow = 12;
        private const int ContentRow = 14;
        private const int GrossRow = 15;
        private const int NoteRow = 17;

        #endregion

        #region Information and Header Columns

        private const int ReportTitleCol = 0;
        private const int ReportDateCol = 3;
        private const int ReportLicenseeCol = 2;
        private const int ProductCol = 3;
        private const int FirstBillingCol = 7;
        private const int BasisBillingCol = 7;
        private const int MinimumFeesCol = 7;
        private const int TermsCol = 7;

        private const int DataLicenseeCol = 8;
        private const int ForexTimeCol = 14;

        #endregion

        #region Report Columns

        private const int DateCol = 0;
        private const int CurrencyCol = 1;
        private const int TurnoverCol = 2;
        private const int TotalTransCol = 3;
        private const int MemberWinLossCol = 4;
        private const int MemberCommCol = 5;
        private const int MemberTotalCol = 6;
        private const int LicenseeWinLossCol = 7;
        private const int LicenseeCommCol = 8;
        private const int LicenseeTotalCol = 9;
        private const int CompanyCol = 10;
        private const int EurCol = 11;
        private const int BaseCurrencyCol = 12;
        private const int EurPerUnitCol = 13;
        private const int UnitsPerEurCol = 14;
        private const int GrossCol = 1;
        private const int FirstNoteCol = 1;
        private const int SecondNoteCol = 2;

        #endregion

        #region Fields

        private readonly string BasisOfBilling = "15% of Net Win";
        private readonly LicenseeSettlementModel _model;
        private readonly string _header;

        #endregion

        #region Constructors

        public Sportsbook()
        {
        }

        public Sportsbook(string header, string template, LicenseeSettlementModel model)
        {
            _header = header;
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
            for (int index = ContentRow; index <= ContentRow; index++)
            {
                sheet.CreateRow(index);
            }

            var row = sheet.CopyRow(TemplateSheet, TitleRow, TitleRow);

            row.Cells[ReportTitleCol].SetCellValue(_header);

            AssignReportDate(sheet);

            AssignReportInformation(sheet);
        }

        public override void AssignSheetData(ISheet sheet)
        {
            AssignHeaderTable(sheet);
        }

        #endregion

        #region Utilities

        private void AssignHeaderTable(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, FirstHeaderRow, FirstHeaderRow);
            var forexTime = "Forex@ " + Utilities.GetForexTime(_model.LicenseeInfor.SettlementDate);

            row.Cells[DataLicenseeCol].SetCellValue(_model.LicenseeInfor.LicenseeName);
            row.Cells[ForexTimeCol].SetCellValue(forexTime);
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

            row = sheet.CopyRow(TemplateSheet, BasisBillingRow, BasisBillingRow);
            row.Cells[BasisBillingCol].SetCellValue(BasisOfBilling);

            row = sheet.CopyRow(TemplateSheet, MinimumFeeRow, MinimumFeeRow);
            row.Cells[MinimumFeesCol].SetCellValue(_model.LicenseeInfor.MinimumFees);

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
