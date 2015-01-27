namespace Fanex.Ares.Core.Export
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// NPOI Result.
    /// </summary>
    public class NPOIResult : ActionResult
    {
        private const string ExcelResponseType = "application/vnd.ms-excel";
        private const string ExcelFileType = ".xls";
        private readonly HSSFWorkbook _file;
        private readonly string _fileName;

        public NPOIResult(HSSFWorkbook hssfWorkBook, string fileName)
        {
            this._file = hssfWorkBook;
            this._fileName = fileName;
        }

        protected string ContentType
        {
            get { return ExcelResponseType; }
        }

        protected string Extension
        {
            get { return ExcelFileType; }
        }

        protected Action<HSSFWorkbook, Stream> SaveAction
        {
            get { return (wb, s) => wb.Write(s); }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = this.ContentType;
            context.HttpContext.Response.AddHeader("Content-disposition", "filename=" + this._fileName + this.Extension);

            this.SaveAction(this._file, context.HttpContext.Response.OutputStream);
        }
    }
}
