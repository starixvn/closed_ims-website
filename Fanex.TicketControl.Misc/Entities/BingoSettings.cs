using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.TicketControl.Misc.Entities
{
    public class BingoSettings
    {
        public static int MaxBall
        {
            get
            {
                return 1;
            }
        }

        public static double MaxCommission
        {
            get
            {
                return 1.0;
            }
        }

        public static double MaxPt
        {
            get
            {
                return 1.0;
            }
        }

        public static double MinPt {
            get {
                return 2.0;
            }
        }
    }
}
