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
    public partial class UserSalaryExport_Lst : System.Web.UI.Page
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
                txtSalaryYears.Text = DateTime.Now.ToString("yyyyMM");
                SysClass.SysUserSalary.FullToSalaryImportRecLst2(ddlImportRec, txtSalaryYears.Text);
                //BindPageData();
            }
        }

        private void BindPageData()
        {
            string sWhereSQL = " And SignStatus=" + ddlSignStatus.SelectedValue.ToString();

            if (txtUserSalaryOpCode.Text.Length > 0)
            {
                sWhereSQL = sWhereSQL + " And a.OpCode = '" + txtUserSalaryOpCode.Text + "'";
            }

            if (txtSalaryYears.Text.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears = '" + txtSalaryYears.Text + "'";
            }
            if (ddlImportRec.SelectedIndex > 0)
            {
                sWhereSQL += " And a.SalaryRecGuid='" + ddlImportRec.SelectedValue.ToString() + "'";
            }

            string sSQL = "select a.ID, a.OpCode, b.OpName, b.Sex, b.IdNumber, a.SalaryYears, a.SalaryDate,a.TotalSalary";                     

            sSQL += " from UserSalary_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OpCode=b.OpCode"
                + " Where a.Status=0 " + sWhereSQL;

            sSQL += " Order By a.SalaryYears Desc";

            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvList2, CyxPack.OperateSqlServer.DataCommon.GetDataByReader(sSQL));            

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
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string sWhereSQL = " And SignStatus=" + ddlSignStatus.SelectedValue.ToString();

            if (txtUserSalaryOpCode.Text.Length > 0)
            {
                sWhereSQL = sWhereSQL + " And a.OpCode = '" + txtUserSalaryOpCode.Text + "'";
            }

            if (txtSalaryYears.Text.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears = '" + txtSalaryYears.Text + "'";
            }
            if (ddlImportRec.SelectedIndex > 0)
            {
                sWhereSQL += " And a.SalaryRecGuid='" + ddlImportRec.SelectedValue.ToString() + "'";
            }

            string sSQL = "select a.ID, a.OpCode, b.OpName, b.Sex, b.IdNumber, a.SalaryYears, a.SalaryDate";
            SqlDataReader sdr = SysClass.SysUserSalary.GetUserSalaryFieldsLstByReader(txtSalaryYears.Text);
            //gvLists.Columns.Clear();
            while (sdr.Read())
            {
                sSQL += "," + sdr["FieldName"].ToString();

                string sPDSQL = " Select top 1 1 From UserSalary_Info a"
                  + " left join SysUser_Info b on b.Status=0 And a.OpCode=b.OpCode"
                  + " Where a.Status=0 " + sWhereSQL;

                if ((sdr["FieldName"].ToString() == "TotalSalary") || (sdr["FieldType"].ToString() != "1"))
                {
                    sPDSQL += " And IsNull(" + sdr["FieldName"].ToString() + ",0) > 0 ";
                }
                else
                {
                    sPDSQL += " And IsNull(" + sdr["FieldName"].ToString() + ",'') <> '' ";
                }

                if (SysClass.SysGlobal.GetExecSqlIsExist(sPDSQL))
                {
                    BoundField nameColumn = new BoundField();
                    nameColumn.HeaderText = sdr["UserFieldTitle"].ToString();
                    nameColumn.DataField = sdr["FieldName"].ToString();

                    gvLists.Columns.Add(nameColumn);
                }
            }
            sdr.Close();

            sSQL += " from UserSalary_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OpCode=b.OpCode"
                + " Where a.Status=0 " + sWhereSQL;

            sSQL += " Order By a.SalaryYears Desc";

            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvLists, CyxPack.OperateSqlServer.DataCommon.GetDataByReader(sSQL));

            if (gvLists.Rows.Count > 0)
            {
                //调用导出方法  
                ExportGridViewForUTF8(gvLists, DateTime.Now.ToString() + ".xls");
            }
            else
            {

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
    }
}
