// -----------------------------------------------------------------------
// <copyright file="Bettype.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.TicketControl.Core.Configurations;
using Fanex.TicketControl.Core.Resources;
using Fanex.TicketControl.Core.Services.Impl;

namespace Fanex.TicketControl.Core.Helper.SportHelper
{
    public static class Bettype
    {
        public static string GetBettype(int bettypeId, string betId)
        {
            string betypeName = string.Empty;
            if (bettypeId >= 1301 && bettypeId <= 1334)
            {
                betypeName = CacheRef.GetBettypeNameSportingSolution(bettypeId, betId);
            }
            else
            {
                betypeName = CacheRef.GetBettype(bettypeId);
                if (1201 <= bettypeId && bettypeId < 1299)
                {
                    betypeName += Labels.BetTypeSuffix;
                }
            }
            return betypeName;

        }

        public static string GetBettype(int bettypeId)
        {
            string betypeName = string.Empty;
            betypeName = CacheRef.GetBettype(bettypeId);
            if (AppSettings.VirtualSportBetTypes.Contains(bettypeId.ToString()))
            {
                betypeName += Labels.BetTypeSuffix;
            }

            return betypeName;

        }


    }
}
