// --------------------------------------------------------------------------
// <copyright file="AdminRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Ares.Admin.Repositories.Impl
{
    using System;
    using Fanex.Ares.Admin.Criteria;
    using Fanex.Ares.Admin.Repositories.Interface;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.Data;

    /// <summary>
    /// Admin Repository.
    /// </summary>
    public class AdminRepository : IAdminRepository
    {
        /// <summary>
        /// Check User Online Status.
        /// </summary>
        /// <param name="criteria">User Status.</param>
        /// <returns>Return result.</returns>
        public bool CheckUserOnlineStatus(AccountingUserCheckOnlineStatusCriteria criteria)
        {
            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    objectDb.ExecuteNonQuery(criteria.GetSpParams());
                    return !Convert.ToBoolean(objectDb.GetParameterValue("iskickedout"));
                }
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return false;
        }
    }
}
