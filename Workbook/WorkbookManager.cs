// --------------------------------------------------------------------------
// <copyright file="WorkbookManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Workbooks
{
    using System.Collections.Generic;
    using System.IO;
    using Fanex.Workbooks.FanexWorkbook;

    /// <summary>
    /// Excel, Word, PDF... extension.
    /// </summary>
    public class WorkbookManager
    {
        public enum DocumentType
        {
            /// <summary>
            /// The XLSX format.
            /// </summary>
            Xlsx,

            /// <summary>
            /// The PDF format.
            /// </summary>
            Pdf
        }

        public enum PageOrientation
        {
            /// <summary>
            /// The landscape.
            /// </summary>
            Landscape,

            /// <summary>
            /// The portrait.
            /// </summary>
            Portrait
        }

        /// <summary>
        /// Creates the document.
        /// </summary>
        /// <param name="htmlContent">Content of the HTML.</param>
        /// <param name="listCssFilePath">The list CSS file path.</param>
        /// <param name="format">The format.</param>
        /// <param name="pageOrientation">The page orientation.</param>
        /// <param name="isAutoFitList">The is automatic fit list.</param>
        /// <returns>Memory Stream.</returns>
        public MemoryStream CreateDocument(
            List<string> htmlContent, 
            List<string> listCssFilePath, 
            DocumentType format, 
            List<PageOrientation> pageOrientation, 
            List<bool> isAutoFitList)
        {
            var workbookService = new FanexWorkbook.WorkBookServiceClient();
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
                page.IsAutoFit = isAutoFitList[i];
                pageList.Add(page);
            }

            var ms = workbookService.CreateDocument(pageList);
            if (ms == null)
            {
                return null;
            }   

            return new MemoryStream(ms);
        }

        /// <summary>
        /// Loads the CSS.
        /// </summary>
        /// <param name="cssFilePath">The CSS file path.</param>
        /// <returns>System String.</returns>
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