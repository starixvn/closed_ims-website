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
using Fanex.TicketControl.Core.Services.Impl;
using Fanex.TicketControl.Misc.ViewModels;
using Fanex.TicketControl.Misc.Entities;

namespace Fanex.TicketControl.Misc.Repositories.Impl
{
    public class TicketListingRepository : BaseRepository, ITicketListingRepository
    {
        public TicketListingModel GetTicketListing(GetTicketsListingCriteria criteria)
        {
            TicketListingModel ticketListingModel = new TicketListingModel();

            try
            {
                if (criteria.IsValid())
                {
                    using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        DataSet dataset = objectDb.ExecuteDataSet(criteria.GetSpParams());

                        // get result info
                        ticketListingModel.Result = GetResultInfo(objectDb);

                        if (ticketListingModel.Result.ErrorCode == 0) // no error
                        {
                            // get match info 
                            var matchTbl = dataset.Tables[0];
                            var matchInfo = new MatchInfo();
                            matchInfo.MatchCode = Converter.ToString2(matchTbl.Rows[0]["matchcode"]);
                            matchInfo.EventDate = Converter.ToDateTime(matchTbl.Rows[0]["eventdate"]).ToShortDateString();
                            matchInfo.HomeId = Convert.ToInt32(matchTbl.Rows[0]["homeid"]);
                            matchInfo.AwayId = Convert.ToInt32(matchTbl.Rows[0]["awayid"]);
                            ticketListingModel.MatchInfo = matchInfo;

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
                            ticketListingModel.Items = new List<TicketListing>();

                            if (criteria.GroupBy == 0)
                            {
                                var groupByLicensee = rawData.GroupBy(t => t.LicenseeName);
                                foreach (var licensee in groupByLicensee)
                                {
                                    var prodListing = new TicketListing
                                    {
                                        Id = licensee.Key,  // licensee name
                                        Name = licensee.Key,
                                        LiveIndicator = false,
                                        TotalTrans = licensee.Sum(t => t.TotalTrans)
                                    };

                                    prodListing.Items = licensee.Select(t => new TicketListing
                                    {
                                        Id = t.Bettype.ToString(),
                                        Name = this.GetBettypeName(t.Bettype, t.LiveIndicator),
                                        LiveIndicator = t.LiveIndicator,
                                        TotalTrans = t.TotalTrans
                                    }).ToList();

                                    ticketListingModel.Items.Add(prodListing);
                                }
                            }
                            else
                            {
                                // group 
                                var groupByBettype = rawData.GroupBy(t => new { t.Bettype, t.LiveIndicator });

                                foreach (var bettype in groupByBettype)
                                {
                                    var prodListing = new TicketListing
                                    {
                                        Id = bettype.Key.Bettype.ToString(),  // bettypeId
                                        Name = this.GetBettypeName(bettype.Key.Bettype, bettype.Key.LiveIndicator),
                                        LiveIndicator = bettype.Key.LiveIndicator,
                                        TotalTrans = bettype.Sum(t => t.TotalTrans)
                                    };

                                    prodListing.Items = bettype.Select(t => new TicketListing
                                    {
                                        Id = t.LicenseeName,
                                        Name = t.LicenseeName,
                                        LiveIndicator = t.LiveIndicator,
                                        TotalTrans = t.TotalTrans
                                    }).ToList();

                                    ticketListingModel.Items.Add(prodListing);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ticketListingModel;
        }
        
        private ActionResult GetResultInfo(ObjectDb db) 
        {
            var errorCode = db.GetParameterValue("ErrCode");
            var errorMessage = db.GetParameterValue("ErrMsg");

            // get result message
            var result = new ActionResult
            {
                ErrorCode = Converter.ToInt(errorCode),
                ErrorMsg = Converter.ToString2(errorMessage)
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

        private List<TicketListing> getTicketsListingGroupByLicensee(DataTable tb)
        {
            throw new NotImplementedException();
        }

        private List<TicketListing> getTicketsListingGroupByBettype(DataTable tb)
        {
            throw new NotImplementedException();
        }
    }
}
