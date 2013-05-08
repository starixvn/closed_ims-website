using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fanex.Ares.Website.Model.Test
{
    public class IssueDTO
    {
        public List<IssueDetail> Details { get; set; }

        public IssueDTO()
        {

            this.Details = new List<IssueDetail>();
            for (var i = 0; i < 10; i++)
            {
                var item = new IssueDetail
                    {
                        EditionNoExpDate = DateTime.Now,
                        ItemName = "item " + i,
                        LotNo = i,
                        Quantity = i + 5,
                        UnitName = "" + i
                    };

                this.Details.Add(item);
            }
        }
    }
}