using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.BetList.Core.Entities;
using Fanex.Common;
using Fanex.TicketControl.Core.Extensions;
using Fanex.TicketControl.Core.Services.Impl;
using Fanex.TicketControl.Misc.Entities;
using Fanex.TicketControl.Misc.Services.Interface;

namespace Fanex.TicketControl.Misc.Services.Impl
{
    public class SportEventService
    {
        #region Members

        private readonly ITicket _ticket;

        private readonly string _systemParlaySelection;

        //private readonly TicketListingDetail _ticketListingDetail;

        private readonly INumberWheelService _numberWheelService;

        #endregion

        public SportEventService(ITicket ticket, string selection = null)
        {
            _ticket = ticket;
            _systemParlaySelection = selection;

            _numberWheelService = new NumberWheelService(); 
        }

        public SportEvent GetSportEvent()
        {
            var sportEvent = new SportEvent();
            switch (_ticket.BetTypeId)
            {
                case 1:
                case 7:
                case 20:
                case 21:
                case 22: //Next Goal bet type
                case 124:
                case 125:
                case 1201:
                    AssignHdp(ref sportEvent);
                    break;
                case 2:
                case 12:
                    AssignOE(ref sportEvent);
                    break;
                case 3:
                case 8:
                case 1203:
                    AssignOU(ref sportEvent);
                    break;
                case 4:
                case 30:
                case 1204:
                    AssignCS(ref sportEvent);
                    break;
                case 5:
                case 1205:
                    Assign1x2(ref sportEvent, true);
                    break;
                case 15:
                    Assign1x2(ref sportEvent, false);
                    break;
                case 6:
                case 14:
                case 16:
                case 1206:
                    AssignTG(ref sportEvent);
                    break;
                case 10:
                    AssignOutRight(ref sportEvent);
                    break;
                case 121:
                case 122:
                case 123:
                    AssignHomeAwayDrawNoBet(ref sportEvent);
                    break;
                //Phuong.Nguyen, HR
                case 31:
                case 32:
                case 33:
                case 41:
                case 42:
                case 43:
                    //GenHREvent(ref eventItem);
                    break;
                case 9:
                    AssignMP(ref sportEvent);
                    break;
                case 29:
                    AssignCombinationMP(ref sportEvent);
                    break;
                case 51: //Betting of Finance
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                case 66:
                    //_home = CacheRef.GetFncIndexName(row["homeid"]);
                    //GenFinancialRow(ref eventItem);
                    break;
                case 71:
                case 72:
                case 73:
                case 112:
                case 113:
                case 114:
                //GenCasino(ref eventItem, bettype, Converter.AsString(row["betid"]));
                //GenCasino(ref eventItem, _bettype, _transId);

                case 81://Number Game
                    Assign1stBallOU(ref sportEvent);
                    break;
                case 82:
                    AssignLastBallOU(ref sportEvent);
                    break;
                case 83:
                    Assign1stBallOE(ref sportEvent);
                    break;
                case 84:
                    AssignLastBallOE(ref sportEvent);
                    break;
                case 85:
                    AssignOverUnder(ref sportEvent);
                    break;
                case 86:
                    AssignOddEven(ref sportEvent);
                    break;
                case 87:
                    //GenEvent_NumberGame(ref eventItem, listType, false);
                    AssignNextHighLow(ref sportEvent);
                    break;
                case 88:
                    AssignWarrior(ref sportEvent);
                    break;
                case 89:
                    AssignNextCombo(ref sportEvent);
                    break;
                case 90:
                    AssignNumberWheel(ref sportEvent);
                    break;
                case 24:
                case 1224:
                    AssignDoubleChance(ref sportEvent);
                    break;
                case 25:
                    AssignDrawNoBet(ref sportEvent);
                    break;
                case 26:
                    AssignBothOneNeither(ref sportEvent);
                    break;
                case 27:
                    AssignToWinToNil(ref sportEvent);
                    break;
                case 13:
                    AssignCleanSheet(ref sportEvent);
                    break;
                case 28:
                    AssignThreeWayHandicap(ref sportEvent);
                    break;
                default:
                    break;
            }
            //sportEvent.MoreBetInfo = oddMP(Converter.ToInt(_ticket.Odds));
            return sportEvent;
        }

        private void AssignNumberWheel(ref SportEvent sportEvent)
        {
            sportEvent.Choice = _numberWheelService.GetChoiceName(Converter.ToInt(_ticket.Handicap1), Converter.ToInt(_ticket.BetTeam));
            sportEvent.Hdp = "";
            //sportEvent.Hdp = "&nbsp;" + (double.Parse(BingoSettings.MaxBall.ToString()) / 2);
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.BetId + "]";
            sportEvent.BetType = "<br />" + CacheRef.GetBettype(_ticket.BetTypeId);
            
        }

        private void AssignHomeAwayDrawNoBet(ref SportEvent sportEvent)
        {
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.LiveHomeScore + "-" + _ticket.LiveAwayScore + "]";
            string home = CacheRef.GetTeamName(_ticket.HomeId);
            string away = CacheRef.GetTeamName(_ticket.AwayId);
            if (_ticket.BetTypeId == 121)
            {
                if (_ticket.BetTeam == "x") sportEvent.Choice = "Draw";
                else if (_ticket.BetTeam == "a") sportEvent.Choice = away;
            }
            else if (_ticket.BetTypeId == 122)
            {
                if (_ticket.BetTeam == "h") sportEvent.Choice = home;
                else if (_ticket.BetTeam == "x") sportEvent.Choice = "Draw";
            }
            else if (_ticket.BetTypeId == 123)
            {
                if (_ticket.BetTeam == "h") sportEvent.Choice = "Draw";
                else if (_ticket.BetTeam == "a") sportEvent.Choice = "No Draw";
            }
            sportEvent.Hdp = "";
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignHdp(ref SportEvent sportEvent)
        {
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.LiveHomeScore + "-" + _ticket.LiveAwayScore + "]";
            string home = CacheRef.GetTeamName(_ticket.HomeId);
            string away = CacheRef.GetTeamName(_ticket.AwayId);
            sportEvent.Choice = _ticket.BetTeam == "h" ? home : away;
            if (_ticket.BetTypeId == 20 || _ticket.BetTypeId == 21)
            {
                sportEvent.LiveScore = string.Empty;
                sportEvent.Hdp = string.Empty;
            }
            else
            {
                sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.LiveHomeScore + "-" + _ticket.LiveAwayScore + "]";
                if (_ticket.BetTeam == "h")
                {
                    sportEvent.Hdp = (Converter.ToDecimal(_ticket.Handicap2) - Converter.ToDecimal(_ticket.Handicap1)).Format(2);
                }
                else
                {
                    sportEvent.Hdp = (Converter.ToDecimal(_ticket.Handicap1) - Converter.ToDecimal(_ticket.Handicap2)).Format(2);
                }
            }

            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignOE(ref SportEvent sportEvent)
        {
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.LiveHomeScore + "-" + _ticket.LiveAwayScore + "]";
            string home = CacheRef.GetTeamName(_ticket.HomeId);
            string away = CacheRef.GetTeamName(_ticket.AwayId);

            sportEvent.Choice = _ticket.BetTeam == "h" ? "ODD" : "EVEN";
            sportEvent.Hdp = "";
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);
        }

        private void Assign1x2(ref SportEvent sportEvent, bool isFT)
        {
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.LiveHomeScore + "-" + _ticket.LiveAwayScore + "]";

            string home = CacheRef.GetTeamName(_ticket.HomeId);
            string away = CacheRef.GetTeamName(_ticket.AwayId);
            sportEvent.Choice = isFT ? "FT." + _ticket.BetTeam : "HT." + _ticket.BetTeam;

            //if (listType == 1) { if (isMP) choice += oddMP(); }
            //if ((listType == 2 || listType == 3) && isMP) choice += " <span class='style_12BB'> @ " + IBCUtils.FormatHandler.FormatNumber20(_odds) + "</span>";

            sportEvent.Hdp = "";
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignTG(ref SportEvent sportEvent)
        {
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.LiveHomeScore + "-" + _ticket.LiveAwayScore + "]";

            string home = CacheRef.GetTeamName(_ticket.HomeId);
            string away = CacheRef.GetTeamName(_ticket.AwayId);
            sportEvent.Choice = _ticket.BetTeam;

            sportEvent.Hdp = "";
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignCS(ref SportEvent sportEvent)
        {
            string home = CacheRef.GetTeamName(_ticket.HomeId);
            string away = CacheRef.GetTeamName(_ticket.AwayId);
            sportEvent.Choice = (_ticket.BetTeam == "5:0" || _ticket.BetTeam == "0:5") ? _ticket.BetTeam + " UP" : _ticket.BetTeam;
            if (_ticket.BetTypeId == 30)
            {
                sportEvent.Choice = (_ticket.BetTeam == "4:0" || _ticket.BetTeam == "0:4") ? _ticket.BetTeam + " UP" : _ticket.BetTeam;
            }

            sportEvent.LiveScore = "";

            sportEvent.Hdp = "";
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignOutRight(ref SportEvent sportEvent)
        {
            sportEvent.LiveScore = "";

            string home = CacheRef.GetTeamName(_ticket.HomeId);
            string away = CacheRef.GetTeamName(_ticket.AwayId);

            sportEvent.Choice = away;

            sportEvent.Hdp = "";
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignOU(ref SportEvent sportEvent)
        {
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.LiveHomeScore + "-" + _ticket.LiveAwayScore + "]";
            string home = CacheRef.GetTeamName(_ticket.HomeId);
            string away = CacheRef.GetTeamName(_ticket.AwayId);

            if (_ticket.BetTeam == "h")
            {
                sportEvent.Choice = "OVER";
            }
            else
            {
                sportEvent.Choice = "UNDER";
            }
            sportEvent.Hdp = (Math.Abs(Converter.ToDecimal(_ticket.Handicap1))).Format(2);
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignDoubleChance(ref SportEvent sportEvent)
        {
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.LiveHomeScore + "-" + _ticket.LiveAwayScore + "]";
            string home = CacheRef.GetTeamName(_ticket.HomeId);
            string away = CacheRef.GetTeamName(_ticket.AwayId);

            if (_ticket.BetTeam.ToLower().Equals("1x"))
                sportEvent.Choice = string.Format("{0} <span class='or'>or</span> Draw", home);
            else if (_ticket.BetTeam.ToLower().Equals("2x"))
                sportEvent.Choice = string.Format("{0} <span class='or'>or</span> Draw", away);
            else if (_ticket.BetTeam.ToLower().Equals("12"))
                sportEvent.Choice = string.Format("{0} <span class='or'>or</span> {1}", home, away);

            sportEvent.Hdp = string.Empty;// FormatHandler.FormatNumber2(Math.Abs(Converter.ToDecimal(_ticket.Handicap1)), "en-EN");
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }
        private void AssignDrawNoBet(ref SportEvent sportEvent)
        {
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.LiveHomeScore + "-" + _ticket.LiveAwayScore + "]";
            string home = CacheRef.GetTeamName(_ticket.HomeId);
            string away = CacheRef.GetTeamName(_ticket.AwayId);

            if (_ticket.BetTeam == "h")
            {
                sportEvent.Choice = home;
            }
            else
            {
                sportEvent.Choice = away;
            }

            sportEvent.Hdp = string.Empty;
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignBothOneNeither(ref SportEvent sportEvent)
        {
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.LiveHomeScore + "-" + _ticket.LiveAwayScore + "]";
            string home = CacheRef.GetTeamName(_ticket.HomeId);
            string away = CacheRef.GetTeamName(_ticket.AwayId);

            if (_ticket.BetTeam.ToLower().Equals("b"))
                sportEvent.Choice = "Both";
            else if (_ticket.BetTeam.ToLower().Equals("o"))
                sportEvent.Choice = "One";
            else if (_ticket.BetTeam.ToLower().Equals("n"))
                sportEvent.Choice = "No Goal";

            sportEvent.Hdp = string.Empty;
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignToWinToNil(ref SportEvent sportEvent)
        {
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.LiveHomeScore + "-" + _ticket.LiveAwayScore + "]";
            string home = CacheRef.GetTeamName(_ticket.HomeId);
            string away = CacheRef.GetTeamName(_ticket.AwayId);

            if (_ticket.BetTeam == "h")
            {
                sportEvent.Choice = home;
            }
            else
            {
                sportEvent.Choice = away;
            }

            sportEvent.Hdp = string.Empty;
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignCleanSheet(ref SportEvent sportEvent)
        {
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.LiveHomeScore + "-" + _ticket.LiveAwayScore + "]";
            string home = CacheRef.GetTeamName(_ticket.HomeId);
            string away = CacheRef.GetTeamName(_ticket.AwayId);

            if (_ticket.BetTeam.ToLower().Equals("hy"))
                sportEvent.Choice = string.Format("{0} <span style='color:#000;'>{1}</span>", home, "YES");
            else if (_ticket.BetTeam.ToLower().Equals("hn"))
                sportEvent.Choice = string.Format("{0} <span style='color:#000;'>{1}</span>", home, "NO");
            else if (_ticket.BetTeam.ToLower().Equals("ay"))
                sportEvent.Choice = string.Format("{0} <span style='color:#000;'>{1}</span>", away, "YES");
            else if (_ticket.BetTeam.ToLower().Equals("an"))
                sportEvent.Choice = string.Format("{0} <span style='color:#000;'>{1}</span>", away, "NO");

            sportEvent.Hdp = string.Empty;
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignThreeWayHandicap(ref SportEvent sportEvent)
        {
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.LiveHomeScore + "-" + _ticket.LiveAwayScore + "]";
            string home = CacheRef.GetTeamName(_ticket.HomeId);
            string away = CacheRef.GetTeamName(_ticket.AwayId);
            decimal hdpResult = 0;
            if (_ticket.BetTeam.ToLower().Equals("1"))
            {
                hdpResult = Converter.ToDecimal(_ticket.Handicap1) - Converter.ToDecimal(_ticket.Handicap2) > 0 ? -(Converter.ToDecimal(_ticket.Handicap1) - Converter.ToDecimal(_ticket.Handicap2)) : Math.Abs(Converter.ToDecimal(_ticket.Handicap1) - Converter.ToDecimal(_ticket.Handicap2));

                sportEvent.Choice = home;
                //sportEvent.Hdp = "<span style='color:#000;'>" + (hdpResult > 0 ? "+&nbsp;" : string.Empty) + FormatHandler.FormatNumber2(hdpResult,"en-EN") + "</span>)";
                sportEvent.Hdp = string.Format("<span style='color:#000;'>({0}{1})</span>", (hdpResult > 0 ? "+" : string.Empty), hdpResult.Format(2));

            }
            else if (_ticket.BetTeam.ToLower().Equals("2"))
            {
                hdpResult = Converter.ToDecimal(_ticket.Handicap2) - Converter.ToDecimal(_ticket.Handicap1) > 0 ? -(Converter.ToDecimal(_ticket.Handicap2) - Converter.ToDecimal(_ticket.Handicap1)) : Math.Abs(Converter.ToDecimal(_ticket.Handicap2) - Converter.ToDecimal(_ticket.Handicap1));
                sportEvent.Choice = away;
                //sportEvent.Hdp = "<span style='color:#000;'>" + (hdpResult > 0 ? "+&nbsp;" : string.Empty) + FormatHandler.FormatNumber2(hdpResult, "en-EN") + "</span>";
                sportEvent.Hdp = string.Format("<span style='color:#000;'>({0}{1})</span>", (hdpResult > 0 ? "+" : string.Empty), hdpResult.Format(2));

            }
            else if (_ticket.BetTeam.ToLower().Equals("x"))
            {
                sportEvent.Choice = "Draw";
                sportEvent.Hdp = string.Format("<span style='color:#000;'>(<span style='color:#ff0000;'>{0}</span> +{1})</span>",
                    Converter.ToDecimal(_ticket.Handicap1) - Converter.ToDecimal(_ticket.Handicap2) > 0 ? "A" : "H", (Math.Abs(Converter.ToDecimal(_ticket.Handicap1) - Converter.ToDecimal(_ticket.Handicap2)).Format(2)));
            }

            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignMP(ref SportEvent sportEvent)
        {
            sportEvent.LiveScore = "";
            sportEvent.Choice = "";
            sportEvent.Hdp = "";
            sportEvent.BetType = "Mix Parlay";
            sportEvent.Event = "";
            sportEvent.SportType = "";
            sportEvent.LeagueName = "";
        }

        private void AssignCombinationMP(ref SportEvent sportEvent)
        {
            string selection = "";

            if (!string.IsNullOrEmpty(_systemParlaySelection))
            {
                switch (_systemParlaySelection)
                {
                    case "110":
                        selection = "Trixie ";
                        break;
                    case "1110":
                        selection = "Yankee";
                        break;
                    case "11110":
                        selection = "Canadian";
                        break;
                    case "111110":
                        selection = "Heinz";
                        break;
                    case "1111110":
                        selection = "Super Heinz";
                        break;
                    case "11111110":
                        selection = "Goliath";
                        break;
                    default:
                        selection = "Trixie!";
                        break;
                }
            }

            sportEvent.LiveScore = "";
            sportEvent.Choice = "";
            sportEvent.Hdp = "";
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);
            sportEvent.Selection = selection;
            sportEvent.Event = "<span class='detailMP' onclick='LatestBet.showCombinationDetailMP(\"" + _ticket.RefNo + "\",\"" + selection + "\")' >" + "Details</span>"; ;
            sportEvent.SportType = "";
            sportEvent.LeagueName = "";
        }

        private void AssignWarrior(ref SportEvent sportEvent)
        {
            sportEvent.Hdp = string.Empty;
            sportEvent.LiveScore = string.Empty;
            sportEvent.Choice = _ticket.BetTeam == "h" ? "2nd ball to win" : "3rd ball to win";
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);
        }

        private void AssignNextCombo(ref SportEvent sportEvent)
        {

            if (_ticket.BetTeam == "1:1")
                sportEvent.Choice = "Over/Odd";
            else if (_ticket.BetTeam == "1:2")
                sportEvent.Choice = "Over/Even";
            else if (_ticket.BetTeam == "2:1")
                sportEvent.Choice = "Under/Odd";
            else if (_ticket.BetTeam == "2:2")
                sportEvent.Choice = "Under/Even";
            sportEvent.Hdp = (double.Parse(BingoSettings.MaxBall.ToString()) / 2).ToString();
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.BetId + "]";
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);


        }

        private void Assign1stBallOU(ref SportEvent sportEvent)
        {
            sportEvent.Choice = _ticket.BetTeam.ToLower() == "h" ? "OVER" : "UNDER";
            sportEvent.Hdp = (double.Parse(BingoSettings.MaxBall.ToString()) / 2).ToString();
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignLastBallOU(ref SportEvent sportEvent)
        {
            sportEvent.Choice = _ticket.BetTeam.ToLower() == "h" ? "OVER" : "UNDER";
            sportEvent.Hdp = (double.Parse(BingoSettings.MaxBall.ToString()) / 2).ToString();
            sportEvent.LiveScore = string.Empty;
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void Assign1stBallOE(ref SportEvent sportEvent)
        {
            sportEvent.Choice = _ticket.BetTeam.ToLower() == "h" ? "ODD" : "EVEN";
            sportEvent.Hdp = string.Empty;
            sportEvent.LiveScore = string.Empty;
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignLastBallOE(ref SportEvent sportEvent)
        {
            sportEvent.Choice = _ticket.BetTeam.ToLower() == "h" ? "ODD" : "EVEN";
            sportEvent.Hdp = string.Empty;
            sportEvent.LiveScore = string.Empty;
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignOverUnder(ref SportEvent sportEvent)
        {
            sportEvent.Choice = _ticket.BetTeam.ToLower() == "h" ? "OVER" : "UNDER";
            sportEvent.Hdp = (double.Parse(BingoSettings.MaxBall.ToString()) / 2).ToString();
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.BetId + "]";
            sportEvent.BetType = (_ticket.IsLive ? "Next " : "") + CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignOddEven(ref SportEvent sportEvent)
        {
            sportEvent.Choice = _ticket.BetTeam.ToLower() == "h" ? "ODD" : "EVEN";
            sportEvent.Hdp = string.Empty;
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.BetId + "]";
            sportEvent.BetType = (_ticket.IsLive ? "Next " : "FT. ") + CacheRef.GetBettype(_ticket.BetTypeId);

        }

        private void AssignNextHighLow(ref SportEvent sportEvent)
        {
            sportEvent.Choice = _ticket.BetTeam.ToLower() == "h" ? "HIGH" : "LOW";
            sportEvent.Hdp = string.Empty;//(double.Parse(BingoSettings.MaxBall.ToString()) / 2).ToString();
            sportEvent.LiveScore = !_ticket.IsLive ? string.Empty : "&nbsp;[" + _ticket.BetId + "]";
            sportEvent.BetType = CacheRef.GetBettype(_ticket.BetTypeId);

        }

        /*private void AssignCasino(ref SportEvent sportEvent, int betType)
        {
            string choice = "";
            if (betType == 71 || betType == 113)
                choice = "Casino Game";
            else if (betType == 72 || betType == 114)
                choice = "P2P Game";
            else if (betType == 112)
                choice = "Casino Game - Transfer";
            else if (betType == 73)
                //choice = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblIBCbetBingo");
                choice = "IBCbet Bingo";
            sportEvent.Choice = choice;

        }*/
    }
}
