using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;
using CyxPack.UserCommonOperation;

namespace JtgTMS.Admin
{
    public partial class User_Lst : System.Web.UI.Page
    {
        public int _OrganID = 0, _DeleteUserID = 0;
        public string _OrganName = "";       
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["OrganID"] != null)
            {
                _OrganID = int.Parse(Request.Params["OrganID"]);
            }
            if (Request.Params["DeleteUserID"] != null)
            {
                _DeleteUserID = int.Parse(Request.Params["DeleteUserID"]);
            }

            if (Request.Params["page"] != null)
            {
                UserCommonOperation.StoreSessionIntValue(SysClass.SysUser.UserLst_PageNo, int.Parse(Request.Params["page"].ToString()));
            }

            if (!Page.IsPostBack)
            {
                txtSearchKeyword.Text = UserCommonOperation.GetSessionStringValue(SysClass.SysUser.User_SearchText);
                if (ddlIsCanLogin.Items.FindByValue(UserCommonOperation.GetSessionStringValue(SysClass.SysUser.User_IsCanLogin).ToString()) != null)
                {
                    ddlIsCanLogin.SelectedValue = UserCommonOperation.GetSessionStringValue(SysClass.SysUser.User_IsCanLogin).ToString();
                }
                chkHasChildren.Checked = UserCommonOperation.GetSessionBoolValue(SysClass.SysUser.User_HasChildren);

                BindPageData();
            }
        }

        private void BindPageData()
        {
            if (_DeleteUserID > 0)
            {
                if (SysClass.SysUser.CheckCanDeleteUser(_DeleteUserID.ToString()))
                {
                    Dialog.OpenDialogInAjax(txtSearchKeyword, "该用户已经有工资记录，不能删除……");
                }
                else
                {
                    ///执行删除操作
                    SysClass.SysUser.DeleteSingleUser(_DeleteUserID);
                }
            }

            _OrganName = SysClass.SysOrgan.GetOrganNameByID(_OrganID);

            string sWhereSQL = "";

            if (txtSearchKeyword.Text.Length > 0)
            {
                sWhereSQL += " And (a.OpName Like '%" + txtSearchKeyword.Text + "%' OR a.OpCode Like '%" + txtSearchKeyword.Text + "%')";
            }
            if (ddlIsCanLogin.SelectedIndex > 0)
            {
                sWhereSQL += " And IsNull(a.IsCanLogin,0)=" + ddlIsCanLogin.SelectedValue.ToString();
            }
            if (chkHasChildren.Checked)
            {
                sWhereSQL += " And a.OrganID in (Select ID From GetOrganChildren(" + _OrganID.ToString() + "))";
            }
            else
            {
                sWhereSQL += " And a.OrganID =" + _OrganID.ToString() ;
            }
            //this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysUser.GetUserLstByWhere(_OrganID, sWhereSQL), gvLists, 15);

            this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysUser.GetSysUserByDataSet(sWhereSQL), gvLists, 15);

            UserCommonOperation.StoreSessionStringValue(SysClass.SysUser.User_SearchText, txtSearchKeyword.Text);
            UserCommonOperation.StoreSessionStringValue(SysClass.SysUser.User_IsCanLogin, ddlIsCanLogin.SelectedValue.ToString());
            UserCommonOperation.StoreSessionBoolValue(SysClass.SysUser.User_HasChildren, chkHasChildren.Checked);
        }        

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (GridViewRow row in this.gvLists.Rows)
            {
                CheckBox CheckRow = (CheckBox)row.FindControl("CheckRow");
                if (CheckRow.Checked)
                {
                    string id = this.gvLists.DataKeys[row.RowIndex].Values["ID"].ToString();

                    
                    //其它处理操作略
                    string SqlText = "Delete from SysUser_Info Where Status=0 And ID=" + id.ToString();

                    if (SysClass.SysUser.CheckCanDeleteUser(id.ToString()))
                    {
                        Dialog.OpenDialogInAjax(txtSearchKeyword, "用户" + SysClass.SysUser.GetOpCodeByUserID(int.Parse(id)).ToString() + "已经有工资记录，不能删除……");
                    }
                    else
                    {
                        if (CyxPack.OperateSqlServer.DataCommon.QueryData(SqlText) > 0)
                        {
                            i++;
                        }
                    }
                }
            }
            if (i > 0)
            {
                BindPageData();
                //Dialog.OpenDialogInAjax(txtSearchKeyword, "恭喜您，" + _OrganName + "选择用户删除成功……");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("User_Edit.aspx?OrganID=" + _OrganID.ToString() + "");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            Response.Redirect("User_Export.aspx?OrganID=" + _OrganID.ToString() + "");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindPageData();
        }

        protected void gvLists_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RestPassword")
            {
                string sPassword = "111111";

                //SqlDataReader sdr = SysClass.SysUser.GetUserInfoByReader(int.Parse(e.CommandArgument.ToString()));
                //if (sdr.Read())
                //{
                //    if (sdr["IDNumber"].ToString().Length > 6)
                //    {
                //        sPassword = sdr["IDNumber"].ToString().Substring(sdr["IDNumber"].ToString().Length - 6, 6);
                //    }
                //}
                //sdr.Close();

                if (CyxPack.OperateSqlServer.DataCommon.QueryData("Update SysUser_Info Set Password='" + sPassword + "' Where ID=" + e.CommandArgument.ToString())> 0)
                {
                    Dialog.OpenDialogInAjax(txtSearchKeyword, "恭喜您，密码重置成功……");
                }
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            Response.Redirect("User_Import.aspx?OrganID=" + _OrganID.ToString() + "");
        }
    }
}
