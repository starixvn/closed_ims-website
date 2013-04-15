// -----------------------------------------------------------------------
// <copyright file="ICommonService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using Fanex.Ares.Core.Criteria;
using Fanex.Ares.Core.Models;

namespace Fanex.Ares.Core.Services.Interface
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICoreService
    {
        IList<Bettypes> GetBettypes(GetBettypeCriteria criteria);

        IList<Licensee> GetAllLicensees();

        MatchInfo GetMatchInfo(GetMatchInfoCriteria criteria);

        IList<Currency> GetCurrency();

        void Test();

        ActionResult CheckMatchIdWithRefno(CheckMatchIdWithRefNoCriteria criteria);
    }
}
