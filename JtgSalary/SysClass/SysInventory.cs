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
    public class SysInventory
    {
        //历史工具盘点
        public static string Inventory_SearchText = "";
        public static string Inventory_ApprovalStatus = "";

        public static string Inventory_TableName = "Inventory_Info";
        public static bool HideGreaterZero_Flag = false;
        public static int Inventory_Draft = 0, Inventory_ApprovalIsOK = 1;
        public static string GetTableRecGuidByID(int _InventoryID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSingleInventoryByReader(_InventoryID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static DataSet GetInventoryLstByDataSet(int _OrganID, string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.*"
                + ", b.OpName as InventoryOpName, c.OrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from Inventory_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.InventoryUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " Where a.Status=0 " + sWhereSQL;
            if (_OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID=" + _OrganID.ToString();
            }
            sSQL = sSQL + " Order By a.InventoryDate Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetInventoryDetailsLstByDataSet(int InventoryID, int CategoryID, string WhereSQL)
        {
            string sSQL = "Select a.*, IsNull(b.Quantity, 0) As Quantity, IsNull(b.BookQuantity,0) As BookQuantity from Tool_Info a "
                + " Left Join InventoryDetails_Info b on b.Status=0 And a.ID=b.ToolID";
            if (InventoryID > 0)
            {
                sSQL += " And b.InventoryID=" + InventoryID.ToString();
            }
            sSQL += " Where a.Status=0" + WhereSQL;
            if (CategoryID > 0)
            {
                sSQL = sSQL + " And a.CategoryID=" + CategoryID.ToString();
            }  

            sSQL = sSQL + " Order By a.SortID, a.ToolNo, a.ToolName";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetInventoryDetailsLstByReader(int InventoryID, string WhereSQL)
        {
            string sSQL = "Select a.*, IsNull(b.Quantity, 0) As Quantity, IsNull(b.BookQuantity,0) As BookQuantity from Tool_Info a "
                + " Left Join InventoryDetails_Info b on b.Status=0 And a.ID=b.ToolID";
            if (InventoryID > 0)
            {
                sSQL += " And b.InventoryID=" + InventoryID.ToString();
            }

            sSQL += " Where a.Status=0" + WhereSQL;            
            sSQL = sSQL + " Order By a.SortID, a.ToolNo, a.ToolName";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static Boolean CheckInventoryNoExists(int InventoryID, string InventoryNo)
        {
            string sSqlText = "Select 1 From Inventory_Info "
                + " Where InventoryNo='" + InventoryNo + "' And ID<>" + InventoryID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //更新添加工具档案信息
        public static int UpdateSingleInventory(int _InventoryID, string[] FieldValues, string DetailsSQL)
        {
            string sSqlText = "begin";
            if (_InventoryID > 0)
            {
                sSqlText = sSqlText + " UPDATE Inventory_Info SET InventoryNo='" + FieldValues.GetValue(1) + "'"
                     + ",InventoryUserID=" + FieldValues.GetValue(2) + ""
                     + ",Description='" + FieldValues.GetValue(3) + "'"
                     + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _InventoryID + "" + ";";

                sSqlText += " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into Inventory_Info (TableRecGuid"
                    + ", InventoryNo"
                    + ", InventoryDate"
                    + ", OrganID"
                    + ", InventoryUserID"
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
            sSqlText = sSqlText + DetailsSQL
               + " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static SqlDataReader GetSingleInventoryByReader(int _InventoryID)
        {
            string sSQL = "Select a.*"
                + ", b.OpCode as InventoryOpCode,  b.OpName as InventoryOpName, c.OrganName, d.OpName as CreateOpName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from Inventory_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.InventoryUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                + " Where a.Status=0 And a.ID=" + _InventoryID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //删除工具申请单
        public static int DeleteSingleInventory(string _InventoryIDs)
        {
            string sSQL = "begin Delete from Inventory_Info Where ID in (" + _InventoryIDs.ToString() + "); ";
            sSQL = sSQL + " Delete from InventoryDetails_Info Where InventoryID in (" + _InventoryIDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static int UpdateDetailsQuantity(int _InventoryID, string _ToolIDs, double _Quantity, string Description)
        {
            string sSqlText = "begin";

            sSqlText += " Insert Into InventoryDetails_Info (InventoryID, ToolID, BookQuantity, Quantity) "
                + " (Select " + _InventoryID.ToString() + ", ID, 0, 0"
                + " From Tool_Info Where Status=0 And ID In (" + _ToolIDs + ")"
                + "  And ID not in (Select ToolID From InventoryDetails_Info Where Status=0 And InventoryID=" + _InventoryID.ToString() + ")); ";

            sSqlText += " Update InventoryDetails_Info Set Quantity=" + _Quantity.ToString()
                + " Where Status=0 And InventoryID=" + _InventoryID.ToString() + " And ToolID In (" + _ToolIDs + ");";

            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }
    }
}
