using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Ares.Core.Repository.Impl;
using Fanex.Ares.Core.Services.Impl;
using Fanex.Ares.Core.Utils;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.Entities.Outright;
using Fanex.Ares.Report.Repositories.Impl;
using Fanex.Ares.Report.Repositories.Interface;
using Fanex.Ares.Report.Services.Interface;
using Fanex.Ares.Report.ViewModels.Outright;

namespace Fanex.Ares.Report.Services.Implement
{
    public class VoidOutrightTicketService : ServiceBase, IVoidOutrightTicketService
    {
        private readonly BaseRepository _repository;
        IVoidOutrightTicketRepository _outrightRepository;
        public VoidOutrightTicketService() {
            _repository = new BaseRepository();
            _outrightRepository = new VoidOutrightTicketRepository();
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
                        Id = Convert.ToString(r["LeagueId"]),
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


        public IList<Team> GetTeamByLeague(GetOutrightTeamCriteria criteria)
        {
            IList<Team> teams = new List<Team>();
            try
            {
                Func<IDataReader, Team> mappingFunc = r =>
                {
                    return new Team
                    {
                        Id = Convert.ToString(r["orid"]),
                        Name = CacheRef.GetTeamName(r["teamid"])
                    };
                };

                teams = _repository.Fetch<Team>(criteria, mappingFunc);               
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return teams;
        }


        public OutrightTicketModel GetOutrightTicket(GetOutrightTicketCriteria criteria)
        {
            OutrightTicketModel model = new OutrightTicketModel();
            try
            {
                var ds = _outrightRepository.GetOutrightTicket(criteria);
                DataTable dt = new DataTable();
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                model.Groups.Add(AssignDataOnGroup(dt));
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return model;
        }

        private Group AssignDataOnGroup(DataTable dt)
        {
            Group group = new Group();
            group.Name = "Normal";
            group.Tickets = new List<TicketInfo>();

            try
            {
                DataRow[] rows = dt.Select();
                List<TicketInfo> tickets = new List<TicketInfo>();

                int index = 0;
                Int64 BetId = 0;
                foreach (DataRow row in rows)
                {
                    index++;
                    BetId = Convert.ToInt64(row["transid"]);

                    var ticketInfo = new TicketInfo()
                    {
                        No = index,
                        BetId = Convert.ToString(BetId),
                        Player = Convert.ToString(row["username"]),
                        TransDate = Convert.ToDateTime(row["transdate"]).ToString(FormatKey.LONG_DATE_TIME_FORMAT),                                                
                        Stake = Convert.ToString(row["stake"]),
                        Odds = Convert.ToString(row["odds"]),
                        CurrencyName = Helper.CurrencyById(Convert.ToInt32(row["currency"])),
                        Status = Convert.ToString(row["status"])
                    };

                    tickets.Add(ticketInfo);

                }

                group.Tickets = tickets;
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return group;
        }

        
    }
}
