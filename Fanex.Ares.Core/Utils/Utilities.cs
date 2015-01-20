namespace Fanex.Ares.Core.Utils
{
    using System;
    using Fanex.Ares.Core.Extensions;

    /// <summary>
    /// Utilities Class.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Get FOREX Time.
        /// </summary>
        /// <param name="settlementDate">Settlement Date.</param>
        /// <returns>FOREX Time.</returns>
        public static string GetForexTime(DateTime settlementDate)
        {
            var forexTime = string.Empty;

            if (DateTime.Now.Year == settlementDate.Year && DateTime.Now.Month == settlementDate.Month)
            {
                forexTime = DateTime.Now.ToAresDayMonthYear();
            }
            else
            {
                forexTime = settlementDate.LastDayOfMonth().ToAresDayMonthYear();
            }

            return forexTime;
        }
    }
}
