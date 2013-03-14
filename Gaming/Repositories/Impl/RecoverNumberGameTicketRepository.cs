using System;
using System.Data;
using Fanex.Data;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Services.Impl;
using Fanex.TicketControl.Gaming.Repositories.Interface;

namespace Fanex.TicketControl.Gaming.Repositories.Impl
{
    public class RecoverNumberGameTicketRepository : IRecoverNumberGameTicketRepository
    {
        public DataTable GetNumberGameTicketRecover(Criteria.GetRecoverNumberGameTicketCriteria criteria, ref ActionResult result)
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

                    var outputParam = objectDb.GetOuputParams();

                    result.ErrorCode = Convert.ToInt32(outputParam["errcode"]);
                    result.ErrorMsg = Convert.ToString(outputParam["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);

                result.ErrorCode = 1;
                result.ErrorMsg = ex.Message;
            }
            return dt;
        }

        public DataSet RecoverNumberGameTicket(Criteria.RecoverNumberGameTicketCriteria criteria, ref ActionResult result)
        {
            DataSet ds = new DataSet();

            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    ds = objectDb.ExecuteDataSet(criteria.GetSpParams());
                    var outputParam = objectDb.GetOuputParams();

                    result.ErrorCode = Convert.ToInt32(outputParam["errcode"]);
                    result.ErrorMsg = Convert.ToString(outputParam["errmsg"]);
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                result.ErrorCode = 1;
                result.ErrorMsg = ex.Message;
            }

            return ds;
        }
    }
}
