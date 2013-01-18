// --------------------------------------------------------------------------
// <copyright file="IMostVisitedPagesService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.Core.Services.Interface
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IMostVisitedPagesService
    {
        IEnumerable<MostVisitedPage> GetMostvisitedPages();

        void UpdateCount(MostVisitedPage mostVisitedPage);

        void UpdatePosition(ICriteria mostVisitedPage);

        void Delete(ICriteria mostVisitedPage);
    }
}
