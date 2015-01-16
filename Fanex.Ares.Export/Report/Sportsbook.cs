namespace Fanex.Artist.Export.Report
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// Export Licensee Settlement Report for Sports book.
    /// </summary>
    public class Sportsbook : ExcelBase
    {
        #region Rows

        private const int TitleRow = 0;
        private const int ReportDateRow = 5;
        private const int LicenseeRow = 6;
        private const int BillingRow = 7;
        private const int MenimumFeeRow = 9;
        private const int TermsRow = 10;

        private const int FirstHeaderRow = 12;
        private const int ContentRow = 14;
        private const int GrossRow = 15;
        private const int NoteRow = 17;

        #endregion

        #region Information and Header Columns

        private const int ReportDateCol = 2;
        private const int ReportLicenseeCol = 2;
        private const int ProductCol = 3;
        private const int FirstBillingCol = 6;
        private const int BasisBillingCol = 6;
        private const int MinimumFeesCol = 6;
        private const int TermsCol = 6;

        private const int DataLicenseeCol = 6;
        private const int ForexCol = 10;

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

        #region Constructors

        public Sportsbook()
        { 
        }

        public Sportsbook(string template)
        {
            TemplatePath = template;
        }

        #endregion

        #region Assign Data

        #endregion

        public override void AssignHeader(ISheet sheet)
        {
        }

        public override void AssignSheetData(ISheet sheet)
        {
        }
    }
}
