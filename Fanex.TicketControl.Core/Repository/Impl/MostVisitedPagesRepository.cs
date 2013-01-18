// --------------------------------------------------------------------------
// <copyright file="MostVisitedPagesRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Fanex.Common.Extensions;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Criteria;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Repository.Interface;

namespace Fanex.TicketControl.Core.Repository.Impl
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class MostVisitedPagesRepository : BaseRepository<MostVisitedPage>, IMostVisitedPagesRepository
    {
        public IEnumerable<MostVisitedPage> FetchMostvisitedPages(SelectMostvisitedPageCriteria criteria)
        {
            //// Create Function will map Data from DataReader to Schedule Object
            Func<IDataReader, MostVisitedPage> mapObjectFunc = (reader) =>
            {
                return new MostVisitedPage
                {
                    MenuId = reader.To<int>("MenuId"),
                    Count = reader.To<ulong>("VisitedCount"),
                    Position = reader.To<int>("Position")
                };
            };

            return base.Fetch<MostVisitedPage>(criteria, mapObjectFunc);
        }

        public void UpdateCount(ICriteria mostVisitedPage)
        {
            base.ExecuteCommand(mostVisitedPage);
        }

        public void UpdatePosition(ICriteria mostVisitedPage)
        {
            base.ExecuteCommand(mostVisitedPage);
        }

        public void Delete(ICriteria mostVisitedPage)
        {
            base.ExecuteCommand(mostVisitedPage);
        }
    }
}
