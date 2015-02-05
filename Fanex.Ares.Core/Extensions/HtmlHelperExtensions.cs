namespace Fanex.Ares.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Fanex.Ares.Core.DataDomain;
    using Fanex.Ares.Core.Models;

    /// <summary>
    /// It contains extension to render common html control.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        #region Date Picker

        /// <summary>
        ///  This function add class date picker into textbox,
        ///  client script explore all textbox which have class date picker and use j-query u-i
        ///  lib to render date picker control.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="name">Name parameter.</param>
        /// <param name="date">Date parameter.</param>
        /// <param name="minDate">Min date parameter.</param>
        /// <param name="maxDate">Max date parameter.</param>
        /// <param name="formatDate">Format date parameter.</param>
        /// <param name="unavailDates">Unavailable date parameter.</param>
        /// <returns>Return value.</returns>
        public static MvcHtmlString DatePicker(
            this HtmlHelper htmlHelper,
            string name,
            DateTime? date,
            DateTime? minDate = null,
            DateTime? maxDate = null,
            string formatDate = DateFormat.ShortDate,
            IEnumerable<DateTime> unavailDates = null)
        {
            string disabledDate = (unavailDates != null && unavailDates.Count() > 0)
                ? string.Join(",", unavailDates.Select(d => d.ToString(DateFormat.ShortDate)))
                : string.Empty;
            var textbox = htmlHelper.TextBox(
                name,
                date.HasValue ? date.Value.ToString(formatDate) : string.Empty,
                new
                {
                    @class = "datepicker",
                    mindate = minDate != null && minDate != DateTime.MinValue ? ((DateTime)minDate).ToString("yyyy/MM/dd") : "null",
                    maxdate = maxDate != null && maxDate != DateTime.MinValue ? ((DateTime)maxDate).ToString("yyyy/MM/dd") : "null",
                    disabledDate = disabledDate
                });

            return MvcHtmlString.Create(textbox.ToString());
        }

        #endregion Date Picker

        #region Dropdownlist

        /// <summary>
        /// Check Box List For.
        /// </summary>
        /// <typeparam name="TModel">Model parameter.</typeparam>
        /// <typeparam name="TProperty">Property parameter.</typeparam>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="expression">Expression parameter.</param>
        /// <param name="selectList">Select List.</param>
        /// <param name="firstItem">First Item.</param>
        /// <param name="htmlAttributes">HTML Attributes.</param>
        /// <returns>Return value.</returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Reviewed")]
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            SelectListItem firstItem,
            object htmlAttributes)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            if (firstItem != null)
            {
                if (firstItem.Value != null)
                {
                    List<SelectListItem> list = new List<SelectListItem>();
                    firstItem.Selected = metaData == null || metaData.Model == null || firstItem.Value == metaData.Model.ToString();

                    list.Add(firstItem);

                    if (selectList != null)
                    {
                        foreach (SelectListItem item in selectList)
                        {
                            item.Selected = metaData != null && metaData.Model != null && firstItem.Value == metaData.Model.ToString();
                            list.Add(item);
                        }
                    }

                    selectList = list;
                }
            }

            var result = SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, htmlAttributes);
            return result;
        }

        /// <summary>
        /// Get Full Domain URL.
        /// </summary>
        /// <returns>Domain URL.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed")]
        public static string GetFullDomain()
        {
            NameValueCollection variables = HttpContext.Current.Request.ServerVariables;
            string protocol = (variables["SERVER_PORT_SECURE"] == "1") ? "https://" : "http://";
            string domain = variables["SERVER_NAME"];
            string port = variables["SERVER_PORT"];

            return protocol + domain + ":" + port + (HttpRuntime.AppDomainAppVirtualPath == "/" ? string.Empty : HttpRuntime.AppDomainAppVirtualPath);
        }

        /// <summary>
        /// To List Select List Item.
        /// </summary>
        /// <param name="listEx">List extension.</param>
        /// <returns>Return value.</returns>
        public static IEnumerable<SelectListItem> ToListSelectListItem(this List<SelectListItemExtension> listEx)
        {
            foreach (var item in listEx)
            {
                yield return item.Item;
            }
        }

        #endregion Dropdownlist

        /// <summary>
        /// Render View To String.
        /// </summary>
        /// <param name="controller">Controller parameter.</param>
        /// <param name="viewName">View name parameter.</param>
        /// <param name="viewData">View data parameter.</param>
        /// <returns>Return value.</returns>
        public static string RenderViewToString(ControllerBase controller, string viewName, object viewData)
        {
            var renderedView = new StringBuilder();
            using (var responseWriter = new StringWriter(renderedView))
            {
                var fakeResponse = new HttpResponse(responseWriter);
                var fakeContext = new HttpContext(HttpContext.Current.Request, fakeResponse);
                var fakeControllerContext = new ControllerContext(
                    new HttpContextWrapper(fakeContext),
                    controller.ControllerContext.RouteData,
                    controller.ControllerContext.Controller);

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