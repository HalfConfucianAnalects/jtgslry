using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;
//Add by lk 20151214 start
using System.Data;
//Add by lk 20151214 end

namespace JtgTMS.Admin
{
    public partial class Dept_Lst : System.Web.UI.Page
    {
        public int _POrganID = 0, _DeleteOrganID = 0;
        public string _OrganName = "";
        int _OrganType = 2;
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
            //Upd by lk 20151214 start
            //if (_DeleteOrganID > 0)
            //{
            //    ///执行删除操作
            //    SysClass.SysOrgan.DeleteSingleOrgan(_DeleteOrganID.ToString());
            //}

            //_OrganName = SysClass.SysOrgan.GetOrganNameByID(_POrganID);
            //this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysOrgan.GetOrganLstByDataSet(_POrganID, _OrganType, txtSearchKeyword.Text), gvLists, 15);

            if (_DeleteOrganID > 0)
            {
                string strSql = " select a.OpCode,a.OpName,a.OrganID,b.OrganName";
                strSql += " from SysUser_Info as a";
                strSql += " inner join SysOrgan_Info as b on a.OrganID = b.ID";
                strSql += " where b.ID = " + _DeleteOrganID;

                DataSet ds = CyxPack.OperateSqlServer.DataCommon.GetDataByDataSet(strSql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _OrganName = SysClass.SysOrgan.GetOrganNameByID(_POrganID);
                    this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysOrgan.GetOrganLstByDataSet(_POrganID, _OrganType, txtSearchKeyword.Text), gvLists, 15);
                    Dialog.OpenDialogInAjax(btnDelete, "部门【" + ds.Tables[0].Rows[0]["OrganName"].ToString() + "】中存在用户,部门无法删除！");
                }
                else
                {
                    //执行删除操作
                    SysClass.SysOrgan.DeleteSingleOrgan(_DeleteOrganID.ToString());

                    _OrganName = SysClass.SysOrgan.GetOrganNameByID(_POrganID);
                    this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysOrgan.GetOrganLstByDataSet(_POrganID, _OrganType, txtSearchKeyword.Text), gvLists, 15);
                }
            }
            else
            {
                _OrganName = SysClass.SysOrgan.GetOrganNameByID(_POrganID);
                this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysOrgan.GetOrganLstByDataSet(_POrganID, _OrganType, txtSearchKeyword.Text), gvLists, 15);
            }
            //Upd by lk 20151214 end
        }        

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string _DeleteIDs = "";

            //Add by lk 20151214 start
            bool bolDelOK = true;
            string orgId = "", orgName = "", strSql = "", errorMsg = "", strUsers;

            foreach (GridViewRow row in this.gvLists.Rows)
            {
                CheckBox CheckRow = (CheckBox)row.FindControl("CheckRow");
                if (CheckRow.Checked)
                {
                    orgId = this.gvLists.DataKeys[row.RowIndex].Values["ID"].ToString();

                    strSql = " ";
                    strSql += " select a.OpCode,a.OpName,a.OrganID,b.OrganName";
                    strSql += " from SysUser_Info as a";
                    strSql += " inner join SysOrgan_Info as b on a.OrganID = b.ID";
                    strSql += " where b.ID = " + orgId;

                    DataSet ds = CyxPack.OperateSqlServer.DataCommon.GetDataByDataSet(strSql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //strUsers = "";
                        //foreach (DataRow dr in ds.Tables[0].Rows)
                        //{
                        //    if (orgName != dr["RoleName"].ToString())
                        //    {
                        //        orgName = dr["RoleName"].ToString();
                        //        errorMsg += "\\n从属与部门【" + orgName + "】的用户如下：\\n";
                        //    }
                        //    strUsers += "[" + dr["OpCode"].ToString() + "-";
                        //    strUsers += dr["OpName"].ToString() + " ] ";
                        //}

                        //errorMsg += strUsers;

                        orgName += " 【"+ ds.Tables[0].Rows[0]["OrganName"].ToString()+"】 ";

                        if (bolDelOK == true)
                        {
                            bolDelOK = false;
                        }
                    }
                }
            }

            if (bolDelOK)
            {
                //Add by lk 20151214 end

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

                //Add by lk 20151214 start
            }
            else
            {
                Dialog.OpenDialogInAjax(btnDelete, "部门" + orgName + "中存在用户，删除处理取消!");
                BindPageData();
            }
            //Add by lk 20151214 end

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
            //    Dialog.OpenDialogInAjax(txtSearchKeyword, "恭喜您，" + _OrganName + "选择机构部门删除成功……");
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
            Response.Redirect("Dept_Edit.aspx?POrganID=" + _POrganID.ToString() + "");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindPageData();
        }
    }
}
