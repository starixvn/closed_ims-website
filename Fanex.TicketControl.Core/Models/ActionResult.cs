using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace Fanex.TicketControl.Core.Models
{
    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ActionResult : BaseModel
    {
        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public dynamic ExtendContent { get; set; }

        public ActionResult()
        {
            ErrorCode = 0;
            ErrorMsg = string.Empty;
        }
    }
}
