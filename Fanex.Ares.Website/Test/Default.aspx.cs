using System;
using System.Collections.Generic;
using System.IO;
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
            var filePath = "D:/img1.jpg";
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                var base64 = Convert.ToBase64String(buffer);
            }
        }
    }
}