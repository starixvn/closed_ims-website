
using Fanex.TicketControl.Gaming.Criteria;
using System.Data;
namespace Fanex.TicketControl.Gaming.Repositories.Interface
{
    public interface IVoidNumberGameTicketRepository
    {
        DataSet GetNumberGameTicket(GetNumberGameTicketCriteria criteria);

        //IList<ActionResult> VoidSingleTicket(VoidSingleTicketCritetia criteria, ref DataSet dsOutData);
    }
}
