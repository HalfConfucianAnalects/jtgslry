using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using CyxPack.CommonOperation;

namespace JtgTMS.Admin
{
    public partial class Notice_Lst : System.Web.UI.Page
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
                txtSearchTitle.Text = SysClass.SysNotice.Notice_SearchTitle;
                txtSearchTime.Text = SysClass.SysNotice.Notice_SearchTime;
                txtSearchOpName.Text = SysClass.SysNotice.Notice_SearchOpName;

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

            string sWhereSQL = "";

            if (txtSearchTitle.Text.Length > 0)
            {
                sWhereSQL += " And (NoticeTitle Like '%" + txtSearchTitle.Text + "%')";
            }
            if (txtSearchTime.Text.Length > 0)
            {
                DateTimeFormatInfo dtf = new DateTimeFormatInfo();
                dtf.ShortDatePattern = "yyyy/MM/dd";
                Debug.WriteLine(txtSearchTime.Text);

                DateTime searchTime = Convert.ToDateTime(txtSearchTime.Text, dtf);// new DateTime(txtSearchTime.Text);
                sWhereSQL += " And CreatedTime >  '" + txtSearchTime.Text + "'";
            }
            if (txtSearchOpName.Text.Length > 0)
            {
                sWhereSQL += " And OpName= '" + txtSearchOpName.Text + "'";
            }
            SysClass.SysNotice.Notice_SearchTitle = txtSearchTitle.Text;
            SysClass.SysNotice.Notice_SearchTime = txtSearchTime.Text;
            SysClass.SysNotice.Notice_SearchOpName = txtSearchOpName.Text;
            Debug.WriteLine(sWhereSQL);
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
                Dialog.OpenDialogInAjax(txtSearchTitle, "恭喜您，通知删除成功……");
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

        protected void txtSearchOpCode_TextChanged(object sender, EventArgs e)
        {
            txtSearchOpCode.Text = CyxPack.CommonOperation.DealwithString.GetStringPrefix(txtSearchOpCode.Text);

            txtSearchUserID.Text = SysClass.SysUser.GetSelfUserIDByOpCode(txtSearchOpCode.Text).ToString();
            txtSearchOpName.Text = SysClass.SysUser.GetSelfUserNameByOpCode(txtSearchOpCode.Text);
            if ((txtSearchUserID.Text == "0") || (txtSearchUserID.Text.Length == 0))
            {
                txtSearchOpCode.Text = "";
                Dialog.OpenDialogInAjax(txtSearchOpCode, "工号" + txtSearchOpCode.Text + "不存在！");
            }
        }
    }
}
