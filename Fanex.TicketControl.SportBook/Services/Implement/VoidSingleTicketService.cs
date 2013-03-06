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
    using Fanex.BetList.Core.Entities;
    using System.Linq;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.TicketHelper;
using Fanex.TicketControl.Core.Repository.Interface;
    using Fanex.TicketControl.Core.Repository.Impl;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class VoidSingleTicketService : ServiceBase, IVoidSingleTicketService
    {
        ICoreService _coreService;
        IVoidSingleTicketRepository _reponsitory;
        IMessageRepository _messageReponsitory;
        public VoidSingleTicketService()
        {
            _coreService = new CoreService();
            _reponsitory = new VoidSingleTicketRepository();
            _messageReponsitory = new MessageRepository();
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
                        BetTeam = Convert.ToString(row["betteam"]),
                        LiveHomeScore = Convert.ToInt32(row["livehomescore"]),
                        LiveAwayScore = Convert.ToInt32(row["liveawayscore"]),

                        Stake = Convert.ToDecimal(row["StakePositiontaking"]),
                        Odds = Convert.ToDecimal(row["odds"]),
                        OddsType = Convert.ToString(row["OddsType"]),
                        Handicap1 = Convert.ToDecimal(row["hdp1"]),
                        Handicap2 = Convert.ToDecimal(row["hdp2"]),

                        Status = Convert.ToString(row["status"]),
                        HomeId = Convert.ToInt64 (row["HomeId"]),
                        AwayId = Convert.ToInt64(row["AwayId"]),
                        LeagueId = Convert.ToInt32(row["LeagueId"]),
                        SportTypeId = Convert.ToInt32(row["SportType"])
                    };

                    ITicketHelper ticketHelper = new TicketHelper();
                    BetListHTMLFactory factory = new BetListHTMLFactory();
                    BetListHTMLBuilder builder = factory.CreateBuilder();
                    GetCachePropertyById funcGetOddsTypeName = Fanex.BetList.Core.TicketHelper.CacheRef.GetOddsTypeName;

                    if (bettype == 9 || bettype == 29) {
                        //ticketHelper.IsShowOddsWithChoice = true;
                    }

                    ticket.ChoiceHtml = builder.BuildChoice(ticket.Ticket, ticketHelper, null, true);
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

        public IList<ActionResult> VoidSingleTicket(VoidSingleTicketCritetia criteria)
        {
            IList<ActionResult> results = new List<ActionResult>();
            try
            {
                criteria.Msg.CreatorId = criteria.UserId;
                criteria.Msg.CreatorName = criteria.UserName;

                DataSet dsOutData = new DataSet();
                results = _reponsitory.VoidSingleTicket(criteria, ref dsOutData);

                DataTable dtMsg = new DataTable();
                if (dsOutData.Tables.Count > 0)
                {
                    dtMsg = dsOutData.Tables[0];
                    var resultPrivateMsg = SendPrivateMsgToUsers(dtMsg, Convert.ToString(criteria.MatchId), criteria.Msg);

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

        private ActionResult SendPrivateMsgToUsers(DataTable dtMsg, string matchId, Message pmsg)
        {
            ActionResult result = new ActionResult();
            try
            {
                #region send private message to users
                if (dtMsg != null && dtMsg.Rows.Count > 0)
                {
                    if (dtMsg.Rows.Count > 0)
                    {
                        Hashtable list = new Hashtable();

                        for (int i = 0; i < dtMsg.Rows.Count; i++)
                        {

                            string[] ArrColumnsNameDownLineID = { "TargetID"};
                            string[] ArrColumnsNameDownLineUserName = { "TargetName"};

                            for (int j = 0; j < ArrColumnsNameDownLineID.Length; j++)
                            {
                                string cnameID = ArrColumnsNameDownLineID[j];
                                string cnameUser = ArrColumnsNameDownLineUserName[j];

                                string custid = dtMsg.Rows[i][cnameID].ToString();
                                string uname = dtMsg.Rows[i][cnameUser].ToString();

                                string key = custid + "^" + uname;
                                string ticknum = dtMsg.Rows[i]["transid"].ToString();

                                if (list[key] != null)
                                {
                                    if (list[key].ToString().IndexOf(ticknum) == -1)
                                    {
                                        string _tmp = list[key].ToString();
                                        list.Remove(key);
                                        list.Add(key, _tmp + "," + ticknum);
                                    }
                                }
                                else
                                    list.Add(key, ticknum);
                            }
                        }

                        DataTable dtDeletedBet = new DataTable();
                        dtDeletedBet.Columns.Add("custid", typeof(int));
                        dtDeletedBet.Columns.Add("username", typeof(string));
                        dtDeletedBet.Columns.Add("transids", typeof(string));
                        dtDeletedBet.Columns.Add("betcount", typeof(int));

                        foreach (DictionaryEntry Key in list)
                        {
                            int custid = Convert.ToInt32(Key.Key.ToString().Split('^')[0]); // This is a CustID of ticket that is deleted [TargetID Field]
                            string uname = Key.Key.ToString().Split('^')[1];//This is a UserName of ticket that is deleted [TargetName Field]

                            DataRow dr = dtDeletedBet.NewRow();
                            dr["custid"] = custid;
                            dr["username"] = uname;
                            string[] ArrTransids = Key.Value.ToString().Split(',');
                            Array.Sort(ArrTransids);
                            string strTransIDs = "";
                            for (int i = 0; i < ArrTransids.Length; i++)
                            {
                                strTransIDs += ArrTransids[i] + ", ";
                            }
                            strTransIDs = strTransIDs.Substring(0, strTransIDs.Length - 2);
                            dr["transids"] = strTransIDs;
                            dr["betcount"] = Key.Value.ToString().Split(',').Length;
                            dtDeletedBet.Rows.Add(dr);
                        }
                        DataView dv = dtDeletedBet.DefaultView;
                        dv.Sort = "betcount asc, transids asc";
                        DataTable dtBets = dv.ToTable();

                        // send private message to users for cancel their transactions                                                                                                                
                        int MessageId = 0;
                        foreach (DataRow dr in dtBets.Rows)
                        {
                            int[] custid = { Convert.ToInt32(dr["custid"]) }; // This is a CustID of ticket that is deleted [TargetID Field]
                            string[] uname = { Convert.ToString(dr["username"]) };//This is a UserName of ticket that is deleted [TargetName Field]

                            if (Convert.ToInt32(dr["custid"]) < 100) continue;//SRecommend < 100 and roleid=3 
                            string[] bettransid = { Convert.ToString(dr["transids"]) };

                            if (MessageId == 0)
                                MessageId = _messageReponsitory.SetMessage(pmsg, custid, uname, Convert.ToInt32(matchId), bettransid);
                            else
                                _messageReponsitory.AddTargt2Message(MessageId, custid, uname, bettransid);
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {                
                LoggingHandler.Log(ex);
                result.ErrorCode = 100;
                result.ErrorMsg = ex.Message;
            }

            return result;
        }

        public ActionResult CheckMatchIDwithRefno(CheckMatchIdWithRefNoCriteria criteria)
        {
            ActionResult result = new ActionResult();
            try
            {
                criteria.Type = Helper.SportType.Sportsbook;
                result = _coreService.CheckMatchIdWithRefno(criteria);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return result;
        }
    }
}
