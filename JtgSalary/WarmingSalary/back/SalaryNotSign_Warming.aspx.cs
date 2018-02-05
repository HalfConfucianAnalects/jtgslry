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
//Add by lk 20151214 end

namespace JtgTMS.WarmingSalary
{
    public partial class SalaryNotSign_Warming : System.Web.UI.Page
    {
        public int _DeleteUserSalaryID = 0, _OrganID = 0;
        //Add by lk 20151214 start
        public bool isTurningPage = true;
        //Add by lk 20151214 end

        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["DeleteUserSalaryID"] != null)
            {
                _DeleteUserSalaryID = int.Parse(Request.Params["DeleteUserSalaryID"]);
            }
            if (!Page.IsPostBack)
            {
                //Add by lk 20151214 start
                //txtSalaryYears.Text = DateTime.Now.ToString("yyyyMM");
                //txtSalaryYears.Text = SysClass.SysUserSalary.UserSalary_SearchText;

                //txtUserSalaryOpCode.Text = SysClass.SysUserSalary.UserSalary_UserSalaryOpCode;
                //txtUserSalaryOpName.Text = SysClass.SysUserSalary.UserSalary_UserSalaryOpName;
                //txtUserSalaryUserID.Text = SysClass.SysUserSalary.UserSalary_UserSalaryUserID;

                //BindPageData();

                txtSalaryYears.Text = "";
                txtSalaryYears2.Text = "";

                if (HttpContext.Current.Request.QueryString["page"] != null)
                {
                    isTurningPage = true;

                    txtSalaryYears.Text = SysClass.SysUserSalary.UserSalary_SearchText;
                    txtSalaryYears2.Text = SysClass.SysUserSalary.UserSalary_SearchText2;

                    txtUserSalaryOpCode.Text = SysClass.SysUserSalary.UserSalary_UserSalaryOpCode;
                    txtUserSalaryOpName.Text = SysClass.SysUserSalary.UserSalary_UserSalaryOpName;
                    txtUserSalaryUserID.Text = SysClass.SysUserSalary.UserSalary_UserSalaryUserID;

                    SysClass.SysUserSalary.FullToSalaryImportRecLst2(ddlImportRec, txtSalaryYears.Text);
                    ddlImportRec.SelectedIndex = SysClass.SysUserSalary.UserSalary_ImportRecIndex;
                    SysClass.SysUserSalary.FullToSalaryImportRecLst2(ddlImportRec2, txtSalaryYears2.Text);
                    ddlImportRec2.SelectedIndex = SysClass.SysUserSalary.UserSalary_ImportRecIndex2;

                    BindPageData();
                }
                //Add by lk 20151214 end
            }
        }
        private void BindPageData()
        {
            if (_DeleteUserSalaryID > 0)
            {
                ///执行删除操作
                SysClass.SysUserSalary.DeleteSingleUserSalary(_DeleteUserSalaryID.ToString());
            }

            string sWhereSQL = " And SignStatus=0";

            if (txtUserSalaryOpCode.Text.Length > 0)
            {
                sWhereSQL = sWhereSQL + " And a.OpCode = '" + txtUserSalaryOpCode.Text + "'";
            }

            //Upd by lk 20151214 start
            //if (txtSalaryYears.Text.Length > 0)
            //{
            //    sWhereSQL += " And a.SalaryYears = '" + txtSalaryYears.Text + "'";
            //}

            //if (ddlImportRec.SelectedIndex > 0)
            //{
            //    sWhereSQL += " And a.SalaryRecGuid='" + ddlImportRec.SelectedValue.ToString() + "'";
            //}

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
            //Upd by lk 20151214 end

            int _SalaryValue = SysClass.SysWarning.GetSalaryValueByOrganID(SysClass.SysGlobal.GetCurrentUserOrganID());

            sWhereSQL = sWhereSQL + " And DateDiff(Day, a.SalaryDate, GetDate()) >= " + _SalaryValue.ToString() + "";

            SysClass.SysUserSalary.UserSalary_UserSalaryOpCode = txtUserSalaryOpCode.Text;
            SysClass.SysUserSalary.UserSalary_UserSalaryOpName = txtUserSalaryOpName.Text;
            SysClass.SysUserSalary.UserSalary_UserSalaryUserID = txtUserSalaryUserID.Text;
            SysClass.SysUserSalary.UserSalary_SearchText = txtSalaryYears.Text;


            //Upd by lk 20151214 start
            //CyxPack.CommonOperation.DataBinder.BindGridViewData(gvLists, SysClass.SysUserSalary.GetSalaryNotSignLstByDataSet(sWhereSQL));

            SysClass.SysUserSalary.UserSalary_ImportRecIndex = ddlImportRec.SelectedIndex;
            SysClass.SysUserSalary.UserSalary_ImportRecValue = ddlImportRec.SelectedValue.ToString();
            SysClass.SysUserSalary.UserSalary_ImportRecText = ddlImportRec.SelectedItem.Text.ToString();
            SysClass.SysUserSalary.UserSalary_ImportRecIndex2 = ddlImportRec2.SelectedIndex;
            SysClass.SysUserSalary.UserSalary_ImportRecValue2 = ddlImportRec2.SelectedValue.ToString();
            SysClass.SysUserSalary.UserSalary_ImportRecText2 = ddlImportRec2.SelectedItem.Text.ToString();
            SysClass.SysUserSalary.UserSalary_SearchText2 = txtSalaryYears2.Text;

            int iPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysUserSalary.GetSalaryNotSignLstByDataSet(sWhereSQL), gvLists, iPageSize, isTurningPage);
            //Upd by lk 20151214 end
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
            //Upd by lk 20151214 start
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
                isTurningPage = false;
                BindPageData();
            }
            //Upd by lk 20151214 end
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
                //Dialog.OpenDialogInAjax(txtUserSalaryOpCode, "工号" + txtUserSalaryOpCode.Text + "不存在！");
            }
        }

        protected void btnSalarys_Click(object sender, EventArgs e)
        {
            SysClass.SysUserSalary.FullToSalaryImportRecLst2(ddlImportRec, txtSalaryYears.Text);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {            
            if (gvLists.Rows.Count > 0)
            {
                //Upd by lk 20151214 start
                //gvLists.Columns[gvLists.Columns.Count - 1].Visible = false;
                ////调用导出方法  
                //ExportGridViewForUTF8(gvLists, DateTime.Now.ToString() + ".xls");

                //gvLists.Columns[gvLists.Columns.Count - 1].Visible = true;

                //数据导出到Excel
                OutputDataToExcle(DateTime.Now.ToString() + ".xls");
                //Upd by lk 20151214 end
            }
            else
            {
                Dialog.OpenDialogInAjax(btnExport, "不存在可导出的查询结果！");
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for 
        }

        private void ExportGridViewForUTF8(GridView GridView, string filename)
        {

            string attachment = "attachment; filename=" + filename;

            Response.ClearContent();
            Response.Buffer = true;
            Response.Write("上海动车段工资电子签收系统");
            Response.AddHeader("content-disposition", attachment);

            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/ms-excel";
            System.IO.StringWriter sw = new System.IO.StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);
            GridView.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

        }

        //Add by lk 20151214 start
        /// <summary>
        /// 数据导出到Excel
        /// </summary>
        /// <returns></returns>
        private void OutputDataToExcle(string strFilename)
        {
            string sWhereSQL = " And SignStatus=0";

            if (SysClass.SysUserSalary.UserSalary_UserSalaryOpCode.Length > 0)
            {
                sWhereSQL = sWhereSQL + " And a.OpCode = '" + SysClass.SysUserSalary.UserSalary_UserSalaryOpCode + "'";
            }

            //月份起始和月份结束同时不为空时，按区间查找数据
            if (SysClass.SysUserSalary.UserSalary_SearchText.Length > 0 && SysClass.SysUserSalary.UserSalary_SearchText2.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears >= '" + SysClass.SysUserSalary.UserSalary_SearchText + "'";

                if (SysClass.SysUserSalary.UserSalary_ImportRecIndex > 0)
                {
                    sWhereSQL += " And a.SalaryDate>='" + SysClass.SysUserSalary.UserSalary_ImportRecText + "'";
                }

                sWhereSQL += " And a.SalaryYears <= '" + SysClass.SysUserSalary.UserSalary_SearchText2 + "'";

                if (SysClass.SysUserSalary.UserSalary_ImportRecIndex2 > 0)
                {
                    sWhereSQL += " And a.SalaryDate <= DateAdd(mi,3,'" + SysClass.SysUserSalary.UserSalary_ImportRecText2 + "')";
                }
            }
            //仅月份起始不为空时，只按月份起始查找数据
            else if (SysClass.SysUserSalary.UserSalary_SearchText.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears = '" + SysClass.SysUserSalary.UserSalary_SearchText + "'";

                if (SysClass.SysUserSalary.UserSalary_ImportRecIndex > 0)
                {
                    sWhereSQL += " And a.SalaryRecGuid ='" + SysClass.SysUserSalary.UserSalary_ImportRecValue + "'";
                }
            }
            //仅月份结束不为空时，只按月份结束查找数据
            else if (SysClass.SysUserSalary.UserSalary_SearchText2.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears = '" + SysClass.SysUserSalary.UserSalary_SearchText2 + "'";

                if (SysClass.SysUserSalary.UserSalary_ImportRecIndex2 > 0)
                {
                    sWhereSQL += " And a.SalaryRecGuid ='" + SysClass.SysUserSalary.UserSalary_ImportRecValue2 + "'";
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

            int _SalaryValue = SysClass.SysWarning.GetSalaryValueByOrganID(SysClass.SysGlobal.GetCurrentUserOrganID());

            sWhereSQL = sWhereSQL + " And DateDiff(Day, a.SalaryDate, GetDate()) >= " + _SalaryValue.ToString() + "";

            DataSet ds = SysClass.SysUserSalary.GetSalaryNotSignLstByDataSet(sWhereSQL);

            //创建DataTable对象
            DataTable dt = new DataTable("output");
            
            //构建表结构,往表中添加列
            dt.Columns.Add("序号", typeof(int));
            dt.Columns.Add("工号", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("单位部门", typeof(string));
            dt.Columns.Add("工资年月", typeof(string));
            dt.Columns.Add("发放日期", typeof(string));
            dt.Columns.Add("发放金额", typeof(decimal));
            dt.Columns.Add("签收状态", typeof(string));
            dt.Columns.Add("未签收天数", typeof(int));

            for (int iLoop = 0; iLoop < ds.Tables[0].Rows.Count;iLoop++)
            {
                //创建DataRow对象
                DataRow dr = dt.NewRow();

                //新增数据
                dr[0] = iLoop+1;
                dr[1] = ds.Tables[0].Rows[iLoop]["OpCode"].ToString();
                dr[2] = ds.Tables[0].Rows[iLoop]["OpName"].ToString();
                dr[3] = ds.Tables[0].Rows[iLoop]["OrganName"].ToString();
                dr[4] = ds.Tables[0].Rows[iLoop]["SalaryYears"].ToString();
                if (ds.Tables[0].Rows[iLoop]["SalaryDate"].ToString().Length > 0)
                {
                    dr[5] = DateTime.Parse(ds.Tables[0].Rows[iLoop]["SalaryDate"].ToString()).ToString("yyyy-MM-dd");
                }
                else
                {
                    dr[5] = "";
                }

                dr[6] = ds.Tables[0].Rows[iLoop]["TotalSalary"].ToString();
                if (ds.Tables[0].Rows[iLoop]["SignStatus"].ToString() == "1")
                {
                    dr[7] = "已签收";
                }
                else
                {
                    dr[7] = "未签收";
                }

                dr[8] = ds.Tables[0].Rows[iLoop]["NotSingDay"].ToString();

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

        protected void btnSalarys2_Click(object sender, EventArgs e)
        {
            SysClass.SysUserSalary.FullToSalaryImportRecLst2(ddlImportRec2, txtSalaryYears2.Text);
        }

        protected void btnChoiceUser_Click(object sender, EventArgs e)
        {
            txtUserSalaryUserID.Text = SysClass.SysParams.UserInfo_ChoiceValues;
            txtUserSalaryOpCode.Text = SysClass.SysUser.GetOpCodeByUserID(int.Parse(SysClass.SysParams.UserInfo_ChoiceValues));
            txtUserSalaryOpName.Text = SysClass.SysUser.GetUserNameByUserID(int.Parse(SysClass.SysParams.UserInfo_ChoiceValues));

            SysClass.SysParams.UserInfo_ChoiceValues = "";
        }
        //Add by lk 20151214 end
    }
}
