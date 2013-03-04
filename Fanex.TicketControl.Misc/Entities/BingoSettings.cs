using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Fanex.Common;

namespace Fanex.TicketControl.Misc.Entities
{
    public class BingoSettings
    {
        public static int MaxBall
        {
            get
            {
                return Converter.ToInt(ConfigurationManager.AppSettings["MaxBall"]);
            }
        }

        public static decimal MaxCommission
        {
            get
            {
                return Converter.ToDecimal(ConfigurationManager.AppSettings["MaxCommission"]);
            }
        }

        public static decimal MaxPt
        {
            get
            {
                return Converter.ToDecimal(ConfigurationManager.AppSettings["MaxPt"]);
            }
        }

        public static decimal MinPt
        {
            get {
                return Converter.ToDecimal(ConfigurationManager.AppSettings["MinPt"]);
            }
        }
    }
}
