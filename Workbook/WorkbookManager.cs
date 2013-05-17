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


        public MemoryStream CreateDocument(string htmlContent, List<string> listCssFilePath, DocumentType format, PageOrientation pageOrientation)
        {

            var css = LoadCss(listCssFilePath);
            var wbService = new FanexWorkbook.WorkBookServiceClient();
            //var test = wbService.GetData(1);

            var documentSetting = new DocumentSetting();
            documentSetting.CssContent = css;
            documentSetting.HtmlContent = htmlContent;
            documentSetting.DocumentType = format.Equals(DocumentType.Pdf)
                                               ? WorkBookManagerDocumentType.Pdf
                                               : WorkBookManagerDocumentType.Xlsx;

            documentSetting.PageOrientation = pageOrientation.Equals(PageOrientation.Landscape)
                                                  ? WorkBookManagerPageOrientation.Landscape
                                                  : WorkBookManagerPageOrientation.Portrait;

            var ms = wbService.CreateDocument(documentSetting);
            if (ms == null)
                return null;

            return new MemoryStream(ms);
            
            


        }


        private static string LoadCss(List<string> listCssFilePath)
        {
            string css = string.Empty;
            if (listCssFilePath == null || !listCssFilePath.Any()) return string.Empty;

            foreach (var cssFilePath in listCssFilePath)
            {
                var streamCss = new StreamReader(cssFilePath);
                css += streamCss.ReadToEnd();

                streamCss.Close();
            }


            return css;
        }

        
    }
}
