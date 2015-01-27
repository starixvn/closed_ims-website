namespace Fanex.Ares.Report.Entities
{
    using System;

    /// <summary>
    /// Exchange Rate.
    /// </summary>
    [Serializable]
    public class ExchangeRate
    {
        public int Currency { get; set; }

        public string CurrencyName { get; set; }

        public double BasePerUnit { get; set; }

        public double UnitsPerBase { get; set; }
    }
}
