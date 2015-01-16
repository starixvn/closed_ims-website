namespace Fanex.Artist.Export.Report
{
    using NPOI.SS.UserModel;

    /// <summary>
    /// Export Licensee Settlement Report for Sports book.
    /// </summary>
    public class Sportsbook : ExcelBase
    {
        #region Row

        #endregion

        #region Information Columns

        #endregion

        #region Report Columns

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
