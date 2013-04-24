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

                    var styles = new StyleSheet();

                    var stylePath = HttpContext.Current.Server.MapPath(@"~\Areas\Report\Views\LicenseeSettlement\Styles\LicenseeSettlementPDF.css");

                    ICssParser cssParser = new CssParser(stylePath);



                    var lsClassName = cssParser.GetListClassName();
                    foreach (var className in lsClassName)
                    {
                        var properties = cssParser.GetProperties(className);
                        
                        if(properties != null)
                            styles.LoadStyle(className, properties as Dictionary<string, string>);
                        
                    }
                    /*

                    styles.LoadStyle("titleBold", "size", "13pt");
                    styles.LoadStyle("titleBold", "align", "left");
                    styles.LoadStyle("titleBold", "font-weight", "1000");

                    styles.LoadStyle("titleSub", "size", "11pt");
                    styles.LoadStyle("titleSub", "align", "left");

                    styles.LoadStyle("cellInfo", "size", "8pt");
                    styles.LoadStyle("cellInfo", "align", "left");

                    styles.LoadStyle("lineHeight200", "style", "line-height: 200%");
                    styles.LoadStyle("lineHeight80", "style", "line-height: 80%");

                

                    styles.LoadStyle("headerReport", "size", "7pt");
                    styles.LoadStyle("headerReport", "align", "center");
                    styles.LoadStyle("headerReport", "style", "line-height: 100%");
                    styles.LoadStyle("headerReport", "color", "#ffffff");
                    styles.LoadStyle("headerReport", "bgcolor", "#618ADA");


                    styles.LoadStyle("cellstyle", "size", "7pt");
                    styles.LoadStyle("cellstyle", "style", "line-height: 100%");
                    styles.LoadStyle("cellstyle", "valign", "middle");

                    styles.LoadStyle("aligncenter", "align", "center");
                    styles.LoadStyle("alignright", "align", "right");
                    styles.LoadStyle("alignleft", "align", "left");

                    styles.LoadStyle("col60", "width", "60");
                    styles.LoadStyle("col30", "width", "30");
                    styles.LoadStyle("col200", "width", "200");

                    styles.LoadStyle("col65", "width", "65");
                    styles.LoadStyle("col40", "width", "40");
                    styles.LoadStyle("col650", "width", "650");

                    styles.LoadStyle("col120", "width", "120");
                    styles.LoadStyle("col100", "width", "100");
                    styles.LoadStyle("col80", "width", "80");
                    styles.LoadStyle("col400", "width", "400");
                    styles.LoadStyle("col140", "width", "140");
                    styles.LoadStyle("col50", "width", "50");

                    styles.LoadStyle("col10percent", "width", "10%");

                    styles.LoadStyle("colborder0", "border", "0");
                    styles.LoadStyle("colborder01", "border", "0.1");
                    
                    //styles.LoadTagStyle("th", "size", "10pt");
                    //styles.LoadStyle("colorcell", "color", "#ffffff");
                    //styles.LoadStyle("colorcell", "bgcolor", "#618ADA");  */

//                        styles.LoadTagStyle("p", "size", "10pt");
//styles.LoadTagStyle("p", "color", "#0000ff");   

                    PdfContentByte cb = pdfWriter.DirectContent;
                    List<IElement> htmlElements = HTMLWorker.ParseToList(new StringReader(htmlText), styles);


                    //IDictionary<String, String> styles = new Dictionary<String, String>();


                    for (int i = 0; i < htmlElements.Count; i++)
                    {
                        // cast the element
                        IElement htmlElement = ((IElement)htmlElements[i]);

                        if (htmlElement is PdfPTable)
                        {
                            var table = (PdfPTable)htmlElement;
                            var width = table.TotalWidth;
                            float height = table.CalculateHeights();
                            foreach (var row in table.Rows)
                            {
                                PdfPCell[] pdfCellTemp = row.GetCells();

                                if (pdfCellTemp.Length == 1)
                                {
                                    //float[] w = { 400, 100 };
                                    //row.SetWidths(w);
                                }
                            }
                        }
                        pdfDocument.Add(htmlElement);
                        //string pos = string.Empty;
                        //if (i == 5)
                        //{
                        //    styles.TryGetValue(HtmlTags.DIV, out pos);

                        //}

                    }

                    int end = 435 - (numericRows * 18);


                    cb.SetLineWidth(2f);
                    cb.MoveTo(22, end); //  - 19
                    cb.LineTo(672, end);
                    cb.Stroke();

                    //int signPos = 470 - ((numericRows + 4) * 19);

                    //cb.SetLineWidth(0f);
                    //cb.MoveTo(10, signPos); //  - 19
                    //cb.LineTo(830, signPos);
                    //cb.Stroke();

                    //using (var htmlViewReader = new StringReader(htmlText))
                    //{
                    //    using (var htmlWorker = new HTMLWorker(pdfDocument))
                    //    {
                    //        htmlWorker.Parse(htmlViewReader);
                    //    }
                    //}
                    //pdfWriter.DirectContent.AddTemplate(
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



    }
}
