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
    using System.Text;
    using System.Drawing;
    using Fanex.TicketControl.Core.Utils;
    using System.Xml;

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
                var ticketListing = _ticketListingRepository.GetTicketListingBetList(criteria);

                #region prepare excel file and sheetname
                ExcelFile excelFile = new ExcelFile();
                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                string sheetName = "Tickets Listing";
                ExcelWorksheet workSheet = worksheets.Add(sheetName);
                int colNumber = 11;
                int currentRow = 0, currentColumn = 0;
                CellRange currentCellRange;
                ExcelCell currentCell;
                #endregion

                #region define the size of each column

                for (int colIndex = 0; colIndex < colNumber; colIndex++)
                {
                    workSheet.Columns[colIndex].Width = 100 * ExcelExtensions.FONT_WIDTH;
                }

                
                workSheet.Columns[0].Width = 120 * ExcelExtensions.FONT_WIDTH;
                workSheet.Columns[1].Width = 160 * ExcelExtensions.FONT_WIDTH;
                workSheet.Columns[2].Width = 160 * ExcelExtensions.FONT_WIDTH;
                workSheet.Columns[3].Width = 350 * ExcelExtensions.FONT_WIDTH;
                workSheet.Columns[4].Width = 120 * ExcelExtensions.FONT_WIDTH;
                workSheet.Columns[8].Width = 150 * ExcelExtensions.FONT_WIDTH;

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

                #region Match Header

                CellRange matchHeaderCellRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 3);
                var headerCellStyle = ExcelExtensions.GetCellTextStyle(ExcelExtensions.HEADER_TITLE_COLOR, true, HorizontalAlignmentStyle.Center);
                currentRow ++;

                matchHeaderCellRange.Style = headerCellStyle;
                currentCell = matchHeaderCellRange[0, 0];
                currentCell.Value = Labels.Code;

                currentCellRange = matchHeaderCellRange.GetSubrangeRelative(0, 1, 2, 1);
                currentCellRange.Merged = true;
                currentCellRange.Value = Labels.Match_Event;
                currentCell = matchHeaderCellRange[0, 3];
                currentCell.Value = Labels.Event_Date;

                #endregion

                #region Match Content

                CellRange matchContentCellRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, 3);
                var contentCellStyle = ExcelExtensions.GetCellTextStyle(ExcelExtensions.BACKGROUND_COLOR, false, HorizontalAlignmentStyle.Center);
                currentRow += 2;
                
                matchContentCellRange.Style = contentCellStyle;
                currentCell = matchContentCellRange[0, 0];
                currentCell.Value = criteria.MatchCode;
                currentCellRange = matchContentCellRange.GetSubrangeRelative(0, 1, 2, 1);
                currentCellRange.Merged = true;
                currentCellRange.Value = string.Format("{0} -vs- {1}", criteria.HomeName, criteria.AwayName);
                currentCell = matchContentCellRange[0, 3];
                currentCell.Value = criteria.EventDate.ToString(FormatKey.SHORT_DATE_TIME_FORMAT);

                #endregion

                #endregion

                #region Bet List

                #region Bet List Header
                CellRange headerTicketCellRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, colNumber - 1);
                headerTicketCellRange.Style = headerCellStyle;
                currentRow++;

                currentCell = headerTicketCellRange[0, 0];
                currentCell.Value = Labels.BetID;

                currentCell = headerTicketCellRange[0, 1];
                currentCell.Value = Labels.TransDate;

                currentCell = headerTicketCellRange[0, 2];
                currentCell.Value = Labels.Member;

                currentCell = headerTicketCellRange[0, 3];
                currentCell.Value = Labels.Choice;

                currentCell = headerTicketCellRange[0, 4];
                currentCell.Value = Labels.Odds;

                currentCell = headerTicketCellRange[0, 5];
                currentCell.Value = Labels.Stake;

                currentCell = headerTicketCellRange[0, 6];
                currentCell.Value = Labels.WinLoss;

                currentCell = headerTicketCellRange[0, 7];
                currentCell.Value = Labels.Currency;

                currentCell = headerTicketCellRange[0, 8];
                currentCell.Value = Labels.Bettype;

                currentCell = headerTicketCellRange[0, 9];
                currentCell.Value = Labels.Status;

                currentCell = headerTicketCellRange[0, 10];
                currentCell.Value = Labels.SA;

                #endregion

                #region Bet List Content

                var oddRowStyle = ExcelExtensions.GetCellTextStyle(ExcelExtensions.ODD_ROW_COLOR, false, HorizontalAlignmentStyle.Center);
                var evenRowStyle = ExcelExtensions.GetCellTextStyle(ExcelExtensions.EVEN_ROW_COLOR, false, HorizontalAlignmentStyle.Center);
                CellStyle decimalNumberStyle;

                if (ticketListing.TotalItem == 0)
                {
                    CellRange eachRowRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, colNumber - 1);
                    eachRowRange.Merged = true;
                    eachRowRange.Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    eachRowRange.Value = Labels.There_is_no_data;
                }
                else
                {
                    foreach (var ticket in ticketListing.Items)
                    {
                        
                        CellRange eachRowRange = workSheet.Cells.GetSubrangeAbsolute(currentRow, 0, currentRow, colNumber - 1);
                        if (ticket.IsMixParlay == true)
                        {
                            workSheet.Rows[currentRow].Height = workSheet.Rows[currentRow].Height * 2;
                        }

                        CellStyle rowStyle;
                        var bettypeID = ticket.Ticket.BetTypeId;

                        if (currentRow % 2 == 0)
                        {
                            rowStyle = evenRowStyle;
                            decimalNumberStyle = ExcelExtensions.GetCellNumberStyle(FormatKey.TWO_DECIMAL_DIGIT_FORMAT, ExcelExtensions.EVEN_ROW_COLOR, false, false, true);
                        }
                        else
                        {
                            rowStyle = oddRowStyle;
                            decimalNumberStyle = ExcelExtensions.GetCellNumberStyle(FormatKey.TWO_DECIMAL_DIGIT_FORMAT, ExcelExtensions.ODD_ROW_COLOR, false, false, true);
                        }

                        eachRowRange.Style = rowStyle;
                        currentCell = eachRowRange[0, 0];
                        currentCell.Value = ticket.Ticket.TransId;

                        currentCell = eachRowRange[0, 1];
                        currentCell.Value = ticket.TransDate;

                        currentCell = eachRowRange[0, 2];
                        currentCell.Value = ticket.Ticket.UserName;
                        currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                        
                        string choice = string.Empty;
                        if (bettypeID == 90)
                        {
                            var liveScore = !ticket.Ticket.IsLive && ticket.Ticket.BetId.HasValue ? string.Empty : "[" + ticket.Ticket.BetId.Value + "]";
                            choice = string.Format("Group {0}-{1} {2}", ticket.Ticket.Handicap1, ticket.Ticket.BetTeam, liveScore);
                        }
                        else
                        {
                            choice = GetChoiceInfo(ticket);
                        }

                        currentCell = eachRowRange[0, 3];
                        currentCell.Value = choice; // choice
                        currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        currentCell.Style.Font.Weight = ExcelFont.BoldWeight;

                        currentCell = eachRowRange[0, 4];
                        currentCell.Value = GetOddsInfo(ticket);
                        currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        currentCell.Style.Font.Weight = ExcelFont.BoldWeight;

                        currentCell = eachRowRange[0, 5];
                        currentCell.Style = decimalNumberStyle;
                        currentCell.Value = ticket.Ticket.Stake;
                        currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        currentCell = eachRowRange[0, 6];
                        currentCell.Style = decimalNumberStyle;
                        if (!string.IsNullOrEmpty(ticket.AgentWinloss))
                        {
                            currentCell.Value = ticket.Ticket.Winlost;
                        }
                        currentCell.Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                        currentCell = eachRowRange[0, 7];
                        currentCell.Value = ticket.Currency;

                        currentCell = eachRowRange[0, 8];
                        currentCell.Value = ticket.BetType;

                        if (bettypeID == 29) // ie. System Parlay
                        {
                            currentCell.Value = ticket.BetType + " - " + ticket.Selection;
                        }

                        currentCell = eachRowRange[0, 9];
                        currentCell.Value = ticket.Ticket.Status;

                        currentCell = eachRowRange[0, 10];
                        currentCell.Value = ticket.SAFlag ? "X" : string.Empty;

                        currentRow++;
                    }
                } 

                #endregion

                #endregion

                // finally, define report file name
                var reportName = string.Format("Ticket Listing for Bettype {0}", criteria.BetTypeName);
                excelFile.ReponseStream(reportName.Replace(' ', '_'));

            }catch(Exception ex)
            {
                LoggingHandler.Log(ex);
            }
        }

        private string GetChoiceInfo(TicketListingDetail ticketDetail)
        {
            var choiceHtml = ticketDetail.ChoiceHtml;
            string betteam, handicap, livescore;

            XmlDocument xmlDoc = new XmlDocument();
            choiceHtml = choiceHtml.Replace("&nbsp;", " ");
            xmlDoc.LoadXml(choiceHtml);
            betteam = string.Empty;
            handicap = string.Empty;
            livescore = string.Empty;

            var foundSpanNodes = xmlDoc.SelectNodes("//span");
            if (foundSpanNodes.Count > 0)
            {
                var betChoiceNode = foundSpanNodes[0];
                int count = betChoiceNode.ChildNodes.Count;

                switch (count)
                {
                    case 0:
                        break;
                    case 1:
                        betteam = betChoiceNode.ChildNodes[0].InnerText.Trim();
                        break;

                    case 2:
                        betteam = betChoiceNode.ChildNodes[0].InnerText.Trim();
                        handicap = betChoiceNode.ChildNodes[1].InnerText.Trim();
                        break;
                    default:
                        betteam = betChoiceNode.ChildNodes[0].InnerText.Trim();
                        handicap = betChoiceNode.ChildNodes[1].InnerText.Trim();
                        livescore = betChoiceNode.ChildNodes[2].InnerText.Trim();
                        break;
                }
            }
            string choice = string.Empty;

            if (!string.IsNullOrEmpty(betteam))
            {
                choice += betteam;
            }

            if (!string.IsNullOrEmpty(handicap))
            {
                choice += " " + handicap;
            }

            if (!string.IsNullOrEmpty(livescore))
            {
                choice += " " + livescore;
            }

            if (ticketDetail.IsMixParlay)
            {
                choice += "\n" + ticketDetail.ParlayBetType;
            }

            return choice;
        }

        private string GetOddsInfo(TicketListingDetail ticketDetail)
        {
            var oddsHtml = ticketDetail.OddsHtml;
            string oddsValue, oddsType;
            oddsValue = string.Empty;
            oddsType = string.Empty;

            XmlDocument xmlDoc = new XmlDocument();
            oddsHtml = oddsHtml.Replace("\"", "'");
            oddsHtml = "<div>" + oddsHtml + "</div>";
            xmlDoc.LoadXml(oddsHtml);
            var foundSpanNodes = xmlDoc.SelectNodes("//span");
            var count = foundSpanNodes.Count;
            switch(count)
            {
                case 0:
                    // do nothing
                    break;
                case 1:
                    oddsValue = foundSpanNodes[0].InnerText.Trim();
                    break;
                default: 
                    oddsValue = foundSpanNodes[0].InnerText.Trim();
                    oddsType = foundSpanNodes[1].InnerText.Trim();
                    break;
            }

            string odds = oddsValue;

            if (!string.IsNullOrEmpty(oddsType))
            {
                odds += " (" + oddsType + ")";
            }

            return odds;
        }
    }
}

