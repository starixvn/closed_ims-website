// -----------------------------------------------------------------------
// <copyright file="Helper.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.TicketControl.Core.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Web;
    using Fanex.TicketControl.Core.Models;
    using Fanex.TicketControl.Core.Services.Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Helper
    {

        public static string CurrencyById(int Id)
        {
            string currencyName = string.Empty;
            ICoreService _coreService;
            try
            {
               IList<Currency> _currencies = CacheManager.Get<IList<Currency>>(CacheServiceKey.KeyCurrency);
               if (_currencies == null)
               {
                   _coreService = new CoreService();
                   _currencies = _coreService.GetCurrency();
                   CacheManager.Set<IList<Currency>>(CacheServiceKey.KeyCurrency, _currencies);
               }

                currencyName = _currencies.Where(s => s.Id == Id).Select(s => s.Name).FirstOrDefault();
                if (string.IsNullOrEmpty(currencyName))
                {
                     _coreService = new CoreService();
                     currencyName = _coreService.GetCurrency().Where(s => s.Id == Id).Select(s => s.Name).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return currencyName;
        }
    }
}
