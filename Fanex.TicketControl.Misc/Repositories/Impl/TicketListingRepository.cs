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

                            var ticketListing = mainTable.AsEnumerable().Select(t => new TicketListing {
                                BetTypeID = Converter.ToInt(t["bettype"]),
                                LiveIndicator = Converter.ToBool(t["LiveIndicator"]),
                                TotalTrans = Converter.ToInt(t["totalTrans"])
                            }).ToList();

                            ticketListingModel.Items =ticketListing;
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

        private string GetBetTypeName(int bettype, bool liveIndicator)
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

        public TicketListingBetListModel GetTicketListingBetList(GetTicketsListingDetailCriteria criteria)
        {
            throw new NotImplementedException();
        }
    }
}
