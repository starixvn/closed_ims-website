// -----------------------------------------------------------------------
// <copyright file="CalcOdds.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Fanex.TicketControl.Core.Helper.SportHelper
{
    public static class CalcOdds
    {
        private readonly static List<int> _betTypes;

        static CalcOdds()
        {
            _betTypes = new List<int>() { 1, 2, 3, 7, 12, 8, 20, 124, 125 };

            for (int i = 81; i <= 88; i++)
            {
                _betTypes.Add(i);
            }

            for (int i = 91; i <= 120; i++)
            {
                _betTypes.Add(i);
            }
        }

        /// <summary>
        /// f_GetOddsByBettype
        /// </summary>
        /// <param name="odds"></param>
        /// <param name="bettype"></param>
        /// <param name="oddsType"></param>
        /// <returns></returns>
        public static decimal GetOdds(decimal odds, int bettype, int oddsType)
        {
            decimal oddsValue = odds;
            if (_betTypes.Contains(bettype) && oddsType == 1)
            {
                oddsValue = odds + 1;
            }
            else if (_betTypes.Contains(bettype) && oddsType == 5)
            {
                oddsValue = odds * 100;
            }

            return oddsValue;
        }

        public static int GetOddsType(int bettype, int oddsType)
        {
            if (_betTypes.Contains(bettype))
            {
                return oddsType;
            }
            return 0;
        }
    }
}