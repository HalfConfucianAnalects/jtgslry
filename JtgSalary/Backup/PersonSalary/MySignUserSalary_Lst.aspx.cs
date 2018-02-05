using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;
//Add by lk 20151221 start
using System.Data;
//Add by lk 20151221 end

namespace JtgTMS.PersonSalary
{
    public partial class MySignUserSalary_Lst : System.Web.UI.Page
    {
        public int _CancelID = 0, _OrganID = 0;
        //Add by lk 20151214 start
        public bool isTurningPage = true;
        //Add by lk 20151214 end

        //Add by lk 20151221 start
        private double mysum = 0;
        //Add by lk 20151221 end

        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["CancelID"] != null)
            {
                _CancelID = int.Parse(Request.Params["CancelID"]);
            }

            if (!Page.IsPostBack)
            {
                //Add by lk 20151214 start
                if (HttpContext.Current.Request.QueryString["page"] != null)
                {
                    isTurningPage = true;
                    txtSalaryYears.Text = SysClass.SysUserSalary.UserSalary_SearchText;
                    txtSalaryYears2.Text = SysClass.SysUserSalary.UserSalary_SearchText2;
                    //Add by lk 20160118 start
                    switch (SysClass.SysUserSalary.UserSalary_FlgValue)
                    {
                        //没有选中任何一项
                        case 0:
                            chkGongzi.Checked = false;
                            chkJiangjin.Checked = false;
                            chkBaoxiao.Checked = false;
                            break;
                        //仅【工资】被选中
                        case 1:
                            chkGongzi.Checked = true;
                            chkJiangjin.Checked = false;
                            chkBaoxiao.Checked = false;
                            break;
                        //仅【奖金】被选中
                        case 2:
                            chkGongzi.Checked = false;
                            chkJiangjin.Checked = true;
                            chkBaoxiao.Checked = false;
                            break;
                        //【工资】和【奖金】被选中
                        case 3:
                            chkGongzi.Checked = true;
                            chkJiangjin.Checked = true;
                            chkBaoxiao.Checked = false;
                            break;
                        //仅【报销】被选中
                        case 4:
                            chkGongzi.Checked = false;
                            chkJiangjin.Checked = false;
                            chkBaoxiao.Checked = true;
                            break;
                        //【工资】和【报销】被选中
                        case 5:
                            chkGongzi.Checked = true;
                            chkJiangjin.Checked = false;
                            chkBaoxiao.Checked = true;
                            break;
                        //【奖金】和【报销】被选中
                        case 6:
                            chkGongzi.Checked = false;
                            chkJiangjin.Checked = true;
                            chkBaoxiao.Checked = true;
                            break;
                        //【工资】【奖金】【报销】都被选中
                        case 7:
                            chkGongzi.Checked = true;
                            chkJiangjin.Checked = true;
                            chkBaoxiao.Checked = true;
                            break;
                    }
                    //Add by lk 20160118 end
                }
                //Add by lk 20151214 end
                BindPageData();
            }
        }
        private void BindPageData()
        {
            if (_CancelID > 0)
            {
                ///执行删除操作
                SysClass.SysUserSalary.CancelSingleUserSalary(_CancelID.ToString());
            }

            gvLists.Columns[gvLists.Columns.Count-1].Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(113, "");

            string sWhereSQL = " And a.OpCode='" + SysClass.SysGlobal.GetCurrentOpCode() + "' And SignStatus=1"
                + " And a.SalaryRecGuid in (Select SalaryRecGuid From UserUserImportRec_Info where ApprovalStatus=1)";

            //Upd by lk 20151214 start
            //if (txtSalaryYears.Text.Length > 0)
            //{
            //    sWhereSQL = sWhereSQL + " And a.SalaryYears like '" + txtSalaryYears.Text + "'";
            //}

            //月份起始和月份结束同时不为空时，按区间查找数据
            if (txtSalaryYears.Text.Length > 0 && txtSalaryYears2.Text.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears >= '" + txtSalaryYears.Text + "'";

                sWhereSQL += " And a.SalaryYears <= '" + txtSalaryYears2.Text + "'";
            }
            //仅月份起始不为空时，只按月份起始查找数据
            else if (txtSalaryYears.Text.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears = '" + txtSalaryYears.Text + "'";
            }
            //仅月份结束不为空时，只按月份结束查找数据
            else if (txtSalaryYears2.Text.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears = '" + txtSalaryYears2.Text + "'";
            }

            SysClass.SysUserSalary.UserSalary_SearchText = txtSalaryYears.Text;
            SysClass.SysUserSalary.UserSalary_SearchText2 = txtSalaryYears2.Text;

            //Add by lk 20160118 start
            SysClass.SysUserSalary.UserSalary_FlgValue = 0;
            if (chkGongzi.Checked)
            {
                SysClass.SysUserSalary.UserSalary_FlgValue += 1;
            }
            if (chkJiangjin.Checked)
            {
                SysClass.SysUserSalary.UserSalary_FlgValue += 2;
            }
            if (chkBaoxiao.Checked)
            {
                SysClass.SysUserSalary.UserSalary_FlgValue += 4;
            }

            switch (SysClass.SysUserSalary.UserSalary_FlgValue)
            {
                //没有选中任何一项
                case 0:
                    sWhereSQL += " And c.Description = ''";
                    break;
                //仅【工资】被选中
                case 1:
                    sWhereSQL += " And c.Description like '%" + chkGongzi.Text + "%'";
                    break;
                //仅【奖金】被选中
                case 2:
                    sWhereSQL += " And c.Description like '%" + chkJiangjin.Text + "%'";
                    break;
                //【工资】和【奖金】被选中
                case 3:
                    sWhereSQL += " And (c.Description like '%" + chkGongzi.Text + "%'";
                    sWhereSQL += " or c.Description like '%" + chkJiangjin.Text + "%')";
                    break;
                //仅【报销】被选中
                case 4:
                    sWhereSQL += " And c.Description like '%" + chkBaoxiao.Text + "%'";
                    break;
                //【工资】和【报销】被选中
                case 5:
                    sWhereSQL += " And (c.Description like '%" + chkGongzi.Text + "%'";
                    sWhereSQL += " or c.Description like '%" + chkBaoxiao.Text + "%')";
                    break;
                //【奖金】和【报销】被选中
                case 6:
                    sWhereSQL += " And (c.Description like '%" + chkJiangjin.Text + "%'";
                    sWhereSQL += " or c.Description like '%" + chkBaoxiao.Text + "%')";
                    break;
                //【工资】【奖金】【报销】都被选中
                case 7:
                    break;
            }
            //Add by lk 20160118 end

            //Add by lk 20151221 start
            mysum = 0;
            DataSet ds = SysClass.SysUserSalary.GetUserSalaryLstByDataSet(sWhereSQL);
            //计算全结果的合计
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                mysum += double.Parse(dr["TotalSalary"].ToString());
            }
            //Add by lk 20151221 end

            //this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysUserSalary.GetUserSalaryLstByDataSet(sWhereSQL), gvLists, 15);
            this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(ds, gvLists, 15, isTurningPage);
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

            //Add by lk 20151221 start
            // 合计
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //Add by lk 20151214 start
                e.Row.Cells[3].Text = "总合计：";
                //Add by lk 20151214 end
                e.Row.Cells[4].Text = mysum.ToString("0.00");
            }
            //Add by lk 20151221 end
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


        //Add by lk 20151221 start
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
            string sWhereSQL = " And a.OpCode='" + SysClass.SysGlobal.GetCurrentOpCode() + "' And SignStatus=1"
    + " And a.SalaryRecGuid in (Select SalaryRecGuid From UserUserImportRec_Info where ApprovalStatus=1)";

            //月份起始和月份结束同时不为空时，按区间查找数据
            if (SysClass.SysUserSalary.UserSalary_SearchText.Length > 0 && SysClass.SysUserSalary.UserSalary_SearchText2.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears >= '" + SysClass.SysUserSalary.UserSalary_SearchText + "'";

                sWhereSQL += " And a.SalaryYears <= '" + SysClass.SysUserSalary.UserSalary_SearchText2 + "'";
            }
            //仅月份起始不为空时，只按月份起始查找数据
            else if (SysClass.SysUserSalary.UserSalary_SearchText.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears = '" + SysClass.SysUserSalary.UserSalary_SearchText + "'";
            }
            //仅月份结束不为空时，只按月份结束查找数据
            else if (SysClass.SysUserSalary.UserSalary_SearchText2.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears = '" + SysClass.SysUserSalary.UserSalary_SearchText2 + "'";
            }

            //Add by lk 20160118 start
            switch (SysClass.SysUserSalary.UserSalary_FlgValue)
            {
                //没有选中任何一项
                case 0:
                    sWhereSQL += " And c.Description = ''";
                    break;
                //仅【工资】被选中
                case 1:
                    sWhereSQL += " And c.Description like '%" + chkGongzi.Text + "%'";
                    break;
                //仅【奖金】被选中
                case 2:
                    sWhereSQL += " And c.Description like '%" + chkJiangjin.Text + "%'";
                    break;
                //【工资】和【奖金】被选中
                case 3:
                    sWhereSQL += " And (c.Description like '%" + chkGongzi.Text + "%'";
                    sWhereSQL += " or c.Description like '%" + chkJiangjin.Text + "%')";
                    break;
                //仅【报销】被选中
                case 4:
                    sWhereSQL += " And c.Description like '%" + chkBaoxiao.Text + "%'";
                    break;
                //【工资】和【报销】被选中
                case 5:
                    sWhereSQL += " And (c.Description like '%" + chkGongzi.Text + "%'";
                    sWhereSQL += " or c.Description like '%" + chkBaoxiao.Text + "%')";
                    break;
                //【奖金】和【报销】被选中
                case 6:
                    sWhereSQL += " And (c.Description like '%" + chkJiangjin.Text + "%'";
                    sWhereSQL += " or c.Description like '%" + chkBaoxiao.Text + "%')";
                    break;
                //【工资】【奖金】【报销】都被选中
                case 7:
                    break;
            }
            //Add by lk 20160118 end

            DataSet ds = SysClass.SysUserSalary.GetUserSalaryLstByDataSet(sWhereSQL);

            //创建DataTable对象
            DataTable dt = new DataTable("output");

            //构建表结构,往表中添加列
            dt.Columns.Add("序号", typeof(int));
            dt.Columns.Add("工号", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("工资年月", typeof(string));
            dt.Columns.Add("发放金额", typeof(decimal));
            dt.Columns.Add("签收状态", typeof(string));
            dt.Columns.Add("签收时间", typeof(string));
            dt.Columns.Add("备注", typeof(string));

            //清空合计结果
            mysum = 0;

            for (int iLoop = 0; iLoop < ds.Tables[0].Rows.Count; iLoop++)
            {
                //创建DataRow对象
                DataRow dr = dt.NewRow();

                //新增数据
                dr[0] = iLoop + 1;
                dr[1] = ds.Tables[0].Rows[iLoop]["OpCode"].ToString();
                dr[2] = ds.Tables[0].Rows[iLoop]["OpName"].ToString();
                dr[3] = ds.Tables[0].Rows[iLoop]["SalaryYears"].ToString();
                dr[4] = ds.Tables[0].Rows[iLoop]["TotalSalary"].ToString();
                if (ds.Tables[0].Rows[iLoop]["SignStatus"].ToString() == "1")
                {
                    dr[5] = "已签收";
                }
                else
                {
                    dr[5] = "未签收";
                }
                dr[6] = ds.Tables[0].Rows[iLoop]["SignDate"].ToString();
                dr[7] = ds.Tables[0].Rows[iLoop]["Description"].ToString();

                //把数据添加到表结构中
                dt.Rows.Add(dr);

                //合计结果累加
                mysum += double.Parse(ds.Tables[0].Rows[iLoop]["TotalSalary"].ToString());
            }

            //添加合计部分数据
            DataRow foot = dt.NewRow();
            foot[3] = "总合计：";
            foot[4] = mysum.ToString("0.00");
            dt.Rows.Add(foot);

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
        //Add by lk 20151221 end

    }
}
