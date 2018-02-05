using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Diagnostics;
using CyxPack.CommonOperation;
using System.Data;

namespace JtgTMS.PersonSalary
{
    public partial class UserNotSignSalary_Lst : System.Web.UI.Page
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
                txtSalaryYears2.Text = SysClass.SysUserSalary.UserSalary_SearchText2;
                
                txtUserSalaryOpCode.Text = SysClass.SysUserSalary.UserSalary_UserSalaryOpCode;
                txtUserSalaryOpName.Text = SysClass.SysUserSalary.UserSalary_UserSalaryOpName;
                txtUserSalaryUserID.Text = SysClass.SysUserSalary.UserSalary_UserSalaryUserID;

                BindPageData();
            }
        }
        private void BindPageData()
        {
            string sWhereSQL = " And SignStatus=0";

            if (txtUserSalaryOpCode.Text.Length > 0)
            {
                sWhereSQL += " And a.OpCode = '" + txtUserSalaryOpCode.Text + "'";
            }

            //月份起始和月份结束同时不为空时，按区间查找数据
            if (txtSalaryYears.Text.Length > 0 && txtSalaryYears2.Text.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears >= '" + txtSalaryYears.Text + "'";

                if (ddlImportRec.SelectedIndex > 0)
                {
                    sWhereSQL += " And a.SalaryDate>='" + ddlImportRec.SelectedItem.Text + "'";
                }

                sWhereSQL += " And a.SalaryYears <= '" + txtSalaryYears2.Text + "'";

                if (ddlImportRec2.SelectedIndex > 0)
                {
                    sWhereSQL += " And a.SalaryDate <= DateAdd(mi,3,'" + ddlImportRec2.SelectedItem.Text + "')";
                }
            }
            //仅月份起始不为空时，只按月份起始查找数据
            else if (txtSalaryYears.Text.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears = '" + txtSalaryYears.Text + "'";

                if (ddlImportRec.SelectedIndex > 0)
                {
                    sWhereSQL += " And a.SalaryRecGuid ='" + ddlImportRec.SelectedValue.ToString() + "'";
                }
            }
            //仅月份结束不为空时，只按月份结束查找数据
            else if (txtSalaryYears2.Text.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears = '" + txtSalaryYears2.Text + "'";

                if (ddlImportRec2.SelectedIndex > 0)
                {
                    sWhereSQL += " And a.SalaryRecGuid ='" + ddlImportRec2.SelectedValue.ToString() + "'";
                }
            }

            //如果不是从属与最高部门的员工，此页面只能查询其所属子部门员工的数据！
            SqlDataReader sdrUser = SysClass.SysUser.GetUserInfoByReader(SysClass.SysGlobal.GetCurrentOpCode());
            if (sdrUser.Read())
            {
                SqlDataReader sdrOrg = SysClass.SysOrgan.GetSingleOrganByReader(Convert.ToInt32(sdrUser["OrganID"]));
                if (sdrOrg.Read())
                {
                    if (sdrOrg["POrganID"].ToString() != "0")
                    {
                        sWhereSQL += " And b.OrganID = " + Convert.ToInt32(sdrUser["OrganID"]);
                    }
                }
                sdrOrg.Close();
            }
            sdrUser.Close();

            SysClass.SysUserSalary.UserSalary_UserSalaryOpCode = txtUserSalaryOpCode.Text;
            SysClass.SysUserSalary.UserSalary_UserSalaryOpName = txtUserSalaryOpName.Text;
            SysClass.SysUserSalary.UserSalary_UserSalaryUserID = txtUserSalaryUserID.Text;
            SysClass.SysUserSalary.UserSalary_SearchText = txtSalaryYears.Text;
            SysClass.SysUserSalary.UserSalary_SearchText2 = txtSalaryYears2.Text;

            this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysUserSalary.GetUserSalaryLstByDataSet(sWhereSQL), gvLists, 15);
            this.btnPrint.Visible = SysClass.SysUser.UserIsAdmin(SysClass.SysGlobal.GetCurrentOpCode());
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
            //BindPageData();
            bool hasError = false;
            //验证开始结束时间关系
            if (txtSalaryYears.Text.Length > 0 && txtSalaryYears2.Text.Length > 0)
            {
                if (Convert.ToInt32(txtSalaryYears.Text) > Convert.ToInt32(txtSalaryYears2.Text))
                {
                    hasError = true;
                    Dialog.OpenDialogInAjax(txtSalaryYears, "起始月份不能大于结束月份！");
                }
            }

            if (!hasError)
            {
                BindPageData();
            }
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
                if (txtUserSalaryOpCode.Text.Length > 0)
                {
                    Dialog.OpenDialogInAjax(txtUserSalaryOpCode, "工号" + txtUserSalaryOpCode.Text + "不存在！");
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (gvLists.Rows.Count > 0)
            {
                //数据导出到Excel
                OutputDataToExcle(DateTime.Now.ToString() + ".xls");
            }
            else
            {
                Dialog.OpenDialogInAjax(btnExport, "不存在可导出的查询结果！");
            }
        }

        /// <summary>
        /// 数据导出到Excel
        /// </summary>
        /// <returns></returns>
        private void OutputDataToExcle(string strFilename)
        {
            string sWhereSQL = " And SignStatus=0";

            if (txtUserSalaryOpCode.Text.Length > 0)
            {
                sWhereSQL += " And a.OpCode = '" + txtUserSalaryOpCode.Text + "'";
            }

            //月份起始和月份结束同时不为空时，按区间查找数据
            if (txtSalaryYears.Text.Length > 0 && txtSalaryYears2.Text.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears >= '" + txtSalaryYears.Text + "'";

                if (ddlImportRec.SelectedIndex > 0)
                {
                    sWhereSQL += " And a.SalaryDate>='" + ddlImportRec.SelectedItem.Text + "'";
                }

                sWhereSQL += " And a.SalaryYears <= '" + txtSalaryYears2.Text + "'";

                if (ddlImportRec2.SelectedIndex > 0)
                {
                    sWhereSQL += " And a.SalaryDate <= DateAdd(mi,3,'" + ddlImportRec2.SelectedItem.Text + "')";
                }
            }
            //仅月份起始不为空时，只按月份起始查找数据
            else if (txtSalaryYears.Text.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears = '" + txtSalaryYears.Text + "'";

                if (ddlImportRec.SelectedIndex > 0)
                {
                    sWhereSQL += " And a.SalaryRecGuid ='" + ddlImportRec.SelectedValue.ToString() + "'";
                }
            }
            //仅月份结束不为空时，只按月份结束查找数据
            else if (txtSalaryYears2.Text.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears = '" + txtSalaryYears2.Text + "'";

                if (ddlImportRec2.SelectedIndex > 0)
                {
                    sWhereSQL += " And a.SalaryRecGuid ='" + ddlImportRec2.SelectedValue.ToString() + "'";
                }
            }

            //如果不是从属与最高部门的员工，此页面只能查询其所属子部门员工的数据！
            SqlDataReader sdrUser = SysClass.SysUser.GetUserInfoByReader(SysClass.SysGlobal.GetCurrentOpCode());
            if (sdrUser.Read())
            {
                SqlDataReader sdrOrg = SysClass.SysOrgan.GetSingleOrganByReader(Convert.ToInt32(sdrUser["OrganID"]));
                if (sdrOrg.Read())
                {
                    if (sdrOrg["POrganID"].ToString() != "0")
                    {
                        sWhereSQL += " And b.OrganID = " + Convert.ToInt32(sdrUser["OrganID"]);
                    }
                }
                sdrOrg.Close();
            }
            sdrUser.Close();

            DataSet ds = SysClass.SysUserSalary.GetUserSalaryLstByDataSet(sWhereSQL);
            //创建DataTable对象
            DataTable dt = new DataTable("output");

            //构建表结构,往表中添加列
            dt.Columns.Add("序号", typeof(int));
            dt.Columns.Add("工号", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("工资年月", typeof(string));
            dt.Columns.Add("发放日期", typeof(string));
            dt.Columns.Add("发放金额", typeof(decimal));
            dt.Columns.Add("签收状态", typeof(string));

            for (int iLoop = 0; iLoop < ds.Tables[0].Rows.Count; iLoop++)
            {
                //创建DataRow对象
                DataRow dr = dt.NewRow();

                //新增数据
                dr[0] = iLoop + 1;
                dr[1] = ds.Tables[0].Rows[iLoop]["OpCode"].ToString();
                dr[2] = ds.Tables[0].Rows[iLoop]["OpName"].ToString();
                dr[3] = ds.Tables[0].Rows[iLoop]["SalaryYears"].ToString();
                string SalaryOnlyDate = ds.Tables[0].Rows[iLoop]["SalaryDate"].ToString();
                dr[4] = SalaryOnlyDate.Length > 0 ? DateTime.Parse(SalaryOnlyDate).ToString("yyyy-MM-dd") : "";
                dr[5] = ds.Tables[0].Rows[iLoop]["TotalSalary"].ToString();
                dr[6] = "未签收";

                //把数据添加到表结构中
                dt.Rows.Add(dr);
            }

            System.Web.UI.WebControls.DataGrid dgExport = null;

            // 当前对话   
            System.Web.HttpContext curContext = System.Web.HttpContext.Current;
            // IO用于导出并返回excel文件   
            System.IO.StringWriter strWriter = null;
            System.Web.UI.HtmlTextWriter htmlWriter = null;

            // 设置编码和附件格式   
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            curContext.Response.Charset = "gb2312";

            // 导出excel文件   
            strWriter = new System.IO.StringWriter();
            htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

            // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid   
            dgExport = new System.Web.UI.WebControls.DataGrid();
            dgExport.DataSource = dt.DefaultView;
            dgExport.AllowPaging = false;
            dgExport.DataBind();

            // 返回客户端   
            dgExport.RenderControl(htmlWriter);
            curContext.Response.Clear();
            curContext.Response.Buffer = true;
            curContext.Response.Write("上海动车段工资电子签收系统");
            curContext.Response.AddHeader("Content-Disposition", "attachment;filename=" + strFilename);
            curContext.Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + strWriter.ToString());
            curContext.Response.Flush();
            curContext.Response.End();
        }
    }
}
