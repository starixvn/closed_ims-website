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

        private string _htmlString;

        private string _outputFilePath;

        private string _cssFilePath;

        #endregion

        #region Constructor

        public PdfRender()
        {
        }

        public PdfRender(string outputfilePath, string cssFilePath, string htmlString)
        {
            _outputFilePath = outputfilePath;
            _htmlString = htmlString;
            _cssFilePath = cssFilePath;
            _pdfDocument = new Document();
            _stream = new MemoryStream();
        }

        #endregion

        #region Properties

        protected string OutputfilePath
        {
            get { return _outputFilePath; }
        }

        protected string HtmlString
        {
            get { return _htmlString; }
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

        public Document DocumentRender()
        {
            Render();
            _pdfDocument.Close();

            return _pdfDocument;
            //// Save();
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
        /// Save data-stream to file.
        /// </summary>
        private void Save()
        {
            if (File.Exists(this.OutputfilePath))
            {
                File.Delete(this.OutputfilePath);
            }

            _pdfDocument.Close();

            var streamData = _stream.ToArray();

            using (var fileStream = new FileStream(OutputfilePath, FileMode.OpenOrCreate))
            {
                fileStream.Write(streamData, 0, streamData.Length);
            }
        }

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
            using (var stringReader = new StringReader(_htmlString))
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
