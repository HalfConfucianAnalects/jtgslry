using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CyxPack.CommonOperation;
using CyxPack.OperateSqlServer;
using System.Data.SqlClient;
using System.Data;

namespace JtgTMS.SysClass
{
    public class SysToolTest
    {
        //
        public static string ToolTest_SearchText = "";

        //工具检验
        public static string ToolTestLog_SearchText = "";
        public static string ToolIestLog_ApprovalStatus = "";

        public static bool HideGreaterZero_Flag = false;
        public static int ToolTest_Qualified = 0, ToolTest_UnQualified = 1;
        public static int ToolTest_Draft = 0, ToolTest_ProcessIsCancel = 1, ToolTest_ProcessIsReturn = 2;
        public static string ToolTest_TableName = "ToolTest_Info";

        public static string GetTableRecGuidByID(int _ToolTestID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSingleToolTestByReader(_ToolTestID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static DataSet GetToolTestLstByDataSet(int _OrganID, string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.*, e.ToolNo, e.ToolName, e.AliasesName, e.Specification, e.MaterialCode, e.Unit"
                + ", b.OpName as TestOpName, c.OrganName, d.ToolCode"
                + ", (Case IsNull(a.TestStatus,0) when 0 then '待检' when 1 then '合格' when 2 then '不合格' end) As TestStatusName"
                + ", (Case IsNull(a.ProcessStatus,0) when 0 then '未处理' when 1 then '注销' when 2 then '修复' end) As ProcessStatusName"
                + " from ToolTest_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.TestUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join ToolStockDetail_Info d on d.Status=0 And a.ToolDetailID=d.ID And d.OrganID=" + _OrganID.ToString()
                + " left join Tool_Info e on e.Status=0 And d.ToolID=e.ID"
                + " Where a.Status=0 " + sWhereSQL;
            if (_OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID=" + _OrganID.ToString();
            }
            sSQL = sSQL + " Order By a.TestDate Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetToolTestInfoLstByDataSet(int OrganID, int CategoryID, string WhereSQL)
        {
            string sSQL = "Select a.*, IsNull(b.Quantity, 0) As Quantity"
                + ", (Case IsNull(b.ToolStatus,0) when 0 then '合格' when 1 then '不合格' end) as ToolStatusName"
                + ", b.TestDate"
                + " from Tool_Info a "
                + " Left Join ToolStock_Info b on b.Status=0 And a.ID=b.ToolID";
            if (OrganID > 0)
            {
                sSQL += " And b.OrganID=" + OrganID.ToString();
            }
            sSQL += " Where a.Status=0" + WhereSQL;
            if (CategoryID > 0)
            {
                sSQL = sSQL + " And a.CategoryID=" + CategoryID.ToString();
            }  

            sSQL = sSQL + " Order By a.SortID, a.ToolNo, a.ToolName";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetSingleToolTestByReader(int _TestID)
        {
            string sSQL = "Select a.*, e.ToolNo, e.ToolName, e.AliasesName, e.Specification, e.MaterialCode, e.Unit"
                + ", b.OpCode as TestOpCode, b.OpName as TestOpName, c.OrganName"
                + ", (Case IsNull(a.TestStatus,0) when 0 then '待检' when 1 then '合格' when 2 then '不合格' end) As TestStatusName"
                + ", (Case IsNull(a.ProcessStatus,0) when 0 then '无' when 1 then '注销'when 2 then '修复' end) As ProcessStatusName"
                + ", d.ToolCode"
                + " from ToolTest_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.TestUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join ToolStockDetail_Info d on d.Status=0 And a.ToolDetailID=d.ID"
                + " left join Tool_Info e on d.Status=0 And d.ToolID=e.ID"
                + " Where a.Status=0 And a.ID=" + _TestID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSingleTestLstByReader(string _TestIDs)
        {
            string sSQL = "Select a.*, e.ToolNo, e.ToolName, e.AliasesName, e.Specification, e.MaterialCode, e.Unit"
                + ", b.OpCode as TestOpCode, b.OpName as TestOpName, c.OrganName"
                + ", (Case IsNull(ToolStatus,0) when 0 then '合格' when 1 then '不合格' end) As ToolStatusName"
                + ", (Case IsNull(ToolStatus,0) when 0 then '无' when 1 then '注销'when 2 then '修复' end) As ProcessStatusName"
                + ", d.TestCode, d.ToolCode"
                + " from ToolTest_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.TestUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join ToolStockDetail_Info d on d.Status=0 And a.ToolDetailID=d.ID And d.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString()
                + " left join Tool_Info e on e.Status=0 And d.ToolID=e.ID"
                + " Where a.Status=0 And a.ID in (" + _TestIDs + ")";

            return DataCommon.GetDataByReader(sSQL);
        }

        //删除工具申请单
        public static int DeleteSingleToolTest(string _TestIDs)
        {
            string sSQL = "begin Delete from ToolTest_Info Where ID in (" + _TestIDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static int UpdateToolTestInfo(int ToolDetailID, int TestUserID, string TestCode, int TestStatus, string Description)
        {
            string sSqlText = "begin";

            sSqlText += " Insert Into ToolTest_Info (ToolDetailID, TestCode, TestDate, TestStatus, Description, TestUserID, OrganID, CreateUserID) "
                + " Values(" + ToolDetailID.ToString() + ", '" + TestCode.ToString() + "', GetDate()," + TestStatus.ToString() 
                + ",'" + Description + "' ," + TestUserID.ToString() + "," + SysGlobal.GetCurrentUserOrganID().ToString()
                + "," + SysGlobal.GetCurrentUserID().ToString()
                + "); ";

            sSqlText += " Update ToolStockDetail_Info Set TestCode='" + TestCode + "', TestStatus=" + TestStatus.ToString() + ", TestDate=GetDate() Where ID=" + ToolDetailID.ToString() + ";";

            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static int UpdateToolTestCode(int _TestID, int _ToolDetailID, string TestCode)
        {
            string sSqlText = "begin";

            sSqlText += " Update ToolTest_Info Set TestCode = '" + TestCode.ToString() + "' Where ID=" + _TestID.ToString() + "; ";

            sSqlText += " Update ToolStockDetail_Info Set TestCode='" + TestCode + "' Where ID=" + _ToolDetailID.ToString() + ";";

            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static int UpdateTestCancelStatus(string MainTableName, string TestIDs, int CancelStatus, string Description)
        {
            string sSqlText = "begin";

            sSqlText += " Insert Into ApprovalLog_Info (MainTableName, MainID, ApprovalStatus, ApprovalUserID, ApprovalTime, Description) "
                + " (Select '" + MainTableName + "', ID, " + CancelStatus.ToString() + "," + SysGlobal.GetCurrentUserID().ToString()
                + ", GetDate(), '" + Description + "' From ToolTest_Info Where Status=0 And ProcessStatus=0 And ID In (" + TestIDs + ")); ";

            if (CancelStatus == ToolTest_ProcessIsCancel)
            {
                sSqlText += " Update ToolStock_Info Set ToolStock_Info.TestQuantity=ToolStock_Info.TestQuantity-b.ACount"
                    + ", ToolStock_Info.CancelQuantity=ToolStock_Info.CancelQuantity+b.ACount"
                    + " From (Select b.ToolID, Count(1) As ACount  From ToolTest_Info a,ToolStockDetail_Info b "
                    + " Where a.Status=0 And a.ToolDetailID=b.ID And b.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString()
                    + " And a.ID In (" + TestIDs + ") Group By b.ToolID) b"
                    + " Where ToolStock_Info.Status=0 And ToolStock_Info.ToolID=b.ToolID ;";

                sSqlText += " Update ToolStockDetail_Info Set ToolStatus=5, TestStatus=" + CancelStatus + " Where OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString()
                    + " And ID in (Select ToolDetailID From ToolTest_Info Where Status=0 And ID in (" + TestIDs + "));";
            }
            else
            {
                sSqlText += " Update ToolStockDetail_Info Set ToolStatus=5, TestStatus=" + CancelStatus + " Where OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString()
                        + " And ID in (Select ToolDetailID From ToolTest_Info Where Status=0 And ID in (" + TestIDs + "));";
            }

            sSqlText += " Update ToolTest_Info Set ProcessStatus=" + CancelStatus.ToString()
                + " Where Status=0 And ProcessStatus=0 And ID In (" + TestIDs + ");";

            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }
    }
}
