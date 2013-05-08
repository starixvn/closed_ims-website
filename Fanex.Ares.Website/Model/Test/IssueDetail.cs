using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fanex.Ares.Website.Model.Test
{
    public class IssueDetail
    {
        public string ItemName { get; set; }
        public string UnitName { get; set; }

        public int LotNo { get; set; }

        public DateTime EditionNoExpDate { get; set; }

        public int Quantity { get; set; }
    }
}