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
    using System.Linq;
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
    using System.Data;
    using Fanex.TicketControl.Misc.Entities;
    using Fanex.BetList.Core.Entities;
    using Fanex.Common.Extensions;

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

        public TicketListingModel GetTicketListing(GetTicketsListingCriteria criteria)
        {
            return _ticketListingRepository.GetTicketListing(criteria);
        }

        public TicketListingBetListModel GetTicketListingBetList(GetTicketsListingDetailCriteria criteria)
        {
            int skip = criteria.PageIndex * criteria.PageSize;
            var result = _ticketListingRepository.GetTicketListingBetList(criteria);
            result.Items = result.Items.Skip(skip).Take(criteria.PageSize).ToList();
            return result;
        }

        public void ExportTicketListing(GetTicketsListingDetailCriteria criteria)
        {
            try
            {
                // get data for report
                var ticketListing = GetTicketListingBetList(criteria);

                #region prepare excel file and sheetname
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                string sheetName = "Tickets Listing";
                ExcelWorksheet workSheet = worksheets.Add(sheetName);
                int startColumn = 0, startRow = 0;
                int currentRow = 0, currentColumn = 0;
                CellRange currentCellRange;
                ExcelCell currentCell;
                #endregion

                #region Report Title 
                
                var headerTextStyle = ExcelExtensions.GetCellTitleStyle(ExcelExtensions.HEADER_TITLE_COLOR);
                var title = string.Format(Labels.Tickets_Listing_for_MatchID_Bettype, criteria.MatchID, criteria.BetTypeName);
                currentCellRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow + 1, currentColumn + 8);

                currentCellRange.Merged = true;
                currentCellRange.Value = title;
                currentCellRange.Style = headerTextStyle;
                currentRow += 3;

                #endregion

                #region Match Info

                CellRange matchCellRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow + 2, currentColumn + 4);
                currentRow += 3;

                #region Match Header
                
                currentCell = matchCellRange[0, 0];
                currentCell.Value = Labels.Code;
                currentCellRange = matchCellRange.GetSubrangeRelative(0, 1, 2, 1);
                currentCellRange.Merged = true;
                currentCellRange.Value = Labels.Match_Event;
                currentCell = matchCellRange[0, 3];
                currentCell.Value = Labels.Event_Date; 

                #endregion

                #region Match Content

                currentCell = matchCellRange[1, 0];
                currentCell.Value = criteria.MatchCode;
                currentCellRange = matchCellRange.GetSubrangeRelative(1, 1, 2, 1);
                currentCellRange.Merged = true;
                currentCellRange.Value = string.Format("{0} -vs- {1}", criteria.HomeName, criteria.AwayName);
                currentCell = matchCellRange[1, 3];
                currentCell.Value = criteria.EventDate;

                #endregion

                #endregion

                #region Tickets List

                #region Ticket Header
                CellRange headerTicket = workSheet.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 10);

                #endregion

                
                foreach (var ticket in ticketListing.Items)
                {

                    currentRow++;
                }

                #endregion

                // finally, define report file name
                var reportName = "Report File Name";
                excelFile.ReponseStream(reportName);

            }catch(Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            throw new NotImplementedException();
        }
    }
}
