// --------------------------------------------------------------------------
// <copyright file="ProductService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.05.28">Created</change>
// </history>

namespace Fanex.Ares.Admin.Services.Party.Implement
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Ares.Admin.Models.Product;
    using Fanex.Ares.Admin.Services.Party.Interface;
    using Fanex.Ares.Core.Extensions;
    using Fanex.Ares.Core.Services.Impl;
    using Fanex.NAP.Framework.Data;

    public class ProductService : ServiceBase, IProductService
    {
        public IEnumerable<ProductModel> GetAllProducts(ICriteria criteria)
        {
            var listproducts = new List<ProductModel>();
            try
            {
                Func<IDataReader, ProductModel> mappingFunc = r =>
                {
                    var res = new ProductModel
                    {
                        ProductID = Convert.ToInt32(r["ProductID"]),
                        ProductName = r["ProductName"].ToString1(),
                        Description = r["Description"].ToString1(),
                        ProductPrefix = r["ProductPrefix"].ToString1()
                    };

                    return res;
                };

                listproducts = _repository.Fetch<ProductModel>(criteria, mappingFunc).ToList();
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return listproducts;
        }

        //public IEnumerable<SelectListItem> GetAvailableOrSelectedParties(ICriteria criteria)
        //{
        //    var listAvailableLicensees = new List<SelectListItem>();
        //    try
        //    {
        //        Func<IDataReader, SelectListItem> mappingFunc = r =>
        //        {
        //            var res = new SelectListItem
        //            {
        //                Value = r["PartyID"].ToString1(),
        //                Text = r["PartyName"].ToString1()
        //            };

        //            return res;
        //        };

        //        listAvailableLicensees = _repository.Fetch<SelectListItem>(criteria, mappingFunc).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHandler.Log(ex);
        //    }

        //    return listAvailableLicensees;
        //}

        /// <summary>
        /// Using for Insert, Update, Delete
        /// </summary>
        /// <param name="criteria">Criteria</param>
        /// <returns>ErrCode, ErrMsg</returns>
        public Hashtable ExecuteCommand(ICriteria criteria)
        {
            Hashtable rs = new Hashtable();
            try
            {
                rs = _repository.ExecuteCommand(criteria);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }

            return rs;
        }
    }
}