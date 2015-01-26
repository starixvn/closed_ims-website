namespace Fanex.Ares.Report.Export.Pdf
{
    using System;
    using System.IO;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using iTextSharp.tool.xml;
    using iTextSharp.tool.xml.html;
    using iTextSharp.tool.xml.parser;
    using iTextSharp.tool.xml.pipeline.css;
    using iTextSharp.tool.xml.pipeline.end;
    using iTextSharp.tool.xml.pipeline.html;

    /// <summary>
    /// PDF Render.
    /// </summary>
    public class PdfRender : IDisposable
    {
        #region Fields

        private PdfWriter _pdfWriter;

        private Document _pdfDocument;

        private MemoryStream _stream;

        private string _htmlText;

        private string _cssFilePath;

        #endregion

        #region Constructor

        public PdfRender()
        {
        }

        public PdfRender(string cssFilePath, string htmlText)
        {
            _htmlText = htmlText;
            _cssFilePath = cssFilePath;
            _pdfDocument = new Document();
            _stream = new MemoryStream();
        }

        #endregion

        #region Properties

        protected string HtmlString
        {
            get { return _htmlText; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dispose all resources.
        /// </summary>
        public void Dispose()
        {
            if (_pdfDocument != null)
            {
                _pdfDocument.Dispose();
            }

            if (_stream != null)
            {
                _stream.Dispose();
            }
        }

        public MemoryStream StreamRender()
        {
            Render();
            _pdfDocument.Close();

            return _stream;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Render data into stream.
        /// </summary>
        private void Render()
        {
            Create();
            AssignData();
        }

        /// <summary>
        /// Create PDF File.
        /// </summary>
        private void Create()
        {
            _stream = new MemoryStream();
            _pdfDocument = new Document(new Rectangle(1600, 960), 20, 20, 40, 40);
            _pdfDocument.SetPageSize(PageSize.A4.Rotate());
            _pdfWriter = PdfWriter.GetInstance(_pdfDocument, _stream);

            _pdfDocument.Open();
        }

        /// <summary>
        /// Assign Data.
        /// </summary>
        private void AssignData()
        {
            using (var stringReader = new StringReader(_htmlText))
            {
                var htmlContext = new HtmlPipelineContext(null);
                var cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);

                htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
                cssResolver.AddCssFile(_cssFilePath, true);

                var pdfWriterPipeline = new PdfWriterPipeline(_pdfDocument, _pdfWriter);
                var htmlPipeline = new HtmlPipeline(htmlContext, pdfWriterPipeline);
                var pipeline = new CssResolverPipeline(cssResolver, htmlPipeline);
                var xmlWorker = new XMLWorker(pipeline, true);
                var xmlParse = new XMLParser(xmlWorker);

                xmlParse.Parse(stringReader);
                xmlParse.Flush();
            }
        }

        #endregion
    }
}
