using System;
using System.Data;
using Fanex.Ares.Core.Models;
using Fanex.Ares.Core.Services.Impl;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.Repositories.Interface;
using Fanex.Data;

namespace Fanex.Ares.Report.Repositories.Impl
{
    public class RecoverTicketRepository : IRecoverTicketRepository
    {
        public DataTable GetTicketRecover(GetRecoverTicketCriteria criteria, ref ActionResult result)
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


        public DataSet RecoverTicket(RecoverTicketCriteria criteria, ref ActionResult result)
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
