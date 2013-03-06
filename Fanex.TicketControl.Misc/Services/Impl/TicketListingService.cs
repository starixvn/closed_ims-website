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

        public void ExportTicketListingBetList(GetTicketsListingDetailCriteria criteria)
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
                int currentRow = 0, currentColumn = 0;
                CellRange currentCellRange;
                ExcelCell currentCell;
                #endregion

                #region define the size of each column
                
                for (int colIndex = 0; colIndex < 10; colIndex++)
                {
                    workSheet.Columns[colIndex].Width = 170 * ExcelExtensions.FONT_WIDTH;
                }

                #endregion


                #region Report Title 
                
                var headerTextStyle = ExcelExtensions.GetCellTitleStyle(ExcelExtensions.HEADER_TITLE_COLOR);
                var title = string.Format(Labels.Tickets_Listing_for_MatchID_Bettype, criteria.MatchID, criteria.BetTypeName);
                //var twoDecimalNumberStyle = ExcelExtensions.GetCellNumberStyle(, ExcelExtensions.BACKGROUND_COLOR, true, false, false);

                currentCellRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow + 1, currentColumn + 5);

                currentCellRange.Merged = true;
                currentCellRange.Value = title;
                currentCellRange.Style = headerTextStyle;
                currentRow += 3;

                #endregion

                #region Match Info

                CellRange matchCellRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow + 2, currentColumn + 4);
                currentRow += 3;

                #region Match Header
                var headerCellStyle = ExcelExtensions.GetCellTextStyle(ExcelExtensions.HEADER_TITLE_COLOR, true, HorizontalAlignmentStyle.Center);

                currentCell = matchCellRange[0, 0];
                currentCell.Value = Labels.Code;
                currentCell.Style = headerCellStyle;

                currentCellRange = matchCellRange.GetSubrangeRelative(0, 1, 2, 1);
                currentCellRange.Merged = true;
                currentCellRange.Style = headerCellStyle;
                currentCellRange.Value = Labels.Match_Event;
                currentCell = matchCellRange[0, 3];
                currentCell.Style = headerCellStyle;
                currentCell.Value = Labels.Event_Date;

                #endregion

                #region Match Content
                var contentCellStyle = ExcelExtensions.GetCellTextStyle(ExcelExtensions.BACKGROUND_COLOR, false, HorizontalAlignmentStyle.Center);
                currentCell = matchCellRange[1, 0];
                currentCell.Style = contentCellStyle;
                currentCell.Value = criteria.MatchCode;
                currentCellRange = matchCellRange.GetSubrangeRelative(1, 1, 2, 1);
                currentCellRange.Merged = true;
                currentCellRange.Style = contentCellStyle;
                currentCellRange.Value = string.Format("{0} -vs- {1}", criteria.HomeName, criteria.AwayName);
                currentCell = matchCellRange[1, 3];
                currentCell.Style = contentCellStyle;
                currentCell.Value = criteria.EventDate;

                #endregion

                #endregion

                #region Tickets List

                #region Ticket Header
                int colNumber = 10;
                CellRange headerTicketCellRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, colNumber);
                currentCell = headerTicketCellRange[0, 0];
                currentCell.Style = headerCellStyle;
                currentCell.Value = Labels.BetID;

                currentCell = headerTicketCellRange[0, 1];
                currentCell.Style = headerCellStyle;
                currentCell.Value = Labels.TransDate;

                currentCell = headerTicketCellRange[0, 2];
                currentCell.Style = headerCellStyle;
                currentCell.Value = Labels.Player;

                currentCell = headerTicketCellRange[0, 3];
                currentCell.Style = headerCellStyle;
                currentCell.Value = Labels.Choice;

                currentCell = headerTicketCellRange[0, 4];
                currentCell.Style = headerCellStyle;
                currentCell.Value = Labels.Odds;

                currentCell = headerTicketCellRange[0, 5];
                currentCell.Style = headerCellStyle;
                currentCell.Value = Labels.Stake;

                currentCell = headerTicketCellRange[0, 6];
                currentCell.Style = headerCellStyle;
                currentCell.Value = Labels.WinLoss;

                currentCell = headerTicketCellRange[0, 7];
                currentCell.Style = headerCellStyle;
                currentCell.Value = Labels.Currency;

                currentCell = headerTicketCellRange[0, 8];
                currentCell.Style = headerCellStyle;
                currentCell.Value = Labels.Status;

                currentCell = headerTicketCellRange[0, 9];
                currentCell.Style = headerCellStyle;
                currentCell.Value = Labels.SA;

                #endregion

                var oddRowStyle = ExcelExtensions.GetCellTextStyle(ExcelExtensions.ODD_ROW_COLOR, false, HorizontalAlignmentStyle.Center);
                var evenRowStyle = ExcelExtensions.GetCellTextStyle(ExcelExtensions.EVEN_ROW_COLOR, false, HorizontalAlignmentStyle.Center);
                currentRow++;

                foreach (var ticket in ticketListing.Items)
                {
                    CellRange eachRowRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, 0, 0, colNumber);
                    CellStyle rowStyle;

                    if (currentRow % 2 == 0)
                    {
                        rowStyle = evenRowStyle;
                    }
                    else
                    {
                        rowStyle = oddRowStyle;
                    }

                    currentCell = eachRowRange[0, 0];
                    currentCell.Style = rowStyle;
                    currentCell.Value = ticket.Ticket.TransId;

                    currentCell = eachRowRange[0, 1];
                    currentCell.Style = rowStyle;
                    currentCell.Value = ticket.TransDate;

                    currentCell = eachRowRange[0, 2];
                    currentCell.Style = rowStyle;
                    currentCell.Value = ticket.Ticket.UserName;

                    currentCell = eachRowRange[0, 3];
                    currentCell.Style = rowStyle;
                    currentCell.Value = ticket.SportEvent.Choice;

                    currentCell = eachRowRange[0, 4];
                    currentCell.Style = rowStyle;
                    currentCell.Value = string.Format("{0} ({1})", ticket.Ticket.Odds, ticket.Ticket.OddsType);

                    currentCell = eachRowRange[0, 5];
                    currentCell.Style = rowStyle;
                    currentCell.Value = ticket.Ticket.Stake;

                    currentCell = eachRowRange[0, 6];
                    currentCell.Style = rowStyle;
                    currentCell.Value = ticket.Ticket.Winlost;

                    currentCell = eachRowRange[0, 7];
                    currentCell.Style = rowStyle;
                    currentCell.Value = ticket.Currency;

                    currentCell = eachRowRange[0, 8];
                    currentCell.Style = rowStyle;
                    currentCell.Value = ticket.Ticket.Status;

                    currentCell = eachRowRange[0, 9];
                    currentCell.Style = rowStyle;
                    currentCell.Value = ticket.SAFlag ? "Yes" : string.Empty;

                    currentRow++;
                }

                #endregion

                // finally, define report file name
                var reportName = "ticket";
                excelFile.ReponseStream(reportName);

            }catch(Exception ex)
            {
                LoggingHandler.Log(ex);
            }
        }
    }
}
