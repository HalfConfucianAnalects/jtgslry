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
    public class SysDepotTool
    {
        //已入库段发货单
        public static string SendOut_SearchText = "";
        public static string SendOut_ApprovalStatus = "";

        //段发货入库
        public static string SendOutNoStorage_SearchText = "";

        //已入库段发货单
        public static string SendOutInStorage_SearchText = "";

        public static string ToolOrder_TableName = "ToolOrder_Info", PurchaseOrder_TableName = "PurchaseOrder_Info", SendOut_TableName = "SendOut_Info";
        public static int DepotTool_Draft = 0, DepotTool_Approvaling = 1, DepotTool_ApprovalIsOK = 2, DepotTool_ApprovalIsBack = 3;
        public static int PurchaseOrder_Draft = 0, PurchaseOrder_IsOK = 1, PurchaseOrder_IsBack = 2;
        public static int SendOut_Draft = 0, SendOut_IsOK = 1, SendOut_In = 2;
        public static string DepotTool_DraftName = "草稿";

        //工具采购申请
        public static string Order_SearchText = "";
        public static string Order_ApprovalStatus = "";

        public static string GetTableRecGuidByID(int _OrderID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSingleDepotToolByReader(_OrderID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static string GetTableRecGuidByPurchaseID(int _PurchaseID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSinglePurchaseOrderByReader(_PurchaseID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static string GetTableRecGuidBySendOutID(int _SendOutID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSingleSendOutByReader(_SendOutID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static DataSet GetToolOrderLstByDataSet(int _OrganID, string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.*"
                + ", b.OpName as OrderOpName, c.OrganName, d.OpName as ApprovalUserName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '审批中' when 2 then '审批通过' else '审批退回' end) As ApprovalStatusName"
                + ", (Case IsNull(OrderType,0) when 0 then '普通' else '特批' end) As OrderTypeName"
                + " from ToolOrder_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OrderUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.ApprovalUserID=d.ID"
                + " Where a.Status=0 " + sWhereSQL;
            if (_OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID=" + _OrganID.ToString();
            }
            sSQL = sSQL + " Order By a.OrderDate Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetToolOrderLstByReader(string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.*"
                + ", b.OpName as OrderOpName, c.OrganName, d.OpName as ApprovalUserName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '审批中' when 2 then '审批通过' else '审批退回' end) As ApprovalStatusName"
                + ", (Case IsNull(OrderType,0) when 0 then '普通' else '特批' end) As OrderTypeName"
                + " from ToolOrder_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OrderUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.ApprovalUserID=d.ID"
                + " Where a.Status=0 " + sWhereSQL;
            
            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetToolOrderExtInfoByReader(string sWhereSQL)
        {
            string sSQL = "Select a.*"
                + ", b.OpName as OrderOpName, c.OrganName, d.OpName as ApprovalUserName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '审批中' when 2 then '审批通过' else '审批退回' end) As ApprovalStatusName"
                + ", (Case IsNull(OrderType,0) when 0 then '普通' else '特批' end) As OrderTypeName"
                + ", IsNull(e.TotalCredit,0) As TotalCredit, IsNull(e.UsedTotalCredit,0) As UsedTotalCredit"
                + " from ToolOrder_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OrderUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.ApprovalUserID=d.ID"
                + " left join PurchaseCredit_Info e on a.OrganID=e.OrganID and Year(a.OrderDate)=e.PurchaseYear And e.Status=0"
                + " Where a.Status=0 " + sWhereSQL;

            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static Boolean GetBeyondTheLimitsByToolOrder(string sWhereSQL)
        {
            string sSQL = "Select 1"
                + " from ToolOrder_Info a "
                + " left join PurchaseCredit_Info b on a.OrganID=b.OrganID and Year(a.OrderDate)=b.PurchaseYear And b.Status=0"
                + " Where a.Status=0 and IsNull(b.UsedTotalCredit,0)+IsNull(a.TotalAmount,0) > IsNull(b.TotalCredit,0) " + sWhereSQL;

            sSQL = sSQL + " Order By a.SortID";

            return SysClass.SysGlobal.GetExecSqlIsExist(sSQL);
        }

        //删除工具申请单
        public static int DeleteSingleDepotTool(string _OrderIDs)
        {
            string sSQL = "begin Delete from ToolOrder_Info Where ID in (" + _OrderIDs.ToString() + "); ";
            sSQL = sSQL + " Delete from ToolOrderDetails_Info Where OrderID in (" + _OrderIDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static SqlDataReader GetSingleDepotToolByReader(int _OrderID)
        {
            string sSQL = "Select a.*"
                + ", b.OpName as OrderOpName, c.OrganName, d.OpName as ApprovalUserName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '审批中' when 2 then '审批通过' else '审批退回' end) As ApprovalStatusName"
                + ", (Case IsNull(OrderType,0) when 0 then '普通' else '特批' end) As OrderTypeName"
                + " from ToolOrder_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OrderUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.ApprovalUserID=d.ID"
                + " Where a.Status=0 and a.ID=" + _OrderID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }       

        public static Boolean CheckToolOrderNoExists(int OrderID, string OrderNo)
        {
            string sSqlText = "Select 1 From ToolOrder_Info Where  OrderNo='" + OrderNo + "' And ID<>" + OrderID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //更新添加工具档案信息
        public static int UpdateSingleToolOrder(int _ID, string[] FieldValues, string DetailsSQL)
        {
            string sSqlText = "begin";            

            if (_ID > 0)
            {
                sSqlText = sSqlText + " UPDATE ToolOrder_Info SET OrderNo='" + FieldValues.GetValue(1)
                     + "',OrderType=" + FieldValues.GetValue(2)
                     + ",Description='" + FieldValues.GetValue(3) + "'"
                     + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'"
                     + ",TotalAmount=" + FieldValues.GetValue(5);
                sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";
                
                sSqlText += " ;";
            }
            else
            {
                //sSqlText += " if not exists(Select OrganID, PurchaseYear From PurchaseCredit_Info Where OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString()
                //    + " And PurchaseYear=year(getdate()))"
                //    + " begin"
                //    + "   Insert Into PurchaseCredit_Info (OrganID, PurchaseYear) Values (" + SysGlobal.GetCurrentUserOrganID().ToString() + ", year(getdate()));"
                //    + " end;";

                sSqlText = sSqlText + " Insert Into ToolOrder_Info (TableRecGuid"
                    + ", OrderNo"
                    + ", OrderDate"
                    + ", OrderType"
                    + ", OrganID"
                    + ", OrderUserID"
                    + ", Description"
                    + ", ApprovalStatus"
                    + ", TotalAmount)"
                    + " Values('" + FieldValues.GetValue(0) + "'"
                    + ",'" + FieldValues.GetValue(1) + "'"
                    + ",GetDate()"
                    + "," + FieldValues.GetValue(2)
                    + "," + SysGlobal.GetCurrentUserOrganID().ToString()
                    + ",'" + SysGlobal.GetCurrentUserID().ToString() + "'"
                    + ",'" + FieldValues.GetValue(3) + "'"
                    + ",'" + FieldValues.GetValue(4) + "'"
                    + "," + FieldValues.GetValue(5) + ")";
                sSqlText = sSqlText + " ;";
            }
            sSqlText = sSqlText + DetailsSQL
               + " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static DataSet GetToolOrderDetailsLstByDataSet(int OrderID, string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification, b.MaterialCode,b.Unit  from ToolOrderDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " Where IsNull(a.OrderID,0)=" + OrderID.ToString() + " And a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static Boolean CheckPurchaseOrderNoExists(int PurchaseID, string OrderNo)
        {
            string sSqlText = "Select 1 From PurchaseOrder_Info Where  OrderNo='" + OrderNo + "' And ID<>" + PurchaseID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        public static Boolean CheckSendOutNoExists(int SendOutID, string SendOutNo)
        {
            string sSqlText = "Select 1 From SendOut_Info Where  SendOutNo='" + SendOutNo + "' And ID<>" + SendOutID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        public static int UpdateApprovalToolOrder(string MainTableName, string OrderIDs, int ApprovalStatus, string Description)
        {
            string sSqlText = "begin";

            sSqlText += " Insert Into ApprovalLog_Info (MainTableName, MainID, ApprovalStatus, ApprovalUserID, ApprovalTime, Description) "
                + " (Select '" + MainTableName + "', ID, " + ApprovalStatus.ToString() + "," + SysGlobal.GetCurrentUserID().ToString()
                + ", GetDate(), '" + Description + "' From ToolOrder_Info Where Status=0 And ID In (" + OrderIDs + ")); ";

            sSqlText += " Update ToolOrder_Info Set ApprovalStatus=" + ApprovalStatus.ToString()
                + ", ApprovalUserID=" + SysGlobal.GetCurrentUserID().ToString() + ", ApprovalTime=GetDate() "
                + " Where Status=0  And ID In (" + OrderIDs + ");";

            if (ApprovalStatus == DepotTool_ApprovalIsOK)
            {
                sSqlText += "Update PurchaseCredit_Info Set UsedTotalCredit=UsedTotalCredit+b.TotalAmount"
                    + ",UsedCredit1=IsNull(UsedCredit1,0)+(Case Month(b.OrderDate) when 1 then b.TotalAmount else 0 end)"
                    + ",UsedCredit2=IsNull(UsedCredit2,0)+(Case Month(b.OrderDate) when 2 then b.TotalAmount else 0 end)"
                    + ",UsedCredit3=IsNull(UsedCredit3,0)+(Case Month(b.OrderDate) when 3 then b.TotalAmount else 0 end)"
                    + ",UsedCredit4=IsNull(UsedCredit4,0)+(Case Month(b.OrderDate) when 4 then b.TotalAmount else 0 end)"
                    + ",UsedCredit5=IsNull(UsedCredit5,0)+(Case Month(b.OrderDate) when 5 then b.TotalAmount else 0 end)"
                    + ",UsedCredit6=IsNull(UsedCredit6,0)+(Case Month(b.OrderDate) when 6 then b.TotalAmount else 0 end)"
                    + ",UsedCredit7=IsNull(UsedCredit7,0)+(Case Month(b.OrderDate) when 7 then b.TotalAmount else 0 end)"
                    + ",UsedCredit8=IsNull(UsedCredit8,0)+(Case Month(b.OrderDate) when 8 then b.TotalAmount else 0 end)"
                    + ",UsedCredit9=IsNull(UsedCredit9,0)+(Case Month(b.OrderDate) when 9 then b.TotalAmount else 0 end)"
                    + ",UsedCredit10=IsNull(UsedCredit10,0)+(Case Month(b.OrderDate) when 10 then b.TotalAmount else 0 end)"
                    + ",UsedCredit11=IsNull(UsedCredit11,0)+(Case Month(b.OrderDate) when 11 then b.TotalAmount else 0 end)"
                    + ",UsedCredit12=IsNull(UsedCredit12,0)+(Case Month(b.OrderDate) when 12 then b.TotalAmount else 0 end)"
                    + " From ToolOrder_Info b Where b.Status=0 And OrderType=0 And b.ID In (" + OrderIDs + ")"
                    + " And PurchaseCredit_Info.OrganID=b.OrganID And PurchaseCredit_Info.PurchaseYear=Year(b.OrderDate);";
            }

            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText); 
        }

        public static SqlDataReader GetSinglePurchaseOrderByReader(int _PurchaseID)
        {
            string sSQL = "Select a.*"
                + ", b.OpName as OrderOpName, c.OrganName as SupplierName, d.OpName as ApprovalUserName, e.OrganName as OrderOrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '审批中' when 2 then '审批通过' else '审批退回' end) As ApprovalStatusName"
                + " from PurchaseOrder_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OrderUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.SupplierID=c.ID"
                + " left join SysOrgan_Info e on e.Status=0 And a.OrderOrganID=e.ID"
                + " left join SysUser_Info d on d.Status=0 And a.ApprovalUserID=d.ID"
                + " Where a.Status=0 and a.ID=" + _PurchaseID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSingleSendOutByReader(int _SendOutID)
        {
            string sSQL = "Select a.*"
                + ", b.OpName as SendOutOpName, c.OrganName as SendOutName, e.OrganName as AcceptOrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '已发货' end) As ApprovalStatusName"
                + " from SendOut_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.SendOutUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.SendOutUserID=c.ID"
                + " left join SysOrgan_Info e on e.Status=0 And a.AcceptOrganID=e.ID"
                + " Where a.Status=0 and a.ID=" + _SendOutID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        public static DataSet GetPurchaseDetailsLstByDataSet(int _PurchaseID, string WhereSQL)
        {
            string sSQL = "select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification"
                + " , b.MaterialCode, b.Unit, c.OrganName"
                + ", (Case ShipingStatus when 0 then '未发货' when 1 then '已发货' when 2 then '已入库' end) ShipingStatusName"
                + "  from PurchaseOrderDetails_Info a"
                + " left join Tool_Info b on b.Status=0 And a.ToolID=b.ID"
                + " left join SysOrgan_Info c on a.OrganID=c.ID"
                + " Where a.Status=0 " + WhereSQL;

            if (_PurchaseID > 0)
            {
                sSQL += " And IsNull(a.PurchaseID,0)=" + _PurchaseID.ToString();
            }

            sSQL = sSQL + " Order By b.ToolNo, c.OrganName";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetSendOutDetailsLstByDataSet(int _SendOutID, string WhereSQL)
        {
            string sSQL = "select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification"
                + " , b.MaterialCode, b.Unit, c.StorageLocation, IsNull(c.Quantity, 0) As StockQuantity, 0 As OldQuantity "
                + ", d.ToolCode, d.TestCode"
                + "  from SendOutDetails_Info a"
                + " left join Tool_Info b on b.Status=0 And a.ToolID=b.ID"
                + " left join ToolStock_Info c on c.Status=0 And c.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + " And b.ID=c.ToolID"
                + " left join ToolStockDetail_Info d on d.Status=0 And d.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + " And d.ID=a.ToolDetailID"
                + " Where a.Status=0 " + WhereSQL;

            if (_SendOutID > 0)
            {
                sSQL += " And IsNull(a.SendOutID,0)=" + _SendOutID.ToString();
            }

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetPurchaseDetailsLstByReader(string _UpOrderIDs)
        {
            
                string sSQL = "Select b.ToolID, c.ToolNo, c.ToolName, c.AliasesName"
                  + ", c.Specification, c.MaterialCode, c.Unit, Sum(IsNull(b.Quantity,0)) as Quantity"
                  + ", Sum(IsNull(b.Amount,0))/Sum(IsNull(b.Quantity,0)) as UnitPrice"
                  + ", Sum(IsNull(b.Amount,0)) as Amount, a.OrganID, d.OrganName"
                  + " from ToolOrder_Info a "
                  + " left join SysOrgan_Info d on d.Status=0 And a.OrganID=d.ID"
                  + ", ToolOrderDetails_Info b"
                  + " left join Tool_Info c on c.Status=0 and b.ToolID=c.ID"
                  + " Where a.ID in (" + _UpOrderIDs + ") And a.Status=0 And b.Status=0 And a.ID=b.OrderID";

                sSQL = sSQL + " group by b.ToolID, c.ToolNo, c.ToolName, c.AliasesName "
                    + ", c.Specification, c.MaterialCode, c.Unit,a.OrganID, d.OrganName"
                    + " Order By c.ToolNo, d.OrganName";                

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSendOutDetailsLstByOrderIDs(string _UpOrderIDs)
        {

            string sSQL = "Select a.ToolID, b.ToolNo, b.ToolName, b.AliasesName"
                + " , c.StorageLocation, IsNull(c.Quantity, 0) As StockQuantity"
                + " , 0 As OldQuantity, b.Specification, b.MaterialCode, b.Unit, a.Quantity"
                + " from ToolOrderDetails_Info a"
                + " left join Tool_Info b on b.Status=0 and a.ToolID=b.ID "
                + " left join ToolStock_Info c on c.Status=0 And c.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + " And b.ID=c.ToolID "
                + " Where a.OrderID in (" + _UpOrderIDs + ") And a.Status=0 And IsNull(b.IsCode,0)=0"
                + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSendOutDetailsLstByReader(string WhereSQL)
        {

            string sSQL = "Select a.SendOutID, a.ToolID, b.ToolNo, b.ToolName, b.AliasesName"
                + " , c.StorageLocation, IsNull(c.Quantity, 0) As StockQuantity"
                + " , 0 As OldQuantity, b.Specification, b.MaterialCode, b.Unit, a.Quantity"
                + " from SendOutDetails_Info a"
                + " left join Tool_Info b on b.Status=0 and a.ToolID=b.ID "
                + " left join ToolStock_Info c on c.Status=0 And c.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + " And b.ID=c.ToolID "
                + " Where a.Status=0 " + WhereSQL
                + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static DataSet GetPurchaseDetailsLstByDataSet(string _OrderIDs)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification, b.MaterialCode,b.Unit, d.OrganName as WorkshopName "
                + " from ToolOrderDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And a.ToolID=b.ID"
                + " left join ToolOrder_Info c on c.Status=0 And c.ID=a.OrderID"
                + " Left join SysOrgan_Info d on d.Status=0 And d.ID=c.OrganID"
                + " Where IsNull(a.OrderID,0) in (" + _OrderIDs.ToString() + ") And a.Status=0";

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //更新添加工具档案信息
        public static int UpdateSinglePurchaseOrder(int _ID, string[] FieldValues, string DetailsSQL)
        {
            string sSqlText = "begin";
            if (_ID > 0)
            {
                sSqlText = sSqlText + " UPDATE PurchaseOrder_Info SET OrderNo='" + FieldValues.GetValue(1)
                     + "',SupplierID=" + FieldValues.GetValue(2)
                     + ",Description='" + FieldValues.GetValue(3) + "'"
                     + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'"
                     + ",TotalAmount=" + FieldValues.GetValue(5);
                sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";

                sSqlText += " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into PurchaseOrder_Info (TableRecGuid"
                    + ", OrderNo"
                    + ", OrderDate"
                    + ", SupplierID"
                    + ", OrderOrganID"
                    + ", OrderUserID"
                    + ", Description"
                    + ", ApprovalStatus"
                    + ", TotalAmount)"
                    + " Values('" + FieldValues.GetValue(0) + "'"
                    + ",'" + FieldValues.GetValue(1) + "'"
                    + ",GetDate()"
                    + "," + FieldValues.GetValue(2)
                    + "," + SysGlobal.GetCurrentUserOrganID().ToString()
                    + ",'" + SysGlobal.GetCurrentUserID().ToString() + "'"
                    + ",'" + FieldValues.GetValue(3) + "'"
                    + ",'" + FieldValues.GetValue(4) + "'"
                    + "," + FieldValues.GetValue(5) + ")";
                sSqlText += " ;";
            }
            sSqlText = sSqlText + DetailsSQL
               + " end;";
            return DataCommon.QueryData(sSqlText);
        }

        //更新添加工具档案信息
        public static int UpdateSingleSendOut(int _ID, string[] FieldValues, string DetailsSQL)
        {
            string sSqlText = "begin";

            sSqlText += " Update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity + b.Quantity "
                + ", ToolStock_Info.SendOutQuantity=ToolStock_Info.SendOutQuantity - b.Quantity"
              + " From (Select ToolID, Sum(Quantity) As Quantity From SendOutDetails_Info a, SendOut_Info b where a.SendOutID=b.Id  "
              + " And b.TableRecGuid='" + FieldValues.GetValue(0) + "' and b.ApprovalStatus=1 And b.Status=0 Group By ToolID) b "
              + " Where ToolStock_Info.ToolID=b.ToolID And ToolStock_Info.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + ";";

            if (_ID > 0)
            {
                sSqlText = sSqlText + " UPDATE SendOut_Info SET SendOutNo='" + FieldValues.GetValue(1) + "'"
                     + ",Description='" + FieldValues.GetValue(3) + "'"
                     + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";

                sSqlText += " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into SendOut_Info (TableRecGuid"
                    + ", SendOutNo"
                    + ", SendOutDate"
                    + ", SendOutOrganID"
                    + ", AcceptOrganID"
                    + ", SendOutUserID"
                    + ", Description"
                    + ", ApprovalStatus)"
                    + " Values('" + FieldValues.GetValue(0) + "'"
                    + ",'" + FieldValues.GetValue(1) + "'"
                    + ",GetDate()"
                    + "," + SysGlobal.GetCurrentUserOrganID().ToString()
                    + "," + FieldValues.GetValue(2)
                    + ",'" + SysGlobal.GetCurrentUserID().ToString() + "'"
                    + ",'" + FieldValues.GetValue(3) + "'"
                    + ",'" + FieldValues.GetValue(4) + "')";
                sSqlText += " ;";
            }
            sSqlText = sSqlText + DetailsSQL;

            sSqlText += " Insert Into ToolStock_Info (OrganID, ToolID, Quantity)"
                + " (Select distinct " + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + ", ToolID, 0 From SendOutDetails_Info Where Status=0 "
                + " And SendOutID In (Select ID From SendOut_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "')"
                + " And ToolID not in (Select ToolID From ToolStock_Info Where OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + "));";

            sSqlText += " Update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity - b.Quantity "
                + ", ToolStock_Info.SendOutQuantity=ToolStock_Info.SendOutQuantity + b.Quantity"
               + " From (Select ToolID, Sum(Quantity) As Quantity From SendOutDetails_Info a, SendOut_Info b where a.SendOutID=b.Id  "
               + " And b.TableRecGuid='" + FieldValues.GetValue(0) + "'and b.ApprovalStatus=1 And b.Status=0 Group By ToolID) b "
               + " Where ToolStock_Info.ToolID=b.ToolID And ToolStock_Info.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + ";";

            sSqlText += " Update ToolStockDetail_Info Set ToolStockDetail_Info.ToolStatus=7 From "
                + " SendOutDetails_Info b where b.SendOutID In (Select ID From SendOut_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "') and ToolStockDetail_Info.ID=b.ToolDetailID"
                + " And ToolStockDetail_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID() + ";";

               sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static DataSet GetPurchaseOrderLstByDataSet(string sWhereSQL)
        {
            string sSQL = "Select a.*"
                + ", b.OpName as OrderOpName, c.OrganName as SupplierName, d.OpName as ApprovalUserName, e.OrganName as OrderOrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '订单提交' when 2 then '订单退回' end) As ApprovalStatusName"
                + " from PurchaseOrder_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OrderUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.SupplierID=c.ID"
                + " left join SysOrgan_Info e on e.Status=0 And a.OrderOrganID=e.ID"
                + " left join SysUser_Info d on d.Status=0 And a.ApprovalUserID=d.ID"
                + " Where a.Status=0 " + sWhereSQL;

            sSQL = sSQL + " Order By a.OrderDate Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetSendOutLstByDataSet(string sWhereSQL)
        {
            string sSQL = "Select a.*"
                + ", b.OpName as SendOutOpName, c.OrganName as SendOutName, d.OpName as SendOutUserName, e.OrganName as AcceptOrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '已发货'  when 2 then '已入库' end) As ApprovalStatusName"
                + " from SendOut_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.SendOutUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.SendOutUserID=c.ID"
                + " left join SysOrgan_Info e on e.Status=0 And a.AcceptOrganID=e.ID"
                + " left join SysUser_Info d on d.Status=0 And a.SendOutUserID=d.ID"
                + " Where a.Status=0 " + sWhereSQL;

            sSQL = sSQL + " Order By a.SendOutDate Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetSendOutLstByReader(string sWhereSQL)
        {
            string sSQL = "Select a.*"
                + ", b.OpName as SendOutOpName, c.OrganName as SendOutName, d.OpName as SendOutUserName, e.OrganName as AcceptOrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '已发货' end) As ApprovalStatusName"
                + " from SendOut_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.SendOutUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.SendOutUserID=c.ID"
                + " left join SysOrgan_Info e on e.Status=0 And a.AcceptOrganID=e.ID"
                + " left join SysUser_Info d on d.Status=0 And a.SendOutUserID=d.ID"
                + " Where a.Status=0 " + sWhereSQL;

            sSQL = sSQL + " Order By a.SendOutDate Desc";

            return DataCommon.GetDataByReader(sSQL);
        }

        //删除工具订单
        public static int DeleteSinglePurchaseOrder(string _PurchaseIDs)
        {
            string sSQL = "begin Delete from PurchaseOrder_Info Where ID in (" + _PurchaseIDs.ToString() + "); ";
            sSQL = sSQL + " Delete from PurchaseOrderDetails_Info Where PurchaseID in (" + _PurchaseIDs.ToString() + "); ";
            sSQL = sSQL + " Update ToolOrder_Info Set PurchaseID=0  Where PurchaseID in (" + _PurchaseIDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        //删除工具订单
        public static int DeleteSingleSendOut(string _SendOutIDs)
        {
            string sSQL = "begin Delete from SendOut_Info Where ID in (" + _SendOutIDs.ToString() + "); ";
            sSQL = sSQL + " Delete from SendOutDetails_Info Where SendOutID in (" + _SendOutIDs.ToString() + "); ";
            sSQL = sSQL + " Update ToolOrder_Info Set SendOutID=0  Where SendOutID in (" + _SendOutIDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }


        public static int UpdateSendOutStorage(string MainTableName, string SendOutIDs, int ShipingStatus, string Description)
        {
            string sSqlText = "begin";

            string sWhereSQL = "And IsNull(a.SendOutID,0) in (" + SendOutIDs + ") And IsNull(b.IsCode,0)=1";

            SqlDataReader sdr = GetSendOutDetailsLstByReader(sWhereSQL);
            while (sdr.Read())
            {
                int _ToolID = int.Parse(sdr["ToolID"].ToString());
                int _Quantity = int.Parse(sdr["Quantity"].ToString());
                if (SysTool.GetIsCodeByToolID(_ToolID))
                {
                    string _ToolNo = SysTool.GetToolNoByToolID(_ToolID);
                    int _ToolMax = SysTool.GetMaxNumByToolID(_ToolID);
                    for (int i = 1; i <= _Quantity; i++)
                    {
                        sSqlText += " Insert Into ToolStockDetail_Info (OrganID, SendOutID, ToolID, ToolCode) Values(" + SysGlobal.GetCurrentUserOrganID().ToString()
                           + "," + sdr["SendOutID"].ToString() + "," + _ToolID.ToString() + ",'" + SysTool.GetToolCodeByToolNo(_ToolNo, _ToolMax + i) + "');";
                    }
                    sSqlText += SysTool.GetUpdateAddNumSQL(_ToolID, _Quantity);
                }
            }
            sdr.Close();

            sSqlText += " Insert Into ApprovalLog_Info (MainTableName, MainID, ApprovalStatus, ApprovalUserID, ApprovalTime, Description) "
                + " (Select '" + MainTableName + "', ID, " + ShipingStatus.ToString() + "," + SysGlobal.GetCurrentUserID().ToString()
                + ", GetDate(), '" + Description + "' From SendOut_Info Where Status=0 And ApprovalStatus=1 And ID In (" + SendOutIDs + ")); ";

            sSqlText += " Update SendOut_Info Set ApprovalStatus=" + ShipingStatus.ToString()
                + " Where Status=0 And ApprovalStatus=1 And ID In (" + SendOutIDs + ");";            

            sSqlText += " Insert Into ToolStock_Info (OrganID, ToolID, Quantity)"
                + " (Select distinct " + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + ", ToolID, 0 From SendOutDetails_Info Where Status=0 "
                + " And SendOutID In (" + SendOutIDs + ")"
                + " And ToolID not in (Select ToolID From ToolStock_Info Where OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + ") Group By ToolID);";

            sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity+b.Quantity "
                + " From (Select ToolID, Sum(Quantity) As Quantity From SendOutDetails_Info where SendOutID In (" + SendOutIDs + ") Group By ToolID) b Where ToolStock_Info.ToolID=b.ToolID"
                + " And ToolStock_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID() + ";";


            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }
    }
}
