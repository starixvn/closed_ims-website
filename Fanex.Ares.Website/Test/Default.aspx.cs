using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Ares.Report.Controllers;
using Fanex.Workbooks;


namespace Fanex.Ares.Website.Test
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Type t = typeof(LicenseeSettlementController);
            MethodInfo[] mi = t.GetMethods();
            foreach (MethodInfo m in mi)
            {
                if (m.IsPublic)
                    if (typeof(ActionResult).IsAssignableFrom(m.ReturnParameter.ParameterType))
                    {
                        var methods = m.Name + Environment.NewLine;
                    }
            }
        }
    }
}