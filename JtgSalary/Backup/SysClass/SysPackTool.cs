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

namespace JtgTMS.SysClass
{
    public class SysPackTool
    {
        public static int _PackToolType = 1;

        public static string GetTableRecGuidByID(int _PackToolID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSinglePackToolsByReader(_PackToolID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static DataSet GetPackToolsLstByDataSet(string WhereSQL)
        {
            string sSQL = "Select a.*, IsNull(b.OrganName,'') As OrganName from Tool_Info a "
                + " Left join SysOrgan_Info b on b.Status=0 And a.OrganID=b.ID"
                + " Where IsNull(a.ToolType,0)=1 And a.Status=0" + WhereSQL;
            

            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetPickToolMemberLstByDataSet(int CategoryID, string WhereSQL)
        {
            string sSQL = "Select a.*,b.ToolNo,b.ToolName,b.Specification,b.Unit from ToolMember_Info a, Tool_Info b Where b.ToolType=" + SysTool._NomalToolType.ToString()
                + " And a.Status=0 and b.Status=0 " + WhereSQL;
            if (CategoryID > 0)
            {
                sSQL = sSQL + " And b.CategoryID=" + CategoryID.ToString();
            }
            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetPackToolsMemberLstByDataSet(int PackToolID)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification, b.MaterialCode,b.Unit  from ToolMember_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " Where IsNull(a.PackToolID,0)=" + PackToolID.ToString() + " And a.Status=0";

            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetSinglePackToolsByReader(int _ID)
        {
            string sSQL = "Select * from Tool_Info Where ToolType=" + _PackToolType.ToString() + " and Status=0 And ID=" + _ID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }
        public static SqlDataReader GetPackToolsByReader()
        {
            string sSql = "Select * from Tool_Info where Status= 0 And ToolType = " + _PackToolType.ToString();
            return DataCommon.GetDataByReader(sSql);
        }

        //更新添加工具档案信息
        public static int UpdateSinglePackTools(int _ID, string[] FieldValues, string DetailSQL)
        {
            string sSqlText = "begin";
            if (_ID > 0)
            {
                sSqlText = sSqlText + " UPDATE Tool_Info SET OrganID=" + FieldValues.GetValue(1) + ", ToolNo='" + FieldValues.GetValue(2)
                     + "',ToolName='" + FieldValues.GetValue(3) + "',Description='" + FieldValues.GetValue(4) + "'";
                sSqlText = sSqlText +" WHERE ID=" + _ID + ";";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into Tool_Info (TableRecGuid, ToolType, OrganID, ToolNo, ToolName, Description) Values('" 
                    + FieldValues.GetValue(0) + "'," + _PackToolType.ToString() + ","
                   + FieldValues.GetValue(1) + ",'" + FieldValues.GetValue(2) + "','" 
                   + FieldValues.GetValue(3) + "','" + FieldValues.GetValue(4) + "')" + ";";
            }
            sSqlText += "Insert Into ToolStock_Info (OrganID, ToolID)"
                + " select "
                + " a.ID As OrganID, b.ID As ToolID from SysOrgan_Info a, Tool_Info b "
                + " Where a.OrganType in (0, 1) and b.ToolNo='" + FieldValues.GetValue(2) + "'"
                + " And Convert(varchar(10),a.ID) + '-' + Convert(varchar(10),b.ID) not in ("
                + " Select Convert(varchar(10),a.OrganID) + '-' + Convert(varchar(10),a.ToolID) From ToolStock_Info a, Tool_Info b "
                + " where a.ToolID=b.ID and b.ToolNo='" + FieldValues.GetValue(2) + "')";

            sSqlText = sSqlText + DetailSQL
                + " end;";
            return DataCommon.QueryData(sSqlText);
        }

        //判断工具编号是否重复
        public static Boolean CheckPackToolNoExists(int CategoryID, string ToolID)
        {
            string sSqlText = "Select 1 From Tool_Info Where ToolType = " + _PackToolType.ToString() + " And ToolNo='" + ToolID + "' And ID<>" + CategoryID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //获取单个车间信息
        public static int DeleteSinglePackTools(int _IDs)
        {
            string sSQL = "begin Delete from Tool_Info Where Status=0 And ID in (" + _IDs.ToString() + "); ";
            sSQL = sSQL + " Delete from ToolMember_Info Where Status=0 And PackToolID in (" + _IDs.ToString() + ");";
            sSQL += " Delete From ToolStock_Info Where ToolID in (" + _IDs.ToString() + ");";
            string sLogText = "删除 系统管理>人员管理 机构部门：ID:" + _IDs.ToString() + "的记录。";
            sSQL = sSQL + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            return DataCommon.QueryData(sSQL);
        }

    }
}

