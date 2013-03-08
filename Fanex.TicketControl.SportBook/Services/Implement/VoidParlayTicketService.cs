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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Core.Models;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Services.Interface;
    using Fanex.TicketControl.Core.Utils;
    using Fanex.TicketControl.SportBook.Criteria;    
    using Fanex.TicketControl.SportBook.Repositories.Impl;
    using Fanex.TicketControl.SportBook.Repositories.Interface;
    using Fanex.TicketControl.SportBook.Services.Interface;
    using Fanex.TicketControl.SportBook.ViewModels.ParlayTicket;
    using Fanex.TicketControl.SportBook.Models.ParlayTicket;    

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class VoidParlayTicketService : ServiceBase, IVoidParlayTicketService
    {
        ICoreService _coreService;
        IVoidParlayTicketRepository _reponsitory;
        IMessageService _messageService;
        public VoidParlayTicketService()
        {
            _coreService = new CoreService();
            _reponsitory = new VoidParlayTicketRepository();
            _messageService = new MessageService();
        }

        public ParlayTicketModel GetParlayTicket(GetParlayTicketCriteria criteria)
        {
            ParlayTicketModel model = new ParlayTicketModel();
            
            try
            {
                GetMatchInfoCriteria criteriaMatchInfo = new GetMatchInfoCriteria();
                criteriaMatchInfo.MatchId = criteria.MatchId;

                model.MatchInfo = _coreService.GetMatchInfo(criteriaMatchInfo);

                var ds = _reponsitory.GetParlayTicket(criteria);
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
                Int64 BetId = 0;
                foreach (DataRow row in rows)
                {
                    index++;
                    BetId = Convert.ToInt64(row["transid"]);

                    var ticketInfo = new TicketInfo()
                    {
                        No = index,
                        BetId = Convert.ToString(BetId),
                        SubTransId = Convert.ToString(row["SubTransId"]),
                        Player = Convert.ToString(row["username"]),
                        TransDate = Convert.ToDateTime(row["transdate"]).ToString("yyyy/MM/dd h:m:s tt"),
                        BetType = Convert.ToInt32(row["bettype"]),                        
                        Stake = Convert.ToString(row["stake"]),
                        Odds = Convert.ToString(row["odds"]),
                        CurrencyName = Helper.CurrencyById(Convert.ToInt32(row["currency"])),
                        Status = Convert.ToString(row["status"]),
                        Score = Convert.ToString(row["choice_livehomescore"]) + "-" + Convert.ToString(row["choice_liveawayscore"]),
                        WinLoss = Convert.ToString(row["winlost"]), 
                        IsDelete = true,
                        BetTypeName = Fanex.TicketControl.Core.Services.Impl.CacheRef.GetBettype(row["bettype"]),

                        
                    };

                    tickets.Add(ticketInfo);
                    
                    TicketDetail ticketDetail = GetTicketDetail(dt, BetId);

                    ticketInfo.Choice = ticketDetail.ChoiceHtml;
                    ticketInfo.Odds = ticketDetail.OddHtml;
                    ticketDetail.StakeHtml = ticketDetail.StakeHtml;
                }

                group.Tickets = tickets;
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return group;
        }

        private TicketDetail GetTicketDetail(DataTable dt, Int64 TransId)
        {
            var ticket = new TicketDetail();

            try
            {
                var rows = dt.AsEnumerable().Where(s=> Convert.ToInt64(s["transid"]) == TransId).Select(s=>s).ToList();

                int bettype = 0;
                foreach(DataRow row in rows)
                {
                    bettype = Convert.ToInt32(row["bettype"]);

                    ticket.Ticket = new Ticket(){
                        BetId = Convert.ToInt64(row["transid"]),
                        BetTypeId = bettype,
                        TransId = Convert.ToInt64(row["subtransID"]),
                        RefNo = Convert.ToString(row["Refno"]),
                        BetTeam = Convert.ToString(row["choice_betteam"]),
                        LiveHomeScore = Convert.ToInt32(row["choice_livehomescore"]),
                        LiveAwayScore = Convert.ToInt32(row["choice_liveawayscore"]),

                        Stake = Convert.ToDecimal(row["Stake"]),
                        Odds = Convert.ToDecimal(row["odds"]),
                        Handicap1 = Convert.ToDecimal(row["choice_hdp1"]),
                        Handicap2 = Convert.ToDecimal(row["choice_hdp2"]),

                        Status = Convert.ToString(row["status"]),
                        HomeId = Convert.ToInt64(row["choice_HomeId"]),
                        AwayId = Convert.ToInt64(row["choice_AwayId"]),
                        LeagueId = Convert.ToInt32(row["choice_LeagueId"]),
                        SportTypeId = Convert.ToInt32(row["choice_SportType"])
                    };

                    ITicketHelper ticketHelper = new TicketHelper();
                    BetListHTMLFactory factory = new BetListHTMLFactory();
                    BetListHTMLBuilder builder = factory.CreateBuilder();
                    GetCachePropertyById funcGetOddsTypeName = Fanex.BetList.Core.TicketHelper.CacheRef.GetOddsTypeName;

                    if (bettype == 9 || bettype == 29) {
                        ticketHelper.IsShowOddsWithChoice = true;
                    }

                    ticket.ChoiceHtml = builder.BuildChoice(ticket.Ticket, ticketHelper, null, false);
                    ticket.StakeHtml = builder.BuildStake(ticket.Ticket);
                    ticket.OddHtml = builder.BuildOdds(ticket.Ticket, null, funcGetOddsTypeName);
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return ticket;
        }

        public string GetParlaySubTicket(GetParlaySubTicketCriteria criteria)
        {
            List<string> subTickets = new List<string>();
            try
            {
                DataTable dtSubTickets = _reponsitory.GetParlaySubTicket(criteria);

                var rows = dtSubTickets.Select();

                int bettype = 0;
                Int64 TransId = 0;
                foreach (DataRow row in rows)
                {
                    bettype = Convert.ToInt32(row["bettype"]);
                    TransId = Convert.ToInt64(row["TransId"]);
                    var ticket = new TicketDetail();

                    ticket.Ticket = new Ticket()
                    {
                        BetId = TransId,
                        BetTypeId = bettype,
                        TransId = TransId,
                        BetTeam = Convert.ToString(row["betteam"]),
                        LiveHomeScore = Convert.ToInt32(row["livehomescore"]),
                        LiveAwayScore = Convert.ToInt32(row["liveawayscore"]),

                        Stake = Convert.ToDecimal(row["StakePositionTaking"]),
                        Odds = Convert.ToDecimal(row["odds"]),
                        Handicap1 = Convert.ToDecimal(row["hdp1"]),
                        Handicap2 = Convert.ToDecimal(row["hdp2"]),

                        HomeId = Convert.ToInt64(row["HomeId"]),
                        AwayId = Convert.ToInt64(row["AwayId"]),
                        LeagueId = Convert.ToInt32(row["LeagueId"]),
                        SportTypeId = Convert.ToInt32(row["SportType"]),
                        IsNeutral = Convert.ToBoolean(row["IsNeutral"]),
                        IsLive = Convert.ToBoolean(row["liveindicator"]),
                    };

                    ITicketHelper ticketHelper = new TicketHelper();
                    BetListHTMLFactory factory = new BetListHTMLFactory();
                    BetListHTMLBuilder builder = factory.CreateBuilder();


                    if (bettype == 9 || bettype == 29)
                    {
                        
                        ticketHelper.IsShowOddsWithChoice = true;
                    }

                    
                    ticket.ChoiceHtml = builder.BuildChoice(ticket.Ticket, ticketHelper, null, false);

                    if (TransId == criteria.TransId)
                    {
                        subTickets.Add("<div class='selected'>" + ticket.ChoiceHtml + "</div>");
                    }
                    else 
                    {
                        subTickets.Add(ticket.ChoiceHtml);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return string.Join("<div class='line'></div>", subTickets);
        }

        public IList<ActionResult> VoidParlayTicket(VoidParlayTicketCritetia criteria)
        {
            IList<ActionResult> results = new List<ActionResult>();
            try
            {
                criteria.Msg.CreatorId = criteria.UserId;
                criteria.Msg.CreatorName = criteria.UserName;

                DataSet dsOutData = new DataSet();
                results = _reponsitory.VoidParlayTicket(criteria, ref dsOutData);

                DataTable dtMsg = new DataTable();
                if (dsOutData.Tables.Count > 0)
                {
                    dtMsg = dsOutData.Tables[0];
                    var resultPrivateMsg  = _messageService.SendPrivateMsgToUsers(dtMsg, Convert.ToString(criteria.MatchId), criteria.Msg);

                    if (resultPrivateMsg.ErrorCode != 0)
                    {
                        results.Add(resultPrivateMsg);
                    }
                }

                if (results.Count == 1 && results[0].ErrorCode == 0) {
                    results[0].ErrorCode = -100;
                    results[0].ErrorMsg = "Bet transactions deleted!";
                }
                
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                results.Add(new ActionResult() { 
                    ErrorCode = 1,
                    ErrorMsg = ex.Message
                });
            }


            return results;
        }
                
    }
}
