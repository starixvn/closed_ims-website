// --------------------------------------------------------------------------
// <copyright file="Currency.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.1.2">Created</change>
// </history>

using System.Diagnostics.CodeAnalysis;

namespace Fanex.TicketControl.Core.Models
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class Currency
    {
        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }
    }
}