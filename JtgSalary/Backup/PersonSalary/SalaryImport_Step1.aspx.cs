using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;
using System.Data;
using System.IO;

namespace JtgTMS.PersonSalary
{
    public partial class SalaryImport_Step1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();
           
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            txtUserSalaryYears.Text = DateTime.Now.ToString("yyyyMM");
        }

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (txtUserSalaryYears.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtUserSalaryYears, "工资月份不能为空！");
            }
            

            return bFlag;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                Response.Redirect("../PersonSalary/SalaryImport_Step2.aspx?SalaryYears=" + txtUserSalaryYears.Text);
            }
        }

            
    }
}
