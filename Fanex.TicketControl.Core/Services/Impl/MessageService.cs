using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Core.Services.Interface;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Repository.Interface;
using Fanex.TicketControl.Core.Repository.Impl;

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

        public int SetMessage(Message Msg, int[] TargetId, string[] TargetName, int MatchId, string[] TransId)
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

        public void AddTargt2Message(int MessageId, int[] TargetId, string[] TargetName, string[] TransId)
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
    }
}
