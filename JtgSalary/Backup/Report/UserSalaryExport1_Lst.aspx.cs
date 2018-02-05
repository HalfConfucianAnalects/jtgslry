using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;
using System.Data;
//Add by lk 20151214 start
using System.Configuration;
//Add by lk 20151214 end

namespace JtgTMS.PersonSalary
{
    public partial class UserSalaryExport1_Lst : System.Web.UI.Page
    {
        public int _DeleteUserSalaryID = 0, _OrganID = 0;
        private double mysum = 0, mysum1 = 0, mysum2 = 0;
        private double _mysum = 0, _mysum1 = 0, _mysum2 = 0;

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
                //SysClass.SysUserSalary.FullToSalaryImportRecLst2(ddlImportRec, txtSalaryYears.Text);
                //BindPageData();

                txtSalaryYears.Text = DateTime.Now.ToString("yyyyMM");
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

                    ddlSignStatus.SelectedValue = SysClass.SysUserSalary.UserSalary_SignStatus;

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

                    BindPageData();
                }

                //Add by lk 20151214 end
            }
        }

        private void BindPageData()
        {
            string sWhereSQL = " ";

            if (ddlSignStatus.SelectedIndex > 0)
            {
                sWhereSQL += " And SignStatus=" + ddlSignStatus.SelectedValue.ToString();
            }

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
                    sWhereSQL += " And d.Description = ''";
                    break;
                //仅【工资】被选中
                case 1:
                    sWhereSQL += " And d.Description like '%" + chkGongzi.Text + "%'";
                    break;
                //仅【奖金】被选中
                case 2:
                    sWhereSQL += " And d.Description like '%" + chkJiangjin.Text + "%'";
                    break;
                //【工资】和【奖金】被选中
                case 3:
                    sWhereSQL += " And (d.Description like '%" + chkGongzi.Text + "%'";
                    sWhereSQL += " or d.Description like '%" + chkJiangjin.Text + "%')";
                    break;
                //仅【报销】被选中
                case 4:
                    sWhereSQL += " And d.Description like '%" + chkBaoxiao.Text + "%'";
                    break;
                //【工资】和【报销】被选中
                case 5:
                    sWhereSQL += " And (d.Description like '%" + chkGongzi.Text + "%'";
                    sWhereSQL += " or d.Description like '%" + chkBaoxiao.Text + "%')";
                    break;
                //【奖金】和【报销】被选中
                case 6:
                    sWhereSQL += " And (d.Description like '%" + chkJiangjin.Text + "%'";
                    sWhereSQL += " or d.Description like '%" + chkBaoxiao.Text + "%')";
                    break;
                //【工资】【奖金】【报销】都被选中
                case 7:
                    break;
            }
            //Add by lk 20160118 end

            string sSQL = "select a.ID, a.OpCode, b.OpName, b.Sex, b.IdNumber, c.OrganName, a.SalaryYears, a.SalaryDate, a.TotalSalary"
                + ", (Case IsNull(SignStatus,0) when 0 then a.TotalSalary else 0 end) As UnSignTotalSalary"
                + ", (Case IsNull(SignStatus,0) when 1 then a.TotalSalary else 0 end) As SignTotalSalary";

            sSQL += " from UserSalary_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OpCode=b.OpCode"
                + " left join SysOrgan_Info c on c.Status=0 And b.OrganID=c.ID"
                //Add by lk 20160118 start
                + " left join UserUserImportRec_Info d on d.SalaryRecGuid=a.SalaryRecGuid"
                //Add by lk 20160118 end
                + " Where a.Status=0 " + sWhereSQL;

            //Upd by lk 20151214 start
            //sSQL += " Order By a.SalaryYears Desc";
            sSQL += " Order By a.SalaryYears Desc,b.OpName";
            
            //CyxPack.CommonOperation.DataBinder.BindGridViewData(gvLists, CyxPack.OperateSqlServer.DataCommon.GetDataByDataSet(sSQL));
            //CyxPack.CommonOperation.DataBinder.BindGridViewData(gvLists2, CyxPack.OperateSqlServer.DataCommon.GetDataByDataSet(sSQL));    

            SysClass.SysUserSalary.UserSalary_UserSalaryOpCode = txtUserSalaryOpCode.Text;
            SysClass.SysUserSalary.UserSalary_UserSalaryOpName = txtUserSalaryOpName.Text;
            SysClass.SysUserSalary.UserSalary_UserSalaryUserID = txtUserSalaryUserID.Text;

            SysClass.SysUserSalary.UserSalary_SearchText = txtSalaryYears.Text;
            SysClass.SysUserSalary.UserSalary_SearchText2 = txtSalaryYears2.Text;

            SysClass.SysUserSalary.UserSalary_ImportRecIndex = ddlImportRec.SelectedIndex;
            SysClass.SysUserSalary.UserSalary_ImportRecValue = ddlImportRec.SelectedValue.ToString();
            SysClass.SysUserSalary.UserSalary_ImportRecText = ddlImportRec.SelectedItem.Text.ToString();

            SysClass.SysUserSalary.UserSalary_ImportRecIndex2 = ddlImportRec2.SelectedIndex;
            SysClass.SysUserSalary.UserSalary_ImportRecValue2 = ddlImportRec2.SelectedValue.ToString();
            SysClass.SysUserSalary.UserSalary_ImportRecText2 = ddlImportRec2.SelectedItem.Text.ToString();

            SysClass.SysUserSalary.UserSalary_SignStatus = ddlSignStatus.SelectedValue.ToString();

            int iPageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);

            mysum = 0;
            mysum1 = 0;
            mysum2 = 0;
            DataSet ds = CyxPack.OperateSqlServer.DataCommon.GetDataByDataSet(sSQL);
            //计算全结果的合计
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                mysum += double.Parse(dr["TotalSalary"].ToString());
                mysum1 += double.Parse(dr["UnSignTotalSalary"].ToString());
                mysum2 += double.Parse(dr["SignTotalSalary"].ToString());
            }

            this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(ds, gvLists, iPageSize, isTurningPage);
            //Upd by lk 20151214 end        
        }        

        private bool SaveCheck()
        {
            bool bFlag = true;

            return bFlag;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
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
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //string sWhereSQL = " And SignStatus=" + ddlSignStatus.SelectedValue.ToString();

            //if (ddlSignStatus.SelectedIndex > 0)
            //{
            //    sWhereSQL += " And SignStatus=" + ddlSignStatus.SelectedValue.ToString();
            //}

            //if (txtUserSalaryOpCode.Text.Length > 0)
            //{
            //    sWhereSQL = sWhereSQL + " And a.OpCode = '" + txtUserSalaryOpCode.Text + "'";
            //}

            //if (txtSalaryYears.Text.Length > 0)
            //{
            //    sWhereSQL += " And a.SalaryYears = '" + txtSalaryYears.Text + "'";
            //}
            //if (ddlImportRec.SelectedIndex > 0)
            //{
            //    sWhereSQL += " And a.SalaryRecGuid='" + ddlImportRec.SelectedValue.ToString() + "'";
            //}

            //string sSQL = "select a.ID, a.OpCode, b.OpName, b.Sex, b.IdNumber, c.OrganName, a.SalaryYears, a.SalaryDate, a.TotalSalary"
            //    + ", (Case IsNull(SignStatus,0) when 0 then a.TotalSalary else 0 end) As UnSignTotalSalary"
            //    + ", (Case IsNull(SignStatus,0) when 1 then a.TotalSalary else 0 end) As SignTotalSalary";

            //sSQL += " from UserSalary_Info a "
            //    + " left join SysUser_Info b on b.Status=0 And a.OpCode=b.OpCode"
            //    + " left join SysOrgan_Info c on c.Status=0 And b.OrganID=c.ID"
            //    + " Where a.Status=0 " + sWhereSQL;

            //sSQL += " Order By a.SalaryYears Desc";

            //CyxPack.CommonOperation.DataBinder.BindGridViewData(gvLists2, CyxPack.OperateSqlServer.DataCommon.GetDataByDataSet(sSQL));

            //Upd by lk 20151214 start
            //if (gvLists2.Rows.Count > 0)
            //{
            //    //调用导出方法  
            //    ExportGridViewForUTF8(gvLists2, DateTime.Now.ToString() + ".xls");
            //}
            //else
            //{

            //}

            if (gvLists.Rows.Count > 0)
            {
                //数据导出到Excel
                OutputDataToExcle(DateTime.Now.ToString() + ".xls");
            }
            else
            {
                Dialog.OpenDialogInAjax(btnExport, "不存在可导出的查询结果！");
            }
            //Upd by lk 20151214 start
        }

        protected void gvLists_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Del by lk 20151214 Start
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    DataRowView myrows = (DataRowView)e.Row.DataItem;

            //    if (myrows["TotalSalary"].ToString().Length > 0)
            //        mysum += double.Parse(myrows["TotalSalary"].ToString());
            //    if (myrows["UnSignTotalSalary"].ToString().Length > 0)
            //        mysum1 += double.Parse(myrows["UnSignTotalSalary"].ToString());
            //    if (myrows["SignTotalSalary"].ToString().Length > 0)
            //        mysum2 += double.Parse(myrows["SignTotalSalary"].ToString());
            //}
            //Del by lk 20151214 End

            // 合计
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //Add by lk 20151214 start
                e.Row.Cells[5].Text = "总合计：";
                //Add by lk 20151214 end
                e.Row.Cells[6].Text = mysum.ToString("0.00");
                e.Row.Cells[7].Text = mysum1.ToString("0.00");
                e.Row.Cells[8].Text = mysum2.ToString("0.00");
            }
        }

        protected void gvLists2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView myrows = (DataRowView)e.Row.DataItem;

                if (myrows["TotalSalary"].ToString().Length > 0)
                    _mysum += double.Parse(myrows["TotalSalary"].ToString());
                if (myrows["UnSignTotalSalary"].ToString().Length > 0)
                    _mysum1 += double.Parse(myrows["UnSignTotalSalary"].ToString());
                if (myrows["SignTotalSalary"].ToString().Length > 0)
                    _mysum2 += double.Parse(myrows["SignTotalSalary"].ToString());
            }
            // 合计
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[6].Text = _mysum.ToString("0.00");
                e.Row.Cells[7].Text = _mysum1.ToString("0.00");
                e.Row.Cells[8].Text = _mysum2.ToString("0.00");
            }
        }

        protected void btnChoiceUser_Click(object sender, EventArgs e)
        {
            txtUserSalaryUserID.Text = SysClass.SysParams.UserInfo_ChoiceValues;
            txtUserSalaryOpCode.Text = SysClass.SysUser.GetOpCodeByUserID(int.Parse(SysClass.SysParams.UserInfo_ChoiceValues));
            txtUserSalaryOpName.Text = SysClass.SysUser.GetUserNameByUserID(int.Parse(SysClass.SysParams.UserInfo_ChoiceValues));

            SysClass.SysParams.UserInfo_ChoiceValues = "";
        }

        protected void btnSalarys_Click(object sender, EventArgs e)
        {
            SysClass.SysUserSalary.FullToSalaryImportRecLst2(ddlImportRec, txtSalaryYears.Text);
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
        protected void btnSalarys2_Click(object sender, EventArgs e)
        {
            SysClass.SysUserSalary.FullToSalaryImportRecLst2(ddlImportRec2, txtSalaryYears2.Text);
        }

        /// <summary>
        /// 数据导出到Excel
        /// </summary>
        /// <returns></returns>
        private void OutputDataToExcle(string strFilename)
        {
            string sWhereSQL = " ";

            if (SysClass.SysUserSalary.UserSalary_SignStatus != "-1")
            {
                sWhereSQL += " And SignStatus=" + SysClass.SysUserSalary.UserSalary_SignStatus;
            }

            if (SysClass.SysUserSalary.UserSalary_UserSalaryOpCode.Length > 0)
            {
                sWhereSQL += " And a.OpCode = '" + SysClass.SysUserSalary.UserSalary_UserSalaryOpCode + "'";
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

            //Add by lk 20160118 start
            switch (SysClass.SysUserSalary.UserSalary_FlgValue)
            {
                //没有选中任何一项
                case 0:
                    sWhereSQL += " And d.Description = ''";
                    break;
                //仅【工资】被选中
                case 1:
                    sWhereSQL += " And d.Description like '%" + chkGongzi.Text + "%'";
                    break;
                //仅【奖金】被选中
                case 2:
                    sWhereSQL += " And d.Description like '%" + chkJiangjin.Text + "%'";
                    break;
                //【工资】和【奖金】被选中
                case 3:
                    sWhereSQL += " And (d.Description like '%" + chkGongzi.Text + "%'";
                    sWhereSQL += " or d.Description like '%" + chkJiangjin.Text + "%')";
                    break;
                //仅【报销】被选中
                case 4:
                    sWhereSQL += " And d.Description like '%" + chkBaoxiao.Text + "%'";
                    break;
                //【工资】和【报销】被选中
                case 5:
                    sWhereSQL += " And (d.Description like '%" + chkGongzi.Text + "%'";
                    sWhereSQL += " or d.Description like '%" + chkBaoxiao.Text + "%')";
                    break;
                //【奖金】和【报销】被选中
                case 6:
                    sWhereSQL += " And (d.Description like '%" + chkJiangjin.Text + "%'";
                    sWhereSQL += " or d.Description like '%" + chkBaoxiao.Text + "%')";
                    break;
                //【工资】【奖金】【报销】都被选中
                case 7:
                    break;
            }
            //Add by lk 20160118 end

            string sSQL = "select a.ID, a.OpCode, b.OpName, b.Sex, b.IdNumber, c.OrganName, a.SalaryYears, a.SalaryDate, a.TotalSalary,d.Description"
    + ", (Case IsNull(SignStatus,0) when 0 then a.TotalSalary else 0 end) As UnSignTotalSalary"
    + ", (Case IsNull(SignStatus,0) when 1 then a.TotalSalary else 0 end) As SignTotalSalary";

            sSQL += " from UserSalary_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OpCode=b.OpCode"
                + " left join SysOrgan_Info c on c.Status=0 And b.OrganID=c.ID"
                //Add by lk 20160118 start
                + " left join UserUserImportRec_Info d on d.SalaryRecGuid=a.SalaryRecGuid"
                //Add by lk 20160118 end
                + " Where a.Status=0 " + sWhereSQL;

            sSQL += " Order By a.SalaryYears Desc,b.OpName";

            DataSet ds = CyxPack.OperateSqlServer.DataCommon.GetDataByDataSet(sSQL);

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
            dt.Columns.Add("未签收金额", typeof(decimal));
            dt.Columns.Add("已签收金额", typeof(decimal));
            dt.Columns.Add("备注", typeof(string));

            //清空合计结果
            mysum = 0;
            mysum1 = 0;
            mysum2 = 0;

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
                dr[7] = ds.Tables[0].Rows[iLoop]["UnSignTotalSalary"].ToString();
                dr[8] = ds.Tables[0].Rows[iLoop]["SignTotalSalary"].ToString();
                dr[9] = ds.Tables[0].Rows[iLoop]["Description"].ToString();

                //把数据添加到表结构中
                dt.Rows.Add(dr);

                //合计结果累加
                mysum += double.Parse(ds.Tables[0].Rows[iLoop]["TotalSalary"].ToString());
                mysum1 += double.Parse(ds.Tables[0].Rows[iLoop]["UnSignTotalSalary"].ToString());
                mysum2 += double.Parse(ds.Tables[0].Rows[iLoop]["SignTotalSalary"].ToString());
            }

            //添加合计部分数据
            DataRow foot = dt.NewRow();
            foot[5] = "总合计：";
            foot[6] = mysum.ToString("0.00");
            foot[7] = mysum1.ToString("0.00");
            foot[8] = mysum2.ToString("0.00");
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

        //Add by lk 20151214 end
    }
}
