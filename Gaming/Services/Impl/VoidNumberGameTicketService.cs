// -----------------------------------------------------------------------
// <copyright file="SingleTickketService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Robert.Tran" date="2013.2.05">Created</change>
// </history>

namespace Fanex.TicketControl.Gaming.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Common;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Core.Models;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Services.Interface;
    using Fanex.TicketControl.Core.Utils;
    using Fanex.TicketControl.Gaming.Criteria;
    using Fanex.TicketControl.Gaming.Repositories.Impl;
    using Fanex.TicketControl.Gaming.Repositories.Interface;
    using Fanex.TicketControl.Gaming.Services.Interface;
    using Fanex.TicketControl.Gaming.ViewModels;
    using Fanex.TicketControl.Gaming.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class VoidNumberGameTicketService : ServiceBase, IVoidNumberGameTicketService
    {
        ICoreService _coreService;
        IVoidNumberGameTicketRepository _repository;
        IMessageService _messageService;

        public VoidNumberGameTicketService()
        {
            _coreService = new CoreService();
            _repository = new VoidNumberGameTicketRepository();
            _messageService = new MessageService();
        }

        public NumberGameTicketModel GetNumberGameTicket(GetNumberGameTicketCriteria criteria)
        {
            NumberGameTicketModel model = new NumberGameTicketModel();

            GetMatchInfoCriteria criteriaMatchInfo = new GetMatchInfoCriteria();
            criteriaMatchInfo.MatchId = criteria.MatchID;

            model.MatchInfo = _coreService.GetMatchInfo(criteriaMatchInfo);

            var ds = _repository.GetNumberGameTicket(criteria);
            int cnt = ds.Tables.Count;
            DataTable dt = cnt > 0 ? ds.Tables[0] : new DataTable();

            model.Groups.Add(AssignDataOnGroup(dt));

            return model;
        }

        private Group AssignDataOnGroup(DataTable dt)
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

        public IList<ActionResult> VoidNumberGameTicket(VoidNumberGameTicketCritetia criteria)
        {
            IList<ActionResult> results = new List<ActionResult>();
            try
            {
                criteria.Msg.CreatorId = criteria.UserId;
                criteria.Msg.CreatorName = criteria.UserName;

                DataSet dsOutData = new DataSet();
                results = _repository.VoidNumberGameTicket(criteria, ref dsOutData);

                if (dsOutData.Tables.Count > 0)
                {
                    var dtMsg = dsOutData.Tables[0];
                    List<string> listTransIdDone = new List<string>();
                    var resultPrivateMsg = _messageService.SendPrivateMsgToUsers(dtMsg, Convert.ToString(criteria.MatchId), criteria.Msg, ref listTransIdDone);

                    if (resultPrivateMsg.ErrorCode != 0)
                    {
                        results.Add(resultPrivateMsg);
                    }
                }

                if (results.Count == 1 && results[0].ErrorCode == 0)
                {
                    results[0].ErrorCode = -100;
                    results[0].ErrorMsg = "Bet transactions deleted!";
                }

            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                results.Add(new ActionResult()
                {
                    ErrorCode = 1,
                    ErrorMsg = ex.Message
                });
            }

            return results;
        }

        #region Helpers

        private Ticket MapTicket(DataRow row)
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
                BetId = Converter.ToInt(row["BetId"])
            };

            long transid;
            bool result = long.TryParse(row["transid"].ToString(), out transid);
            ticket.TransId = transid;
            return ticket;
        } 

        #endregion


        
    }
}
