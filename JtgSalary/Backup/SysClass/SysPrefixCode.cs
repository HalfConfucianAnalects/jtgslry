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
    public class SysPrefixCode
    {
        //获取地图列表首个ID
        public static int GetTopPrefixCodeID()
        {
            int _TopMapID = 0;
            SqlDataReader sdr = DataCommon.GetDataByReader("Select Top 1 * From SysPrefixCode_Info Where Status=0 And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " Order By SortID");
            if (sdr.Read())
            {
                _TopMapID = int.Parse(sdr["ID"].ToString());
            }
            sdr.Close();
            return _TopMapID;
        }

        public static DataSet GetPrefixCodeLstByDataSet(string WhereSQL)
        {
            string sSQL = "Select * from SysPrefixCode_Info Where Status=0 And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString();


            sSQL = sSQL + WhereSQL;
            

            sSQL = sSQL + " Order By SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetSinglePrefixCodeByReader(int ID)
        {
            string sSQL = "select * from SysPrefixCode_Info where Status=0 and IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " and ID=" + ID.ToString();
            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetUserPrefixCodePurviewByReader(int UserID)
        {
            string sSQL = "select * from SysPrefixCode_Info where Status=0 and IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " and ID In (Select PrefixCodeID From SysUserPrefixCodes_Info Where Status=0 and IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " And UserID=" + UserID.ToString() + ")";
            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetPrefixCodeLstByReader()
        {
            string sSQL = "select * from SysPrefixCode_Info where Status=0 And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString();
            return DataCommon.GetDataByReader(sSQL);
        }

        public static int DeleteSinglePrefixCode(int _ID)
        {
            string sSQL = "begin delete from SysPrefixCode_Info where Status =0 And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " and ID=" + _ID.ToString() + "; ";
            string sLogText = "删除 系统管理>单号前缀管理：单号前缀列表:" + _ID.ToString() + "的记录。";
            sSQL = sSQL + SysLogs.GetOperatorLogSQL(sLogText) + " End;";

            return DataCommon.QueryData(sSQL);
        }

        public static int DeleteUserByPrefixCode(int _UserID, int _PrefixCodeID)
        {
            string sSQL = "Update User_Info Set UserPrefixCodes=Replace(','+UserPrefixCodes+',','," + _PrefixCodeID.ToString() + ",','') where IsNull(SystemID,0)=" 
                + SysParams.GetPurviewSystemID().ToString() + " Status =0 and ID=" + _UserID.ToString() + ";";
            sSQL = sSQL + "";
            return DataCommon.QueryData(sSQL);
        }

        //判断预案编号已被使用
        public static bool CheckPrefixCodeIsUse(int PrefixCodeID)
        {
            string sSqlText = "Select top 1 1 From User_Info Where Status=0 And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And ','+UserPrefixCodes+',' Like '%," + PrefixCodeID.ToString() + ",%'";

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        public static int UpdateSinglePrefixCode(int ID, string[] FieldValues)
        {
            string sSqlText = "";
            if (ID > 0)
            {
                sSqlText = "begin update SysPrefixCode_Info set CodeName='" + FieldValues.GetValue(0)
                      + "',TableName='" + FieldValues[1] + ""
                      + "',PrefixCode='" + FieldValues[2] + ""
                      + "',Description='" + FieldValues[3] + "'"
                      + " WHERE ID =" + ID + ";";
                string sLogText = "更新 系统管理>单号前缀管理:单号前缀列表：" + FieldValues.GetValue(0) + "记录。";

                sSqlText += SysLogs.GetOperatorLogSQL(sLogText) + " end;";
            }
            else
            {
                sSqlText = "begin Insert Into SysPrefixCode_Info(SystemID, CodeName, TableName, PrefixCode, Description)Values("
                    + SysParams.GetPurviewSystemID().ToString() + ",'"
                    + FieldValues.GetValue(0) + "','" + FieldValues.GetValue(1) + "','" + FieldValues.GetValue(2)
                    + "','" + FieldValues.GetValue(3) + "')" + ";";
                string sLogText = "新增 系统管理>单号前缀管理:单号前缀列表：" + FieldValues.GetValue(0) + "记录。";
                sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            }
            return DataCommon.QueryData(sSqlText);
        }

        //获取车站列表至控件
        public static void FullToUserPrefixCodeLst(DropDownList ddlList, bool HasAll)
        {
            ddlList.Items.Clear();

            if (HasAll)
            {
                ListItem liItem = new ListItem();
                liItem.Value = "";
                liItem.Text = "请选择";
                ddlList.Items.Add(liItem);
            }

            string sSQL = "Select * from SysPrefixCode_Info Where Status=0 And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["ID"].ToString();
                liItem.Text = sdr["PrefixCodeName"].ToString();
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }
        //获取单号前缀
        public static SqlDataReader GetSinglePrefixCodeNameByReader(int _ID)
        {
            string sSQL = "Select * from SysPrefixCode_Info Where Status=0 And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " And ID=" + _ID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //获取单号前缀是否重复
        public static bool CheckPrefixCodeNameExists(int ID, string PrefixCodeName)
        {
            string sSqlText = "select 1 from SysPrefixCode_Info where IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " And PrefixCodeName ='" + PrefixCodeName.ToString() + "' and ID<>" + ID.ToString();
            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //获取授权单号前缀
        public static string GetPrefixCodeNameByID(int ID)
        {
            string _PrefixCodeName = "";
            SqlDataReader sdr = GetSinglePrefixCodeNameByReader(ID);
            if (sdr.Read())
            {
                _PrefixCodeName = sdr["PrefixCodeName"].ToString();
            }
            sdr.Close();
            return _PrefixCodeName;
        }

        public static DataSet GetPrefixCodeUserByDataSet(int PrefixCodeID, string SearchText)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM User_Info a "
                + " left join Organ_Info b on a.OrganID=b.ID and b.Status=0 And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " WHERE a.Status=0";
            if (PrefixCodeID > 0)
            {
                sSQL = sSQL + " And a.ID in (Select UserID From SysPrefixCode_Info where Status=0 And IsNull(SystemID,0)=" + SysClass.SysParams.GetPurviewSystemID().ToString() + " And PrefixCodeID=" + PrefixCodeID.ToString() + ")";
            }
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (a.OpCode Like '%" + SearchText + "%' Or a.OpName Like '%" + SearchText + "%')";
            }
            sSQL = sSQL + " And IsNull(a.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " Order By a.SortID";
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetPrefixCodeNotUserByDataSet(int PrefixCodeID, int OrganID, string SearchText)
        {
            string sSQL = "SELECT a.*,b.OrganName FROM User_Info a "
                + " left join Organ_Info b on a.OrganID=b.ID and b.Status=0 And IsNull(b.SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " WHERE a.Status=0";
            if (PrefixCodeID > 0)
            {
                sSQL = sSQL + " And a.ID not in (Select UserID From SysPrefixCode_Info where Status=0 And IsNull(SystemID,0)=" 
                    + SysClass.SysParams.GetPurviewSystemID().ToString() + " And PrefixCodeID=" + PrefixCodeID.ToString() + ")";
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

        public static int DeletePrefixCodeUser(int _ID)//===
        {
            string sSQL = "begin Delete from SysPrefixCode_Info Where Status =0 And ID=" + _ID.ToString() + "; ";
            string sLogText = "删除 系统管理>单号前缀下属人员:" + _ID.ToString() + "的记录。";
            sSQL = sSQL + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            return DataCommon.QueryData(sSQL);
        }

        //获取车站列表至控件
        public static void FullToPrefixCodeLst(DropDownCheckList ddlList)
        {
            ddlList.Items.Clear();

            string sSQL = "Select * from SysPrefixCode_Info Where Status=0 And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() + " Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            while (sdr.Read())
            {
                ListItem liItem = new ListItem();
                liItem.Value = sdr["ID"].ToString();
                liItem.Text = sdr["PrefixCodeName"].ToString();
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }
    }
}
