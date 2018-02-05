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
    public partial class framework_userPassword : System.Web.UI.Page
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
            ltUserName.Text = SysClass.SysGlobal.GetCurrentOpCode() + " | " + SysClass.SysGlobal.GetCurrentOpName();
        }

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (!SysClass.SysUser.CheckUserPassword(SysClass.SysGlobal.GetCurrentUserID(), CurrentPassword.Text))
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
                if (SysClass.SysUser.UpdateUserPassword(SysClass.SysGlobal.GetCurrentUserID(), NewPassword.Text.ToString()) > 0)
                {
                    Dialog.OpenDialogInAjax(NewPassword, "恭喜您，修改用户密码成功……");
                }
            }
        }
    }
}
