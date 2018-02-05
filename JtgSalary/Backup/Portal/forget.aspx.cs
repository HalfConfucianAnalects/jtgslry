using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;

namespace JtgTMS.Platform
{
    public partial class forget : System.Web.UI.Page
    {
        private int _UserID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["UserID"] != null)
            {
                _UserID = int.Parse(Request.Params["UserID"]);
            }

            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            ltUserName.Text = SysClass.SysUser.GetOpCodeByUserID(_UserID) + " | " + SysClass.SysUser.GetUserNameByUserID(_UserID);
        }

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (!SysClass.SysUser.CheckUserPassword(_UserID, CurrentPassword.Text))
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(CurrentPassword, "当前密码不正确！");
            }
            else if (NewPassword.Text.ToString() == CurrentPassword.Text.ToString())
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(NewPassword, "当前密码和新密码不能重复！");
            }
            else if (NewPassword.Text.ToString() != ConfirmNewPassword.Text.ToString())
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(ConfirmNewPassword, "确认密码必须和新密码相同！");
            }
            return bFlag;
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                if (SysClass.SysUser.UpdateUserPassword(_UserID, NewPassword.Text.ToString()) > 0)
                {
                    Dialog.OpenDialogInAjax(NewPassword, "修改用户密码成功，请重新登录……", "logout.aspx");
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("logout.aspx");
        }
    }
}
