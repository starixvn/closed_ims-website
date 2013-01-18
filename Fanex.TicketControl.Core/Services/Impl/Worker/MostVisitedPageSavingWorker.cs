// --------------------------------------------------------------------------
// <copyright file="MostVisitedPageSavingWorker.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2012.12.17">Created</change>
// </history>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using Fanex.TicketControl.Core.Configurations;
using Fanex.TicketControl.Core.Criteria;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Repository.Impl;
using Fanex.TicketControl.Core.Web;

namespace Fanex.TicketControl.Core.Services.Impl.Worker
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class MostVisitedPageSavingWorker : BaseWorker
    {
        public MostVisitedPageSavingWorker(string threadName)
            : base(threadName)
        {
        }

        /// <summary>
        /// Run Main Function
        /// </summary>
        protected override void Run()
        {
            while (true)
            {
                Thread.Sleep(AppSettings.IntervalGetMostVisitedPage * 1000);

                if (this.IsStop)
                {
                    break;
                }

                this.SaveMostVisitedPageCount();
            }
        }

        private void SaveMostVisitedPageCount()
        {
            try
            {
                lock (Constants.MOST_VISITED_PAGE_LOCK_OBJECT)
                {
                    List<MostVisitedPage> mostVisitedPages = CacheManager.Get<List<MostVisitedPage>>(Constants.MOST_VISITED_PAGE_CACHE_KEY);
                    if (mostVisitedPages != null && mostVisitedPages.Count > 0)
                    {
                        //// Save to Database

                        var criteria = new UpdateMostVisitedPageCountCriteria
                        {
                            CustId = mostVisitedPages.First().UserId,
                            MenuId = string.Join<string>(Constants.SP_PARAMS_SEPERATOR,
                                        mostVisitedPages.Select(mvp => mvp.MenuId.ToString())),
                            Count = string.Join<string>(Constants.SP_PARAMS_SEPERATOR,
                                        mostVisitedPages.Select(mvp => mvp.Count.ToString())),
                        };

                        var repository = new MostVisitedPagesRepository();
                        repository.UpdateCount(criteria);

                        //// Update Cache
                        CacheManager.Set<List<MostVisitedPage>>(Constants.MOST_VISITED_PAGE_CACHE_KEY, new List<MostVisitedPage>());
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
        }
    }
}
