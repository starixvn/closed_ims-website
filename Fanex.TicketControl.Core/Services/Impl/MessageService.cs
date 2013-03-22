using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Core.Services.Interface;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Repository.Interface;
using Fanex.TicketControl.Core.Repository.Impl;
using System.Data;
using System.Collections;

namespace Fanex.TicketControl.Core.Services.Impl
{
    public class MessageService : IMessageService
    {
        IMessageRepository _repository;
        public MessageService() {
            _repository = new MessageRepository();
        }

        string CorrectSpace(string scontent)
        {
            if (scontent == null)
                return string.Empty;
            if (scontent.Length == 0)
                return scontent;
            int index = scontent.IndexOf("@@");

            if (index == -1) return scontent;

            if (index == 0)
            {
                if (scontent.Substring(0, 1).Trim().Length > 0)
                    scontent = scontent.Replace("@@", "@@ ");
            }
            else
            {
                if (scontent.Substring(index - 1, 1).Trim().Length > 0)
                    scontent = scontent.Replace("@@", " @@");


            }
            return scontent;
        }

        private int SetMessage(Message Msg, int[] TargetId, string[] TargetName, int MatchId, string[] TransId)
        {
            try
            {
                // for insert check
                if ((Msg.MessageId == 0) && (TargetId.Length < 1))
                    return 0;

                Msg.Content_en = CorrectSpace(Msg.Content_en);
                Msg.Content_ch = CorrectSpace(Msg.Content_ch);
                Msg.Content_cs = CorrectSpace(Msg.Content_cs);
                Msg.Content_it = CorrectSpace(Msg.Content_it);
                Msg.Content_jp = CorrectSpace(Msg.Content_jp);
                Msg.Content_th = CorrectSpace(Msg.Content_th);
                Msg.Content_ko = CorrectSpace(Msg.Content_ko);
                Msg.Content_es = CorrectSpace(Msg.Content_es);

                Msg.MessageId = _repository.SetMessage(Msg, TargetId, TargetName, MatchId, TransId);
                if (Msg.MessageId > 0)
                {
                    _repository.AddTargt2Message(Msg.MessageId, TargetId, TargetName, TransId);
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return Msg.MessageId;
        }

        private void AddTargt2Message(int MessageId, int[] TargetId, string[] TargetName, string[] TransId)
        {
            try
            {
                _repository.AddTargt2Message(MessageId, TargetId, TargetName, TransId);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
        }


        public ActionResult SendPrivateMsgToUsers(DataTable dtMsg, string matchId, Message pmsg)
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

                            string[] ArrColumnsNameDownLineID = { "TargetID" };
                            string[] ArrColumnsNameDownLineUserName = { "TargetName" };

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

                           
                            string[] bettransid = { Convert.ToString(dr["transids"]) };

                            if (MessageId == 0)
                                MessageId = SetMessage(pmsg, custid, uname, Convert.ToInt32(matchId), bettransid);
                            else
                                AddTargt2Message(MessageId, custid, uname, bettransid);
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
    }
}
