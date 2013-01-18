// --------------------------------------------------------------------------
// <copyright file="MostVisitedPage.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

using System;
using System.Diagnostics.CodeAnalysis;

namespace Fanex.TicketControl.Core.Models
{
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [Serializable]
    public class MostVisitedPage
    {
        public MostVisitedPage()
        {
            Count = 1;
        }

        public string Id { get; set; }

        public int UserId { get; set; }

        public int MenuId { get; set; }

        public string UserName { get; set; }

        public DateTime LastVisitDay { get; set; }

        public string MenuName { get; set; }

        public string MenuURL { get; set; }

        public string Title { get; set; }

        public ulong Count { get; set; }

        /// <summary>
        /// Gets or sets pin value, use for determining most visited page 
        /// is pinned or not
        /// </summary>
        public bool IsPinned { get; set; }

        /// <summary>
        /// Gets or sets pin position of most visited page 
        /// if unpinned position = 0 else from 1 to top range
        /// </summary>
        public int Position { get; set; }
    }
}
