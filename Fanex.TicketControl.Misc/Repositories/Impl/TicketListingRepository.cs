using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.TicketControl.Misc.Repositories.Interface;
using Fanex.TicketControl.Core.Repository.Impl;
using Fanex.TicketControl.Misc.Criteria;
using Fanex.TicketControl.Core.Models;
using Fanex.Data;
using System.Data;
using Fanex.Common;
using Fanex.TicketControl.Misc.DTO;
using Fanex.TicketControl.Core.Services.Impl;

namespace Fanex.TicketControl.Misc.Repositories.Impl
{
    public class TicketListingRepository : BaseRepository, ITicketListingRepository
    {
        public ProductListingDTO GetTicketListing(GetTicketsListingCriteria criteria)
        {
            ProductListingDTO product = new ProductListingDTO();

            string name = Fanex.TicketControl.Core.Services.Impl.CacheRef.GetBettype(1);

            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objectDb.AddInParameter("MatchID", DbType.Int32, criteria.MatchID);
                    objectDb.AddInParameter("LicenseeID", DbType.Int32, criteria.LicenseeID);
                    objectDb.AddInParameter("Bettypes", DbType.String, criteria.BetTypes == null ? null : string.Join(",", criteria.BetTypes));
                    objectDb.AddInParameter("BettypesLive", DbType.String, criteria.LiveBetTypes == null ? null : string.Join(",", criteria.LiveBetTypes));
                    objectDb.AddInParameter("LiveType", DbType.Int16, criteria.EventType);
                    objectDb.AddInParameter("Livehomescore", DbType.Int16, criteria.LiveHomeScore);
                    objectDb.AddInParameter("Liveawayscore", DbType.Int16, criteria.LiveAwayScore);
                    objectDb.AddInParameter("FromTransdate", DbType.DateTime, criteria.FromTransDate);
                    objectDb.AddInParameter("ToTransdate", DbType.DateTime, criteria.ToTransDate);
                    objectDb.AddInParameter("Type", DbType.Int16, (int)criteria.Product);
                    objectDb.AddInParameter("SAFilter", DbType.Int16, criteria.TicketType);
                    objectDb.AddOutParameter("ErrCode", DbType.Int16, 0);
                    objectDb.AddOutParameter("ErrMsg", DbType.String, 1000);

                    DataSet dataset = objectDb.ExecuteDataSet();

                    // get result info
                    product.Result = GetResultInfo(objectDb);

                    if(product.Result.Id == 0 ) // no error
                    {
                        // get match info 
                        var matchTbl = dataset.Tables[0];
                        var matchInfo = new MatchInfo();
                        matchInfo.MatchCode = Converter.ToString2(matchTbl.Rows[0]["matchcode"]);
                        matchInfo.EventDate = Converter.ToDateTime(matchTbl.Rows[0]["eventdate"]).ToShortDateString();
                        var homeId = Converter.ToInt(matchTbl.Rows[0]["homeid"]);
                        matchInfo.Home = CacheRef.GetTeamName(homeId);
                        matchInfo.Away = CacheRef.GetTeamName(matchTbl.Rows[0]["awayid"]);
                        product.MatchInfo = matchInfo;

                        // need the function get team name from team id

                        // get the tickets listing
                        var mainTable = dataset.Tables[1];

                        // GroupBy ways:
                        // 0: Licensee and bettype
                        // 1: Bettype and licensee
                        if (criteria.GroupBy == 0)
                        {

                        }
                        else
                        {

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return product;
        }

        private Result GetResultInfo(ObjectDb db) 
        {
            var errorCode = db.GetParameterValue("ErrCode");
            var errorMessage = db.GetParameterValue("ErrMsg");
            // get result message
            var result = new Result
            {
                Id = Converter.ToInt(errorCode),
                Name = Converter.ToString2(errorMessage)
            };

            return result;
        }
    }
}
