// -----------------------------------------------------------------------
// <copyright file="ExchangeCurrencyRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Common.Extensions;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Repository.Interface;

namespace Fanex.TicketControl.Core.Repository.Impl
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ExchangeCurrencyRepository : BaseRepository<Currency>, IExchangeCurrencyRepository
    {
        /// <summary>
        /// Get exchange currencies
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public List<Currency> GetExchangeCurrencies(ICriteria criteria)
        {
            //// Create Function will map Data from DataReader to Schedule Object
            Func<IDataReader, Currency> mapObjectFunc = (reader) =>
            {
                return new Currency
                {
                    CurrencyId = reader.ToInt("exchangeid"),
                    CurrencyName = reader.To<string>("currency")
                };
            };

            return base.Fetch<Currency>(criteria, mapObjectFunc);
        }
    }
}