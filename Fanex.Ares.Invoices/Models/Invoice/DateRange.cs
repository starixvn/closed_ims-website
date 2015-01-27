namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;

    /// <summary>
    /// Date Range.
    /// </summary>
    [Serializable]
    public class DateRange
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
