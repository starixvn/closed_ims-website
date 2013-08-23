// -----------------------------------------------------------------------
// <copyright file="InvoiceHelper.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Invoice.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Common;
    using Fanex.Ares.Core.Extensions;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class InvoiceHelper
    {
        public static string NegativeFormat(double input)
        {
            string output = string.Empty;

            output = input >= 0 ? "<span style=\"font-weight: bold;\">" + Converter.To<string>(input.Format(2)) + "</span>" : "<span style=\"color: red;font-weight: bold;\">(" + Converter.To<string>((input * -1).Format(2)) + ")</span>";

            return output;
        }

        public static string GetNegativeClass(double number)
        {
            return number < 0 ? "negative-number" : string.Empty;
        }

        public static string GetEvenClass(int number)
        {
            return number % 2 == 0 ? "background-color: #e4f2fd;" : string.Empty;
        }
    }
}
