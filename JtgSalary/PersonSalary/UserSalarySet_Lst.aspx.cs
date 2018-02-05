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
    public partial class UserSalarySet_Lst : System.Web.UI.Page
    {
        public int _DeleteUserSalarySetID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["DeleteUserSalarySetID"] != null)
            {
                _DeleteUserSalarySetID = int.Parse(Request.Params["DeleteUserSalarySetID"]);
            }
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            if (_DeleteUserSalarySetID > 0)
            {
                //执行删除操作
                SysClass.SysUserSalary.DeleteSingleUserSalarySet(_DeleteUserSalarySetID.ToString());
            }

            string sWhereSQL = "";

            if (txtSearchKeyword.Text.Length > 0)
            {
                sWhereSQL += " And (BeginYears Like '%" + txtSearchKeyword.Text + "%' OR EndYears Like '%" + txtSearchKeyword.Text + "%')";
            }

            this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysUserSalary.GetUserSalarySetLstByDataSet(sWhereSQL), gvLists, 15);
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
                    string SqlText = " begin Delete from UserSalarySet_Info Where Status=0 And ID=" + id.ToString() + ";";
                    SqlText += " Delete from UserSalarySet_Fields_Info Where Status=0 And MasterID=" + id.ToString() + "; end;";
                    if (CyxPack.OperateSqlServer.DataCommon.QueryData(SqlText) > 0)
                    {
                        i++;
                    }
                }
            }
            if (i > 0)
            {
                BindPageData();
                Dialog.OpenDialogInAjax(txtSearchKeyword, "恭喜您，删除所选择的字段设置成功……");
            }
        }

        protected void gvLists_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button ibDelete = (Button)e.Row.FindControl("ibDelete");
            if (ibDelete != null)
            {
                ibDelete.Attributes.Add("onclick", "return confirm('你确定要删除所选择的记录吗?');");
            }
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserSalarySet_Edit.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindPageData();
        }
    }
}
