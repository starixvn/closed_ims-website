// -----------------------------------------------------------------------
// <copyright file="PrintHeaderFooter.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Core.PdfHelper
{
    using System;
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    /// <summary>
    /// This class represents the standard header and footer for a PDF print.
    /// application.
    /// </summary>
    public class PrintHeaderFooter : PdfPageEventHelper
    {
        private PdfContentByte _pdfContent;
        private PdfTemplate _pageNumberTemplate;
        private BaseFont _baseFont;
        private DateTime _printTime;

        public string Title { get; set; }

        /// <summary>
        /// Open document.
        /// </summary>
        /// <param name="writer">Writer parameter.</param>
        /// <param name="document">Document parameter.</param>
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            _printTime = DateTime.Now;
            _baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            _pdfContent = writer.DirectContent;
            _pageNumberTemplate = _pdfContent.CreateTemplate(50, 50);
        }

        /// <summary>
        /// Start page.
        /// </summary>
        /// <param name="writer">Writer parameter.</param>
        /// <param name="document">Document parameter.</param>
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);

            Rectangle pageSize = document.PageSize;
            if (Title != string.Empty)
            {
                _pdfContent.BeginText();
                _pdfContent.SetFontAndSize(_baseFont, 11);
                _pdfContent.SetRGBColorFill(0, 0, 0);
                _pdfContent.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetTop(40));
                _pdfContent.ShowText(Title);
                _pdfContent.EndText();
            }
        }

        /// <summary>
        /// On end page.
        /// </summary>
        /// <param name="writer">Writer parameter.</param>
        /// <param name="document">Document parameter.</param>
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            int pageN = writer.PageNumber;
            string text = pageN + " - ";
            float len = _baseFont.GetWidthPoint(text, 8);

            Rectangle pageSize = document.PageSize;
            _pdfContent = writer.DirectContent;
            _pdfContent.SetRGBColorFill(100, 100, 100);
        }

        /// <summary>
        /// Close document.
        /// </summary>
        /// <param name="writer">Writer parameter.</param>
        /// <param name="document">Document parameter.</param>
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            _pageNumberTemplate.BeginText();
            _pageNumberTemplate.SetFontAndSize(_baseFont, 8);
            _pageNumberTemplate.SetTextMatrix(0, 0);
            _pageNumberTemplate.ShowText(string.Empty + (writer.PageNumber - 1));
            _pageNumberTemplate.EndText();
        }
    }
}
