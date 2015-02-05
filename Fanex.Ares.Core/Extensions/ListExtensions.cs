namespace Fanex.Ares.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;
    using Fanex.Common;

    /// <summary>
    /// List Extensions.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Convert List object to List Select list item using for HtmlHelper.
        /// </summary>
        /// <typeparam name="T">Source type.</typeparam>
        /// <param name="source">Source Object.</param>
        /// <param name="valueField">Value Field reference from Source Object.</param>
        /// <param name="textField">Display text Field reference from Source Object.</param>
        /// <param name="textPriorField">Display prior text Field reference from Source Object. If this field is empty, the textField will take place.</param>
        /// <param name="selectedValue">Selected Value.</param>
        /// <returns>Return value.</returns>
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(
            this IEnumerable<T> source, 
            string valueField, 
            string textField, 
            string textPriorField = "", 
            string selectedValue = null)
        {
            foreach (var item in source)
            {
                var tmpText = item.GetType().GetProperty(textField).GetValue(item, null).ToString1();
                var tmpPriorText = string.IsNullOrEmpty(textPriorField) ? string.Empty : item.GetType().GetProperty(textPriorField).GetValue(item, null).ToString1();
                var tmpValue = item.GetType().GetProperty(valueField).GetValue(item, null).ToString1();
                yield return new SelectListItem()
                {
                    Text = string.IsNullOrEmpty(tmpPriorText) ? tmpText : tmpPriorText,
                    Value = tmpValue,
                    Selected = selectedValue != null && selectedValue == tmpText
                };
            }
        }
    }
}