// -----------------------------------------------------------------------
// <copyright file="HtmlViewRenderer.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Core.PdfHelper
{
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    /// <summary>
    /// This class is responsible for rendering a HTML view into a string. 
    /// </summary>
    public class HtmlViewRenderer
    {
        /// <summary>
        /// Render View To String.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="viewName">View Name.</param>
        /// <param name="viewData">View Data.</param>
        /// <returns>Return value.</returns>
        public string RenderViewToString(Controller controller, string viewName, object viewData)
        {
            var renderedView = new StringBuilder();
            using (var responseWriter = new StringWriter(renderedView))
            {
                var fakeResponse = new HttpResponse(responseWriter);
                var fakeContext = new HttpContext(HttpContext.Current.Request, fakeResponse);
                var fakeControllerContext = new ControllerContext(new HttpContextWrapper(fakeContext), controller.ControllerContext.RouteData, controller.ControllerContext.Controller);

                var oldContext = HttpContext.Current;
                HttpContext.Current = fakeContext;

                using (var viewPage = new ViewPage())
                {
                    var html = new HtmlHelper(CreateViewContext(responseWriter, fakeControllerContext), viewPage);
                    html.RenderPartial(viewName, viewData);
                    HttpContext.Current = oldContext;
                }
            }

            return renderedView.ToString();
        }

        /// <summary>
        /// Create View Context.
        /// </summary>
        /// <param name="responseWriter">Response Writer.</param>
        /// <param name="fakeControllerContext">Fake Controller Context.</param>
        /// <returns>Return value.</returns>
        private static ViewContext CreateViewContext(TextWriter responseWriter, ControllerContext fakeControllerContext)
        {
            return new ViewContext(fakeControllerContext, new FakeView(), new ViewDataDictionary(), new TempDataDictionary(), responseWriter);
        }
    }
}
