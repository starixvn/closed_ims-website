// --------------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web.Mvc;
    using Fanex.Common;

    /// <summary>
    /// List Extensions.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "List Extensions")]
    public static class ListExtensions
    {
        /// <summary>
        /// Sort By Property.
        /// </summary>
        /// <typeparam name="T">Type parameter.</typeparam>
        /// <param name="dataSource">Data source.</param>
        /// <param name="sortType">Sort type.</param>
        /// <param name="sortCol">Sort column.</param>
        public static void SortByProperty<T>(this List<T> dataSource, string sortType, string sortCol)
        {
            PropertyInfo propInfo = typeof(T).GetProperty(sortCol);
            if (propInfo == null || dataSource == null || dataSource.Count == 0)
            {
                return;
            }

            bool isAsc = false;
            if (sortType.ToLower().Equals("asc"))
            {
                isAsc = true;
            }
            else
            {
                isAsc = false;
            }

            Comparison<T> compare = delegate(T a, T b)
            {
                object valueA = isAsc
                    ? Converter.ToDecimal(propInfo.GetValue(a, null))
                    : Converter.ToDecimal(propInfo.GetValue(b, null));

                object valueB = isAsc
                    ? Converter.ToDecimal(propInfo.GetValue(b, null))
                    : Converter.ToDecimal(propInfo.GetValue(a, null));

                return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
            };

            dataSource.Sort(compare);
        }

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
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> source, string valueField, string textField, string textPriorField = "", string selectedValue = null)
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