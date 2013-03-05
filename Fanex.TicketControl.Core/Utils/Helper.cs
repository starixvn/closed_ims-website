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
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Helper
    {
        public enum SportType
        {
            Sportsbook = 0,
            VirtualSports = 1,
            NumberGame = 2,
            LiveCasino = 3
        }

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

        public static string GetClientIP()
        {
            string ip = string.Empty;
            try
            {
                if (HttpContext.Current.Request.IsLocal)
                    ip = "127.0.0.1";
                else 
                    ip = Convert.ToString(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return ip;
        }
    }
}
