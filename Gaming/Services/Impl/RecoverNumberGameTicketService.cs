using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Gaming.Services.Interface;
using Fanex.TicketControl.Gaming.ViewModels;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Gaming.Criteria;
using Fanex.TicketControl.Core.Services.Interface;
using Fanex.TicketControl.Gaming.Repositories.Interface;
using Fanex.TicketControl.Core.Services.Impl;
using Fanex.TicketControl.Gaming.Repositories.Impl;
using Fanex.TicketControl.Core.Criteria;
using System.Data;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.TicketHelper;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.Builder;
using Fanex.Common;
using Fanex.TicketControl.Core.Utils;
using Fanex.TicketControl.Gaming.Entities;

namespace Fanex.TicketControl.Gaming.Services.Impl
{
    public class RecoverNumberGameTicketService : IRecoverNumberGameTicketService
    {
        ICoreService _coreService;
        IRecoverNumberGameTicketRepository _repository;
        IMessageService _messageService;

        public RecoverNumberGameTicketService()
        {
            _coreService = new CoreService();
            _repository = new RecoverNumberGameTicketRepository();
            _messageService = new MessageService();
        }

        public NumberGameTicketModel GetNumberGameTicketRecover(GetRecoverNumberGameTicketCriteria criteria, ref ActionResult result)
        {
            NumberGameTicketModel model = new NumberGameTicketModel();
            try
            {

                GetMatchInfoCriteria _matchInfoCriteria = new GetMatchInfoCriteria();
                _matchInfoCriteria.TransId = criteria.TransId;

                model.MatchInfo = _coreService.GetMatchInfo(_matchInfoCriteria);
                DataTable dt = _repository.GetNumberGameTicketRecover(criteria, ref result);

                model.Groups.Add(AssignDataOnGroup(dt));
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return model;
        }

        private static Group AssignDataOnGroup(DataTable dt)
        {
            // Now, we only have one Normal group.
            Group group = new Group();
            group.Name = "Normal";
            group.DeleteOtionDefault = true;
            group.IsAllowOptionDelete = true;
            var rows = dt.AsEnumerable();

            ITicketHelper ticketHelper = new TicketHelper();
            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            GetCachePropertyById funcGetOddsTypeName = Fanex.TicketControl.Core.Services.Impl.CacheRef.GetOddsTypeName;
            ticketHelper.ShowBetType = false;
            ticketHelper.ShowLeague = false;
            ticketHelper.ShowMatch = false;

            int index = 1;
            foreach (var row in rows)
            {
                var ticket = MapTicket(row);
                var numberGameTicket = new NumberGameTicket();
                numberGameTicket.Ticket = ticket;

                if (ticket.BetTypeId == 9 || ticket.BetTypeId == 29)
                {
                    ticketHelper.IsShowOddsWithChoice = true;
                }
                numberGameTicket.No = index;
                numberGameTicket.Currency = Helper.CurrencyById(Convert.ToInt32(row["currency"]));
                numberGameTicket.ChoiceHtml = builder.BuildChoice(ticket, ticketHelper, null, false);
                numberGameTicket.StakeHtml = builder.BuildStake(ticket);
                numberGameTicket.OddHtml = builder.BuildOdds(ticket, null, funcGetOddsTypeName);
                group.Tickets.Add(numberGameTicket);

                index++;
            }
            return group;
        }

        public ActionResult RecoverNumberGameTicket(RecoverNumberGameTicketCriteria criteria)
        {
            ActionResult result = new ActionResult();

            try
            {
                DataSet ds = _repository.RecoverNumberGameTicket(criteria, ref result);
                DataTable dtMsg = (ds != null && ds.Tables.Count > 0) ? ds.Tables[0] : null;
                string matchId = "0";
                var resultPrivateMsg = _messageService.SendPrivateMsgToUsers(dtMsg, matchId, criteria.Msg);

                if (resultPrivateMsg.ErrorCode != 0)
                {
                    result = resultPrivateMsg;
                }

                if (result.ErrorCode == 0)
                {
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

        #region Helpers

        private static Ticket MapTicket(DataRow row)
        {
            var ticket = new Ticket
            {
                UserName = Converter.ToString2(row["username"]),
                TransId = Converter.ToInt(row["transid"]),
                TransDate = Converter.ToDateTime(row["transdate"]),
                Odds = Converter.ToDecimal(row["odds"]),
                OddsType = Converter.ToString2(row["OddsType"]),
                BetTypeId = Converter.ToInt(row["bettype"]),
                BetTeam = Converter.ToString2(row["betteam"]),
                Handicap1 = Converter.ToDecimal(row["hdp1"]),
                Handicap2 = Converter.ToDecimal(row["hdp2"]),
                LiveHomeScore = Converter.ToInt(row["livehomescore"]),
                LiveAwayScore = Converter.ToInt(row["liveawayscore"]),
                Stake = Converter.ToDecimal(row["stake"]),
                Winlost = Converter.ToDecimal(row["winlost"]),
                Comm = Converter.ToDecimal(row["comm"]),
                Status = Converter.ToString2(row["status"]),
                IsLive = Converter.ToBool(row["liveindicator"]),
            };

            long transid;
            bool result = long.TryParse(row["transid"].ToString(), out transid);
            ticket.TransId = transid;
            return ticket;
        }

        #endregion
    }
}
