using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stimulsoft.Report;

namespace PPIC.Reports
{
    public partial class WebRptForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string codemmr = string.Empty;
            string typePrint = string.Empty; ;
            var codeMMR = Session["codeMMR"];
            var toPrint = Session["TypePrint"];
            codemmr = Convert.ToString(codeMMR);
            typePrint = Convert.ToString(toPrint);
            StiReport report = new StiReport();
            if (typePrint == "MR")
                report.Load(Server.MapPath("~/Reports/MatReceive.mrt"));
            else if (typePrint == "DKBM")
                report.Load(Server.MapPath("~/Reports/DeliveryToFinish.mrt"));

            report.Compile();
            report["parC"] = codemmr;
            report.Render();
            StiWebViewer1.Report = report;
        }
    }
}