using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;
//Add by lk 20151214 start
using System.Configuration;
using System.Data;
using System.Web.UI.HtmlControls;

//Add by lk 20151214 end

namespace JtgTMS.WarmingSalary
{
    public partial class SalaryNotSign_Warming : System.Web.UI.Page
    {
        public bool isTurningPage = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (!Page.IsPostBack)
            {
                txtSalaryDateStart.Text = "";
                txtSalaryDateEnd.Text = "";

                isTurningPage = true;

                txtSalaryDateStart.Text = SysClass.SysUserSalary.UserSalary_SearchDateStart;
                txtSalaryDateEnd.Text = SysClass.SysUserSalary.UserSalary_SearchDateEnd;
                txtDepartmentName.Text = SysClass.SysUserSalary.UserSalary_DepartmentName;

                BindPageData();

            }
        }

        private void BindPageData()
        {
            string sWhereSQL = "";

            //如果不是从属与最高部门的员工，此页面只能查询其所属子部门员工的数据！
            SqlDataReader sdrUser = SysClass.SysUser.GetUserInfoByReader(SysClass.SysGlobal.GetCurrentOpCode());
            if (sdrUser.Read())
            {
                SqlDataReader sdrOrg = SysClass.SysOrgan.GetSingleOrganByReader(Convert.ToInt32(sdrUser["OrganID"]));
                if (sdrOrg.Read())
                {
                    if (sdrOrg["POrganID"].ToString() != "0")
                    {
                        sWhereSQL += " OrganID = " + Convert.ToInt32(sdrUser["OrganID"]) + " And ";
                    }
                }
                sdrOrg.Close();
            }
            sdrUser.Close();

            int _SalaryValue = SysClass.SysWarning.GetSalaryValueByOrganID(SysClass.SysGlobal.GetCurrentUserOrganID());

            sWhereSQL = sWhereSQL + " DateDiff(Day, SalaryDate, GetDate()) >= " + _SalaryValue.ToString() + "";

            if (txtDepartmentName.Text.Length > 0)
            {
                sWhereSQL += " And DepartmentName = '" + txtDepartmentName.Text + "'";
            }

            //月份起始和月份结束同时不为空时，按区间查找数据
            if (txtSalaryDateStart.Text.Length > 0 && txtSalaryDateEnd.Text.Length > 0)
            {
                sWhereSQL += " And SalaryDateOnly >= '" + txtSalaryDateStart.Text + "'";

                sWhereSQL += " And SalaryDateOnly <= '" + txtSalaryDateEnd.Text + "'";
            }
            //仅月份起始不为空时，只按月份起始查找数据
            else if (txtSalaryDateStart.Text.Length > 0)
            {
                sWhereSQL += " And SalaryDateOnly = '" + txtSalaryDateStart.Text + "'";
            }
            //仅月份结束不为空时，只按月份结束查找数据
            else if (txtSalaryDateEnd.Text.Length > 0)
            {
                sWhereSQL += " And SalaryDateOnly = '" + txtSalaryDateEnd.Text + "'";
            }

            SysClass.SysUserSalary.UserSalary_DepartmentName = txtDepartmentName.Text;
            SysClass.SysUserSalary.UserSalary_SearchDateStart = txtSalaryDateStart.Text;
            SysClass.SysUserSalary.UserSalary_SearchDateEnd = txtSalaryDateEnd.Text;

            int iPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            this.PageInfo.InnerHtml =
                SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysUserSalary.GetWarmingLstByDataSet(sWhereSQL),
                    gvLists, iPageSize, isTurningPage);

            OperateGridView.UnitRow(this.gvLists, 1, "lblWorkShopName");
            OperateGridView.UnitRow(this.gvLists, 2, "lblDepartmentName");
        }

        //private void BindPageData()
        //{
        //    if (_DeleteUserSalaryID > 0)
        //    {
        //        ///执行删除操作
        //        SysClass.SysUserSalary.DeleteSingleUserSalary(_DeleteUserSalaryID.ToString());
        //    }

        //    string sWhereSQL = " And SignStatus=0";

        //    if (txtUserSalaryOpCode.Text.Length > 0)
        //    {
        //        sWhereSQL = sWhereSQL + " And a.OpCode = '" + txtUserSalaryOpCode.Text + "'";
        //    }

        //    //Upd by lk 20151214 start
        //    //if (txtSalaryYears.Text.Length > 0)
        //    //{
        //    //    sWhereSQL += " And a.SalaryYears = '" + txtSalaryYears.Text + "'";
        //    //}

        //    //if (ddlImportRec.SelectedIndex > 0)
        //    //{
        //    //    sWhereSQL += " And a.SalaryRecGuid='" + ddlImportRec.SelectedValue.ToString() + "'";
        //    //}

        //    //月份起始和月份结束同时不为空时，按区间查找数据
        //    if (txtSalaryYears.Text.Length > 0 && txtSalaryYears2.Text.Length > 0)
        //    {
        //        sWhereSQL += " And a.SalaryYears >= '" + txtSalaryYears.Text + "'";

        //        if (ddlImportRec.SelectedIndex > 0)
        //        {
        //            sWhereSQL += " And a.SalaryDate>='" + ddlImportRec.SelectedItem.Text + "'";
        //        }

        //        sWhereSQL += " And a.SalaryYears <= '" + txtSalaryYears2.Text + "'";

        //        if (ddlImportRec2.SelectedIndex > 0)
        //        {
        //            sWhereSQL += " And a.SalaryDate <= DateAdd(mi,3,'" + ddlImportRec2.SelectedItem.Text + "')";
        //        }
        //    }
        //    //仅月份起始不为空时，只按月份起始查找数据
        //    else if (txtSalaryYears.Text.Length > 0)
        //    {
        //        sWhereSQL += " And a.SalaryYears = '" + txtSalaryYears.Text + "'";

        //        if (ddlImportRec.SelectedIndex > 0)
        //        {
        //            sWhereSQL += " And a.SalaryRecGuid ='" + ddlImportRec.SelectedValue.ToString() + "'";
        //        }
        //    }
        //    //仅月份结束不为空时，只按月份结束查找数据
        //    else if (txtSalaryYears2.Text.Length > 0)
        //    {
        //        sWhereSQL += " And a.SalaryYears = '" + txtSalaryYears2.Text + "'";

        //        if (ddlImportRec2.SelectedIndex > 0)
        //        {
        //            sWhereSQL += " And a.SalaryRecGuid ='" + ddlImportRec2.SelectedValue.ToString() + "'";
        //        }
        //    }

        //    //如果不是从属与最高部门的员工，此页面只能查询其所属子部门员工的数据！
        //    SqlDataReader sdrUser = SysClass.SysUser.GetUserInfoByReader(SysClass.SysGlobal.GetCurrentOpCode());
        //    if (sdrUser.Read())
        //    {
        //        SqlDataReader sdrOrg = SysClass.SysOrgan.GetSingleOrganByReader(Convert.ToInt32(sdrUser["OrganID"]));
        //        if (sdrOrg.Read())
        //        {
        //            if (sdrOrg["POrganID"].ToString() != "0")
        //            {
        //                sWhereSQL += " And b.OrganID = " + Convert.ToInt32(sdrUser["OrganID"]);
        //            }
        //        }
        //        sdrOrg.Close();
        //    }
        //    sdrUser.Close();
        //    //Upd by lk 20151214 end

        //    int _SalaryValue = SysClass.SysWarning.GetSalaryValueByOrganID(SysClass.SysGlobal.GetCurrentUserOrganID());

        //    sWhereSQL = sWhereSQL + " And DateDiff(Day, a.SalaryDate, GetDate()) >= " + _SalaryValue.ToString() + "";

        //    SysClass.SysUserSalary.UserSalary_UserSalaryOpCode = txtUserSalaryOpCode.Text;
        //    SysClass.SysUserSalary.UserSalary_UserSalaryOpName = txtUserSalaryOpName.Text;
        //    SysClass.SysUserSalary.UserSalary_UserSalaryUserID = txtUserSalaryUserID.Text;
        //    SysClass.SysUserSalary.UserSalary_SearchText = txtSalaryYears.Text;


        //    //Upd by lk 20151214 start
        //    //CyxPack.CommonOperation.DataBinder.BindGridViewData(gvLists, SysClass.SysUserSalary.GetSalaryNotSignLstByDataSet(sWhereSQL));

        //    SysClass.SysUserSalary.UserSalary_ImportRecIndex = ddlImportRec.SelectedIndex;
        //    SysClass.SysUserSalary.UserSalary_ImportRecValue = ddlImportRec.SelectedValue.ToString();
        //    SysClass.SysUserSalary.UserSalary_ImportRecText = ddlImportRec.SelectedItem.Text.ToString();
        //    SysClass.SysUserSalary.UserSalary_ImportRecIndex2 = ddlImportRec2.SelectedIndex;
        //    SysClass.SysUserSalary.UserSalary_ImportRecValue2 = ddlImportRec2.SelectedValue.ToString();
        //    SysClass.SysUserSalary.UserSalary_ImportRecText2 = ddlImportRec2.SelectedItem.Text.ToString();
        //    SysClass.SysUserSalary.UserSalary_SearchText2 = txtSalaryYears2.Text;

        //    int iPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
        //    this.PageInfo.InnerHtml =
        //        SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysUserSalary.GetSalaryNotSignLstByDataSet(sWhereSQL),
        //            gvLists, iPageSize, isTurningPage);
        //    //Upd by lk 20151214 end

        //    OperateGridView.UnitRow(this.gvLists, 3, "lblOrganName");
        //    OperateGridView.UnitRow(this.gvLists, 4, "lblSalaryYears");
        //}

        protected void gvLists_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblApprovalStatus = (Label) e.Row.FindControl("lblApprovalStatus");
            Label lblApprovalStatusName = (Label) e.Row.FindControl("lblApprovalStatusName");

            if ((lblApprovalStatus != null) && (lblApprovalStatusName != null))
            {
                if (lblApprovalStatus.Text == SysClass.SysUserSalary.UserSalary_ApprovalIsOK.ToString())
                {
                    //审批通过
                    lblApprovalStatusName.ForeColor = System.Drawing.Color.Green;
                }

                HyperLink hyDelete = (HyperLink) e.Row.FindControl("hyDelete");
                if (hyDelete != null)
                {
                    hyDelete.Visible = int.Parse(lblApprovalStatus.Text) == SysClass.SysUserSalary.UserSalary_Draft;
                }

                CheckBox CheckRow = (CheckBox) e.Row.FindControl("CheckRow");
                if (CheckRow != null)
                {
                    CheckRow.Visible = int.Parse(lblApprovalStatus.Text) == SysClass.SysUserSalary.UserSalary_Draft;
                }

                HyperLink hyEdit = (HyperLink) e.Row.FindControl("hyEdit");

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
            bool hasError = false;
            //验证开始结束时间关系
            if (txtSalaryDateStart.Text.Length > 0 && txtSalaryDateEnd.Text.Length > 0)
            {
                if (string.Compare(txtSalaryDateStart.Text, txtSalaryDateEnd.Text)>0)
                {
                    hasError = true;
                    Dialog.OpenDialogInAjax(txtSalaryDateStart, "起始月份不能大于结束月份！");
                }
            }

            if (!hasError)
            {
                isTurningPage = false;
                BindPageData();
            }
        }
    }
}


 