using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;

namespace JtgTMS.BasicData
{
    public partial class BasicDictionaries_Lst : System.Web.UI.Page
    {
        public int _MainID = 0, _DeleteMianID = 0;
        public string _CategoryName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["MainID"] != null)
            {
                _MainID = int.Parse(Request.Params["MainID"]);
            }
            if (Request.Params["DeleteToolID"] != null)
            {
                _DeleteMianID = int.Parse(Request.Params["DeleteToolID"]);
            }
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }
        private void BindPageData()
        {
            if (_DeleteMianID > 0)
            {
                ///执行删除操作
                SysClass.SysBasicDictionaries.DeleteMainDictionaries(_DeleteMianID);
            }

            _CategoryName = SysClass.SysBasicDictionaries.GetCategoryNameByID(_MainID);
            this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysBasicDictionaries.GetMainstByDataSet(_MainID, txtSearchKeyword.Text), gvLists, 15);
        }
        protected void gvLists_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button ibDelete = (Button)e.Row.FindControl("ibDelete");
            if (ibDelete != null)
            {
                ibDelete.Attributes.Add("onclick", "return confirm('你确定要删除所选择的记录吗?');");
            }
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
                    string SqlText = "Delete from SysBaseDetail_Info Where Status=0 And ID=" + id.ToString();
                    if (CyxPack.OperateSqlServer.DataCommon.QueryData(SqlText) > 0)
                    {
                        i++;
                    }
                }
            }
            if (i > 0)
            {
                BindPageData();
                Dialog.OpenDialogInAjax(txtSearchKeyword, "恭喜您，" + _CategoryName + "选择基础字典删除成功……");
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("BasicDictionaries_Edit.aspx?MainID=" + _MainID.ToString() + "");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindPageData();
        }
    }
}
