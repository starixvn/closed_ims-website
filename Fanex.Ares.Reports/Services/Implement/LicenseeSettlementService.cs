using Fanex.Ares.Core.Models;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.ViewModels.SingleTicket;
using System.Collections.Generic;
using Fanex.Ares.Core.Services.Impl;
using Fanex.Ares.Core.Repository.Impl;
using System;
using System.Data;
using System.Web.Mvc;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;

namespace Fanex.Ares.Report.Services.Interface
{
    public class LicenseeSettlementService : ServiceBase, ILicenseeSettlementService
    {
        private readonly BaseRepository _repository;

        public LicenseeSettlementService()
        {
            _repository = new BaseRepository();
        }


        public IList<SelectListItem> GetAllLicensees()
        {
            IList<SelectListItem> listLicensees = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    return new SelectListItem
                    {
                        Value = Convert.ToString(r["LicenseeID"]),
                        Text = Convert.ToString(r["LicenseeName"])
                    };
                };

                listLicensees = _repository.Fetch<SelectListItem>(new IMS_LicenseeSelCriteria(), mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return listLicensees;
        }


        public IList<SelectListItem> GetProductsByLicensee(IMS_LicenseeProductSelCriteria criteria)
        {
            IList<SelectListItem> listProducts = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    return new SelectListItem
                    {
                        Value = Convert.ToString(r["productid"]),
                        Text = Convert.ToString(r["productname"])
                    };
                };

                listProducts = _repository.Fetch<SelectListItem>(criteria, mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return listProducts;
        }

        public IList<SelectListItem> GetCurrencyByLicensee(IMS_LicenseeCurrencySelCriteria criteria)
        {
            IList<SelectListItem> listProducts = new List<SelectListItem>();
            try
            {
                Func<IDataReader, SelectListItem> mappingFunc = r =>
                {
                    return new SelectListItem
                    {
                        Value = Convert.ToString(r["currency"]),
                        Text = Convert.ToString(r["currencyname"])
                    };
                };

                listProducts = _repository.Fetch<SelectListItem>(criteria, mappingFunc);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
            }
            return listProducts;
        }
    }
}