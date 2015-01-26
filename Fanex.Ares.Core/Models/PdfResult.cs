namespace Fanex.Ares.Core.Models
{
    using System.IO;
    using System.Web.Mvc;

    /// <summary>
    /// PDF Result.
    /// </summary>
    public class PdfResult : ActionResult
    {
        private readonly MemoryStream _stream;
        private readonly string _fileName;

        public PdfResult(MemoryStream stream, string fileName)
        {
            this._stream = stream;
            this._fileName = fileName;
        }

        protected string ContentType
        {
            get
            {
                return "application/pdf";
            }
        }

        protected string Extension
        {
            get
            {
                return ".pdf";
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.ContentType = this.ContentType;
            context.HttpContext.Response.AddHeader("Content-disposition", "filename=" + this._fileName + this.Extension);
            byte[] array = this._stream.ToArray();
            context.HttpContext.Response.OutputStream.Write(array, 0, array.Length);
        }
    }
}
