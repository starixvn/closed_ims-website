namespace Fanex.Artist.Export
{
    using System.IO;
    using Fanex.Artist.Export.Extension;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Excel Base.
    /// </summary>
    public abstract class ExcelBase
    {
        #region Property

        private const string SheetName = "Sheet1";

        private const string DataName = "Data";

        public string TemplatePath { get; set; }

        public string ReportTitle { get; set; }

        public IWorkbook Workbook { get; set; }

        public ISheet TemplateSheet { get; set; }

        #endregion Property

        /// <summary>
        /// Load template.
        /// </summary>
        public virtual void LoadTemplate()
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(TemplatePath, FileMode.Open, FileAccess.Read);
                Workbook = new HSSFWorkbook(stream);
                TemplateSheet = Workbook.GetSheet(SheetName);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public void Export()
        {
            LoadTemplate();

            AssignData();

            RemoveTemplate();
        }

        public abstract void AssignHeader(ISheet sheet);

        public abstract void AssignSheetData(ISheet sheet);

        protected virtual void RemoveTemplate()
        {
            Workbook.RemoveSheet(SheetName);
        }

        protected virtual void AssignData()
        {
            var sheet = Workbook.CloneSheet(SheetName, DataName);

            AssignHeader(sheet);

            AssignSheetData(sheet);
        }
    }
}