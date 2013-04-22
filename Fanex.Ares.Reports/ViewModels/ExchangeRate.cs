// -----------------------------------------------------------------------
// <copyright file="ExchangeRate.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Ares.Report.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class ExchangeRate
    {
        public int CurrencyID { get; set; }

        public string CurrencyName { get; set; }

        public double CurrencyPerUnit { get; set; }

        public double UnitsPerCurrency { get; set; }
    }
}
