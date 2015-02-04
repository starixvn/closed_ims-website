namespace Fanex.Ares.Core.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;

     public interface IServiceBase
    {
        IEnumerable<T> GetEntites<T>(ICriteria criteria);
    }
}
