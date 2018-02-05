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
    public class SysBackTest
    {
        //工具送检单查询
        public static string BackTest_SearchText = "";
        public static string BackTest_ApprovalStatus = "";

        public static string BackTest_TableName = "BackTest_Info";
        public static int BackTest_Draft = 0, BackTest_ApprovalIsOK = 1;
        public static string GetTableRecGuidByID(int _BackTestID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSingleBackTestByReader(_BackTestID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static DataSet GetBackTestLstByDataSet(int _OrganID, string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.*"
                + ", b.OpName as BackTestOpName, c.OrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from BackTest_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.BackTestUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " Where a.Status=0 " + sWhereSQL;
            if (_OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID=" + _OrganID.ToString();
            }
            sSQL = sSQL + " Order By a.BackTestDate Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetBackTestLstByReader(int _OrganID, string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.*"
                + ", b.OpName as BackTestOpName, c.OrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from BackTest_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.BackTestUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " Where a.Status=0 " + sWhereSQL;
            if (_OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID=" + _OrganID.ToString();
            }
            sSQL = sSQL + " Order By a.BackTestDate Desc";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static DataSet GetBackTestDetailsLstByDataSet(int BackTestID, string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName,"
                + " b.Specification, b.MaterialCode,b.Unit, IsNull(c.TestQuantity,0) As StockQuantity, a.Quantity As OldQuantity "
                + ", c.StorageLocation, d.ToolCode, d.TestCode"
                + " from BackTestDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " left Join ToolStock_Info c on c.Status=0 And c.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + " and c.ToolID=a.ToolID"
                + " left join ToolStockDetail_Info d on d.Status=0 And a.ToolDetailID=d.ID"
                + " Where IsNull(a.BackTestID,0)=" + BackTestID.ToString()
                + " And a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetBackTestDetailsLstByReader(int BackTestID, string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification, b.MaterialCode,b.Unit  from BackTestDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " Where a.Status=0" + WhereSQL;

            if (BackTestID > 0)
            {
                sSQL += " And IsNull(a.BackTestID,0)=" + BackTestID.ToString() ;
            }

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetBackTestDetailsLstByReader(string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification, b.MaterialCode,b.Unit from BackTestDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " Where a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static Boolean CheckBackTestNoExists(int BackTestID, string BackTestNo)
        {
            string sSqlText = "Select 1 From BackTest_Info "
                + " Where BackTestNo='" + BackTestNo + "' And ID<>" + BackTestID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //更新添加工具档案信息
        public static int UpdateSingleBackTest(int _BackTestID, string[] FieldValues, string DetailsSQL)
        {
            string sSqlText = "begin";

            //sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity-b.Quantity "
            //    + ", ToolStock_Info.TestQuantity=ToolStock_Info.TestQuantity + b.Quantity"
            //    + " From BackTestDetails_Info b where b.BackTestID In (Select ID From BackTest_Info Where Status=0 "
            //    + " And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "') and ToolStock_Info.ToolID=b.ToolID;";

            sSqlText += " Update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity - b.Quantity "
               + ", ToolStock_Info.TestQuantity=ToolStock_Info.TestQuantity + b.Quantity"
               + " From (Select ToolID, Sum(Quantity) As Quantity From BackTestDetails_Info a, BackTest_Info b where a.BackTestID=b.Id  "
               + " And b.TableRecGuid='" + FieldValues.GetValue(0) + "' and b.ApprovalStatus=1 And b.Status=0 Group By ToolID) b "
               + " Where ToolStock_Info.ToolID=b.ToolID And ToolStock_Info.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + ";";

            if (_BackTestID > 0)
            {
                sSqlText = sSqlText + " UPDATE BackTest_Info SET BackTestNo='" + FieldValues.GetValue(1) + "'"
                     + ",BackTestUserID=" + FieldValues.GetValue(2) + ""
                     + ",Description='" + FieldValues.GetValue(3) + "'"
                     + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _BackTestID + "" + ";";

                sSqlText += " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into BackTest_Info (TableRecGuid"
                    + ", BackTestNo"
                    + ", BackTestDate"
                    + ", OrganID"
                    + ", BackTestUserID"
                    + ", CreateUserID"
                    + ", Description"
                    + ", ApprovalStatus)"
                    + " Values('" + FieldValues.GetValue(0) + "'"
                    + ",'" + FieldValues.GetValue(1) + "'"
                    + ",GetDate()"
                    + "," + SysGlobal.GetCurrentUserOrganID().ToString()
                    + "," + FieldValues.GetValue(2) + ""
                    + "," + SysGlobal.GetCurrentUserID().ToString() + ""
                    + ",'" + FieldValues.GetValue(3) + "'"
                    + "," + FieldValues.GetValue(4) + ")";
                sSqlText = sSqlText + " ;";
            }
            sSqlText = sSqlText + DetailsSQL;

            sSqlText += " Insert Into ToolStock_Info (OrganID, ToolID, Quantity)"
               + " (Select distinct " + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + ", ToolID, 0 From BackTestDetails_Info Where Status=0 "
               + " And BackTestID In (Select ID From BackTest_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "')"
               + " And ToolID not in (Select ToolID From ToolStock_Info Where OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + "));";

            //sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity+b.Quantity "
            //    + ", ToolStock_Info.TestQuantity=ToolStock_Info.TestQuantity - b.Quantity"
            //    + " From BackTestDetails_Info b where b.BackTestID In (Select ID From BackTest_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 "
            //    + " And TableRecGuid='" + FieldValues.GetValue(0) + "') and ToolStock_Info.ToolID=b.ToolID"
            //    + " And ToolStock_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID() + ";";

            sSqlText += " Update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity + b.Quantity "
               + ", ToolStock_Info.TestQuantity=ToolStock_Info.TestQuantity - b.Quantity"
               + " From (Select ToolID, Sum(Quantity) As Quantity From BackTestDetails_Info a, BackTest_Info b where a.BacktestID=b.Id  "
               + " And b.TableRecGuid='" + FieldValues.GetValue(0) + "'and b.ApprovalStatus=1 And b.Status=0 Group By ToolID) b "
               + " Where ToolStock_Info.ToolID=b.ToolID And ToolStock_Info.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + ";";

            sSqlText += " Update ToolStockDetail_Info Set ToolStockDetail_Info.ToolStatus=0, ToolStockDetail_Info.TestStatus=0 From "
                + " BackTestDetails_Info b where b.BackTestID In (Select ID From BackTest_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "') and ToolStockDetail_Info.ID=b.ToolDetailID"
                + " And ToolStockDetail_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID() + ";";

               sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static SqlDataReader GetSingleBackTestByReader(int _BackTestID)
        {
            string sSQL = "Select a.*"
                + ", b.OpCode as BackTestOpCode,  b.OpName as BackTestOpName, c.OrganName, d.OpName as CreateOpName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from BackTest_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.BackTestUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                + " Where a.Status=0 And a.ID=" + _BackTestID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //删除工具申请单
        public static int DeleteSingleBackTest(string _BackTestIDs)
        {
            string sSQL = "begin ";

            sSQL += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity + b.Quantity "
                + ", ToolStock_Info.TestQuantity=ToolStock_Info.TestQuantity - b.Quantity"
	                + " From "
                + " (select a.ToolID, Sum(a.Quantity) As Quantity From BackTestDetails_Info a, BackTest_Info b "
                + " where a.BackTestID=b.ID And IsNull(b.ApprovalStatus,0)=1 And b.ID in (" + _BackTestIDs.ToString() + ")"
                + " group by a.ToolID) b where  ToolStock_Info.ToolID=b.ToolID";

            sSQL += " Delete from BackTest_Info Where ID in (" + _BackTestIDs.ToString() + "); ";
            sSQL = sSQL + " Delete from BackTestDetails_Info Where BackTestID in (" + _BackTestIDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static int UpdateApprovalBackTest(string MainTableName, string IDs, int ApprovalStatus, string Description)
        {
            string sSqlText = "begin";

            sSqlText += " Insert Into ApprovalLog_Info (MainTableName, MainID, ApprovalStatus, ApprovalUserID, ApprovalTime, Description) "
                + " (Select '" + MainTableName + "', ID, " + ApprovalStatus.ToString() + "," + SysGlobal.GetCurrentUserID().ToString()
                + ", GetDate(), '" + Description + "' From BackTest_info Where Status=0 And ID In (" + IDs + ")); ";

            sSqlText += " Update BackTest_info Set ApprovalStatus=" + ApprovalStatus.ToString()
                + " Where Status=0  And ID In (" + IDs + ");";

            if (ApprovalStatus == BackTest_ApprovalIsOK)
            {
                sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity-b.Quantity "
                    + ", ToolStock_Info.TestQuantity=ToolStock_Info.TestQuantity + b.Quantity"
                    + " From "
                    + " (select a.ToolID, Sum(a.Quantity) As Quantity From BackTestDetails_Info a, BackTest_Info b "
                    + " where a.BackTestID=b.ID And IsNull(b.ApprovalStatus,0)=1 And b.ID in (" + IDs.ToString() + ")"
                    + " group by a.ToolID) b where  ToolStock_Info.ToolID=b.ToolID";
            }

            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }
    }
}
