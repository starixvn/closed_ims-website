using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.SportBook.Services.Interface;
using Fanex.TicketControl.SportBook.ViewModels.SingleTicket;
using Fanex.TicketControl.SportBook.Criteria;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Services.Impl;
using Fanex.TicketControl.Core.Services.Interface;
using Fanex.TicketControl.SportBook.Repositories.Interface;
using Fanex.TicketControl.SportBook.Repositories.Impl;
using Fanex.TicketControl.Core.Criteria;
using System.Data;
using Fanex.TicketControl.SportBook.Models.SingleTicket;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.Builder;
using Fanex.TicketControl.Core.Utils;
using Fanex.BetList.Core.TicketHelper;

namespace Fanex.TicketControl.SportBook.Services.Implement
{
    public class RecoverTicketService : IRecoverTicketService
    {
        ICoreService _coreService;
        IRecoverTicketRepository _repository;
        IMessageService _messageService;

        public RecoverTicketService() {
            _coreService = new CoreService();
            _repository = new RecoverTicketRepository();
            _messageService = new MessageService();
        }

        public SingleTicketModel GetTicketRecover(GetRecoverTicketCriteria criteria, ref ActionResult result)
        {
            SingleTicketModel model = new SingleTicketModel();
            try
            {

                GetMatchInfoCriteria _matchInfoCriteria = new GetMatchInfoCriteria();
                _matchInfoCriteria.TransId = criteria.TransId;

                model.MatchInfo = _coreService.GetMatchInfo(_matchInfoCriteria);
                DataTable dt = _repository.GetTicketRecover(criteria, ref result);

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
                        
                        Player = Convert.ToString(row["username"]),
                        TransDate = Convert.ToDateTime(row["transdate"]).ToString("yyyy/MM/dd h:m:s tt"),
                        BetType = Convert.ToInt32(row["bettype"]),
                        Stake = Convert.ToString(row["stakepositiontaking"]),
                        Odds = Convert.ToString(row["odds"]),
                        CurrencyName = Helper.CurrencyById(Convert.ToInt32(row["currency"])),
                        Status = Convert.ToString(row["status"]),
                        Score = Convert.ToString(row["livehomescore"]) + "-" + Convert.ToString(row["liveawayscore"]),                        
                        IsDelete = true,
                        BetTypeName = Fanex.TicketControl.Core.Services.Impl.CacheRef.GetBettype(row["bettype"]),


                    };

                    tickets.Add(ticketInfo);

                    TicketDetail ticketDetail = GetTicketDetail(dt, BetId);

                    ticketInfo.Choice = ticketDetail.ChoiceHtml;
                    ticketInfo.Odds = ticketDetail.OddHtml;
                    ticketInfo.Stake = ticketDetail.StakeHtml;
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
                var rows = dt.AsEnumerable().Where(s => Convert.ToInt64(s["transid"]) == TransId).Select(s => s).ToList();

                int bettype = 0;
                foreach (DataRow row in rows)
                {
                    bettype = Convert.ToInt32(row["bettype"]);

                    ticket.Ticket = new Ticket()
                    {
                        BetId = Convert.ToInt64(row["transid"]),
                        BetTypeId = bettype,                                                
                        BetTeam = Convert.ToString(row["betteam"]),
                        LiveHomeScore = Convert.ToInt32(row["livehomescore"]),
                        LiveAwayScore = Convert.ToInt32(row["liveawayscore"]),

                        Stake = Convert.ToDecimal(row["Stakepositiontaking"]),
                        Odds = Convert.ToDecimal(row["odds"]),
                        Handicap1 = Convert.ToDecimal(row["hdp1"]),
                        Handicap2 = Convert.ToDecimal(row["hdp2"]),

                        Status = Convert.ToString(row["status"]),
                        HomeId = 9079,
                        AwayId = 844,
                        LeagueId = 4114,
                        SportTypeId = 1
                    };

                    ITicketHelper ticketHelper = new TicketHelper();
                    BetListHTMLFactory factory = new BetListHTMLFactory();
                    BetListHTMLBuilder builder = factory.CreateBuilder();
                    GetCachePropertyById funcGetOddsTypeName = Fanex.BetList.Core.TicketHelper.CacheRef.GetOddsTypeName;

                    if (bettype == 9 || bettype == 29)
                    {
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


        public ActionResult RecoverTicket(RecoverTicketCriteria criteria)
        {
            ActionResult result = new ActionResult();
            try
            {
                DataSet ds = _repository.RecoverTicket(criteria, ref result);
                DataTable dtMsg = ds.Tables[0];
                string matchId = "0";
                var resultPrivateMsg = _messageService.SendPrivateMsgToUsers(dtMsg, matchId, criteria.Msg);

                if (resultPrivateMsg.ErrorCode != 0)
                {
                    result = resultPrivateMsg;
                }

                if (result.ErrorCode == 0) {
                    result.ErrorCode = -100;
                    result.ErrorMsg = "Transaction " + criteria.TransId + " was recovered.";
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                result.ErrorCode = 1;
                result.ErrorMsg = ex.Message;
            }
            return result;
        }
    }
}
