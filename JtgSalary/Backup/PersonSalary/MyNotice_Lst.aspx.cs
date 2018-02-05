using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;

namespace JtgTMS.Admin
{
    public partial class MyNotice_Lst : System.Web.UI.Page
    {
        public int _DeleteNoticeID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();
       
            if (Request.Params["DeleteNoticeID"] != null)
            {
                _DeleteNoticeID = int.Parse(Request.Params["DeleteNoticeID"]);
            }
            if (Request.Params["page"] != null)
            {
                SysClass.SysNotice.Notice_PageNo = int.Parse(Request.Params["page"].ToString());
            }
            if (!Page.IsPostBack)
            {
                txtSearchKeyword.Text = SysClass.SysNotice.Notice_SearchText;

                BindPageData();
            }
        }

        private void BindPageData()
        {            

            if (_DeleteNoticeID > 0)
            {
                ///执行删除操作
                SysClass.SysNotice.DeleteSingleToolsNotice(_DeleteNoticeID);
            }

            string sWhereSQL = " And a.OrganID in (select ID from [GetParentOrganByID]("+SysClass.SysGlobal.GetCurrentUserOrganID()+"))";

            if (txtSearchKeyword.Text.Length > 0)
            {
                sWhereSQL += " And (NoticeTitle Like '%" + txtSearchKeyword.Text + "%')";
            }            

            SysClass.SysNotice.Notice_SearchText = txtSearchKeyword.Text;

            this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysNotice.GetToolsNoticeLstByDataSet(sWhereSQL), gvLists, 15);
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
                    string SqlText = "Delete from Notice_Info Where Status=0 And ID=" + id.ToString();
                    if (CyxPack.OperateSqlServer.DataCommon.QueryData(SqlText) > 0)
                    {
                        i++;
                    }
                }
            }
            if (i > 0)
            {
                BindPageData();
                Dialog.OpenDialogInAjax(txtSearchKeyword, "恭喜您，通知删除成功……");
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
            Response.Redirect("Notice_Edit.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindPageData();
        }
    }
}
