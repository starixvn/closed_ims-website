// -----------------------------------------------------------------------
// <copyright file="SingleTickketService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Robert.Tran" date="2013.2.05">Created</change>
// </history>

namespace Fanex.TicketControl.SportBook.Services.Implement
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Core.Models;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Services.Interface;
    using Fanex.TicketControl.SportBook.Services.Interface;
    using Fanex.TicketControl.SportBook.ViewModels.SingleTicket;
    using Fanex.TicketControl.SportBook.Criteria;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.TicketControl.Misc.Repositories.Interface;
    using Fanex.TicketControl.Misc.Repositories.Impl;
    using Fanex.TicketControl.SportBook.Models.SingleTicket;
    using Fanex.TicketControl.Core.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class VoidSingleTicketService : ServiceBase, IVoidSingleTicketService
    {
        ICoreService _coreService;
        IVoidSingleTicketRepository _reponsitory;
        public VoidSingleTicketService()
        {
            _coreService = new CoreService();
            _reponsitory = new VoidSingleTicketRepository();
        }

        public SingleTicketModel GetSingleTicket(GetSingleTicketCriteria criteria)
        {
            SingleTicketModel model = new SingleTicketModel();
            
            try
            {
                GetMatchInfoCriteria criteriaMatchInfo = new GetMatchInfoCriteria();
                criteriaMatchInfo.MatchId = criteria.MatchId;

                model.MatchInfo = _coreService.GetMatchInfo(criteriaMatchInfo);

                var ds = _reponsitory.GetSingleTicket(criteria);
                int cnt = ds.Tables.Count;
                DataTable dt = cnt > 0 ? ds.Tables[0] : new DataTable();

                model.Groups.Add(AssignDataOnGroup(dt));
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return model;
        }

        private Group AssignDataOnGroup(DataTable dt)
        {
            Group group = new Group();
            group.DeleteOtionDefault = true;
            group.IsAllowOptionDelete = true;
            group.Name = "Normal";
            group.Tickets = new List<TicketInfo>();

            try
            {
                DataRow[] rows = dt.Select();
                List<TicketInfo> tickets = new List<TicketInfo>();

                int index = 0;
                foreach (DataRow row in rows)
                {
                    index++;
                    
                    tickets.Add(new TicketInfo() { 
                        No = index,
                        BetId = Convert.ToString(row["transid"]),
                        Player = Convert.ToString(row["username"]),
                        TransDate = Convert.ToDateTime(row["transdate"]).ToString(),
                        BetType = Convert.ToInt32(row["bettype"]),
                        HDP = Convert.ToString(row["hdp1"]),
                        Stake = Convert.ToString(row["stakepositiontaking"]),
                        Odds = Convert.ToString(row["odds"]),
                        CurrencyName = Helper.CurrencyById(Convert.ToInt32(row["currency"])),
                        Status = Convert.ToString(row["status"]),
                        Score = Convert.ToString(row["livehomescore"]) + "-" + Convert.ToString(row["liveawayscore"]),
                        SA = Convert.ToInt32(row["SecAcceptFlag"]) == 1 ? "" : "<span class='icon-tick'></span>",
                        IsDelete = true,
                        BetTypeName = CacheRef.GetBettype(row["bettype"]),
                        Choice = "WHAT???"
                    });
                }

                group.Tickets = tickets;
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return group;
        }
    }
}
