using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.Ares.Report.Criteria.LicenseeSettlement;
using Fanex.Ares.Report.Criteria;
using Fanex.Ares.Report.ViewModels;
using Fanex.Ares.Report.ViewModels.LicenseeSettlement;

namespace Fanex.Ares.Report.Services.Interface
{
    public interface ISportsbookExcelService
    {
        void ExportReportToExcel(string exchange, string product, string fileName, bool isSaveFile = false);
    }
}