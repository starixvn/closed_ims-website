
namespace Fanex.Ares.Admin.Services.Interface
{
    using Fanex.Ares.Admin.Criteria;

    public interface IAdminService
    {
        bool CheckUserOnlineStatus(AccountingUserCheckOnlineStatusCriteria criteria);
    }
}
