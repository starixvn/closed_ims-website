// --------------------------------------------------------------------------
// <copyright file="EntityBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

using System;
using System.Diagnostics.CodeAnalysis;
using Fanex.NAP.Framework.Domain;

namespace Fanex.TicketControl.Core.Models
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [Serializable]
    public class EntityBase<TId, TName> : IEntity<TId, TName>
    {
        public TId Id
        {
            get;
            set;
        }

        public TName Name
        {
            get;
            set;
        }
    }
}