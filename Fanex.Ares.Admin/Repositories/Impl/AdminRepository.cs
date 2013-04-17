
namespace Fanex.Ares.Admin.Repositories.Impl
{
    using System;
    using Fanex.Ares.Admin.Criteria;
    using Fanex.Ares.Admin.Repositories.Interface;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.Data;

    public class AdminRepository: IAdminRepository
    {
        public bool CheckUserOnlineStatus(AccountingUserCheckOnlineStatusCriteria criteria)
        {
            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objectDb.ExecuteNonQuery(criteria.GetSpParams());
                    return !Convert.ToBoolean(objectDb.GetParameterValue("iskickedout"));
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return false;
        }
    }
}
