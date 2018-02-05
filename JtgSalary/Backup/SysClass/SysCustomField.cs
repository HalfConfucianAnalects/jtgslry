using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CyxPack.OperateSqlServer;
using System.Data.SqlClient;
using CyxPack.UserCommonOperation;
using System.Net;
using System.Data;

namespace JtgTMS.SysClass
{
    public class SysCustomField
    {
        public static string SelCustom_SearchName = "", SelCustom_SearchText = "";
        public const string UserInfo_TableNo = "UserInfo";

        public static SqlDataReader QueryOpNameLst(string SearchKey, int count)
        {
            int iRecCount = 20;
            if (count > 0)
            {
                iRecCount = count;
            }
            string sSQL = "Select distinct Top " + iRecCount + " OpCode, OpName from SysCustomField_Info"
                + " Where (OpCode Like '%" + SearchKey + "%' OR OpCode Like '%" + SearchKey + "%')"
                + " and Status=0";
            return DataCommon.GetDataByReader(sSQL);
        }        

        public static string GetRecGuidByCustomID(int CustomID)
        {
            string _RecGuid = "";
            SqlDataReader sdr = GetCustomInfoByReader(CustomID);
            if (sdr.Read())
            {
                _RecGuid = sdr["RecGuid"].ToString();
            }
            sdr.Close();
            return _RecGuid;
        }

        public static string GetTableExtNameByTableNo(string TableNo)
        {
            string TableExtName = "";
            SqlDataReader sdr = GetSingleTableByReader(TableNo);
            if (sdr.Read())
            {
                TableExtName = sdr["TableExtName"].ToString();
            }
            sdr.Close();
            return TableExtName;
        }

        public static string GetTableTitleByTableNo(string TableNo)
        {
            string TableExtName = "";
            SqlDataReader sdr = GetSingleTableByReader(TableNo);
            if (sdr.Read())
            {
                TableExtName = sdr["TableTitle"].ToString();
            }
            sdr.Close();
            return TableExtName;
        }

        //获取用户信息
        public static SqlDataReader GetSingleTableByReader(string TableNo)
        {
            string sSQL = "Select a.* FROM SysCustomTable_Info a "
                + " Where a.Status=0  And a.TableNo='" + TableNo.ToString() + "'";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取自定义表名
        public static SqlDataReader GetCustomInfoByReader()
        {
            string sSQL = "Select a.* FROM SysCustomTable_Info a "
                + " Where a.Status=0"
                + " Order By SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取用户信息
        public static SqlDataReader GetCustomInfoByReader(int CustomID)
        {
            string sSQL = "Select a.* FROM SysCustomField_Info a "
                + " Where a.Status=0  And a.ID=" + CustomID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取用户信息
        public static SqlDataReader GetCustomInfoByReader(string OpCode)
        {
            string sSQL = "Select a.*, b.OrganName FROM SysCustomField_Info a "
                + " Left Join SysOrgan_Info b on b.Status=0 And a.OrganID = b.ID"
                + " Where a.Status=0 "
                + " And a.OpCode='" + OpCode + "'";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取用户列表
        public static DataSet GetCustomLstByDataSet(string TableNo, string WhereSQL)
        {
            string sSQL = "select a.* FROM v_SysCustomField_Info a"
                + " where a.Status=0 and a.TableNo='" + TableNo.ToString() + "'";

            sSQL += WhereSQL + " Order By a.SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //获取用户列表
        public static SqlDataReader GetCustomFieldLstByReader(string WhereSQL)
        {
            string sSQL = "select a.* FROM v_SysCustomField_Info a"
                + " where a.Status=0 ";

            sSQL += WhereSQL + " Order By a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取用户列表
        public static DataSet GetCustomFieldLstByDataset(string WhereSQL)
        {
            string sSQL = "select a.* FROM v_SysCustomField_Info a"
                + " where a.Status=0 ";

            sSQL += WhereSQL + " Order By a.SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //获取用户列表
        public static SqlDataReader GetCustomLstByReader(string TableNo, string WhereSQL)
        {
            string sSQL = "select a.* FROM v_SysCustomField_Info a"
                + " where a.Status=0 and a.TableNo='" + TableNo.ToString() + "'";

            sSQL += WhereSQL + " Order By a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取用户列表
        public static DataSet GetCustomLstByDataSet(string WhereSQL)
        {
            string sSQL = "select a.* FROM SysCustomField_Info a"
                + " where a.Status=0 ";

            sSQL += WhereSQL + " Order By a.SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //获取用户列表
        public static DataSet GetCanLoginCustomLstByDataSet(int OrganID, string SearchText)
        {
            string sSQL = "select a.*,b.RoleName,b.Purview,(Case IsNull(IsCanLogin,0) when 0 then '锁定' else '正常' end) as StatusName FROM SysCustomField_Info a"
                + " left join SysRole_Info b on a.RoleID=b.ID and b.status=0 "
                + " where a.Status=0 And a.OrganID=" + OrganID.ToString() + "";

            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpName Like '%" + SearchText + "%' OR a.OpCode Like '%" + SearchText + "%')";
            }

            sSQL = sSQL + " And IsNull(IsCanLogin,0)=1 Order By a.SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //删除用户信息
        public static int DeleteCustoms(string _IDs, string _TableNo)
        {
            string sSqlText = "";

            string _TableExtName = GetTableExtNameByTableNo(_TableNo);

            SqlDataReader sdr = GetCustomFieldLstByReader(" And ID in (" + _IDs.ToString() + ")");
            while (sdr.Read())
            {
                SqlDataReader sdr1 = DataCommon.GetDataByReader("select  b.name from sysobjects b join syscolumns a on b.id = a.cdefault "
                    + " where a.id = object_id('" + _TableExtName + "') and a.name ='Column_" + sdr["FieldName"].ToString() + "'");
                while (sdr1.Read())
                {
                    sSqlText += "alter   table  " + _TableExtName + "   drop   constraint " + sdr1["name"].ToString() + ";";
                }
                sdr1.Close();

                sSqlText += "IF EXISTS (select 1 From syscolumns Where"
                    + " id=object_id('" + _TableExtName + "') And name='Column_" + sdr["FieldName"].ToString() + "')  ";
                sSqlText += " begin ";

                sSqlText += " ALTER TABLE " + _TableExtName + " DROP COLUMN Column_" + sdr["FieldName"].ToString() + ";";

                sSqlText += " end; ";
            }
            sdr.Close();

            sSqlText += " exec usp_RefreshAllView;";

            sSqlText += "Delete From SysCustomField_Info Where ID in (" + _IDs.ToString() + ")";

            return DataCommon.QueryData(sSqlText);
        }

        //获取公司列表至控件
        public static void FullCustomToRadioButtonList(RadioButtonList rblList, int OrganID, bool HasAll)
        {
            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "";
                liItem.Text = "全部";
                rblList.Items.Add(liItem);
            }

            string sSQL = "Select * from SysOrgan_Info Where Status=0 And POrganID=" + OrganID.ToString()
               + " Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                sSQL = "Select * from SysCustomField_Info Where OrganID=" + sdr["ID"].ToString() + " Order By SortID";
                SqlDataReader sdrCustom = DataCommon.GetDataByReader(sSQL);
                while (sdrCustom.Read())
                {
                    ListItem liItem = new ListItem();
                    liItem.Value = sdrCustom["ID"].ToString();
                    liItem.Text = sdrCustom["OpCode"].ToString() + "|" + sdrCustom["OpName"].ToString();
                    rblList.Items.Add(liItem);
                }
                sdrCustom.Close();

                FullCustomToRadioButtonList(rblList, int.Parse(sdr["ID"].ToString()), false);
            }
            sdr.Close();
        }

        ////更新用户信息
        //public static int UpdateSingleCustom(int CustomID, string[] FieldValues)
        //{
        //    string sSqlText = "";
        //    if (CustomID > 0)
        //    {
        //        sSqlText = "UPDATE SysCustomField_Info SET OpName='"
        //            + FieldValues.GetValue(0) + "',OpCode='"                     
        //            + FieldValues.GetValue(1) + "',RoleID="
        //            + FieldValues.GetValue(2) + ",Comment='"
        //            + FieldValues[3] + "' WHERE ID=" + CustomID;
        //    }
        //    else
        //    {
        //        sSqlText = "Insert Into SysCustomField_Info (OpName,OpCode,RoleID,Comment,Status, Password) Values('"
        //        + FieldValues.GetValue(0) + "','" + FieldValues.GetValue(1) + "','"
        //        + FieldValues.GetValue(2) + "','" + FieldValues.GetValue(3) + "',"
        //        + "0,'111111' )";

        //    }
        //    return DataCommon.QueryData(sSqlText);
        //}

        //更新用户信息
        public static int UpdatePCustomIDByCustomIDs(string CustomIDs, int PCustomID)
        {
            string sSqlText = "Update SysCustomField_Info Set PCustomID=" + PCustomID.ToString() + " Where ID in (" + CustomIDs + ")";

            return DataCommon.QueryData(sSqlText);
        }

        //更新用户信息
        public static int UpdateCurrentCustom(int CustomID, string[] FieldValues)
        {
            string sSqlText = "";
            if (CustomID > 0)
            {
                sSqlText = "UPDATE SysCustomField_Info SET OpName='"
                    + FieldValues.GetValue(0) + "',Comment='"
                    + FieldValues[1] + "' WHERE ID=" + CustomID;
            }
            else
            {
                sSqlText = "Insert Into SysCustomField_Info (OpName,Comment,Status, Password) Values('"
                + FieldValues.GetValue(0) + "','" + FieldValues.GetValue(1) + "',"
                + "0,'111111' )";

            }
            return DataCommon.QueryData(sSqlText);
        }

        //更新用户密码
        public static Boolean CheckCustomLoginPassword(string OpCode, string Password)
        {
            string sSqlText = "Select 1 From SysCustomField_Info Where Password='" + Password + "' And OpCode='" + OpCode + "'";

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //更新用户密码
        public static Boolean CheckCustomPassword(int CustomID, string Password)
        {
            string sSqlText = "Select 1 From SysCustomField_Info Where Password='" + Password + "' And ID=" + CustomID;

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //更新用户密码
        public static Boolean CheckFieldNameExists(int CustomID, string TableNo, string FieldName)
        {
            string sSqlText = "Select 1 From SysCustomField_Info Where TableNo='" + TableNo + "' And FieldName='" + FieldName + "' And ID<>" + CustomID;

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //更新用户密码
        public static int UpdateCustomPassword(int CustomID, string Password)
        {
            string sSqlText = "begin UPDATE SysCustomField_Info SET Password='" + Password + "' WHERE ID=" + CustomID + "; ";
            string sLogText = "修改登录密码。";
            sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";

            return DataCommon.QueryData(sSqlText);
        }

        //开通用户登录
        public static int PassCustomByCustom(int CustomID)
        {
            string sSqlText = "begin UPDATE SysCustomField_Info SET IsCanLogin=1 WHERE ID=" + CustomID + "; ";
            string sLogText = "系统管理>权限管理> 开通用户: " + CustomID.ToString() + " 登录。";
            sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            return DataCommon.QueryData(sSqlText);
        }

        //锁定用户登录
        public static int StopCustomByCustom(int CustomID)
        {
            string sSqlText = "begin UPDATE SysCustomField_Info SET IsCanLogin=0 WHERE ID=" + CustomID + "; ";
            string sLogText = "系统管理>权限管理> 锁定用户: " + CustomID.ToString() + " 登录。";
            sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            return DataCommon.QueryData(sSqlText);
        }

        public static string GetCreateGUID()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static SqlDataReader GetCustomLogin(string sOpCode, string sPassword)
        {
            ///执行SQL
            string SqlText = "SELECT a.*,b.RoleName FROM SysCustomField_Info a "
                + " left join SysRole_Info b on a.RoleID=b.ID "
                + " WHERE a.OpCode='" + sOpCode + "' AND a.Password='" + sPassword
                + "' AND a.Status=0 And IsNull(a.IsCanLogin,1)=1 ";
            return DataCommon.GetDataByReader(SqlText);
        }

        public static string GetPurviewByRoles(string Roles)
        {
            string sPurviews = "";

            for (int i = 0; i < 500; i++)
            {
                sPurviews = sPurviews + "0";
            }
            string sCustomRoles = Roles + ",";
            char[] delimit = new char[] { ',' };
            foreach (string substr in sCustomRoles.Split(delimit))
            {
                if (substr.Length > 0)
                {
                    SqlDataReader sdr = SysClass.SysRole.GetSingleRoleByReader(int.Parse(substr));
                    if (sdr.Read())
                    {
                        string stPurviews = sdr["Purview"].ToString();
                        string tPurviews = "";
                        for (int i = 0; i < 500; i++)
                        {
                            if ((sPurviews[i] == char.Parse("1")) | (stPurviews[i] == char.Parse("1")))
                            {
                                tPurviews = tPurviews + "1";
                            }
                            else
                            {
                                tPurviews = tPurviews + "0";
                            }
                        }
                        sPurviews = tPurviews;
                    }
                    sdr.Close();
                }
            }
            return sPurviews;
        }

        public static int DeleteCustomByIsCanlogin(int _CustomID)
        {
            string sSQL = "Update SysCustomField_Info Set IsCanLogin=0 where Status =0 and ID=" + _CustomID.ToString() + ";";
            sSQL = sSQL + "";
            return DataCommon.QueryData(sSQL);
        }

        public static DataSet GetSysOpCodeLstByNoLogin(int OrganID, string SearchText)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM SysCustomField_Info a "
                + " left join SysOrgan_Info b on a.OrganID=b.ID and b.Status=0 "
                + " WHERE a.Status=0";
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpCode Like '%" + SearchText + "%' Or a.OpName Like '%" + SearchText + "%')";
            }
            if (OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID = " + OrganID.ToString();
            }
            sSQL = sSQL + " And IsNull(a.IsCanLogin,0)=0 Order By a.SortID";
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetSysOpCodeLstByNoRole(int RoleID, int OrganID, string SearchText)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM SysCustomField_Info a "
                + " left join SysOrgan_Info b on a.OrganID=b.ID and b.Status=0 "
                + " WHERE a.Status=0";
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpCode Like '%" + SearchText + "%' Or a.OpName Like '%" + SearchText + "%')";
            }
            if (OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID = " + OrganID.ToString();
            }
            sSQL = sSQL + " And charindex('," + RoleID.ToString() + ",',','+IsNull(CustomRoles,'')+',')=0 Order By a.SortID";
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetSysOpCodeLstByRole(int RoleID, string SearchText)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM SysCustomField_Info a "
                + " left join SysOrgan_Info b on a.OrganID=b.ID and b.Status=0 "
                + " WHERE a.Status=0";
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpCode Like '%" + SearchText + "%' Or a.OpName Like '%" + SearchText + "%')";
            }
            sSQL = sSQL + " And charindex('," + RoleID.ToString() + ",',','+IsNull(CustomRoles,'')+',')>0 Order By a.SortID";
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetSysCustomByDataSet1(int OrganID, string SearchText)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM SysCustomField_Info a "
                + " left join SysOrgan_Info b on a.OrganID=b.ID and b.Status=0 "
                + " WHERE a.Status=0";
            if (OrganID > 0)
            {

                sSQL = sSQL + " And IsNull(a.OrganID,0)=" + OrganID.ToString();
            }
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpCode Like '%" + SearchText + "%' Or a.OpName Like '%" + SearchText + "%')";
            }
            sSQL = sSQL + " Order By a.SortID";
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetChildrenCustomByDataSet(string CategoryNo, int OrganID, string SearchText)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM SysCustomField_Info a "
                + " left join SysOrgan_Info b on a.OrganID=b.ID and b.Status=0 "
                + " WHERE a.Status=0";
            if (CategoryNo.Length > 0)
            {
                sSQL = sSQL + " And a.CategoryNo='" + CategoryNo.ToString() + "'";
            }

            if (OrganID > 0)
            {

                sSQL = sSQL + " And (IsNull(a.OrganID,0) in (Select ID From dbo.GetOrganChildren(" + OrganID.ToString() + ", 1)))";
            }
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpCode Like '%" + SearchText + "%' Or a.OpName Like '%" + SearchText + "%')";
            }
            sSQL = sSQL + " Order By a.SortID";
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetSysCustomByReader(int OrganID)
        {
            string sSQL = "SELECT a.*,b.OrganName"
                + " FROM SysCustomField_Info a "
                + " left join SysOrgan_Info b on a.OrganID=b.ID and b.Status=0 "
                + " WHERE a.Status=0";

            sSQL = sSQL + " And a.OrganID=" + OrganID.ToString();

            sSQL = sSQL + "  Order By a.SortID";
            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetChildrenCustomByReader(int OrganID, string WhereSQL)
        {
            string sSQL = "SELECT a.*,b.OrganName,(Case IsNull(a.WorkStatus,0) When 0 then '在岗' when 1 then '出差' when 2 then '请假' when 3 then '临时外出' end) as WorkStatusName"
                 + ",b.OrganName,(Case IsNull(a.WorkStatus,0) When 0 then 'Normal' when 1 then 'Travel' when 2 then 'Leave' when 3 then 'TemporaryOutgoing' end) as WorkStatusClass"
                 + " FROM SysCustomField_Info a "
                 + " left join SysOrgan_Info b on a.OrganID=b.ID and b.Status=0 "
                 + " WHERE a.Status=0 " + WhereSQL;
            if (OrganID > 0)
            {
                sSQL = sSQL + " And IsNull(a.OrganID,0) in (Select ID From dbo.GetOrganChildren(" + OrganID.ToString() + ", 1))";
            }
            sSQL = sSQL + " Order By a.SortID";
            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSingleCustomFieldByReader(int ID)
        {
            string sSQL = "SELECT a.* FROM SysCustomField_Info a"
                + " WHERE a.Status =0 And a.ID=" + ID.ToString();
            return DataCommon.GetDataByReader(sSQL);
        }       

        public static int DeleteSingleCustom(int _ID)//===
        {
            string sSQL = "begin Delete from SysCustomField_Info Where Status =0 And ID=" + _ID.ToString() + "; ";
            string sLogText = "删除 系统管理>人员管理:" + _ID.ToString() + "的记录。";
            sSQL = sSQL + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            return DataCommon.QueryData(sSQL);
        }

        //更新用户密码
        public static Boolean CheckCustomNoExists(int _ID, string OpCode)
        {
            string sSqlText = "Select 1 From SysCustomField_Info Where OpCode='" + OpCode + "' And ID<>" + _ID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        public static string GetRecGuidByCustomFieldID(int CustomID)
        {
            string _RecGuid = "";
            SqlDataReader sdr = GetSingleCustomFieldByReader(CustomID);
            if (sdr.Read())
            {
                _RecGuid = sdr["RecGuid"].ToString();
            }
            sdr.Close();
            return _RecGuid;
        }

        public static int UpdateSingleCustom(int ID, string[] FieldValues, string DetailSQL)
        {
            string sSqlText = "begin ";
            if (ID > 0)
            {
                sSqlText = sSqlText + "begin UPDATE SysCustomField_Info SET FieldTitle = '" + FieldValues.GetValue(3)
                    + "',IsReadonly=" + FieldValues.GetValue(5)
                    + ",Description='" + FieldValues.GetValue(6) + "'";
                sSqlText = sSqlText + " WHERE ID =" + ID + ";";
                sSqlText = sSqlText + " End;";
            }
            else
            {
                sSqlText = sSqlText + "begin Insert Into SysCustomField_Info(RecGuid, TableNo, FieldName, FieldTitle, FieldType, IsReadonly, Description) "
                    + " Values('" + FieldValues.GetValue(0) + "','"
                    + FieldValues.GetValue(1) + "','" + FieldValues.GetValue(2) + "','"
                    + FieldValues.GetValue(3) + "'," + FieldValues.GetValue(4) + ","
                    + FieldValues.GetValue(5) + ",'" + FieldValues.GetValue(6) + "'";
                sSqlText = sSqlText + " ); ";
                sSqlText = sSqlText + " End;";
            }
            sSqlText = sSqlText + DetailSQL + " end";
            return DataCommon.QueryData(sSqlText);
        }

        public static int UpdateSingleCustomInfoEx(int ID, string[] FieldValues)
        {
            string sSqlText = "";
            if (ID > 0)
            {
                sSqlText = sSqlText + "UPDATE SysCustomField_Info SET "
                    + "OpName='" + FieldValues.GetValue(0)
                    + "',Position='" + FieldValues.GetValue(1)
                    + "',Place='" + FieldValues.GetValue(2)
                    + "',Phone='" + FieldValues.GetValue(3)
                    + "',TelNo='" + FieldValues.GetValue(4)
                    + "',Address='" + FieldValues.GetValue(5)
                    + "',CustomDesc='" + FieldValues.GetValue(6)
                    + "'";
                sSqlText = sSqlText + " WHERE ID =" + ID + ";";
            }
            return DataCommon.QueryData(sSqlText);
        }

        public static string GetCustomPathNameByID(int CustomID)
        {
            string _Title = "";
            SqlDataReader sdr = SysCustomField.GetSingleCustomFieldByReader(CustomID);
            if (sdr.Read())
            {
                _Title = SysOrgan.GetOrganPathNameByID(int.Parse(sdr["OrganID"].ToString()), "") + "->" + sdr["OpName"].ToString();
            }
            sdr.Close();
            return _Title;
        }

        public static int GetCustomIDByOpCode(string _OpCode)
        {
            int _CustomID = 0;
            SqlDataReader sdr = DataCommon.GetDataByReader("Select ID From SysCustomField_Info Where OpCode='" + _OpCode + "'");
            if (sdr.Read())
            {
                _CustomID = int.Parse(sdr["ID"].ToString());
            }
            sdr.Close();
            return _CustomID;
        }

        public static string GetCustomNameByOpCode(string _OpCode)
        {
            string _CustomName = "";
            SqlDataReader sdr = DataCommon.GetDataByReader("Select OpName From SysCustomField_Info Where OpCode='" + _OpCode + "'");
            if (sdr.Read())
            {
                _CustomName = sdr["OpName"].ToString();
            }
            sdr.Close();
            return _CustomName;
        }

        public static string GetCustomNameByCustomID(int _CustomID)
        {
            string _CustomName = "";
            SqlDataReader sdr = GetCustomInfoByReader(_CustomID);
            if (sdr.Read())
            {
                _CustomName = sdr["OpName"].ToString();
            }
            sdr.Close();
            return _CustomName;
        }

        public static string GetOpCodeByCustomID(int _CustomID)
        {
            string _CustomName = "";
            SqlDataReader sdr = GetCustomInfoByReader(_CustomID);
            if (sdr.Read())
            {
                _CustomName = sdr["OpCode"].ToString();
            }
            sdr.Close();
            return _CustomName;
        }

        public static int UpdateSingleCustomWorkStatus(int ID, int WorkStatus, string WorkDesc)
        {
            string sSQL = "Update SysCustomField_Info Set WorkStatus=" + WorkStatus.ToString() + ", WorkDesc='" + WorkDesc + "' Where ID=" + ID.ToString();
            return DataCommon.QueryData(sSQL);
        }

        //获取车站列表至控件
        public static void FullToCustomLst(string TableNo, DropDownList ddlList, bool HasAll, bool IsClear)
        {
            if (IsClear)
            {
                ddlList.Items.Clear();
            }

            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "0";
                liItem.Text = "请选择";
                ddlList.Items.Add(liItem);
            }

            string sSQL = "Select * from SysCustomField_Info Where Status=0 And TableNo='" + TableNo + "' Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["FieldType"].ToString() + "|" + sdr["FieldName"].ToString();
                liItem.Text = sdr["FieldTitle"].ToString() + "|扩展";
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }
       
    }
}
