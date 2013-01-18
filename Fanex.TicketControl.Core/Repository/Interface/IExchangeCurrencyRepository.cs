// --------------------------------------------------------------------------
// <copyright file="IExchangeCurrencyRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.1.2">Created</change>
// </history>


using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Fanex.NAP.Framework.Data;
using Fanex.TicketControl.Core.Models;

namespace Fanex.TicketControl.Core.Repository.Interface
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IExchangeCurrencyRepository
    {
        List<Currency> GetExchangeCurrencies(ICriteria criteria);
    }
}
