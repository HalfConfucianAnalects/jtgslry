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
    public partial class SalaryNotSign_WarmingDetail : System.Web.UI.Page
    {
        public string SalaryYears = "", OrganID = "";
        public bool isTurningPage = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();
            if (Request.Params["SalaryYears"] != null)
            {
                SalaryYears = Request.Params["SalaryYears"];
            }
            if (Request.Params["OrganID"] != null)
            {
                OrganID = Request.Params["OrganID"];
            }

            if (!Page.IsPostBack)
            {
                //if (HttpContext.Current.Request.QueryString["page"] != null)
                {
                    isTurningPage = true;

                    txtUserSalaryOpCode.Text = SysClass.SysUserSalary.UserSalary_UserSalaryOpCode;
                    txtUserSalaryOpName.Text = SysClass.SysUserSalary.UserSalary_UserSalaryOpName;
                    txtUserSalaryUserID.Text = SysClass.SysUserSalary.UserSalary_UserSalaryUserID;

                    BindPageData();
                }
            }
        }
        private void BindPageData()
        {
            string sWhereSQL = " And SignStatus=0 And SalaryYears='" + SalaryYears+"' ";
            sWhereSQL += " And b.OrganID = " + Convert.ToInt32(OrganID);

            if (txtUserSalaryOpCode.Text.Length > 0)
            {
                sWhereSQL = sWhereSQL + " And a.OpCode = '" + txtUserSalaryOpCode.Text + "'";
            }

            ////如果不是从属与最高部门的员工，此页面只能查询其所属子部门员工的数据！
            //SqlDataReader sdrUser = SysClass.SysUser.GetUserInfoByReader(SysClass.SysGlobal.GetCurrentOpCode());
            //if (sdrUser.Read())
            //{
            //    SqlDataReader sdrOrg = SysClass.SysOrgan.GetSingleOrganByReader(Convert.ToInt32(sdrUser["OrganID"]));
            //    if (sdrOrg.Read())
            //    {
            //        if (sdrOrg["POrganID"].ToString() != "0")
            //        {
            //            sWhereSQL += " And b.OrganID = " + Convert.ToInt32(sdrUser["OrganID"]);
            //        }
            //    }
            //    sdrOrg.Close();
            //}
            //sdrUser.Close();

            int _SalaryValue = SysClass.SysWarning.GetSalaryValueByOrganID(SysClass.SysGlobal.GetCurrentUserOrganID());

            sWhereSQL = sWhereSQL + " And DateDiff(Day, a.SalaryDate, GetDate()) >= " + _SalaryValue.ToString() + "";

            SysClass.SysUserSalary.UserSalary_UserSalaryOpCode = txtUserSalaryOpCode.Text;
            SysClass.SysUserSalary.UserSalary_UserSalaryOpName = txtUserSalaryOpName.Text;
            SysClass.SysUserSalary.UserSalary_UserSalaryUserID = txtUserSalaryUserID.Text;

            int iPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysUserSalary.GetSalaryNotSignLstByDataSet(sWhereSQL), gvLists, iPageSize, isTurningPage);
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
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            isTurningPage = false;
            BindPageData();
        }

        protected void txtUserSalaryOpCode_TextChanged(object sender, EventArgs e)
        {
            txtUserSalaryOpCode.Text = CyxPack.CommonOperation.DealwithString.GetStringPrefix(txtUserSalaryOpCode.Text);

            txtUserSalaryUserID.Text = SysClass.SysUser.GetSelfUserIDByOpCode(txtUserSalaryOpCode.Text).ToString();
            txtUserSalaryOpName.Text = SysClass.SysUser.GetSelfUserNameByOpCode(txtUserSalaryOpCode.Text);
            if ((txtUserSalaryUserID.Text == "0") || (txtUserSalaryUserID.Text.Length == 0))
            {
                txtUserSalaryOpCode.Text = "";
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

            for (int iLoop = 0; iLoop < ds.Tables[0].Rows.Count; iLoop++)
            {
                //创建DataRow对象
                DataRow dr = dt.NewRow();

                //新增数据
                dr[0] = iLoop + 1;
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