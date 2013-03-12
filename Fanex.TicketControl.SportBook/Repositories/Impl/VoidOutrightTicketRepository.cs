using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.SportBook.Repositories.Interface;
using System.Data;
using Fanex.TicketControl.SportBook.Criteria;
using Fanex.TicketControl.Core.Services.Impl;
using Fanex.Data;

namespace Fanex.TicketControl.SportBook.Repositories.Impl
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
