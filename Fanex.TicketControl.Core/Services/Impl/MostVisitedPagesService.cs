// --------------------------------------------------------------------------
// <copyright file="MostVisitedPagesService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System;
using System.Linq;
using Fanex.Common;
using Fanex.NAP.Core.Service.Impl;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Configurations;
using Fanex.TicketControl.Core.Criteria;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Repository.Impl;
using Fanex.TicketControl.Core.Repository.Interface;
using Fanex.TicketControl.Core.Services.Interface;
using Fanex.TicketControl.Core.Web;

namespace Fanex.TicketControl.Core.Services.Impl
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class MostVisitedPagesService : BaseService, IMostVisitedPagesService
    {
        private readonly IMostVisitedPagesRepository _mostVisitedPagesRepository;
        private readonly MenuService _menuService;

        public MostVisitedPagesService()
            : this(null, null)
        {
        }

        public MostVisitedPagesService(IMostVisitedPagesRepository mostVisitedPagesRepository,
            MenuService menuService)
        {
            this._mostVisitedPagesRepository = mostVisitedPagesRepository ?? new MostVisitedPagesRepository();
            this._menuService = menuService ?? new MenuService();
        }

        private bool IsInRange(int position, int maxPages)
        {
            return 1 <= position && position <= maxPages;
        }

        private void SortMostvisitedPages(ref List<MostVisitedPage> mostvisitedPages)
        {
            try
            {
                if (mostvisitedPages != null)
                {
                    mostvisitedPages = mostvisitedPages.Take(SelectMostvisitedPageCriteria.MaxPages).ToList();

                    var pinSite = mostvisitedPages.Where(mvp => this.IsInRange(mvp.Position, SelectMostvisitedPageCriteria.MaxPages)).ToList();
                    var unpinSite = mostvisitedPages.Where(mvp => !this.IsInRange(mvp.Position, SelectMostvisitedPageCriteria.MaxPages)).ToList();
                    mostvisitedPages = unpinSite;

                    for (int i = 0; i < pinSite.Count; i++)
                    {
                        var mostVisitedPage = pinSite[i];
                        if (mostVisitedPage.Position <= mostvisitedPages.Count)
                        {
                            mostvisitedPages.Insert(mostVisitedPage.Position - 1, mostVisitedPage);
                        }
                        else
                        {
                            mostvisitedPages.Add(mostVisitedPage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        public IEnumerable<MostVisitedPage> GetMostvisitedPages()
        {
            try
            {
                if (!AppSettings.IsDisableMostvisitedPage)
                {
                    var rawData = this._mostVisitedPagesRepository.FetchMostvisitedPages(new SelectMostvisitedPageCriteria());
                    var mostvisitedPages = new List<MostVisitedPage>();
                    foreach (var row in rawData)
                    {
                        var menu = this._menuService.GetMenuById(Converter.ToInt(row.MenuId));
                        if (menu != null)
                        {
                            mostvisitedPages.Add(new MostVisitedPage
                            {
                                MenuName = menu.Name,
                                MenuURL = menu.Url.Trim(),
                                MenuId = menu.Id,
                                Count = row.Count,
                                Position = Converter.ToInt(row.Position),
                                Title = (menu.Parent != null
                                            ? menu.Parent.Name + MenuService.URL_SEPARATOR
                                            : string.Empty) + menu.Name
                            });
                        }
                    }

                    this.SortMostvisitedPages(ref mostvisitedPages);
                    return mostvisitedPages;
                }
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return null;
        }

        public void UpdateCount(MostVisitedPage mostVisitedPage)
        {
            try
            {
                if (!AppSettings.IsDisableMostvisitedPage)
                {
                    lock (Constants.MOST_VISITED_PAGE_LOCK_OBJECT)
                    {
                        ////Update to Memory
                        List<MostVisitedPage> mostVisitedPages = CacheManager.Get<List<MostVisitedPage>>(Constants.MOST_VISITED_PAGE_CACHE_KEY);
                        if (mostVisitedPages == null)
                        {
                            mostVisitedPages = new List<MostVisitedPage>();
                        }

                        var mostVisitedPagesCount = mostVisitedPages.Count;
                        var isExist = false;
                        for (int i = 0; i < mostVisitedPagesCount; i++)
                        {
                            if (mostVisitedPages[i].MenuId == mostVisitedPage.MenuId)
                            {
                                mostVisitedPages[i].Count++;
                                isExist = true;
                                break;
                            }
                        }

                        if (!isExist)
                        {
                            mostVisitedPages.Add(new MostVisitedPage
                            {
                                UserId = AppSession.CurrentUser.Id,
                                MenuId = mostVisitedPage.MenuId
                            });
                        }

                        //// Save to cache.
                        CacheManager.Set<List<MostVisitedPage>>(Constants.MOST_VISITED_PAGE_CACHE_KEY, mostVisitedPages);
                    }
                }
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        public void UpdatePosition(ICriteria mostVisitedPage)
        {
            try
            {
                if (!AppSettings.IsDisableMostvisitedPage)
                {
                    this._mostVisitedPagesRepository.UpdatePosition(mostVisitedPage);
                }
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        public void Delete(ICriteria mostVisitedPage)
        {
            try
            {
                if (!AppSettings.IsDisableMostvisitedPage)
                {
                    this._mostVisitedPagesRepository.Delete(mostVisitedPage);
                }
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }
    }
}