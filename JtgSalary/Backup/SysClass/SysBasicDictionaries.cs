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
    public class SysBasicDictionaries
    {
        public static int GetTopBaseMainID()
        {
            int _TopMapID = 0;
            SqlDataReader sdr = DataCommon.GetDataByReader("Select ID From SysBaseMain_Info Where Status=0 Order By SortID");
            if (sdr.Read())
            {
                _TopMapID = int.Parse(sdr["ID"].ToString());
            }
            sdr.Close();
            return _TopMapID;
        }
        //获取车间列表
        public static SqlDataReader GetSysBaseMainLstByReader()
        {
            string sSQL = "Select *  from SysBaseMain_Info Where Status=0 Order By SortID";

            return DataCommon.GetDataByReader(sSQL);
        }
        public static string GetCategoryNameByID(int MainID)
        {
            string _CategoryName = "";
            SqlDataReader sdr = GetSingleMianByReader(MainID);
            if (sdr.Read())
            {
                _CategoryName = sdr["MainName"].ToString();
            }
            sdr.Close();
            return _CategoryName;
        }
        //获取单个车间信息
        public static SqlDataReader GetSingleMianByReader(int _ID)
        {
            string sSQL = "Select * from SysBaseMain_Info Where Status=0 And ID=" + _ID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }
        public static DataSet GetMainstByDataSet(int MianID, string SearchText)
        {
            string sSQL = "Select *  from SysBaseDetail_Info Where  Status=0"
                + " And MainNo=" + MianID.ToString();
            if (SearchText.Length > 0)
            {
                sSQL = sSQL + " And (DetailNo Like '%" + SearchText + "%' Or DetailName Like '%" + SearchText + "%')";
            }

            sSQL = sSQL + " Order By SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }
        public static int DeleteMainDictionaries(int _ID)
        {
            string sSQL = "begin Delete from SysBaseDetail_Info Where Status=0 And ID=" + _ID.ToString() + "; ";
            string sLogText = "删除 系统管理>人员管理 机构部门：ID:" + _ID.ToString() + "的记录。";
            sSQL = sSQL + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            return DataCommon.QueryData(sSQL);
        }
        public static SqlDataReader GetSingleMainDsByReader(int _ID)
        {
            string sSQL = "Select * from SysBaseDetail_Info Where Status=0 And ID=" + _ID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }
        //判断工具编号是否重复
        public static Boolean CheckMainNoExists(int MainID, string MainNo)
        {
            string sSqlText = "Select 1 From SysBaseDetail_Info Where MainNo='" + MainNo + "' And ID<>" + MainID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }
        //更新添加工具档案信息
        public static int UpdateSingleBaseDate(int _ID, string[] FieldValues)
        {
            string sSqlText = "";
            if (_ID > 0)
            {
                sSqlText = "begin UPDATE SysBaseDetail_Info SET MainNo=" + FieldValues.GetValue(0) + ", DetailNo='" + FieldValues.GetValue(1)
                     + "',DetailName='" + FieldValues.GetValue(2) + "',Description='" + FieldValues.GetValue(3) + "'";
                sSqlText = sSqlText + ",SortID=" + FieldValues.GetValue(4) + " WHERE ID=" + _ID + "" + ";";
                string sLogText = "更新 人员管理>机构部门：" + FieldValues.GetValue(2) + "记录。";

                sSqlText += SysLogs.GetOperatorLogSQL(sLogText) + " end;";
            }
            else
            {
                sSqlText = "begin Insert Into SysBaseDetail_Info ( MainNo, DetailNo, DetailName, Description,SortID) Values("
                   + FieldValues.GetValue(0) + ",'" + FieldValues.GetValue(1) + "','"
                   + FieldValues.GetValue(2) + "','" + FieldValues.GetValue(3) + "'," + FieldValues.GetValue(4) + ")" + ";";
                string sLogText = "新增 人员管理>机构部门：" + FieldValues.GetValue(2) + "记录。";
                sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            }
            return DataCommon.QueryData(sSqlText);
        }

    }
}
