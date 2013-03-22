using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Core.Repository.Interface;
using Fanex.TicketControl.Core.Services.Impl;
using Fanex.TicketControl.Core.Models;
using Fanex.Data;
using Fanex.TicketControl.Core.Criteria;

namespace Fanex.TicketControl.Core.Repository.Impl
{
    public class MessageRepository : ServiceBase, IMessageRepository
    {
        public int SetMessage(Message Msg, int[] TargetId, string[] TargetName, int MatchId, string[] TransId)
        {
            try
            {
                SetMessageCriteria criteria = new SetMessageCriteria();
                criteria.Creator = Msg.CreatorId;
                criteria.CreatorName = Msg.CreatorName;
                criteria.MatchId = MatchId;

                criteria.Content_en = Msg.Content_en;
                criteria.Content_ch = Msg.Content_ch;
                criteria.Content_cs = Msg.Content_cs;
                criteria.Content_it = Msg.Content_it;
                criteria.Content_jp = Msg.Content_jp;
                criteria.Content_th = Msg.Content_th;
                criteria.Content_ko = Msg.Content_ko;
                criteria.Content_es = Msg.Content_es;

                // for insert check
                if ((Msg.MessageId == 0) && (TargetId.Length < 1))
                    return 0;

                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var ds = objectDb.ExecuteDataSet(criteria.GetSpParams());
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Msg.MessageId = Convert.ToInt32(ds.Tables[0].Rows[0]["MessageId"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return Msg.MessageId;
        }

        public void AddTargt2Message(int MessageId, int[] TargetId, string[] TargetName, string[] TransID)
        {
            try
            {
                for (int i = 0; i < TargetId.Length; i++)
                {
                    SetUsrMessageCriteria criteria = new SetUsrMessageCriteria();
                    criteria.MessageId = MessageId;
                    criteria.TargetId = TargetId[i];
                    criteria.TargetName = TargetName[i];
                    criteria.BettransId = TransID[i];

                    if (TargetId[i] != 0) {
                        using (ObjectDb objDB = new ObjectDb(criteria.GetSettingKey()))
                        {
                            objDB.ExecuteNonQuery(criteria.GetSpParams());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
        }
    }
}
