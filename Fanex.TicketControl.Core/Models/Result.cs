using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace Fanex.TicketControl.Core.Models
{
    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class Result : BaseModel
    {
        // Convention:
        // Id: is Error Code
        // Name: is Error Message
    }
}
