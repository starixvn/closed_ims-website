namespace Fanex.Ares.Core.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.Mvc;
    using Fanex.Ares.Core.Repository.Impl;
    using Fanex.Ares.Core.Services.Interfaces;
    using Fanex.Artist.Services;
    using Fanex.Common.Helper;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Service base.
    /// </summary>
    public class ServiceBase : IServiceBase
    {
        #region Fields

        private readonly IDynamicRepository _dynamicRepository;

        private readonly BaseRepository _baseRepository;

        private readonly ICurrentContext _context;

        private readonly ICurrentSession _session;

        #endregion Fields

        #region Constructors

        public ServiceBase(IDynamicRepository repository = null)
        {
            _dynamicRepository = repository ?? new DynamicRepository();
            _baseRepository = new BaseRepository();
            _context = new CurrentContext();
            _session = new CurrentSession();
        }

        #endregion Constructors

        #region Properties

        protected IDynamicRepository Repository
        {
            get { return _dynamicRepository; }
        }

        protected BaseRepository BaseRepository
        {
            get { return _baseRepository; }
        }

        protected ICurrentContext Context
        {
            get { return _context; }
        }

        protected ICurrentSession Session
        {
            get { return _session; }
        }

        #endregion Properties

        /// <summary>
        /// Get Entities.
        /// </summary>
        /// <typeparam name="T">Type of Object return.</typeparam>
        /// <param name="criteria">Criteria to Query DB.</param>
        /// <returns>Enumerable of Type T.</returns>
        public virtual IEnumerable<T> GetEntites<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _dynamicRepository.Fetch<T>(criteria);
                }
            }
            catch (SqlException exception)
            {
                LoggingHandler.Log(exception.Message);
                throw new ServiceException(exception.Message, criteria.GetSettingKey(), criteria.GetSpParams());
            }

            return new List<T>();
        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <typeparam name="T">The input object type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns a object T.</returns>
        public T GetEntity<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _dynamicRepository.Get<T>(criteria);
                }
            }
            catch (SqlException exception)
            {
                LoggingHandler.Log(exception.Message);
                throw new ServiceException(exception.Message, criteria.GetSettingKey(), criteria.GetSpParams());
            }

            return (T)new object();
        }

        #region Get data for combobox

        /// <summary>
        /// Get Base Currency.
        /// </summary>
        /// <param name="criteria">Base currency criteria.</param>
        /// <returns>Return result.</returns>
        public IEnumerable<SelectListItem> GetBaseCurrency(ICriteria criteria)
        {
            IList<SelectListItem> listProducts = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    return new SelectListItem
                    {
                        Value = Convert.ToString(r["currency"]),
                        Text = Convert.ToString(r["currencyname"]),
                        Selected = string.Equals(Convert.ToString(r["currencyname"]), "GBP", StringComparison.OrdinalIgnoreCase)
                    };
                };

                listProducts = BaseRepository.Fetch<SelectListItem>(criteria, mappingFunc);
            }
            catch (SqlException exception)
            {
                LoggingHandler.Log(exception.Message);
                throw new ServiceException(exception.Message, criteria.GetSettingKey());
            }

            return listProducts;
        }

        #endregion Get data for combobox
    }
}