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
    public partial class login : System.Web.UI.Page
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
            txtLoginCode.Text = "";
            txtOpName.Text = "";
        }

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (!SysClass.SysUser.UserLogin(txtLoginCode.Text, txtPassword.Text, Session))
            {
                bFlag = false;
                txtPassword.Focus();
                Dialog.OpenDialogInAjax(txtLoginCode, "抱歉，登录不成功，请检查用户名和密码！");
            }
            return bFlag;
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //登录
            if (SaveCheck())
            {
                string _Password = txtPassword.Text;

                if (_Password == "111111")
                {
                    Response.Redirect("~/Portal/forget.aspx?UserID=" + SysClass.SysGlobal.GetCurrentUserID());
                }
                else
                {
                    SysClass.SysGlobal.UpdatePurviewSystemID(2, "工资电子签收系统");
                    Response.Redirect("~/main.aspx");
                    //Page.RegisterClientScriptBlock("pagechange", "<script>alert('hello');window.location.href='/main.aspx';</" + "script>");
                }
                //Response.Redirect("~/Portal/main.aspx");
                return;
            }          
        }

        protected void txtLoginCode_TextChanged(object sender, EventArgs e)
        {
            txtOpName.Text = "";
            SqlDataReader sdr = SysClass.SysUser.GetUserInfoByReader(txtLoginCode.Text);
            if (sdr.Read())
            {
                txtOpName.Text = sdr["OpName"].ToString();
                txtOpName.Text += "『" + sdr["OrganName"].ToString() + "』";                
            }
            sdr.Close();
        }
    }
}
