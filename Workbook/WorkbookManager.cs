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

        public MemoryStream CreateDocument(List<string> htmlContent, List<string> listCssFilePath, DocumentType format, List<PageOrientation> pageOrientation, List<bool> IsAutoFitList, string sheetName)
        {
            //var css = LoadCss(listCssFilePath);
            var wbService = new FanexWorkbook.WorkBookServiceClient();

            List<PageSetting> pageList = new List<PageSetting>();

            PageSetting mainPage = new PageSetting();
            mainPage.HtmlContent = htmlContent[0];

            mainPage.CssContent = LoadCss(listCssFilePath[0]);
            mainPage.PageOrientation = pageOrientation[0].Equals(PageOrientation.Landscape)
                                              ? WorkBookManagerPageOrientation.Landscape
                                              : WorkBookManagerPageOrientation.Portrait;

            mainPage.IsAutoFit = IsAutoFitList[0];

            for (int i = 1; i < pageOrientation.Count; i++)
            {
                PageSetting page = new PageSetting();
                page.HtmlContent = htmlContent[i];

                page.CssContent = LoadCss(listCssFilePath[i]);
                page.PageOrientation = pageOrientation[i].Equals(PageOrientation.Landscape)
                                                  ? WorkBookManagerPageOrientation.Landscape
                                                  : WorkBookManagerPageOrientation.Portrait;

                page.IsAutoFit = IsAutoFitList[i];

                pageList.Add(page);
            }

            DocumentFile documentFile = new DocumentFile();
            documentFile.DocumentType = format.Equals(DocumentType.Pdf)
                                            ? WorkBookManagerDocumentType.Pdf
                                            : WorkBookManagerDocumentType.Xlsx;
            documentFile.FileName = sheetName;

            documentFile.PageDetailList = pageList;

            documentFile.MainPage = mainPage;

            var ms = wbService.CreateDocument(documentFile);
            if (ms == null)
                return null;

            return new MemoryStream(ms);
        }

        private static string LoadCss(string cssFilePath)
        {
            string css = string.Empty;
            if (cssFilePath == null)
            {
                return string.Empty;
            }

            var streamCss = new StreamReader(cssFilePath);
            css += streamCss.ReadToEnd();
            streamCss.Close();

            return css;
        }
    }
}
