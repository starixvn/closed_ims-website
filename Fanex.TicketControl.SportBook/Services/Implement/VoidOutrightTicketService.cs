using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.SportBook.Services.Interface;
using Fanex.TicketControl.SportBook.Entities.Outright;
using Fanex.TicketControl.SportBook.Criteria;
using Fanex.TicketControl.Core.Services.Impl;
using System.Data;
using Fanex.TicketControl.Core.Repository.Impl;

namespace Fanex.TicketControl.SportBook.Services.Implement
{
    public class VoidOutrightTicketService : ServiceBase, IVoidOutrightTicketService
    {
        private readonly BaseRepository _repository;
        public VoidOutrightTicketService() {
            _repository = new BaseRepository();
        }

        public IList<League> GetOutrightLeague()
        {
            IList<League> leagues = new List<League>();
            try
            {
                Func<IDataReader, League> mappingFunc = r =>
                {
                    return new League
                    {
                        Id = Convert.ToInt32(r["LeagueId"]),
                        Name = CacheRef.GetLeagueName(r["LeagueId"])
                    };
                };

                leagues = _repository.Fetch<League>(new GetOutrightLeagueCriteria(), mappingFunc);                
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return leagues;
        }
    }
}
