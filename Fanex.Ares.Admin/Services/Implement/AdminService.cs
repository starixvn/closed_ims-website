
namespace Fanex.Ares.Admin.Services.Implement
{
    using System;
    using Fanex.Ares.Admin.Criteria;
    using Fanex.Ares.Admin.Repositories.Impl;
    using Fanex.Ares.Admin.Repositories.Interface;
    using Fanex.Ares.Admin.Services.Interface;
    using Fanex.Ares.Core.Services.Impl;

    public class AdminService: IAdminService
    {
        IAdminRepository _adminRepository;

        public AdminService()
        {
            _adminRepository = new AdminRepository();
        }

        public bool CheckUserOnlineStatus(AccountingUserCheckOnlineStatusCriteria criteria)
        {
            var onlineStatus = false;
            try
            {
                onlineStatus = _adminRepository.CheckUserOnlineStatus(criteria);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return onlineStatus;
        }
    }
}
