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
    public partial class CustomField_Lst : System.Web.UI.Page
    {
        public int _DeleteCustomFieldID = 0;
        public string _TableNo = "", _TableTitle = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["DeleteCustomFieldID"] != null)
            {
                _DeleteCustomFieldID = int.Parse(Request.Params["DeleteCustomFieldID"]);
            }
            if (Request.Params["TableNo"] != null)
            {
                _TableNo = Request.Params["TableNo"];
            }
            _TableTitle = SysClass.SysCustomField.GetTableTitleByTableNo(_TableNo);
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            if (_DeleteCustomFieldID > 0)
            {
                ///执行删除操作
                SysClass.SysCustomField.DeleteCustoms(_DeleteCustomFieldID.ToString(), _TableNo);
            }

            string sWhereSQL = "";
            if (txtSearchKeyword.Text.Length > 0)
            {
                sWhereSQL += " And (FieldNo Like '%" + txtSearchKeyword.Text + "%' OR FieldName Like '%" + txtSearchKeyword.Text + "%')";
            }

            this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysCustomField.GetCustomLstByDataSet(_TableNo, sWhereSQL), gvLists, 15);
        }        

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string _DeleteIDs = "";
            foreach (GridViewRow row in this.gvLists.Rows)
            {
                CheckBox CheckRow = (CheckBox)row.FindControl("CheckRow");
                if (CheckRow.Checked)
                {
                    if (_DeleteIDs.Length > 0)
                    {
                        _DeleteIDs += ",";
                    }
                    _DeleteIDs += this.gvLists.DataKeys[row.RowIndex].Values["ID"].ToString();
                }
            }
            if ((_DeleteIDs.Length > 0) && (SysClass.SysCustomField.DeleteCustoms(_DeleteIDs, _TableNo) > 0))
            {
                BindPageData();
                //Dialog.OpenDialogInAjax(gvLists, "恭喜您，自定义字段删除成功……");
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
            Response.Redirect("CustomField_Edit.aspx?TableNo=" + _TableNo);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindPageData();
        }
    }
}
