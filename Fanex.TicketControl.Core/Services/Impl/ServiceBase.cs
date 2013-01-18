// --------------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.10.9">Created</change>
//   <change who="Ricky.Nguyen" date="2012.11.6">Move</change>
// </history>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Fanex.Common.Helper;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Services.Interface;

namespace Fanex.TicketControl.Core.Services.Impl
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ServiceBase : IServiceBase
    {
        protected readonly IDynamicRepository DynamicRepository;

        public ServiceBase(IDynamicRepository repository = null)
        {
            DynamicRepository = repository ?? new DynamicRepository();
        }

        /// <summary>
        ///  Get dynamic repository
        /// </summary>
        protected IDynamicRepository Repository
        {
            get { return DynamicRepository; }
        }

        /// <summary>
        /// Handler Exception
        /// </summary>
        /// <param name="ex"></param>
        public void HandleException(Exception ex)
        {
            //LogService.Error(ex.Message, ex);
            LoggingHandler.Log(ex);
        }

        /// <summary>
        /// Get Report Data with Criteria
        /// </summary>
        /// <typeparam name="T">Type of Object return</typeparam>
        /// <param name="criteria">Criteria to Query DB</param>
        /// <returns>Enumerable of Type T</returns>
        public virtual IEnumerable<T> GetReportData<T>(ICriteria criteria)
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
                HandleException(ex);
                return null;
            }
        }
    }
}