using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CyxPack.CommonOperation;
using System.Data.SqlClient;

namespace JtgTMS.Portal
{
    public partial class Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            //
        }


        protected void btnToolApp_Click(object sender, EventArgs e)
        {
            //SysClass.SysParams.AppcationSystemID = 0;
            SysClass.SysGlobal.UpdatePurviewSystemID(0, "无线电台管理系统");
            //SysClass.SysParams.GetPurviewSystemID() = 0;
            
            Response.Redirect("~/main.aspx");
        }

        protected void btnInterPhone_Click(object sender, EventArgs e)
        {
            //SysClass.SysParams.AppcationSystemID = 1;
            SysClass.SysGlobal.UpdatePurviewSystemID(1, "工资电子签收系统");
            Response.Redirect("~/main.aspx");
        }

        protected void btnSalary_Click(object sender, EventArgs e)
        {
            //SysClass.SysParams.AppcationSystemID = 1;
            SysClass.SysGlobal.UpdatePurviewSystemID(2, "工资电子签收系统");
            Response.Redirect("~/main.aspx");
        }
    }
}
