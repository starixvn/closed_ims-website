// --------------------------------------------------------------------------
// <copyright file="NumberExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Core.Extensions
{
    using System;
using Fanex.Common;
using Fanex.Ares.Core.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// Format int.
        /// </summary>
        /// <param name="value">Value parameter.</param>
        /// <returns>Return value.</returns>
        public static string Format(this int value)
        {
            return DoFormat(value, 0);
        }

        /// <summary>
        /// Format Object.
        /// </summary>
        /// <param name="value">Value parameter.</param>
        /// <returns>Return value.</returns>
        public static string Format(this object value)
        {
            return DoFormat(value, 0);
        }

        /// <summary>
        /// Format Decimal Value.
        /// </summary>
        /// <param name="value">Value parameter.</param>
        /// <param name="decDigits">Decimal digits.</param>
        /// <returns>Return value.</returns>
        public static string Format(this decimal value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Format Object value.
        /// </summary>
        /// <param name="value">Value parameter.</param>
        /// <param name="decDigits">Decimal digits.</param>
        /// <returns>Return value.</returns>
        public static string Format(this object value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Format Double Value.
        /// </summary>
        /// <param name="value">Value parameter.</param>
        /// <param name="decDigits">Decimal digits.</param>
        /// <returns>Return value.</returns>
        public static string Format(this double value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Format Float Value.
        /// </summary>
        /// <param name="value">Value parameter.</param>
        /// <param name="decDigits">Decimal digits.</param>
        /// <returns>Return value.</returns>
        public static string Format(this float value, byte decDigits)
        {
            return DoFormat(value, decDigits);
        }

        /// <summary>
        /// Format Double Value Quotation mark.
        /// </summary>
        /// <param name="value">Value parameter.</param>
        /// <param name="decDigits">Decimal digits.</param>
        /// <returns>Return value.</returns>
        public static string FormatNegative(this double value, byte decDigits)
        {
            var res = string.Empty;
            if (value < 0)
            {
                value = -value;
                res = DoFormat(value, decDigits);
                res = "(" + res + ")";
            }
            else
            {
                res = DoFormat(value, decDigits);
            }

            return res;
        }

        /// <summary>
        /// To String 1.
        /// </summary>
        /// <param name="source">Source parameter.</param>
        /// <returns>Return value.</returns>
        public static string ToString1(this object source)
        {
            if (source == null)
            {
                return string.Empty;
            }

            return source.ToString();
        }

        /// <summary>
        /// Format specified number value.
        /// </summary>
        /// <param name="value">Value parameter.</param>
        /// <param name="decDigits">Decimal digits.</param>
        /// <returns>Value after rounding and formatting.</returns>
        private static string DoFormat(object value, byte decDigits)
        {
            if (value != null)
            {
                string formatString = "{0:n" + decDigits.ToString() + "}";
                string result = null;
                if (value != null)
                {
                    result = string.Format(formatString, value);
                }

                return result;
            }
            else
            {
                throw new NullReferenceException();
            }
        }
        
        /// <summary>
        /// Negatives the format.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        public static string NegativeFormat(double input)
        {
            string output = string.Empty;

            output = input >= 0 ? "<span style=\"font-weight: bold;\">" + Converter.To<string>(input.Format(2)) + "</span>" : "<span style=\"color: red;font-weight: bold;\">(" + Converter.To<string>((input * -1).Format(2)) + ")</span>";

            return output;
        }
        
        /// <summary>
        /// Gets the negative class.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>System.String.</returns>
        public static string GetNegativeClass(double number)
        {
            return number < 0 ? "negative-number" : string.Empty;
        }

        /// <summary>
        /// Gets the even class.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>System.String.</returns>
        public static string GetEvenClass(int number)
        {
            return number % 2 == 0 ? "background-color: #e4f2fd;" : string.Empty;
        }
    }
}
