// -----------------------------------------------------------------------
// <copyright file="CommonService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Robert.Tran" date="2013.2.05">Created</change>
// </history>


namespace Fanex.TicketControl.Core.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.TicketControl.Core.Services.Interface;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.TicketControl.Core.Models;
    using Fanex.TicketControl.Core.Criteria;
    using System.Data;
    using Fanex.Common.Extensions;
using Fanex.TicketControl.Core.Repository.Impl;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class CommonService : ServiceBase, ICommonService
    {
        private readonly BaseRepository _repository;

        public CommonService()
        {
            _repository = new BaseRepository();
        }

        public IList<Bettypes> SelBettypes(GetBettypeCriteria criteria)
        {
            Func<IDataReader, Bettypes> mappingFunc = r => {
                return new Bettypes { 
                    Id = r.ToInt("typeid"),
                    Name = r.ToString("bettypename"),
                    LiveIndicator = r.ToBool("liveindicator")
                };
            };

            IList<Bettypes> bettypes = _repository.Fetch<Bettypes>(criteria, mappingFunc);
            return bettypes;
        }
    }
}
