using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using CyxPack.CommonOperation;

namespace JtgTMS.WarmingSalary
{
    public partial class SalaryRemind : System.Web.UI.Page
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
            lblOrganName.Text = SysClass.SysGlobal.GetCurrentUserOrganName();
            txtValue.Text = SysClass.SysWarning.GetSalaryValueByOrganID(SysClass.SysGlobal.GetCurrentUserOrganID()).ToString();
        }

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (txtValue.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtValue, "签收提醒天数不能为空！");
            }
            return bFlag;
        }


        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (SysClass.SysWarning.UpdateSingleSalaryValue(SysClass.SysGlobal.GetCurrentUserOrganID(),
                    double.Parse(txtValue.Text)) > 0)
            {
                Dialog.OpenDialogInAjax(txtValue, "签收提醒天数保存成功！");
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "info", "<script>alert('数据保存失败.');</script>");
            }
        }
    }
}
