// --------------------------------------------------------------------------
// <copyright file="IMS_ProductnfoUpd.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.05.30">Created</change>
// </history>

namespace Fanex.Ares.Admin.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Product info Update.
    /// </summary>
    public class IMS_ProductnfoUpd : CriteriaBase
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public string ProductPrefix { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        /// <summary>
        /// Get setting key.
        /// </summary>
        /// <returns>Return setting key.</returns>
        public override string GetSettingKey()
        {
            return "IMS_ProductnfoUpd";
        }

        /// <summary>
        /// Check is valid.
        /// </summary>
        /// <returns>Return result.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}