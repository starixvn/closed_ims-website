namespace Fanex.Ares.Invoice.Export
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using iTextSharp.tool.xml;
    using iTextSharp.tool.xml.html;
    using iTextSharp.tool.xml.parser;
    using iTextSharp.tool.xml.pipeline.css;
    using iTextSharp.tool.xml.pipeline.end;
    using iTextSharp.tool.xml.pipeline.html;
    using iTextSharp.tool.xml.exceptions;

    /// <summary>
    /// PDF Render.
    /// </summary>
    public class PdfRender : IDisposable
    {
        #region Fields

        private PdfWriter _pdfWriter;

        private Document _pdfDocument;

        private MemoryStream _stream;

        private ICSSResolver _cssResolver;

        private List<string> _htmlTextList;

        private List<string> _cssPathList;

        #endregion

        #region Constructor

        public PdfRender()
        {
            _htmlTextList = new List<string>();
            _cssPathList = new List<string>();
            _cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
            _pdfDocument = new Document();
            _stream = new MemoryStream();
        }

        public PdfRender(List<string> cssPathList, List<string> htmlTextList)
        {
            _htmlTextList = new List<string>();
            _cssPathList = new List<string>();
            _htmlTextList.AddRange(htmlTextList);
            _cssPathList.AddRange(cssPathList);
            _cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(true);
            _pdfDocument = new Document();
            _stream = new MemoryStream();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dispose all resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public MemoryStream SteamRender()
        {
            Render();
            _pdfDocument.Close();

            return _stream;
        }

        public byte[] ContentRender()
        {
            Render();
            _pdfDocument.Close();

            return _stream.ToArray();
        }

        /// <summary>
        /// Clear all resources.
        /// </summary>
        /// <param name="disposing">Is Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
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
            _pdfDocument = new Document(new Rectangle(1600, 960), 20, 20, 20, 20);
            _pdfDocument.SetPageSize(PageSize.A4.Rotate());
            _pdfWriter = PdfWriter.GetInstance(_pdfDocument, _stream);

            _pdfDocument.Open();
        }

        /// <summary>
        /// Assign Data.
        /// </summary>
        private void AssignData()
        {
            AssignCss();
            AssignContent();
        }

        /// <summary>
        /// Assign Product Page.
        /// </summary>
        private void AssignCss()
        {
            foreach (var cssPath in _cssPathList)
            {
                _cssResolver.AddCssFile(cssPath, true);
            }
        }

        /// <summary>
        /// Assign Invoice Page.
        /// </summary>
        private void AssignContent()
        {
            foreach (var htmlText in _htmlTextList)
            {
                using (var stringReader = new StringReader(htmlText))
                {
                    var htmlContext = new HtmlPipelineContext(null);

                    htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());

                    var pdfWriterPipeline = new PdfWriterPipeline(_pdfDocument, _pdfWriter);
                    var htmlPipeline = new HtmlPipeline(htmlContext, pdfWriterPipeline);
                    var pipeline = new CssResolverPipeline(_cssResolver, htmlPipeline);
                    var xmlWorker = new XMLWorker(pipeline, true);
                    var xmlParse = new XMLParser(xmlWorker);

                    xmlParse.Parse(stringReader);
                    xmlParse.Flush();

                    _pdfDocument.NewPage();
                }
            }
        }

        #endregion
    }
}
