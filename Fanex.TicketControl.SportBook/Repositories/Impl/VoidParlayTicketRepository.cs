using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Repository.Impl;
using Fanex.TicketControl.Core.Services.Impl;
using Fanex.TicketControl.SportBook.Criteria;
using Fanex.TicketControl.SportBook.Repositories.Interface;

namespace Fanex.TicketControl.SportBook.Repositories.Impl
{
    public class VoidParlayTicketRepository : BaseRepository, IVoidParlayTicketRepository
    {

        public DataSet GetParlayTicket(GetParlayTicketCriteria criteria)
        {
            DataSet ds = new DataSet();
            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    ds = objectDb.ExecuteDataSet(criteria.GetSpParams());
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return ds;
        }

        public DataTable GetParlaySubTicket(GetParlaySubTicketCriteria criteria)
        {
            DataTable dtSubTicket = new DataTable();
            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var ds = objectDb.ExecuteDataSet(criteria.GetSpParams());
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dtSubTicket = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return dtSubTicket;
        }

        public IList<ActionResult> VoidParlayTicket(VoidParlayTicketCritetia criteria, ref DataSet dsOutData)
        {
            IList<ActionResult> results = new List<ActionResult>();
            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    dsOutData = objectDb.ExecuteDataSet(criteria.GetSpParams());
                    var hashOutputParams = objectDb.GetOuputParams();

                    results.Add(new ActionResult() { 
                        ErrorCode = Convert.ToInt32(hashOutputParams["errcode"]),
                        ErrorMsg = Convert.ToString(hashOutputParams["errmsg"])
                    });

                   
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
