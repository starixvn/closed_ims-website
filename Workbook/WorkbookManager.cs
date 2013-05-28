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

        public MemoryStream CreateDocument(List<string> htmlContent, List<string> listCssFilePath, DocumentType format, List<PageOrientation> pageOrientation)
        {

            var css = LoadCss(listCssFilePath);
            var wbService = new FanexWorkbook.WorkBookServiceClient();
            //var test = wbService.GetData(1);

            var documentSetting = new DocumentSetting();
            documentSetting.CssContent = css;
           
            documentSetting.DocumentType = format.Equals(DocumentType.Pdf)
                                               ? WorkBookManagerDocumentType.Pdf
                                               : WorkBookManagerDocumentType.Xlsx;

            List<PageSetting> PageList = new List<PageSetting>();
            for (int i = 0; i < pageOrientation.Count; i++)
            {
                PageSetting page = new PageSetting();
                page.HtmlContent = htmlContent[i];

                page.PageOrientation = pageOrientation[i].Equals(PageOrientation.Landscape)
                                                  ? WorkBookManagerPageOrientation.Landscape
                                                  : WorkBookManagerPageOrientation.Portrait;
                PageList.Add(page);
            }

            documentSetting.PageList = PageList;
            //documentSetting.PageOrientation = pageOrientation.Equals(PageOrientation.Landscape)
            //                                      ? WorkBookManagerPageOrientation.Landscape
            //                                      : WorkBookManagerPageOrientation.Portrait;

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
