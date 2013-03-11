using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Repository.Impl;
using Fanex.TicketControl.Core.Services.Impl;
using Fanex.TicketControl.Gaming.Repositories.Interface;
using Fanex.TicketControl.Gaming.Criteria;

namespace Fanex.TicketControl.Gaming.Repositories.Impl
{
    public class VoidNumberGameTicketRepository : BaseRepository, IVoidNumberGameTicketRepository
    {
        //public IList<ActionResult> VoidSingleTicket(VoidSingleTicketCritetia criteria, ref DataSet dsOutData)
        //{
        //    IList<ActionResult> results = new List<ActionResult>();
        //    try
        //    {
        //        using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
        //        {
        //            dsOutData = objectDb.ExecuteDataSet(criteria.GetSpParams());
        //            var hashOutputParams = objectDb.GetOuputParams();

        //            results.Add(new ActionResult() { 
        //                ErrorCode = Convert.ToInt32(hashOutputParams["errcode"]),
        //                ErrorMsg = Convert.ToString(hashOutputParams["errmsg"])
        //            });
                   
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHandler.Log(ex);
        //        results.Add(new ActionResult() { 
        //            ErrorCode = 1,
        //            ErrorMsg = ex.Message
        //        });
        //    }
        //    return results;
        //}
        
        public DataSet GetNumberGameTicket(GetNumberGameTicketCriteria criteria)
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
    }
}
