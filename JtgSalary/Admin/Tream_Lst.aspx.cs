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
    public partial class Tream_Lst : System.Web.UI.Page
    {
        public int _POrganID = 0, _DeleteOrganID = 0;
        public string _OrganName = "";
        int _OrganType = 3;
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["POrganID"] != null)
            {
                _POrganID = int.Parse(Request.Params["POrganID"]);
            }
            if (Request.Params["DeleteOrganID"] != null)
            {
                _DeleteOrganID = int.Parse(Request.Params["DeleteOrganID"]);
            }
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            if (_DeleteOrganID > 0)
            {
                ///执行删除操作
                SysClass.SysOrgan.DeleteSingleOrgan(_DeleteOrganID.ToString());
            }

            _OrganName = SysClass.SysOrgan.GetOrganNameByID(_POrganID);
            this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysOrgan.GetOrganLstByDataSet(_POrganID, _OrganType, txtSearchKeyword.Text), gvLists, 15);
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
            if ((_DeleteIDs.Length > 0) && (SysClass.SysOrgan.DeleteSingleOrgan(_DeleteIDs) > 0))
            {
                BindPageData();
                Dialog.OpenDialogInAjax(txtSearchKeyword, "恭喜您，" + _OrganName + "选择机构部门删除成功……");
            }

            //int i = 0;
            //foreach (GridViewRow row in this.gvLists.Rows)
            //{
            //    CheckBox CheckRow = (CheckBox)row.FindControl("CheckRow");
            //    if (CheckRow.Checked)
            //    {
            //        string id = this.gvLists.DataKeys[row.RowIndex].Values["ID"].ToString();
            //        //其它处理操作略
            //        string SqlText = "Delete from SysOrgan_Info Where Status=0 And ID=" + id.ToString();
            //        if (CyxPack.OperateSqlServer.DataCommon.QueryData(SqlText) > 0)
            //        {
            //            i++;
            //        }
            //    }
            //}
            //if (i > 0)
            //{
            //    BindPageData();
            //    Dialog.OpenDialogInAjax(txtSearchKeyword, "恭喜您，" + _OrganName + "选择机构班组删除成功……");
            //}
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
            Response.Redirect("Tream_Edit.aspx?POrganID=" + _POrganID.ToString() + "");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindPageData();
        }
    }
}
