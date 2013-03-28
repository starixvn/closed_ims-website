using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Misc.Services.Interface;
using Fanex.TicketControl.Core.Services.Impl;
using Fanex.TicketControl.Misc.Repositories.Interface;
using Fanex.TicketControl.Misc.Repositories.Impl;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Services.Interface;
using Fanex.TicketControl.Misc.Criteria;
using System.Data;

namespace Fanex.TicketControl.Misc.Services.Impl
{
    public class BatchMessageService : IBatchMessageService
    {
        IBatchMessageRepository _repository;
        IMessageService _messageService;

        int MaxTransIdPerBatch = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BatchMsg.MaxTransIdPerBatch"]);

        public BatchMessageService ()
        {
            _repository = new BatchMessageRepository();
            _messageService = new MessageService();
            
        }

        

        private List<string> GetTransIdsByBatch(string transIds)
        {
            List<string> batches = new List<string>();
            try
            {
                int transIdPerBatch = MaxTransIdPerBatch;
                int num = 0;
                string[] arrTransids = transIds.Split(',');
                num = arrTransids.Length / transIdPerBatch;
                if (arrTransids.Length % transIdPerBatch > 0)
                    num++;


                for (int i = 0; i < num; i++)
                {
                    var transidsPerBatch = arrTransids.Select(s => s).Skip(transIdPerBatch * (i)).Take(transIdPerBatch).ToList();
                    batches.Add(string.Join(",", transidsPerBatch));
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return batches;
        }

        public List<ActionResult> UploadMsg(Message pmsg, string TransIds)
        {
            List<ActionResult> results = new List<ActionResult>();
            try
            {
                var arrTransIds = GetTransIdsByBatch(TransIds);
                List<string> listTransIdDone = new List<string>();

                foreach (string transids in arrTransIds)
                {
                    GetBatchMessageCriteria criteria = new GetBatchMessageCriteria();
                    criteria.TransIds = transids;

                    DataTable dtMsg =  _repository.GetBatchMessage(criteria);                    
                    var rs = _messageService.SendPrivateMsgToUsers(dtMsg, "0", pmsg, ref listTransIdDone);                 
                }

                List<string> listTransIdFail = new List<string>();

                listTransIdFail = TransIds.Replace(" ", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Except(listTransIdDone).ToList();

                string strTransIdDone = string.Empty;
                string strTransIdFail = string.Empty;

                var idsDone = listTransIdDone.Select(s => s).Distinct().ToList();

                strTransIdDone = String.Join(",", idsDone);
                strTransIdFail = String.Join(",", listTransIdFail);

                if (strTransIdDone.Length > 0)
                {
                    results.Add(new ActionResult() { 
                        ErrorCode = -100,
                        ErrorMsg = "Update Batch Msg successfully with TransId: " + strTransIdDone + "."
                    });
                }

                if (strTransIdFail.Length > 0)
                {
                    results.Add(new ActionResult()
                    {
                        ErrorCode = 100,
                        ErrorMsg = "Update Batch Msg unsuccessfully with TransId: " + strTransIdFail + "."
                    });
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                results.Add(new ActionResult() { 
                    ErrorCode = 100,
                    ErrorMsg = ex.Message
                });
            }
            return results;
        }
    }
}
