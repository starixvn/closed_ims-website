using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;

namespace Fanex.TicketControl.Core.Criteria
{
    public class TestCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Test_BetTypeSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
