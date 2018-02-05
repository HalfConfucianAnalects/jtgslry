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
    public partial class Role_Lst : System.Web.UI.Page
    {
        public int _DeleteRoleID = 0;
        //Add by lk 20151214 start
        public string strSql = " ", strUsers = "";
        //Add by lk 20151214 end

        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["DeleteRoleID"] != null)
            {
                _DeleteRoleID = int.Parse(Request.Params["DeleteRoleID"]);
            }
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            //Upd by lk 20151214 start
            //if (_DeleteRoleID > 0)
            //{
            //    ///执行删除操作
            //    SysClass.SysRole.DeleteSingleRole(_DeleteRoleID);
            //}

            //this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysRole.GetRoleLstByDataSet(txtSearchKeyword.Text), gvLists, 15);

            if (_DeleteRoleID > 0)
            {
                ///执行删除操作
                //SysClass.SysRole.DeleteSingleRole(_DeleteRoleID);

                strSql = " ";
                strSql += "select a.UserID,a.RoleID,b.OpCode,b.OpName,c.RoleName";
                strSql += " from SysUserRoles_Info as a";
                strSql += " inner join SysUser_Info as b on a.UserID=b.id";
                strSql += " inner join SysRole_Info as c on a.RoleID = c.id";
                //strSql += " where a.SystemID = 2";
                strSql += " where a.RoleID = " + _DeleteRoleID;

                DataSet ds = CyxPack.OperateSqlServer.DataCommon.GetDataByDataSet(strSql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strUsers = "";
                    string strRoleName = "";
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (strRoleName == "")
                        {
                            strRoleName = dr["RoleName"].ToString();
                        }
                        strUsers += "[" + dr["OpCode"].ToString() + "-";
                        strUsers += dr["OpName"].ToString() + " ] \\n";
                    }
                    this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysRole.GetRoleLstByDataSet(txtSearchKeyword.Text), gvLists, 15);
                    Dialog.OpenDialogInAjax(btnDelete, "角色【" + strRoleName + "】无法删除！存在从属与该角色的用户:\\n" + strUsers);
                }
                else
                {
                    //执行删除操作
                    SysClass.SysRole.DeleteSingleRole(_DeleteRoleID);

                    this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysRole.GetRoleLstByDataSet(txtSearchKeyword.Text), gvLists, 15);
                }
            }
            else
            {
                this.PageInfo.InnerHtml = SysClass.SysPageNums.GetPageRawUrlNum(SysClass.SysRole.GetRoleLstByDataSet(txtSearchKeyword.Text), gvLists, 15);
            }
            //Upd by lk 20151214 end
        }        

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int i = 0;

            //Add by lk 20151214 start
            bool bolDelOK = true;
            string roleId = "",roleName="",errorMsg="";

            foreach (GridViewRow row in this.gvLists.Rows)
            {
                CheckBox CheckRow = (CheckBox)row.FindControl("CheckRow");
                if (CheckRow.Checked)
                {
                    roleId = this.gvLists.DataKeys[row.RowIndex].Values["ID"].ToString();

                    strSql = " ";
                    strSql += "select a.UserID,a.RoleID,b.OpCode,b.OpName,c.RoleName";
                    strSql += " from SysUserRoles_Info as a";
                    strSql += " inner join SysUser_Info as b on a.UserID=b.id";
                    strSql += " inner join SysRole_Info as c on a.RoleID = c.id";
                    //strSql += " where a.SystemID = 2";
                    strSql += " where a.RoleID = " + roleId;

                    DataSet ds = CyxPack.OperateSqlServer.DataCommon.GetDataByDataSet(strSql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strUsers = "";
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (roleName != dr["RoleName"].ToString())
                            {
                                roleName = dr["RoleName"].ToString();
                                errorMsg += "\\n从属与角色【" + roleName + "】的用户如下：\\n";
                            }
                            strUsers += "[" + dr["OpCode"].ToString() + "-";
                            strUsers += dr["OpName"].ToString() + " ] ";
                        }

                        errorMsg += strUsers;

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
                        string id = this.gvLists.DataKeys[row.RowIndex].Values["ID"].ToString();
                        //其它处理操作略
                        string SqlText = "Delete from SysRole_Info Where Status=0 And ID=" + id.ToString();
                        if (CyxPack.OperateSqlServer.DataCommon.QueryData(SqlText) > 0)
                        {
                            i++;
                        }
                    }
                }
                if (i > 0)
                {
                    BindPageData();
                    Dialog.OpenDialogInAjax(txtSearchKeyword, "恭喜您，删除所选择的角色成功……");
                }

                //Add by lk 20151214 start
            }
            else
            {
                Dialog.OpenDialogInAjax(btnDelete, "存在从属与角色的用户，删除处理取消!" + errorMsg);
                BindPageData();
            }
            //Add by lk 20151214 end
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
            Response.Redirect("Role_Edit.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindPageData();
        }
    }
}
