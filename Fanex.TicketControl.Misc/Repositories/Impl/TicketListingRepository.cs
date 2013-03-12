using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fanex.Common;
using Fanex.Data;
using Fanex.TicketControl.Core.Models;
using Fanex.TicketControl.Core.Repository.Impl;
using Fanex.TicketControl.Core.Services.Impl;
using Fanex.TicketControl.Misc.Criteria;
using Fanex.TicketControl.Misc.Entities;
using Fanex.TicketControl.Misc.Repositories.Interface;
using Fanex.TicketControl.Misc.ViewModels;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.TicketHelper;
using Fanex.TicketControl.Core.Utils;

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
                        // NOTE: Must convert GTM+ 8 into GTM- 4
                        if (criteria.FromTransDate.HasValue && criteria.ToTransDate.HasValue)
                        {
                            criteria.FromTransDate = criteria.FromTransDate.Value.AddHours(-12);
                            criteria.ToTransDate = criteria.ToTransDate.Value.AddHours(-12);
                        }

                        DataSet dataset = objectDb.ExecuteDataSet(criteria.GetSpParams());

                        // get result info
                        ticketListingModel.Result = GetResultInfo(objectDb);

                        if (ticketListingModel.Result.ErrorCode == 0) // no error
                        {
                            // get match info 
                            var matchTbl = dataset.Tables[0];
                            var matchInfo = new MatchInfo();
                            matchInfo.MatchCode = Converter.ToString2(matchTbl.Rows[0]["matchcode"]);
                            matchInfo.EventDate = Converter.ToDateTime(matchTbl.Rows[0]["eventdate"]).ToString("yyyy-MM-dd");
                            matchInfo.HomeId = Convert.ToInt32(matchTbl.Rows[0]["homeid"]);
                            matchInfo.AwayId = Convert.ToInt32(matchTbl.Rows[0]["awayid"]);
                            ticketListingModel.MatchInfo = matchInfo;

                            // need the function get team name from team id

                            // get the tickets listing
                            var mainTable = dataset.Tables[1];

                            var ticketListing = mainTable.AsEnumerable().Select(t => new TicketListing {
                                BetTypeID = Converter.ToInt(t["BetType"]),
                                LiveIndicator = Converter.ToBool(t["LiveIndicator"]),
                                TotalTrans = Converter.ToInt(t["TotalTrans"])
                            }).ToList();

                            ticketListingModel.Items = ticketListing;
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

        public DataSet GetTicketListingBetList(GetTicketsListingDetailCriteria criteria)
        {
            try
            {
                if (criteria.IsValid())
                {
                    using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        // NOTE: Must convert GTM+ 8 into GTM- 4
                        if (criteria.FromTransDate.HasValue && criteria.ToTransDate.HasValue)
                        {
                            criteria.FromTransDate = criteria.FromTransDate.Value.AddHours(-12);
                            criteria.ToTransDate = criteria.ToTransDate.Value.AddHours(-12);
                        }

                        return objectDb.ExecuteDataSet(criteria.GetSpParams());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        #region Helpers

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

        //private string GetBetTypeName(int bettype, bool liveIndicator)
        //{
        //    var bettypeName = Fanex.TicketControl.Core.Services.Impl.CacheRef.GetBettype(bettype);

        //    switch (bettype)
        //    {
        //        case 86:
        //            bettypeName = liveIndicator ? "Next Odd/Even" : "FT. Odd/Even";
        //            break;
        //        case 85:
        //            bettypeName = "Next Over/Under";
        //            break;
        //        case 90:
        //            bettypeName += liveIndicator ? " (Live Game)" : " (Pre Game)";
        //            break;
        //        default:
        //            break;
        //    }

        //    return bettypeName;
        //}

        //private static Ticket MappingTicket(DataRow row)
        //{
        //    var ticket = new Ticket
        //    {
        //        UserName = Converter.ToString2(row["username"]),
        //        TransId = Converter.ToInt(row["transid"]),
        //        TransDate = Converter.ToDateTime(row["transdate"]),
        //        Odds = Converter.ToDecimal(row["odds"]),
        //        Stake = Converter.ToDecimal(row["stake"]),
        //        Status = Converter.ToString2(row["status"]),
        //        HomeId = Converter.ToInt(row["homeID"]),
        //        AwayId = Converter.ToInt(row["awayID"]),                
        //        BetTeam = Converter.ToString2(row["betteam"]),
        //        BetTypeId = Converter.ToInt(row["bettype"]),
        //        Handicap1 = Converter.ToDecimal(row["hdp1"]),
        //        Handicap2 = Converter.ToDecimal(row["hdp2"]),
        //        LiveHomeScore = Converter.ToInt(row["livehomescore"]),
        //        LiveAwayScore = Converter.ToInt(row["liveawayscore"]),
        //        IsLive = Converter.ToBool(row["liveindicator"]),
        //        IsNeutral = Converter.ToBool(row["isNeutral"]),
        //        OddsType = Converter.ToString2(row["OddsType"]),
        //        Winlost = Converter.ToDecimal(row["winlost"]),
        //    };

        //    long transid;
        //    bool result = long.TryParse(row["transid"].ToString(), out transid);
        //    ticket.TransId = transid;
        //    return ticket;
        //}

        //private string GetSystemParlaySelection(string selection)
        //{
        //    string systemParlaySelection = "";

        //    if (!string.IsNullOrEmpty(selection))
        //    {
        //        switch (selection)
        //        {
        //            case "110":
        //                systemParlaySelection = "Trixie ";
        //                break;
        //            case "1110":
        //                systemParlaySelection = "Yankee";
        //                break;
        //            case "11110":
        //                systemParlaySelection = "Canadian";
        //                break;
        //            case "111110":
        //                systemParlaySelection = "Heinz";
        //                break;
        //            case "1111110":
        //                systemParlaySelection = "Super Heinz";
        //                break;
        //            case "11111110":
        //                systemParlaySelection = "Goliath";
        //                break;
        //            default:
        //                systemParlaySelection = "Trixie!";
        //                break;
        //        }
        //    }

        //    return systemParlaySelection;
        //}

        #endregion
    }
}
