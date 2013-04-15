using System;
using System.Data;
using Fanex.Ares.Core.Services.Impl;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.Repositories.Interface;
using Fanex.Data;

namespace Fanex.Ares.Report.Repositories.Impl
{
    public class VoidOutrightTicketRepository : IVoidOutrightTicketRepository
    {
        public DataSet GetOutrightTicket(GetOutrightTicketCriteria criteria)
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
