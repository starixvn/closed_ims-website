using System.Collections.Generic;
using System.IO;
using System.Linq;
using Fanex.Workbooks.FanexWorkbook;

namespace Fanex.Workbooks
{
    /// <summary>
    /// Excel, Word, PDF... extension
    /// </summary>
    public class WorkbookManager
    {
        public enum DocumentType
        {
            Xlsx, Pdf
        }

        public enum PageOrientation
        {
            Landscape, Portrait
        }

        public MemoryStream CreateDocument(List<string> htmlContent, List<string> listCssFilePath, DocumentType format, List<PageOrientation> pageOrientation, List<bool> IsAutoFitList)
        {
            //var css = LoadCss(listCssFilePath);
            var wbService = new FanexWorkbook.WorkBookServiceClient();

            List<DocumentSetting> pageList = new List<DocumentSetting>();
            for (int i = 0; i < pageOrientation.Count; i++)
            {
                DocumentSetting page = new DocumentSetting();
                page.HtmlContent = htmlContent[i];

                page.CssContent = LoadCss(listCssFilePath[i]);
                page.PageOrientation = pageOrientation[i].Equals(PageOrientation.Landscape)
                                                  ? WorkBookManagerPageOrientation.Landscape
                                                  : WorkBookManagerPageOrientation.Portrait;
                page.DocumentType = format.Equals(DocumentType.Pdf)
                                               ? WorkBookManagerDocumentType.Pdf
                                               : WorkBookManagerDocumentType.Xlsx;
                page.IsAutoFit = IsAutoFitList[i];
                pageList.Add(page);
            }

            var ms = wbService.CreateDocument(pageList);
            if (ms == null)
                return null;

            return new MemoryStream(ms);
        }

        private static string LoadCss(string cssFilePath)
        {
            string css = string.Empty;
            if (cssFilePath == null) { return string.Empty; }

                var streamCss = new StreamReader(cssFilePath);
                css += streamCss.ReadToEnd();
                streamCss.Close();

            return css;
        }
    }
}
