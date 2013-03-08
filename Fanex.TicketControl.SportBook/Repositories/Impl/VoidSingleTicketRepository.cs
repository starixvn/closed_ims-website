using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Misc.Repositories.Interface;
using Fanex.TicketControl.Core.Repository.Impl;

using Fanex.TicketControl.Core.Models;
using Fanex.Data;
using System.Data;
using Fanex.Common;

using Fanex.TicketControl.Core.Services.Impl;
using Fanex.TicketControl.SportBook.Criteria;

namespace Fanex.TicketControl.Misc.Repositories.Impl
{
    public class VoidSingleTicketRepository : BaseRepository, IVoidSingleTicketRepository
    {

        public DataSet GetSingleTicket(GetSingleTicketCriteria criteria)
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


        public IList<ActionResult> VoidSingleTicket(VoidSingleTicketCritetia criteria, ref DataSet dsOutData)
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
