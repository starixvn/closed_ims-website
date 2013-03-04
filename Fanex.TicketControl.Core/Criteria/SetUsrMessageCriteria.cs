using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.NAP.Framework.Data;

namespace Fanex.TicketControl.Core.Criteria
{
    public class SetUsrMessageCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Core_Dot_Msg_SetUsrMessage";
        }

        public int MessageId { get; set; }

        public int TargetId { get; set; }

        public string TargetName { get; set; }

        public bool IsRead {
            get { return false; }
        }

        public Int64 BettransId { get; set; }
    }
}
