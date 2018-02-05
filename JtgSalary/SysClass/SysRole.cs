using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CyxPack.CommonOperation;
using CyxPack.OperateSqlServer;
using System.Data.SqlClient;
using UNLV.IAP.WebControls;

namespace JtgTMS.SysClass
{
    public class SysRole
    {        

        public static DataSet GetRoleLstByDataSet(string SearchText)
        {
            string sSQL = "Select * from SysRole_Info Where Status=0 And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString();

            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And RoleName Like '%" + SearchText + "%'";
            }

            sSQL = sSQL + " Order By SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetSingleRoleByReader(int ID)
        {
            string sSQL = "select * from SysRole_Info where Status=0 "//and IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " and ID=" + ID.ToString();
            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetUserRolePurviewByReader(int UserID)
        {
            string sSQL = "select * from SysRole_Info where Status=0 and IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " and ID In (Select RoleID From SysUserRoles_Info Where Status=0 "// IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And UserID=" + UserID.ToString() + ")";
            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetRoleLstByReader()
        {
            string sSQL = "select * from SysRole_Info where Status=0 ";//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString();
            return DataCommon.GetDataByReader(sSQL);
        }

        public static int DeleteSingleRole(int _ID)
        {
            string sSQL = "begin delete from SysRole_Info where Status =0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " and ID=" + _ID.ToString() + "; ";
            string sLogText = "删除 系统管理>角色管理：角色列表:" + _ID.ToString() + "的记录。";
            sSQL = sSQL + SysLogs.GetOperatorLogSQL(sLogText) + " End;";

            return DataCommon.QueryData(sSQL);
        }

        public static int DeleteUserByRole(int _UserID, int _RoleID)
        {
            string sSQL = "Update User_Info Set UserRoles=Replace(','+UserRoles+',','," + _RoleID.ToString() + ",','') where IsNull(SystemID,0)=" 
                + SysParams.GetPurviewSystemID().ToString() + " Status =0 and ID=" + _UserID.ToString() + ";";
            sSQL = sSQL + "";
            return DataCommon.QueryData(sSQL);
        }

        //判断预案编号已被使用
        public static bool CheckRoleIsUse(int RoleID)
        {
            string sSqlText = "Select top 1 1 From User_Info Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And ','+UserRoles+',' Like '%," + RoleID.ToString() + ",%'";

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        public static int UpdateSingleRole(int ID, string[] FieldValues)
        {
            string sSqlText = "";
            if (ID > 0)
            {
                sSqlText = "begin update SysRole_Info set RoleName='" + FieldValues.GetValue(0)
                      + "',Description='" + FieldValues[1] + ""
                      + "',Purview='" + FieldValues[2] + "'"
                      + " WHERE ID =" + ID + ";";
                string sLogText = "更新 系统管理>角色管理:角色列表：" + FieldValues.GetValue(0) + "记录。";

                sSqlText += SysLogs.GetOperatorLogSQL(sLogText) + " end;";
            }
            else
            {
                sSqlText = "begin Insert Into SysRole_Info(SystemID, RoleName,Description, Purview,Status) Values(" + SysParams.GetPurviewSystemID().ToString() + ",'"
                    + FieldValues.GetValue(0) + "','" + FieldValues.GetValue(1) + "','" + FieldValues.GetValue(2) + "',0 )" + ";";
                string sLogText = "新增 系统管理>角色管理:角色列表：" + FieldValues.GetValue(0) + "记录。";
                sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            }
            return DataCommon.QueryData(sSqlText);
        }

        //获取车站列表至控件
        public static void FullToUserRoleLst(DropDownList ddlList, bool HasAll)
        {
            ddlList.Items.Clear();

            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "";
                liItem.Text = "请选择";
                ddlList.Items.Add(liItem);
            }

            string sSQL = "Select * from SysRole_Info Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["ID"].ToString();
                liItem.Text = sdr["RoleName"].ToString();
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }
        //获取角色
        public static SqlDataReader GetSingleRoleNameByReader(int _ID)
        {
            string sSQL = "Select * from SysRole_Info Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " And ID=" + _ID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取角色是否重复
        public static bool CheckRoleNameExists(int ID, string RoleName)
        {
            string sSqlText = "select 1 from SysRole_Info where "//IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " RoleName ='" + RoleName.ToString() + "' and ID<>" + ID.ToString();
            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //获取授权角色
        public static string GetRoleNameByID(int ID)
        {
            string _RoleName = "";
            SqlDataReader sdr = GetSingleRoleNameByReader(ID);
            if (sdr.Read())
            {
                _RoleName = sdr["RoleName"].ToString();
            }
            sdr.Close();
            return _RoleName;
        }

        public static DataSet GetRoleUserByDataSet(int RoleID, string SearchText)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM User_Info a "
                + " left join Organ_Info b on a.OrganID=b.ID and b.Status=0 "//And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " WHERE a.Status=0";
            if (RoleID > 0)
            {
                sSQL = sSQL + " And a.ID in (Select UserID From SysRole_Info where Status=0 "//And IsNull(SystemID,0)=" + SysClass.SysParams.GetPurviewSystemID().ToString() 
                    + " And RoleID=" + RoleID.ToString() + ")";
            }
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpCode Like '%" + SearchText + "%' Or a.OpName Like '%" + SearchText + "%')";
            }
            sSQL = sSQL + " And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " Order By a.SortID";
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetRoleNotUserByDataSet(int RoleID, int OrganID, string SearchText)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM User_Info a "
                + " left join Organ_Info b on a.OrganID=b.ID and b.Status=0 "//And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " WHERE a.Status=0";
            if (RoleID > 0)
            {
                sSQL = sSQL + " And a.ID not in (Select UserID From SysRole_Info where Status=0 And IsNull(SystemID,0)=" 
                    + SysClass.SysParams.GetPurviewSystemID().ToString() + " And RoleID=" + RoleID.ToString() + ")";
            }
            if (OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID=" + OrganID.ToString();
            }
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpCode Like '%" + SearchText + "%' Or a.OpName Like '%" + SearchText + "%')";
            }
            sSQL = sSQL + " And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " Order By a.SortID";
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static int DeleteRoleUser(int _ID)//===
        {
            string sSQL = "begin Delete from SysRole_Info Where Status =0 And ID=" + _ID.ToString() + "; ";
            string sLogText = "删除 系统管理>角色下属人员:" + _ID.ToString() + "的记录。";
            sSQL = sSQL + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            return DataCommon.QueryData(sSQL);
        }

        //获取车站列表至控件
        public static void FullToRoleLst(DropDownCheckList ddlList)
        {
            ddlList.Items.Clear();

            string sSQL = "Select * from SysRole_Info Where Status=0 And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["ID"].ToString();
                liItem.Text = sdr["RoleName"].ToString();
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }
    }
}
