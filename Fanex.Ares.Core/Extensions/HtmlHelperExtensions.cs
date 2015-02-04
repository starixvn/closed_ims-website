namespace Fanex.Ares.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Fanex.Ares.Core.Configurations;
    using Fanex.Ares.Core.DataDomain;

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
            if (string.IsNullOrWhiteSpace(formatDate))
            {
                throw new ArgumentNullException("formatDate cannot be null");
            }

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

        /// <summary>
        /// Date Picker For.
        /// </summary>
        /// <typeparam name="TModel">Model parameter.</typeparam>
        /// <typeparam name="TValue">Value parameter.</typeparam>
        /// <param name="html">HTML parameter.</param>
        /// <param name="expression">Expression parameter.</param>
        /// <param name="minDate">Min date parameter.</param>
        /// <param name="maxDate">Max date parameter.</param>
        /// <param name="formatDate">Format date parameter.</param>
        /// <param name="unavailDates">Unavailable date parameter.</param>
        /// <returns>Return value.</returns>
        public static MvcHtmlString DatePickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html, 
            Expression<Func<TModel, TValue>> expression,
            DateTime? minDate = null, 
            DateTime? maxDate = null,
            string formatDate = DateFormat.ShortDate, 
            IEnumerable<DateTime> unavailDates = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            DateTime? value = DateTime.Now;
            if (metaData.Model is DateTime? || metaData.Model is DateTime)
            {
                value = (DateTime?)metaData.Model;
            }
            else
            {
                value = null;
            }

            return html.DatePicker(metaData.PropertyName, value, minDate, maxDate, formatDate, unavailDates);
        }

        #endregion Date Picker

        #region Year Picker

        /// <summary>
        ///  Generate year selection item,
        ///  it add class year picker to text-box.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="name">Name parameter.</param>
        /// <param name="fromYear">From year - Default is 2000.</param>
        /// <param name="currentYear">Current year - Default is 2000.</param>
        /// <returns>Return value.</returns>
        public static MvcHtmlString YearPicker(this HtmlHelper htmlHelper, string name, int fromYear = 2000, int currentYear = 2000)
        {
            var list = new List<SelectListItem>();
            var endYear = DateTime.Now.Year;

            for (var i = endYear; i >= fromYear; i--)
            {
                list.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = i == currentYear
                });
            }

            var dropDownList = htmlHelper.DropDownList(name, list, new { @class = "yearpicker" });
            return MvcHtmlString.Create(dropDownList.ToString());
        }

        /// <summary>
        ///  Generate year selection items.
        /// </summary>
        /// <typeparam name="TModel">Model parameter.</typeparam>
        /// <typeparam name="TValue">Value parameter.</typeparam>
        /// <param name="html">HTML parameter.</param>
        /// <param name="expression">Expression parameter.</param>
        /// <param name="fromYear">Default from year 2000.</param>
        /// <returns>Return value.</returns>
        public static MvcHtmlString YearPickerFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, int fromYear = 2000)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var currentYear = (int)metaData.Model;

            return html.YearPicker(metaData.PropertyName, fromYear, currentYear);
        }

        #endregion Year Picker

        #region CheckBoxList

        /// <summary>
        /// Check Box List.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="name">Checkbox name.</param>
        /// <returns>Return value.</returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name)
        {
            return CheckBoxList(htmlHelper, name, null);
        }

        /// <summary>
        /// Check Box List.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="name">Checkbox name.</param>
        /// <param name="selectList">Select List.</param>
        /// <returns>Return value.</returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxList(htmlHelper, name, selectList, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// Check Box List.
        /// </summary>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="name">Checkbox name.</param>
        /// <param name="selectList">Select List.</param>
        /// <param name="htmlAttributes">HTML Attributes.</param>
        /// <returns>Return value.</returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
        {
            return CheckBoxList(htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Check Box List For.
        /// </summary>
        /// <typeparam name="TModel">Model parameter.</typeparam>
        /// <typeparam name="TProperty">Property parameter.</typeparam>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="expression">Expression parameter.</param>
        /// <returns>Return value.</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return CheckBoxListFor(htmlHelper, expression, null);
        }

        /// <summary>
        /// Check Box List For.
        /// </summary>
        /// <typeparam name="TModel">Model parameter.</typeparam>
        /// <typeparam name="TProperty">Property parameter.</typeparam>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="expression">Expression parameter.</param>
        /// <param name="selectList">Select List.</param>
        /// <returns>Return value.</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression, 
            IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxListFor(htmlHelper, expression, selectList, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// Check Box List For.
        /// </summary>
        /// <typeparam name="TModel">Model parameter.</typeparam>
        /// <typeparam name="TProperty">Property parameter.</typeparam>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="expression">Expression parameter.</param>
        /// <param name="selectList">Select List.</param>
        /// <param name="htmlAttributes">HTML Attributes.</param>
        /// <returns>Return value.</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression, 
            IEnumerable<SelectListItem> selectList, 
            object htmlAttributes)
        {
            return CheckBoxListFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Check Box List For.
        /// </summary>
        /// <typeparam name="TModel">Model parameter.</typeparam>
        /// <typeparam name="TProperty">Property parameter.</typeparam>
        /// <param name="htmlHelper">HTML helper.</param>
        /// <param name="expression">Expression parameter.</param>
        /// <param name="selectList">Select List.</param>
        /// <param name="htmlAttributes">HTML Attributes.</param>
        /// <returns>Return value.</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression, 
            IEnumerable<SelectListItem> selectList, 
            IDictionary<string, object> htmlAttributes)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return CheckBoxList(htmlHelper, ExpressionHelper.GetExpressionText(expression), selectList, htmlAttributes);
        }

        #endregion CheckBoxList

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
        /// Drop Down List For Month.
        /// </summary>
        /// <typeparam name="TModel">Model parameter.</typeparam>
        /// <typeparam name="TProperty">Property parameter.</typeparam>
        /// <param name="htmlHelper">HTML helper parameter.</param>
        /// <param name="expression">Expression parameter.</param>
        /// <param name="htmlAttributes">HTML Attributes parameter.</param>
        /// <returns>Return value.</returns>
        public static MvcHtmlString DropDownListForMonth<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression, 
            object htmlAttributes)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            int month = metaData == null ? 1 : metaData.Model == null ? 1 : (int)metaData.Model;
            var newitems = DateTimeFormatInfo
                .InvariantInfo
                .MonthNames
                .Where(m => m.Trim().Length != 0)
                .Select((monthName, index) => new SelectListItem
                {
                    Value = (index + 1).ToString(),
                    Text = monthName,
                    Selected = month == (index + 1)
                });

            return SelectExtensions.DropDownListFor(htmlHelper, expression, newitems, htmlAttributes);
        }

        /// <summary>
        /// Drop Down List For Year.
        /// </summary>
        /// <typeparam name="TModel">Model parameter.</typeparam>
        /// <typeparam name="TProperty">Property parameter.</typeparam>
        /// <param name="htmlHelper">HTML helper parameter.</param>
        /// <param name="expression">Expression parameter.</param>
        /// <param name="htmlAttributes">HTML Attributes parameter.</param>
        /// <returns>Return value.</returns>
        public static MvcHtmlString DropDownListForYear<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression, 
            object htmlAttributes)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            int year = metaData == null ? 1 : metaData.Model == null ? 1 : (int)metaData.Model;
            List<SelectListItem> newitems = new List<SelectListItem>();

            for (var i = 2005; i <= DateTime.Now.Year; i++)
            {
                newitems.Add(new SelectListItem()
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = year == i
                });
            }

            return SelectExtensions.DropDownListFor(htmlHelper, expression, newitems.OrderByDescending(m => m.Value), htmlAttributes);
        }

        /// <summary>
        /// Raw With Line Break.
        /// </summary>
        /// <param name="helper">HTML helper parameter.</param>
        /// <param name="htmlString">HTML string.</param>
        /// <returns>Return value.</returns>
        public static MvcHtmlString RawWithLineBreak(this HtmlHelper helper, string htmlString)
        {
            return new MvcHtmlString(htmlString.Replace("\n", "<br />"));
        }

        /// <summary>
        /// Get Full Domain URL.
        /// </summary>
        /// <returns>Domain URL.</returns>
        public static string GetFullDomainURL()
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
        public static IEnumerable<SelectListItem> ToListSelectListItem(this List<SelectListItemEx> listEx)
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
        public static string RenderViewToString(Controller controller, string viewName, object viewData)
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

    /// <summary>
    /// Fake View.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here.")]
    public class FakeView : IView
    {
        #region IView Members
        /// <summary>
        /// Render content.
        /// </summary>
        /// <param name="viewContext">View Context.</param>
        /// <param name="writer">Text Writer.</param>
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Select List Item Ex.
    /// </summary>
    public class SelectListItemEx
    {
        public SelectListItemEx()
        {
            Item = new SelectListItem();
            Extension = new Dictionary<string, object>();
        }

        public SelectListItem Item { get; set; }

        public Dictionary<string, object> Extension { get; set; }
    }
}