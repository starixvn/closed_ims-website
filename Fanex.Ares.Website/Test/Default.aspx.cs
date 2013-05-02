using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fanex.Ares.Website.Test
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var pdfSection = Fanex.Core.PdfHelper.Configuration.PdfHelperSection.Instance;

            var test = pdfSection.StyleSheet["SpecialStyle"].Items[0];
        }
    }
}