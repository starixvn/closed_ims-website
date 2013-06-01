// --------------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Fanex.Common;
using System.Web.Mvc;

namespace Fanex.Ares.Core.Extensions
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public static class ListExtensions
    {
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

        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this List<T> source, string textField, string valueField, string selectedValue = null)
        {
            foreach (var item in source)
            {
                var tmpText = item.GetType().GetProperty(textField).GetValue(item, null).ToString();
                var tmpValue = item.GetType().GetProperty(textField).GetValue(item, null).ToString();
                yield return new SelectListItem()
                {
                    Text = tmpText,
                    Value = tmpValue,
                    Selected = selectedValue != null && selectedValue == tmpText
                };
            }
        }
    }
}