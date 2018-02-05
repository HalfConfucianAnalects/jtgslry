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
    public class SysBackService
    {
        //工具送检单查询
        public static string BackService_SearchText = "";
        public static string BackService_ApprovalStatus = "";

        public static string BackService_TableName = "BackService_Info";
        public static int BackService_Draft = 0, BackService_ApprovalIsOK = 1;
        public static string GetTableRecGuidByID(int _BackServiceID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSingleBackServiceByReader(_BackServiceID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static DataSet GetBackServiceLstByDataSet(int _OrganID, string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.*"
                + ", b.OpName as BackServiceOpName, c.OrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from BackService_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.BackServiceUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " Where a.Status=0 " + sWhereSQL;
            if (_OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID=" + _OrganID.ToString();
            }
            sSQL = sSQL + " Order By a.BackServiceDate Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetBackServiceLstByReader(int _OrganID, string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.*"
                + ", b.OpName as BackServiceOpName, c.OrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from BackService_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.BackServiceUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " Where a.Status=0 " + sWhereSQL;
            if (_OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID=" + _OrganID.ToString();
            }
            sSQL = sSQL + " Order By a.BackServiceDate Desc";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static DataSet GetBackServiceDetailsLstByDataSet(int BackServiceID, string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName,"
                + " b.Specification, b.MaterialCode,b.Unit, IsNull(c.ServiceQuantity,0) As StockQuantity, a.Quantity As OldQuantity "
                + ", c.StorageLocation, d.ToolCode, d.TestCode"
                + " from BackServiceDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " left Join ToolStock_Info c on c.Status=0 And c.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + " and c.ToolID=a.ToolID"
                + " left join ToolStockDetail_Info d on d.Status=0 And a.ToolDetailID=d.ID"
                + " Where IsNull(a.BackServiceID,0)=" + BackServiceID.ToString()
                + " And a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetBackServiceDetailsLstByReader(int BackServiceID, string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification, b.MaterialCode,b.Unit  from BackServiceDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " Where a.Status=0" + WhereSQL;

            if (BackServiceID > 0)
            {
                sSQL += " And IsNull(a.BackServiceID,0)=" + BackServiceID.ToString() ;
            }

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetBackServiceDetailsLstByReader(string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification, b.MaterialCode,b.Unit from BackServiceDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " Where a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static Boolean CheckBackServiceNoExists(int BackServiceID, string BackServiceNo)
        {
            string sSqlText = "Select 1 From BackService_Info "
                + " Where BackServiceNo='" + BackServiceNo + "' And ID<>" + BackServiceID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //更新添加工具档案信息
        public static int UpdateSingleBackService(int _BackServiceID, string[] FieldValues, string DetailsSQL)
        {
            string sSqlText = "begin";

            //sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity-b.Quantity "
            //    + ", ToolStock_Info.ServiceQuantity=ToolStock_Info.ServiceQuantity + b.Quantity"
            //    + " From BackServiceDetails_Info b where b.BackServiceID In (Select ID From BackService_Info Where Status=0 "
            //    + " And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "') and ToolStock_Info.ToolID=b.ToolID;";

            sSqlText += " Update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity - b.Quantity "
               + ", ToolStock_Info.ServiceQuantity=ToolStock_Info.ServiceQuantity + b.Quantity"
               + " From (Select ToolID, Sum(Quantity) As Quantity From BackServiceDetails_Info a, BackService_Info b where a.BackServiceID=b.Id  "
               + " And b.TableRecGuid='" + FieldValues.GetValue(0) + "' and b.ApprovalStatus=1 And b.Status=0 Group By ToolID) b "
               + " Where ToolStock_Info.ToolID=b.ToolID And ToolStock_Info.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + ";";

            if (_BackServiceID > 0)
            {
                sSqlText = sSqlText + " UPDATE BackService_Info SET BackServiceNo='" + FieldValues.GetValue(1) + "'"
                     + ",BackServiceUserID=" + FieldValues.GetValue(2) + ""
                     + ",Description='" + FieldValues.GetValue(3) + "'"
                     + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _BackServiceID + "" + ";";

                sSqlText += " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into BackService_Info (TableRecGuid"
                    + ", BackServiceNo"
                    + ", BackServiceDate"
                    + ", OrganID"
                    + ", BackServiceUserID"
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
               + " (Select distinct " + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + ", ToolID, 0 From BackServiceDetails_Info Where Status=0 "
               + " And BackServiceID In (Select ID From BackService_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "')"
               + " And ToolID not in (Select ToolID From ToolStock_Info Where OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + "));";

            //sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity + b.Quantity "
            //    + ", ToolStock_Info.ServiceQuantity=ToolStock_Info.ServiceQuantity - b.Quantity"
            //    + " From BackServiceDetails_Info b where b.BackServiceID In (Select ID From BackService_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 "
            //    + " And TableRecGuid='" + FieldValues.GetValue(0) + "') and ToolStock_Info.ToolID=b.ToolID"
            //    + " And ToolStock_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID() + ";";

            sSqlText += " Update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity + b.Quantity "
               + ", ToolStock_Info.ServiceQuantity=ToolStock_Info.ServiceQuantity - b.Quantity"
               + " From (Select ToolID, Sum(Quantity) As Quantity From BackServiceDetails_Info a, BackService_Info b where a.BackServiceID=b.Id  "
               + " And b.TableRecGuid='" + FieldValues.GetValue(0) + "'and b.ApprovalStatus=1 And b.Status=0 Group By ToolID) b "
               + " Where ToolStock_Info.ToolID=b.ToolID And ToolStock_Info.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + ";";

            sSqlText += " Update ToolStockDetail_Info Set ToolStockDetail_Info.ToolStatus=0 From "
                + " BackServiceDetails_Info b where b.BackServiceID In (Select ID From BackService_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "') and ToolStockDetail_Info.ID=b.ToolDetailID"
                + " And ToolStockDetail_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID() + ";";

               sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static SqlDataReader GetSingleBackServiceByReader(int _BackServiceID)
        {
            string sSQL = "Select a.*"
                + ", b.OpCode as BackServiceOpCode,  b.OpName as BackServiceOpName, c.OrganName, d.OpName as CreateOpName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from BackService_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.BackServiceUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                + " Where a.Status=0 And a.ID=" + _BackServiceID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //删除工具申请单
        public static int DeleteSingleBackService(string _BackServiceIDs)
        {
            string sSQL = "begin ";

            sSQL += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity + b.Quantity "
                + ", ToolStock_Info.ServiceQuantity=ToolStock_Info.ServiceQuantity - b.Quantity"
	                + " From "
                + " (select a.ToolID, Sum(a.Quantity) As Quantity From BackServiceDetails_Info a, BackService_Info b "
                + " where a.BackServiceID=b.ID And IsNull(b.ApprovalStatus,0)=1 And b.ID in (" + _BackServiceIDs.ToString() + ")"
                + " group by a.ToolID) b where  ToolStock_Info.ToolID=b.ToolID";

            sSQL += " Delete from BackService_Info Where ID in (" + _BackServiceIDs.ToString() + "); ";
            sSQL = sSQL + " Delete from BackServiceDetails_Info Where BackServiceID in (" + _BackServiceIDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static int UpdateApprovalBackService(string MainTableName, string IDs, int ApprovalStatus, string Description)
        {
            string sSqlText = "begin";

            sSqlText += " Insert Into ApprovalLog_Info (MainTableName, MainID, ApprovalStatus, ApprovalUserID, ApprovalTime, Description) "
                + " (Select '" + MainTableName + "', ID, " + ApprovalStatus.ToString() + "," + SysGlobal.GetCurrentUserID().ToString()
                + ", GetDate(), '" + Description + "' From BackService_info Where Status=0 And ID In (" + IDs + ")); ";

            sSqlText += " Update BackService_info Set ApprovalStatus=" + ApprovalStatus.ToString()
                + " Where Status=0  And ID In (" + IDs + ");";

            if (ApprovalStatus == BackService_ApprovalIsOK)
            {
                sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity-b.Quantity "
                    + ", ToolStock_Info.ServiceQuantity=ToolStock_Info.ServiceQuantity + b.Quantity"
                    + " From "
                    + " (select a.ToolID, Sum(a.Quantity) As Quantity From BackServiceDetails_Info a, BackService_Info b "
                    + " where a.BackServiceID=b.ID And IsNull(b.ApprovalStatus,0)=1 And b.ID in (" + IDs.ToString() + ")"
                    + " group by a.ToolID) b where  ToolStock_Info.ToolID=b.ToolID";
            }

            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }
    }
}
