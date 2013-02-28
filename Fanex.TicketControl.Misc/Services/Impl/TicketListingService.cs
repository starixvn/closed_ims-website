// --------------------------------------------------------------------------
// <copyright file="TicketListingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.2.4">Created</change>
// </history>

namespace Fanex.TicketControl.Misc.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using ExcelUtil.Spreadsheet;
    using Fanex.TicketControl.Core.Criteria;
    using Fanex.TicketControl.Core.Extensions;
    using Fanex.TicketControl.Core.Models;
    using Fanex.TicketControl.Core.Resources;
    using Fanex.TicketControl.Core.Services.Impl;
    using Fanex.TicketControl.Core.Services.Interface;
    using Fanex.TicketControl.Misc.Criteria;
    using Fanex.TicketControl.Misc.Repositories.Impl;
    using Fanex.TicketControl.Misc.Services.Interface;
    using Fanex.TicketControl.Misc.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TicketListingService : ServiceBase, ITicketListingService
    {
        private readonly TicketListingRepository _ticketListingRepository;
        private readonly ICoreService _coreService;

        public TicketListingService()
        {
            _ticketListingRepository = new TicketListingRepository();
            _coreService = new CoreService();
        }

        public IList<Bettypes> GetAllBettypes(GetBettypeCriteria criteria)
        {
            return _coreService.GetBettypes(criteria);
        }

        public TicketListingModel GetTicketListing(GetTicketsListingCriteria criteria)
        {
            return _ticketListingRepository.GetTicketListing(criteria);
        }

        public TicketListingBetListModel GetTicketListingBetList(GetTicketsListingDetailCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public void ExportTicketListing(GetTicketsListingCriteria criteria)
        {
            try
            {
                var ticketListing = GetTicketListing(criteria);

                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                string sheetName = "Tickets Listing";
                ExcelWorksheet workSheet = worksheets.Add(sheetName);

                // write the main content into excel
                var headerTextStyle = ExcelExtensions.GetCellTitleStyle(ExcelExtensions.HEADER_TITLE_COLOR);

                // first, the report title
                int currentRow = 0, currentColumn = 0;
                var title = string.Format(Labels.Tickets_Listing_for_MatchID_Bettype, criteria.MatchID, criteria.BetTypes);
                CellRange currentCellRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow + 1, currentColumn + 5);
                currentCellRange.Merged = true;
                currentCellRange.Value = title;
                currentCellRange.Style = headerTextStyle;
                currentRow += 3;



                // 2nd, the report data 
                // match info


                // finally, define report file name
                var reportName = "Report File Name";
                excelFile.ReponseStream(reportName);

            }catch(Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            throw new NotImplementedException();
        }

        public void ExportTicketListingBetList(GetTicketsListingDetailCriteria criteria)
        {
            try
            {
                var ticketListing = GetTicketListingBetList(criteria);

                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                string sheetName = "Tickets Listing";
                ExcelWorksheet workSheet = worksheets.Add(sheetName);

                // write the main content into excel

                // first, the report title


                // 2nd, the report data 

                // finally, define report file name
                var reportName = "Report File Name";
                excelFile.ReponseStream(reportName);

            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            throw new NotImplementedException();
        }


        public void ExportTicketListingBetList(GetTicketsListingCriteria criteria)
        {
            throw new NotImplementedException();
        }
    }
}
