using System.Data;
using Fanex.Ares.Report.Criteria;

namespace Fanex.Ares.Report.Repositories.Interface
{
    public interface IVoidOutrightTicketRepository
    {
        DataSet GetOutrightTicket(GetOutrightTicketCriteria criteria);
    }
}
