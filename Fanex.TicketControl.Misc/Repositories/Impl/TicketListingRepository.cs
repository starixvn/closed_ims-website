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
            ProductListingDTO prodListingDTO = new ProductListingDTO();

            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {

                    
                    objectDb.AddInParameter("MatchID", DbType.Int32, criteria.MatchID);
                    objectDb.AddInParameter("LicenseeID", DbType.Int32, criteria.LicenseeID);
                    objectDb.AddInParameter("Bettypes", DbType.String, criteria.BetTypes);
                    objectDb.AddInParameter("BettypesLive", DbType.String, criteria.LiveBetTypes);
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
                    prodListingDTO.Result = GetResultInfo(objectDb);

                    if(prodListingDTO.Result.Id == 0 ) // no error
                    {
                        // get match info 
                        var matchTbl = dataset.Tables[0];
                        var matchInfo = new MatchInfo();
                        matchInfo.MatchCode = Converter.ToString2(matchTbl.Rows[0]["matchcode"]);
                        matchInfo.EventDate = Converter.ToDateTime(matchTbl.Rows[0]["eventdate"]).ToShortDateString();
                        matchInfo.Home = CacheRef.GetTeamName(matchTbl.Rows[0]["homeid"]);
                        matchInfo.Away = CacheRef.GetTeamName(matchTbl.Rows[0]["awayid"]);
                        prodListingDTO.MatchInfo = matchInfo;

                        // need the function get team name from team id

                        // get the tickets listing
                        var mainTable = dataset.Tables[1];

                        var rawData = mainTable.AsEnumerable().Select(t => new
                        {
                            TotalTrans = Converter.ToInt(t["TotalTrans"]),
                            Bettype = Converter.ToInt(t["bettype"]),
                            LiveIndicator = Converter.ToBool(t["LiveIndicator"]),
                            LicenseeName = Converter.ToString2(t["LicenseeName"])
                        });

                        // GroupBy ways:
                        // 0: Licensee and bettype
                        // 1: Bettype and licensee
                        prodListingDTO.Items = new List<ProductListing>();

                        if (criteria.GroupBy == 0)
                        {
                            var groupByLicensee = rawData.GroupBy(t => t.LicenseeName);
                            foreach (var licensee in groupByLicensee)
                            {
                                var prodListing = new ProductListing { 
                                    Id = licensee.Key,  // licensee name
                                    Name = licensee.Key,
                                    LiveIndicator = false,
                                    TotalTrans = licensee.Sum(t => t.TotalTrans)
                                };
                                
                                prodListing.Items = licensee.Select(t => new ProductListing
                                {
                                    Id = t.Bettype.ToString(),
                                    Name = this.GetBettypeName(t.Bettype, t.LiveIndicator),
                                    LiveIndicator = t.LiveIndicator,
                                    TotalTrans = t.TotalTrans
                                }).ToList();

                                prodListingDTO.Items.Add(prodListing);
                            }
                        }
                        else
                        {
                            // group 
                            var groupByBettype = rawData.GroupBy(t => new { t.Bettype, t.LiveIndicator });

                            foreach (var bettype in groupByBettype)
                            {
                                var prodListing = new ProductListing { 
                                    Id = bettype.Key.Bettype.ToString(),  // bettypeId
                                    Name = this.GetBettypeName(bettype.Key.Bettype, bettype.Key.LiveIndicator),
                                    LiveIndicator = bettype.Key.LiveIndicator,
                                    TotalTrans = bettype.Sum(t => t.TotalTrans)
                                };

                                prodListing.Items = bettype.Select(t => new ProductListing
                                {
                                    Id = t.LicenseeName,
                                    Name = t.LicenseeName,
                                    LiveIndicator = t.LiveIndicator,
                                    TotalTrans = t.TotalTrans
                                }).ToList();

                                prodListingDTO.Items.Add(prodListing);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return prodListingDTO;
        }
        
        private ActionResult GetResultInfo(ObjectDb db) 
        {
            var errorCode = db.GetParameterValue("ErrCode");
            var errorMessage = db.GetParameterValue("ErrMsg");

            // get result message
            var result = new ActionResult
            {
                Id = Converter.ToInt(errorCode),
                Name = Converter.ToString2(errorMessage)
            };

            return result;
        }

        private string GetBettypeName(int bettype, bool liveIndicator)
        {
            var bettypeName = CacheRef.GetBettype(bettype);

            switch (bettype)
            {
                case 86:
                    bettypeName = liveIndicator ? "Next Odd/Even" : "FT. Odd/Even";
                    break;
                case 85:
                    bettypeName = "Next Over/Under";
                    break;
                case 90:
                    bettypeName += liveIndicator ? " (Live Game)" : " (Pre Game)";
                    break;
                default:
                    break;
            }

            return bettypeName;
        }

        private List<ProductListing> getTicketsListingGroupByLicensee(DataTable tb)
        {
            throw new NotImplementedException();
        }

        private List<ProductListing> getTicketsListingGroupByBettype(DataTable tb)
        {
            throw new NotImplementedException();
        }
    }
}
