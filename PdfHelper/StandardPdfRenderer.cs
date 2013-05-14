// -----------------------------------------------------------------------
// <copyright file="StandardPdfRenderer.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.Core.PdfHelper.Configuration;
using Fanex.Web.HtmlHelper;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.Web;

namespace Fanex.Core.PdfHelper
{
    /// <summary>
    /// This class is responsible for rendering a html text string to a PDF document using the html renderer of iTextSharp.
    /// </summary>
    public class StandardPdfRenderer
    {
        private const int HorizontalMargin = 20;
        private const int VerticalMargin = 20;

        public byte[] Render(string htmlText, string pageTitle, int numericRows, string cssFilePath, bool isLandscape = true)
        {
            byte[] renderedBuffer;



            using (var outputMemoryStream = new MemoryStream())
            {
                //using (var pdfDocument = new Document(PageSize.A4, HorizontalMargin, HorizontalMargin, VerticalMargin, VerticalMargin))
                using (var pdfDocument = new Document(isLandscape ? PageSize.A4.Rotate() : PageSize.A4, HorizontalMargin, HorizontalMargin, VerticalMargin, VerticalMargin))
                {
                    PdfWriter pdfWriter = PdfWriter.GetInstance(pdfDocument, outputMemoryStream);
                    pdfWriter.CloseStream = false;
                    pdfWriter.PageEvent = new PrintHeaderFooter { Title = pageTitle };
                    pdfDocument.Open();

                    var styles = LoadStyleSheet(cssFilePath);


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

        private void RotatePdf(string inputFile, string outputFile)
        {
            using (var outStream = new FileStream(outputFile, FileMode.Create))
            {
                var reader = new PdfReader(inputFile);
                var stamper = new PdfStamper(reader, outStream);

                var pageDict = reader.GetPageN(1);
                int desiredRot = 90; // 90 degrees clockwise from what it is now
                var rotation = pageDict.GetAsNumber(PdfName.ROTATE);

                if (rotation != null)
                {
                    desiredRot += rotation.IntValue;
                    desiredRot %= 360; // must be 0, 90, 180, or 270
                }
                pageDict.Put(PdfName.ROTATE, new PdfNumber(desiredRot));

                stamper.Close();
            }
        }


        private static StyleSheet LoadStyleSheet(string cssFilePath)
        {
            var styles = new StyleSheet();

            if (string.IsNullOrEmpty(cssFilePath))
                return styles;

            //var stylePath = HttpContext.Current.Server.MapPath(@"~\Areas\Report\Views\LicenseeSettlement\Styles\LicenseeSettlementPDF.css");

            var cssParser = new CssParser(cssFilePath);



            var pdfSection = PdfHelperSection.Instance;

            var listException = pdfSection != null ? pdfSection.StyleSheet["SpecialStyle"].Items : new PdfHelperCollection();

            var lsExceptionStyle = new List<string>();

            for (var i = 0; i < listException.Count; i ++)
            {
                
                lsExceptionStyle.Add(listException[i].Name);
            }
           

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

                    if (lsExceptionStyle.Contains(attributeName))
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
