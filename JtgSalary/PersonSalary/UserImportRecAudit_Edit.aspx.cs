using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;

namespace JtgTMS.DepotTool
{
    public partial class UserImportRecAudit_Edit : System.Web.UI.Page
    {
        private string _IDs = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["IDs"] != null)
            {
                _IDs = Request.Params["IDs"];
            }

            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            if (_IDs.Length > 0)
            {
                string sWhereSQL = "And a.ApprovalStatus=0 And a.ID in (" + _IDs + ")";

                SqlDataReader sdr = SysClass.SysUserSalary.GetUserImportRecLstByWhere(sWhereSQL);
                while (sdr.Read())
                {
                    if (ltSalaryDates.Text.Length > 0)
                    {
                        ltSalaryDates.Text += "<br/>";
                    }
                    ltSalaryDates.Text += "<label>" + sdr["SalaryDate"].ToString();
                }
                sdr.Close();
            }
        }

        private bool SaveCheck()
        {
            bool bFlag = true;
           
            return bFlag;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                if (SysClass.SysUserSalary.UpdateUserImportRecAuditByID(_IDs) > 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "info", "<script>UpdateSuccess();</script>");
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "info", "<script>alert('数据保存失败.');</script>");
                }
            }
        }
    }
}
