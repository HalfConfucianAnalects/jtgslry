using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CyxPack.CommonOperation;
using CyxPack.OperateSqlServer;
using System.Data.SqlClient;

namespace JtgTMS.SysClass
{
    public class SysDelivery
    {
        public static string Delivery_TableName = "Delivery_Info";
        public static int Delivery_Draft = 0, Delivery_Shipped = 1, Delivery_InStorage = 2;

        //工具配送入库
        public static string NoStorage_SearchText = "";

        //已入库工具配送单
        public static string InStorage_SearchText = "";

        public static string GetTableRecGuidByDeliveryID(int _DeliveryID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSingleDeliveryByReader(_DeliveryID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static DataSet GetDeliveryDetailsLstByDataSet(int _DeliveryID, string WhereSQL)
        {
            string sSQL = "select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification"
                + " , b.MaterialCode, b.Unit"
                + "  from DeliveryDetails_Info a"
                + " left join Tool_Info b on b.Status=0 And a.ToolID=b.ID"
                + " Where a.Status=0 " + WhereSQL;

            if (_DeliveryID > 0)
            {
                sSQL += " And IsNull(a.DeliveryID,0)=" + _DeliveryID.ToString();
            }

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetDeliveryDetailsLstByReader(string WhereSQL)
        {
            string sSQL = "select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification"
                + " , b.MaterialCode, b.Unit"
                + "  from DeliveryDetails_Info a"
                + " left join Tool_Info b on b.Status=0 And a.ToolID=b.ID"
                + " Where a.Status=0 " + WhereSQL;

            //if (_DeliveryID > 0)
            //{
            //    sSQL += " And IsNull(a.DeliveryID,0)=" + _DeliveryID.ToString();
            //}

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        //更新添加工具档案信息
        public static int UpdateSingleDeliveryOrder(int _ID, string[] FieldValues, string DetailsSQL)
        {
            string sSqlText = "begin";
            if (_ID > 0)
            {
                sSqlText = sSqlText + " UPDATE Delivery_Info SET DeliveryNo='" + FieldValues.GetValue(1) + "'"
                     + ",Description='" + FieldValues.GetValue(3) + "'"
                     + ",ShipingStatus=" + FieldValues.GetValue(4) + ""
                     + ",TotalAmount=" + FieldValues.GetValue(5);
                sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";

                sSqlText += " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into Delivery_Info (TableRecGuid"
                    + ", DeliveryNo"
                    + ", DeliveryDate"
                    + ", SupplierID"
                    + ", DeliveryOrganID"
                    + ", DeliveryUserID"
                    + ", Description"
                    + ", ShipingStatus"
                    + ", TotalAmount)"
                    + " Values('" + FieldValues.GetValue(0) + "'"
                    + ",'" + FieldValues.GetValue(1) + "'"
                    + ",GetDate()"
                    + "," + SysGlobal.GetCurrentUserOrganID().ToString()
                    + "," + FieldValues.GetValue(2)
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

        public static Boolean CheckDeliveryNoExists(int DeliveryID, string DeliveryNo)
        {
            string sSqlText = "Select 1 From Delivery_Info Where DeliveryNo='" + DeliveryNo + "' And ID<>" + DeliveryID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        public static SqlDataReader GetNotSendPurchaseDetailsByReader(int _SupplierID, string WhereSQL)
        {
            string sSQL = "select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification"
                + " , b.MaterialCode, b.Unit, c.OrganName, d.OrderDate, d.OrderNo"
                + "  from PurchaseOrderDetails_Info a"
                + " left join Tool_Info b on b.Status=0 And a.ToolID=b.ID"
                + " left join SysOrgan_Info c on a.OrganID=c.ID"
                + " left join PurchaseOrder_Info d on a.PurchaseID=d.ID"
                + " Where a.Status=0 And a.DeliveryDetailID=0" + WhereSQL;

            if (_SupplierID > 0)
            {
                sSQL += " And IsNull(d.SupplierID,0)=" + _SupplierID.ToString();
            }

            sSQL = sSQL + " Order By b.ToolNo, c.OrganName";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetNotSendPurchaseDetailsByReader(string _UpOrderDetailIDs)
        {
            string sSQL = "select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification"
                + " , b.MaterialCode, b.Unit, c.OrganName, d.OrderDate, d.OrderNo"
                + "  from PurchaseOrderDetails_Info a"
                + " left join Tool_Info b on b.Status=0 And a.ToolID=b.ID"
                + " left join SysOrgan_Info c on a.OrganID=c.ID"
                + " left join PurchaseOrder_Info d on a.PurchaseID=d.ID"
                + " Where a.Status=0 And a.ShipingStatus=0";

            if (_UpOrderDetailIDs.Length > 0)
            {
                sSQL += " And IsNull(a.ID,0) in (" + _UpOrderDetailIDs.ToString() + ')';
            }

            sSQL = sSQL + " Order By b.ToolNo, c.OrganName";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSingleDeliveryByReader(int _DeliveryID)
        {
            string sSQL = "Select a.*"
                  + ", b.OpName as DeliveryOpName, c.OrganName as SupplierName, d.OrganName as DeliveryOrganName"
                  + ", (Case IsNull(ShipingStatus,0) when 0 then '草稿' when 1 then '已发货' when 2 then '已入库' end) As ShipingStatusName"
                  + " from Delivery_Info a "
                  + " left join SysUser_Info b on b.Status=0 And a.DeliveryUserID=b.ID"
                  + " left join SysOrgan_Info c on c.Status=0 And a.SupplierID=c.ID"
                  + " left join SysOrgan_Info d on d.Status=0 And a.DeliveryOrganID=d.ID"
                  + " Where a.Status=0 and a.ID=" + _DeliveryID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        public static DataSet GetDeliveryLstByDataSet(string sWhereSQL)
        {
            string sSQL = "Select a.*"
                  + ", b.OpName as DeliveryOpName, c.OrganName as SupplierName, d.OrganName as DeliveryOrganName"
                  + ", (Case IsNull(ShipingStatus,0) when 0 then '草稿' when 1 then '已发货' when 2 then '已入库' end) As ShipingStatusName"
                  + " from Delivery_Info a "
                  + " left join SysUser_Info b on b.Status=0 And a.DeliveryUserID=b.ID"
                  + " left join SysOrgan_Info c on c.Status=0 And a.SupplierID=c.ID"
                  + " left join SysOrgan_Info d on d.Status=0 And a.DeliveryOrganID=d.ID"
                  + " Where a.Status=0 "  + sWhereSQL;

            sSQL = sSQL + " Order By a.DeliveryDate Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetDeliveryLstByReader(string sWhereSQL)
        {
            string sSQL = "Select a.*"
                  + ", b.OpName as DeliveryOpName, c.OrganName as SupplierName, d.OrganName as DeliveryOrganName"
                  + ", (Case IsNull(ShipingStatus,0) when 0 then '草稿' when 1 then '已发货' when 2 then '已入库' end) As ShipingStatusName"
                  + " from Delivery_Info a "
                  + " left join SysUser_Info b on b.Status=0 And a.DeliveryUserID=b.ID"
                  + " left join SysOrgan_Info c on c.Status=0 And a.SupplierID=c.ID"
                  + " left join SysOrgan_Info d on d.Status=0 And a.DeliveryOrganID=d.ID"
                  + " Where a.Status=0 " + sWhereSQL;

            sSQL = sSQL + " Order By a.DeliveryDate Desc";

            return DataCommon.GetDataByReader(sSQL);
        }

        //删除工具订单
        public static int DeleteSingleDelivery(string _DeliveryIDs)
        {
            string sSQL = "begin Delete from Delivery_Info Where ID in (" + _DeliveryIDs.ToString() + "); ";
            sSQL = sSQL + " Update PurchaseOrderDetails_Info Set DeliveryDetailID=0, ShipingStatus=0"
                + " Where DeliveryDetailID in (Select ID From DeliveryDetails_Info Where DeliveryID in (" + _DeliveryIDs.ToString() + ")) And DeliveryDetailID>0; ";
            sSQL = sSQL + " Delete from DeliveryDetails_Info Where DeliveryID in (" + _DeliveryIDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static int UpdateDeliveryShiping(string MainTableName, string DeliveryIDs, int ShipingStatus, string Description)
        {
            string sSqlText = "begin";

            string sWhereSQL = "And IsNull(a.DeliveryID,0) in (" + DeliveryIDs + ") And IsNull(b.IsCode,0)=1";

            SqlDataReader sdr = GetDeliveryDetailsLstByReader(sWhereSQL);
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
                        sSqlText += " Insert Into ToolStockDetail_Info (OrganID, DeliveryID, ToolID, ToolCode) Values(" + SysGlobal.GetCurrentUserOrganID().ToString()
                           + "," + sdr["DeliveryID"].ToString() + "," + _ToolID.ToString() + ",'" + SysTool.GetToolCodeByToolNo(_ToolNo, _ToolMax + i) + "');";
                    }
                    sSqlText += SysTool.GetUpdateAddNumSQL(_ToolID, _Quantity);
                }
            }
            sdr.Close();

            sSqlText += " Insert Into ApprovalLog_Info (MainTableName, MainID, ApprovalStatus, ApprovalUserID, ApprovalTime, Description) "
                + " (Select '" + MainTableName + "', ID, " + ShipingStatus.ToString() + "," + SysGlobal.GetCurrentUserID().ToString()
                + ", GetDate(), '" + Description + "' From Delivery_Info Where Status=0 And ShipingStatus=1 And ID In (" + DeliveryIDs + ")); ";

            sSqlText += " Update Delivery_Info Set ShipingStatus=" + ShipingStatus.ToString()
                + " Where Status=0 And ShipingStatus=1 And ID In (" + DeliveryIDs + ");";

            sSqlText += " Update PurchaseOrderDetails_Info Set DeliveryDetailID=DeliveryDetails_Info.ID, ShipingStatus=" + ShipingStatus.ToString()
                    + " From DeliveryDetails_Info Where DeliveryDetails_Info.DeliveryID in (Select ID From Delivery_Info Where ID in (" + DeliveryIDs + ")) "
                    + " And PurchaseOrderDetails_Info.ToolID=DeliveryDetails_Info.ToolID"
                    + " And PurchaseOrderDetails_Info.DeliveryDetailID>0;";

            sSqlText += " Insert Into ToolStock_Info (OrganID, ToolID, Quantity)"
                + " (Select " + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + ", ToolID, 0 From DeliveryDetails_Info Where Status=0 "
                + " And DeliveryID In (" + DeliveryIDs + ")"
                + " And ToolID not in (Select ToolID From ToolStock_Info Where OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + ") Group By ToolID);";

            sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity+b.Quantity "
                + " From (Select ToolID, Sum(Quantity) As Quantity From DeliveryDetails_Info where DeliveryID In (" + DeliveryIDs + ") Group By ToolID) b Where ToolStock_Info.ToolID=b.ToolID"
                + " And ToolStock_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID() + ";";


            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }
    }
}
