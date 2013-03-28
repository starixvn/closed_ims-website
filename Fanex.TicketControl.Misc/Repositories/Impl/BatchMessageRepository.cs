using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Misc.Repositories.Interface;
using System.Data;
using Fanex.TicketControl.Misc.Criteria;
using Fanex.Data;

namespace Fanex.TicketControl.Misc.Repositories.Impl
{
    public class BatchMessageRepository : IBatchMessageRepository
    {
        public DataTable GetBatchMessage(GetBatchMessageCriteria criteria)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {                    
                    var ds = objectDb.ExecuteDataSet(criteria.GetSpParams());
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
    }
}
