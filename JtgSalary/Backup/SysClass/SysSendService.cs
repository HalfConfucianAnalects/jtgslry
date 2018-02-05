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
    public class SysSendService
    {
        //工具送检单查询
        public static string SendService_SearchText = "";
        public static string SendService_ApprovalStatus = "";

        public static string SendService_TableName = "SendService_Info";
        public static int SendService_Draft = 0, SendService_ApprovalIsOK = 1;
        public static string GetTableRecGuidByID(int _SendServiceID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSingleSendServiceByReader(_SendServiceID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static DataSet GetSendServiceLstByDataSet(int _OrganID, string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.*"
                + ", b.OpName as SendServiceOpName, c.OrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from SendService_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.SendServiceUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " Where a.Status=0 " + sWhereSQL;
            if (_OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID=" + _OrganID.ToString();
            }
            sSQL = sSQL + " Order By a.SendServiceDate Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetSendServiceLstByReader(int _OrganID, string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.*"
                + ", b.OpName as SendServiceOpName, c.OrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from SendService_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.SendServiceUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " Where a.Status=0 " + sWhereSQL;
            if (_OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID=" + _OrganID.ToString();
            }
            sSQL = sSQL + " Order By a.SendServiceDate Desc";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static DataSet GetSendServiceDetailsLstByDataSet(int SendServiceID, string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName,"
                + " b.Specification, b.MaterialCode,b.Unit, IsNull(c.Quantity,0) As StockQuantity, a.Quantity As OldQuantity "
                + ", c.StorageLocation, d.ToolCode, d.TestCode"
                + " from SendServiceDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " left Join ToolStock_Info c on c.Status=0 And c.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + " and c.ToolID=a.ToolID"
                + " left join ToolStockDetail_Info d on d.Status=0 And a.ToolDetailID=d.ID"
                + " Where IsNull(a.SendServiceID,0)=" + SendServiceID.ToString()
                + " And a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetSendServiceDetailsLstByReader(int SendServiceID, string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification, b.MaterialCode,b.Unit  from SendServiceDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " Where a.Status=0" + WhereSQL;

            if (SendServiceID > 0)
            {
                sSQL += " And IsNull(a.SendServiceID,0)=" + SendServiceID.ToString() ;
            }

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSendServiceDetailsLstByReader(string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification, b.MaterialCode,b.Unit from SendServiceDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " Where a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static Boolean CheckSendServiceNoExists(int SendServiceID, string SendServiceNo)
        {
            string sSqlText = "Select 1 From SendService_Info "
                + " Where SendServiceNo='" + SendServiceNo + "' And ID<>" + SendServiceID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //更新添加工具档案信息
        public static int UpdateSingleSendService(int _SendServiceID, string[] FieldValues, string DetailsSQL)
        {
            string sSqlText = "begin";

            //sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity+b.Quantity "
            //    + ", ToolStock_Info.ServiceQuantity=ToolStock_Info.ServiceQuantity - b.Quantity"
            //    + " From SendServiceDetails_Info b where b.SendServiceID In (Select ID From SendService_Info Where Status=0 "
            //    + " And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "') and ToolStock_Info.ToolID=b.ToolID;";

            sSqlText += " Update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity + b.Quantity "
               + ", ToolStock_Info.ServiceQuantity=ToolStock_Info.ServiceQuantity - b.Quantity"
               + " From (Select ToolID, Sum(Quantity) As Quantity From SendServiceDetails_Info a, SendService_Info b where a.SendServiceID=b.Id  "
               + " And b.TableRecGuid='" + FieldValues.GetValue(0) + "' and b.ApprovalStatus=1 And b.Status=0 Group By ToolID) b "
               + " Where ToolStock_Info.ToolID=b.ToolID And ToolStock_Info.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + ";";

            if (_SendServiceID > 0)
            {
                sSqlText = sSqlText + " UPDATE SendService_Info SET SendServiceNo='" + FieldValues.GetValue(1) + "'"
                     + ",SendServiceUserID=" + FieldValues.GetValue(2) + ""
                     + ",Description='" + FieldValues.GetValue(3) + "'"
                     + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _SendServiceID + "" + ";";

                sSqlText += " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into SendService_Info (TableRecGuid"
                    + ", SendServiceNo"
                    + ", SendServiceDate"
                    + ", OrganID"
                    + ", SendServiceUserID"
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
               + " (Select distinct " + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + ", ToolID, 0 From SendServiceDetails_Info Where Status=0 "
               + " And SendServiceID In (Select ID From SendService_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "')"
               + " And ToolID not in (Select ToolID From ToolStock_Info Where OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + "));";

            //sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity-b.Quantity "
            //    + ", ToolStock_Info.ServiceQuantity=ToolStock_Info.ServiceQuantity + b.Quantity"
            //    + " From SendServiceDetails_Info b where b.SendServiceID In (Select ID From SendService_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 "
            //    + " And TableRecGuid='" + FieldValues.GetValue(0) + "') and ToolStock_Info.ToolID=b.ToolID"
            //    + " And ToolStock_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID() + ";";

            sSqlText += " Update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity - b.Quantity "
               + ", ToolStock_Info.ServiceQuantity=ToolStock_Info.ServiceQuantity + b.Quantity"
               + " From (Select ToolID, Sum(Quantity) As Quantity From SendServiceDetails_Info a, SendService_Info b where a.SendServiceID=b.Id  "
               + " And b.TableRecGuid='" + FieldValues.GetValue(0) + "'and b.ApprovalStatus=1 And b.Status=0 Group By ToolID) b "
               + " Where ToolStock_Info.ToolID=b.ToolID And ToolStock_Info.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + ";";

            sSqlText += " Update ToolStockDetail_Info Set ToolStockDetail_Info.ToolStatus=4 From "
                + " SendServiceDetails_Info b where b.SendServiceID In (Select ID From SendService_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "') and ToolStockDetail_Info.ID=b.ToolDetailID"
                + " And ToolStockDetail_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID() + ";";

               sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static SqlDataReader GetSingleSendServiceByReader(int _SendServiceID)
        {
            string sSQL = "Select a.*"
                + ", b.OpCode as SendServiceOpCode,  b.OpName as SendServiceOpName, c.OrganName, d.OpName as CreateOpName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from SendService_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.SendServiceUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                + " Where a.Status=0 And a.ID=" + _SendServiceID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //删除工具申请单
        public static int DeleteSingleSendService(string _SendServiceIDs)
        {
            string sSQL = "begin ";

            sSQL += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity-b.Quantity "
                + ", ToolStock_Info.ServiceQuantity=ToolStock_Info.ServiceQuantity + b.Quantity"
	                + " From "
                + " (select a.ToolID, Sum(a.Quantity) As Quantity From SendServiceDetails_Info a, SendService_Info b "
                + " where a.SendServiceID=b.ID And IsNull(b.ApprovalStatus,0)=1 And b.ID in (" + _SendServiceIDs.ToString() + ")"
                + " group by a.ToolID) b where  ToolStock_Info.ToolID=b.ToolID";

            sSQL += " Delete from SendService_Info Where ID in (" + _SendServiceIDs.ToString() + "); ";
            sSQL = sSQL + " Delete from SendServiceDetails_Info Where SendServiceID in (" + _SendServiceIDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static int UpdateApprovalSendService(string MainTableName, string IDs, int ApprovalStatus, string Description)
        {
            string sSqlText = "begin";

            sSqlText += " Insert Into ApprovalLog_Info (MainTableName, MainID, ApprovalStatus, ApprovalUserID, ApprovalTime, Description) "
                + " (Select '" + MainTableName + "', ID, " + ApprovalStatus.ToString() + "," + SysGlobal.GetCurrentUserID().ToString()
                + ", GetDate(), '" + Description + "' From SendService_info Where Status=0 And ID In (" + IDs + ")); ";

            sSqlText += " Update SendService_info Set ApprovalStatus=" + ApprovalStatus.ToString()
                + " Where Status=0  And ID In (" + IDs + ");";

            if (ApprovalStatus == SendService_ApprovalIsOK)
            {
                sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity-b.Quantity "
                    + ", ToolStock_Info.ServiceQuantity=ToolStock_Info.ServiceQuantity + b.Quantity"
                    + " From "
                    + " (select a.ToolID, Sum(a.Quantity) As Quantity From SendServiceDetails_Info a, SendService_Info b "
                    + " where a.SendServiceID=b.ID And IsNull(b.ApprovalStatus,0)=1 And b.ID in (" + IDs.ToString() + ")"
                    + " group by a.ToolID) b where  ToolStock_Info.ToolID=b.ToolID";
            }

            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }
    }
}
