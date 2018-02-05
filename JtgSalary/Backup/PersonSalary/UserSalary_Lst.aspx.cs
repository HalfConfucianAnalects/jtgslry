using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;

namespace JtgTMS.PersonSalary
{
    public partial class UserSalary_Lst : System.Web.UI.Page
    {
        public int _DeleteUserSalaryID = 0, _OrganID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["DeleteUserSalaryID"] != null)
            {
                _DeleteUserSalaryID = int.Parse(Request.Params["DeleteUserSalaryID"]);
            }
            if (!Page.IsPostBack)
            {
                txtSalaryYears.Text = SysClass.SysUserSalary.UserSalary_SearchText;
                
                txtUserSalaryOpCode.Text = SysClass.SysUserSalary.UserSalary_UserSalaryOpCode;
                txtUserSalaryOpName.Text = SysClass.SysUserSalary.UserSalary_UserSalaryOpName;
                txtUserSalaryUserID.Text = SysClass.SysUserSalary.UserSalary_UserSalaryUserID;

                BindPageData();
            }
        }
        private void BindPageData()
        {
            if (_DeleteUserSalaryID > 0)
            {
                ///执行删除操作
                SysClass.SysUserSalary.DeleteSingleUserSalary(_DeleteUserSalaryID.ToString());
            }

            string sWhereSQL = " And SignStatus=1";

            if (txtUserSalaryOpCode.Text.Length > 0)
            {
                sWhereSQL = sWhereSQL + " And a.OpCode = '" + txtUserSalaryOpCode.Text + "'";
            }

            if (txtSalaryYears.Text.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears = '" + txtSalaryYears.Text + "'";
            }

            SysClass.SysUserSalary.UserSalary_UserSalaryOpCode = txtUserSalaryOpCode.Text;
            SysClass.SysUserSalary.UserSalary_UserSalaryOpName = txtUserSalaryOpName.Text;
            SysClass.SysUserSalary.UserSalary_UserSalaryUserID = txtUserSalaryUserID.Text;
            SysClass.SysUserSalary.UserSalary_SearchText = txtSalaryYears.Text;

            this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysUserSalary.GetUserSalaryLstByDataSet(sWhereSQL), gvLists, 15);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string _DeleteUserSalaryIDs = "";
            foreach (GridViewRow row in this.gvLists.Rows)
            {
                CheckBox CheckRow = (CheckBox)row.FindControl("CheckRow");
                if (CheckRow.Checked)
                {
                    if (_DeleteUserSalaryIDs.Length > 0)
                    {
                        _DeleteUserSalaryIDs += ",";
                    }
                    _DeleteUserSalaryIDs += this.gvLists.DataKeys[row.RowIndex].Values["ID"].ToString();                  
                }
            }
            if ((_DeleteUserSalaryIDs.Length > 0) && (SysClass.SysUserSalary.DeleteSingleUserSalary(_DeleteUserSalaryIDs) > 0))
            {
                BindPageData();
                Dialog.OpenDialogInAjax(txtSalaryYears, "恭喜您，选择的领用订单删除成功……");
            }
        }

        protected void gvLists_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblApprovalStatus = (Label)e.Row.FindControl("lblApprovalStatus");
            Label lblApprovalStatusName = (Label)e.Row.FindControl("lblApprovalStatusName");

            if ((lblApprovalStatus != null) && (lblApprovalStatusName != null))
            {
                if (lblApprovalStatus.Text == SysClass.SysUserSalary.UserSalary_ApprovalIsOK.ToString())
                {
                    //审批通过
                    lblApprovalStatusName.ForeColor = System.Drawing.Color.Green;
                }

                HyperLink hyDelete = (HyperLink)e.Row.FindControl("hyDelete");                
                if (hyDelete != null)
                {
                    hyDelete.Visible = int.Parse(lblApprovalStatus.Text) == SysClass.SysUserSalary.UserSalary_Draft;
                }

                CheckBox CheckRow = (CheckBox)e.Row.FindControl("CheckRow");
                if (CheckRow != null)
                {
                    CheckRow.Visible = int.Parse(lblApprovalStatus.Text) == SysClass.SysUserSalary.UserSalary_Draft;
                }

                HyperLink hyEdit = (HyperLink)e.Row.FindControl("hyEdit");

                if (hyEdit != null)
                {
                    if (int.Parse(lblApprovalStatus.Text) == SysClass.SysUserSalary.UserSalary_ApprovalIsOK)
                    {
                        hyEdit.Text = "查看";
                    }
                    else
                    {
                        hyEdit.Text = "编辑";
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindPageData();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("../PersonSalary/UserSalary_Edit.aspx");
        }

        protected void txtUserSalaryOpCode_TextChanged(object sender, EventArgs e)
        {
            txtUserSalaryOpCode.Text = CyxPack.CommonOperation.DealwithString.GetStringPrefix(txtUserSalaryOpCode.Text);

            txtUserSalaryUserID.Text = SysClass.SysUser.GetSelfUserIDByOpCode(txtUserSalaryOpCode.Text).ToString();
            txtUserSalaryOpName.Text = SysClass.SysUser.GetSelfUserNameByOpCode(txtUserSalaryOpCode.Text);
            if ((txtUserSalaryUserID.Text == "0") || (txtUserSalaryUserID.Text.Length == 0))
            {
                txtUserSalaryOpCode.Text = "";
                Dialog.OpenDialogInAjax(txtUserSalaryOpCode, "工号" + txtUserSalaryOpCode.Text + "不存在！");
            }
        }
    }
}
