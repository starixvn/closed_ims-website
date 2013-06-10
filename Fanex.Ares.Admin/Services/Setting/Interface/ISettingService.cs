// --------------------------------------------------------------------------
// <copyright file="ILicenseeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.06.03">Created</change>
// </history>

namespace Fanex.Ares.Admin.Services.Party.Interface
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Ares.Admin.Models.Setting;
    using Fanex.Ares.Core.Criteria;
    using Fanex.NAP.Framework.Data;

    public interface ISettingService
    {
        IEnumerable<SettingModel> GetAllSettings(ICriteria criteria);
        Hashtable ExecuteCommand(ICriteria criteria);
        IEnumerable<SelectListItem> GetCurrencyByLicensee(IMS_LicenseeCurrencySelCriteria criteria);
        IEnumerable<SelectListItem> GetProductsByLicenseeParty(ICriteria criteria);
        IEnumerable<LicenseeProductChargeModel> GetAllLicenseeProductCharge(ICriteria criteria);
        IEnumerable<LicenseeProductDiscountModel> GetAllLicenseeProductDiscount(ICriteria criteria);
    }
}
