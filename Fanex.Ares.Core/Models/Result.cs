using System;
using System.Diagnostics.CodeAnalysis;

namespace Fanex.Ares.Core.Models
{
    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class Result
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

       // public dynamic ExtendContent { get; set; }

        public Result()
        {
            ErrorCode = 0;
            ErrorMessage = string.Empty;
        }
    }
}
