namespace Fanex.Ares.Core.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Mvc;
    using Fanex.Ares.Core.Criteria;
    using Fanex.Ares.Core.Repository.Impl;
    using Fanex.Ares.Core.Services.Interface;
    using Fanex.Caching;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Service base.
    /// </summary>
   public class ServiceBase : IServiceBase
    {
        protected readonly IDynamicRepository DynamicRepository;

        protected readonly ICache CacheManager;

        protected readonly BaseRepository BaseRepository;

        protected readonly ICurrentContext Context;

        protected readonly ICurrentSession Session;

        public ServiceBase(IDynamicRepository repository = null)
        {
            this.DynamicRepository = repository ?? new DynamicRepository();
            this.CacheManager = CacheService.CreateInstance();
            this.BaseRepository = new BaseRepository();
            this.Context = this.Context ?? new CurrentContext();
            this.Session = this.Session ?? new CurrentSession();
        }

        /// <summary>
        ///  Gets dynamic Repository.
        /// </summary>
        /// <value>The Repository value.</value>
        protected IDynamicRepository Repository
        {
            get { return this.DynamicRepository; }
        }

        /// <summary>
        /// Get Report Data with Criteria.
        /// </summary>
        /// <typeparam name="T">Type of Object return.</typeparam>
        /// <param name="criteria">Criteria to Query DB.</param>
        /// <returns>Enumerable of Type T.</returns>
        public virtual IEnumerable<T> GetListData<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return DynamicRepository.Fetch<T>(criteria);
                }

                return new List<T>();
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                return null;
            }
        }

        #region Get data for combobox
        /// <summary>
        /// Get Base Currency.
        /// </summary>
        /// <param name="criteria">Base currency criteria.</param>
        /// <returns>Return result.</returns>
        public IEnumerable<SelectListItem> GetBaseCurrency(IMS_BaseCurrencySel criteria)
        {
            IList<SelectListItem> listProducts = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    return new SelectListItem
                    {
                        Value = Convert.ToString(r["currency"]),
                        Text = Convert.ToString(r["currencyname"])
                    };
                };

                listProducts = BaseRepository.Fetch<SelectListItem>(criteria, mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listProducts;
        }
        #endregion
    }
}