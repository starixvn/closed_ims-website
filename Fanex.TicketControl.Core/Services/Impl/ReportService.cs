// --------------------------------------------------------------------------
// <copyright file="ReportUtils.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Criteria;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Repository.Impl;
using Fanex.TicketControl.Core.Web;

namespace Fanex.TicketControl.Core.Services.Impl
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public static class ReportService
    {
        #region StaticFields

        public const string CACHE_BALANCE_UP = "Fanex.AccountingReports.Core.ReportUtils.BalanceUp";
        public const string CACHE_EXCHANGE_CURRENCIES = "Fanex.AccountingReports.Core.ReportService.ExchangeCurrencies";
        public const string BET_TRANS14_SP_SUFFIX = "_14";
        public const string WARE_HOUSE_SP_SUFFIX = "_WH";
        private static readonly IDynamicRepository _balanceUpRepository;

        #endregion StaticFields

        static ReportService()
        {
            _balanceUpRepository = new DynamicRepository();
        }

        #region StaticProperties

        public static BalanceUp BalanceUp
        {
            get
            {
                var balanceUp = CacheManager.Get<BalanceUp>(CACHE_BALANCE_UP);
                if (balanceUp == null)
                {
                    var criteria = new BalanceUpCriteria();
                    balanceUp = _balanceUpRepository.Get<BalanceUp>(criteria);
                    CacheManager.Set<BalanceUp>(CACHE_BALANCE_UP, balanceUp, new TimeSpan(5,0,0,0));
                }

                return balanceUp;
            }
        }

        public static Dictionary<int, string> ExchangeCurrencies
        {
            get
            {
                var exchangeCurrencies = CacheManager.Get<Dictionary<int, string>>(CACHE_EXCHANGE_CURRENCIES);
                if (exchangeCurrencies == null)
                {
                    exchangeCurrencies = GetExchangeCurrencies(new GetExchangeCurrenciesCriteria());
                    CacheManager.Set<Dictionary<int, string>>(CACHE_EXCHANGE_CURRENCIES, exchangeCurrencies, new TimeSpan(5, 0, 0, 0));
                }
                return exchangeCurrencies;
            }
        }

        public static string GetCurrencyNameById(int Id)
        {
            string currencyName = "";
            if (ExchangeCurrencies.ContainsKey(Id))
            {
                currencyName = ExchangeCurrencies[Id];
            }
            else
            {
                UpdateLatestExchangeCurrencies();
                if (ExchangeCurrencies.ContainsKey(Id))
                {
                    currencyName = ExchangeCurrencies[Id];
                }
            }
            return currencyName;
        }

        #endregion StaticProperties

        #region StaticMethods

        public static string GetStoreMappingSubfix(DateTime reportDate)
        {
            if (reportDate > BalanceUp.MoveBetsDay)
            {
                return string.Empty;
            }
            else if (reportDate >= BalanceUp.BalanceUpDay)
            {
                return BET_TRANS14_SP_SUFFIX;
            }
            else
            {
                return WARE_HOUSE_SP_SUFFIX;
            }
        }

        public static void UpdateLatestExchangeCurrencies()
        {
            var exchangeCurrencies = GetExchangeCurrencies(new GetExchangeCurrenciesCriteria());
            if(exchangeCurrencies.Count > 0)
            {
                CacheManager.Set<Dictionary<int, string>>(CACHE_EXCHANGE_CURRENCIES, exchangeCurrencies, new TimeSpan(5, 0, 0, 0));
            }
        }

        private static Dictionary<int, string> GetExchangeCurrencies(GetExchangeCurrenciesCriteria criteria)
        {
            var exchangeCurrencies = new Dictionary<int, string>();
            try
            {
                var rawData = (new ExchangeCurrencyRepository()).GetExchangeCurrencies(criteria);
                if (rawData != null)
                {
                    rawData.ForEach(currency =>
                    {
                        if (!exchangeCurrencies.ContainsKey(currency.CurrencyId))
                        {
                            exchangeCurrencies.Add(currency.CurrencyId, currency.CurrencyName);
                        }
                    });
                }

            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return exchangeCurrencies;
        }

        #endregion StaticMethods
    }
}