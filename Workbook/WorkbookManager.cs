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

            //var css = LoadCss(listCssFilePath);
            var wbService = new FanexWorkbook.WorkBookServiceClient();
            ////var test = wbService.GetData(1);

            //var documentSetting = new DocumentSetting();
            //documentSetting.CssContent = css;
           
            //documentSetting.DocumentType = format.Equals(DocumentType.Pdf)
            //                                   ? WorkBookManagerDocumentType.Pdf
            //                                   : WorkBookManagerDocumentType.Xlsx;

            List<DocumentSetting> sheetList = new List<DocumentSetting>();
            for (int i = 0; i < pageOrientation.Count; i++)
            {
                DocumentSetting sheet = new DocumentSetting();
                sheet.HtmlContent = htmlContent[i];

                sheet.CssContent = LoadCss(listCssFilePath[i]);
                sheet.PageOrientation = pageOrientation[i].Equals(PageOrientation.Landscape)
                                                  ? WorkBookManagerPageOrientation.Landscape
                                                  : WorkBookManagerPageOrientation.Portrait;
                sheet.DocumentType = format.Equals(DocumentType.Pdf)
                                               ? WorkBookManagerDocumentType.Pdf
                                               : WorkBookManagerDocumentType.Xlsx;
                sheetList.Add(sheet);
            }

            //documentSetting.PageList = sheetList;
            //documentSetting.PageOrientation = pageOrientation.Equals(PageOrientation.Landscape)
            //                                      ? WorkBookManagerPageOrientation.Landscape
            //                                      : WorkBookManagerPageOrientation.Portrait;

            var ms = wbService.CreateDocument(sheetList);
            if (ms == null)
                return null;

            return new MemoryStream(ms);

        }

        private static string LoadCss(string cssFilePath)
        {
            string css = string.Empty;
            if (cssFilePath == null) return string.Empty;

           
                var streamCss = new StreamReader(cssFilePath);
                css += streamCss.ReadToEnd();
                streamCss.Close();


            return css;
        }

        //private static string LoadCss(List<string> listCssFilePath)
        //{
        //    string css = string.Empty;
        //    if (listCssFilePath == null || !listCssFilePath.Any()) return string.Empty;

        //    foreach (var cssFilePath in listCssFilePath)
        //    {
        //        var streamCss = new StreamReader(cssFilePath);
        //        css += streamCss.ReadToEnd();

        //        streamCss.Close();
        //    }


        //    return css;
        //}

        
    }
}
