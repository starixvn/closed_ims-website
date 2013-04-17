
namespace Fanex.Ares.Admin.Repositories.Interface
{
    using Fanex.Ares.Admin.Criteria;

    public interface IAdminRepository
    {
        bool CheckUserOnlineStatus(AccountingUserCheckOnlineStatusCriteria criteria);
    }
}
