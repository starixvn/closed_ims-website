// -----------------------------------------------------------------------
// <copyright file="StandardPdfRenderer.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.Web.HtmlHelper;

namespace PdfHelper
{
    using System.IO;

    using iTextSharp.text;
    using iTextSharp.text.html.simpleparser;
    using iTextSharp.text.pdf;
    using System.Collections.Generic;
    using System;
    using iTextSharp.text.html;
    
    using System.Web;

    /// <summary>
    /// This class is responsible for rendering a html text string to a PDF document using the html renderer of iTextSharp.
    /// </summary>
    public class StandardPdfRenderer
    {
        private const int HorizontalMargin = 20;
        private const int VerticalMargin = 20;

        public byte[] Render(string htmlText, string pageTitle, int numericRows)
        {
            byte[] renderedBuffer;



            using (var outputMemoryStream = new MemoryStream())
            {
                //using (var pdfDocument = new Document(PageSize.A4, HorizontalMargin, HorizontalMargin, VerticalMargin, VerticalMargin))
                using (var pdfDocument = new Document(PageSize.A4.Rotate(), HorizontalMargin, HorizontalMargin, VerticalMargin, VerticalMargin))
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, outputMemoryStream);
                    pdfWriter.CloseStream = false;
                    pdfWriter.PageEvent = new PrintHeaderFooter { Title = pageTitle };
                    pdfDocument.Open();

                    var styles = LoadStyleSheet();


                    PdfContentByte cb = pdfWriter.DirectContent;
                    List<IElement> htmlElements = HTMLWorker.ParseToList(new StringReader(htmlText), styles);

                    for (int i = 0; i < htmlElements.Count; i++)
                    {
                        // cast the element
                        IElement htmlElement = ((IElement)htmlElements[i]);

                        pdfDocument.Add(htmlElement);
                    }
                }
                renderedBuffer = new byte[outputMemoryStream.Position];
                outputMemoryStream.Position = 0;
                outputMemoryStream.Read(renderedBuffer, 0, renderedBuffer.Length);
            }

            return renderedBuffer;
        }

        private void RotatePDF(string inputFile, string outputFile)
        {
            using (FileStream outStream = new FileStream(outputFile, FileMode.Create))
            {
                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(inputFile);
                iTextSharp.text.pdf.PdfStamper stamper = new iTextSharp.text.pdf.PdfStamper(reader, outStream);

                iTextSharp.text.pdf.PdfDictionary pageDict = reader.GetPageN(1);
                int desiredRot = 90; // 90 degrees clockwise from what it is now
                iTextSharp.text.pdf.PdfNumber rotation = pageDict.GetAsNumber(iTextSharp.text.pdf.PdfName.ROTATE);

                if (rotation != null)
                {
                    desiredRot += rotation.IntValue;
                    desiredRot %= 360; // must be 0, 90, 180, or 270
                }
                pageDict.Put(iTextSharp.text.pdf.PdfName.ROTATE, new iTextSharp.text.pdf.PdfNumber(desiredRot));

                stamper.Close();
            }
        }


        private static StyleSheet LoadStyleSheet()
        {
            var styles = new StyleSheet();

            var stylePath = HttpContext.Current.Server.MapPath(@"~\Areas\Report\Views\LicenseeSettlement\Styles\LicenseeSettlementPDF.css");

            ICssParser cssParser = new CssParser(stylePath);

            var test = cssParser.GetProperties(".lineHeight200");



            var lsClassName = cssParser.GetListClassName();
            foreach (var data in lsClassName)
            {
                var className = data;
                var properties = cssParser.GetProperties(className);

                if (className.StartsWith(".") || className.StartsWith("#"))
                    className = className.Substring(1, className.Length - 1);

                foreach (var property in properties)
                {
                    var attributeName = property.Key;
                    var attributeValue = property.Value;

                    if (attributeName.Contains("line-height"))
                    {
                        attributeValue = string.Join(":", attributeName, attributeValue);
                        attributeName = "style";
                        
                        //styles.LoadStyle("lineHeight200", "style", "line-height: 200%");
                    }

                    

                    styles.LoadStyle(className, attributeName, attributeValue);
                }

            }

            return styles;
        }


    }
}
