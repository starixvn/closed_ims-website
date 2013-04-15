using System;
using System.Diagnostics.CodeAnalysis;

namespace Fanex.Ares.Core.Models
{
    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ActionResult
    {
        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

       // public dynamic ExtendContent { get; set; }

        public ActionResult()
        {
            ErrorCode = 0;
            ErrorMsg = string.Empty;
        }
    }
}
