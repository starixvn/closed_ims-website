// --------------------------------------------------------------------------
// <copyright file="DateTimeExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Core.Extensions
{
    using System;

    /// <summary>
    /// Contains date extension methods.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Get First Date of Month.
        /// </summary>
        /// <param name="dateTime">Date parameter.</param>
        /// <returns>Return value.</returns>
        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// Get Last Date Of Month.
        /// </summary>
        /// <param name="dateTime">Date time parameter.</param>
        /// <returns>Return value.</returns>
        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            return dateTime.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Format Short Date.
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>Return value.</returns>
        public static string ToShortFormatedDate(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToShortFormatedDate();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format a date time value to string (MMM dd, yyyy).
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>Return value.</returns>
        public static string ToShortFormatedDate(this DateTime date)
        {
            return date.ToString(Constants._SHORT_DATE_FORMAT);
        }

        /// <summary>
        /// Format a null able date time value to string (MMM dd, yyyy hh:mm).
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>Return value.</returns>
        public static string ToLongFormatedDate(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToLongFormatedDate();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format a date time value to string (MMM dd, yyyy hh:mm).
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>Return value.</returns>
        public static string ToLongFormatedDate(this DateTime date)
        {
            return date.ToString(Constants._LONG_DATE_FORMAT);
        }

        /// <summary>
        /// Format a null able date time value to string (dd-MMM).
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>Return value.</returns>
        public static string ToMonthAndDay(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToMonthAndDay();
            }

            return string.Empty;
        }

        /// <summary>
        /// Foarmat da date time value to string (dd-MMM).
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>Return value.</returns>
        public static string ToMonthAndDay(this DateTime date)
        {
            return date.ToString("dd-MMM");
        }

        /// <summary>
        /// Format a null able date time value to string (MMM yyyy).
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>Return value.</returns>
        public static string ToMonthYear(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToMonthYear();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format date time value to string.
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>String with format (MMM yyyy).</returns>
        public static string ToMonthYear(this DateTime date)
        {
            return date.ToString(Constants._MONTH_YEAR_DATE_FORMAT);
        }

        /// <summary>
        /// Format Short Date (MM/dd/yyyy).
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>Return value.</returns>
        public static string ToShortFormatedDateWithSlash(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToShortFormatedDateWithSlash();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format a datetime value to string (MM/dd/yyyy).
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>Return value.</returns>
        public static string ToLongFormatedDateWithSlash(this DateTime date)
        {
            return date.ToString(Constants._LONG_DATE_FORMAT_WITH_SLASH);
        }

        /// <summary>
        /// Format a datetime value to string (MM/dd/yyyy).
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>Return value.</returns>
        public static string ToShortFormatedDateWithSlash(this DateTime date)
        {
            return date.ToString(Constants._SHORT_DATE_FORMAT_WITH_SLASH);
        }

        /// <summary>
        /// Format a null able date time value to string (MM/yyyy).
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>Return value.</returns>
        public static string ToMonthYearWithSlash(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToMonthYearWithSlash();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format date time value to string.
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>String with format (MM/yyyy).</returns>
        public static string ToMonthYearWithSlash(this DateTime date)
        {
            return date.ToString(Constants._MONTH_YEAR_DATE_FORMAT_WITH_SLASH);
        }

        /// <summary>
        /// Foarmat da date time value to string (dd-MMM-yy).
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>Return value.</returns>
        public static string ToAresDayMonthYear(this DateTime date)
        {
            return date.ToString(Constants._ARES_DATE_FORMAT);
        }

        /// <summary>
        /// Format date time value to string (MMM-yy).
        /// </summary>
        /// <param name="date">Date parameter.</param>
        /// <returns>Return value.</returns>
        public static string ToAresMonthYear(this DateTime date)
        {
            return date.ToString(Constants._ARES_MONTH_YEAR_FORMAT);
        }
    }
}