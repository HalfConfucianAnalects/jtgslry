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
    public class SysToolOut
    {
        //调拨出库确认
        public static string ToolOutApproval_SearchText = "";

        //工具调拨出库单查询
        public static string ToolOut_ToolOutOpCode = "", ToolOut_ToolOutOpName = "", ToolOut_ToolOutUserID = "";
        public static string ToolOut_SearchText = "";
        public static string ToolOut_ApprovalStatus = "";

        public static int ToolOut_Draft = 0, ToolOut_ApprovalIsOK = 1, ToolOutType_ToolOutValue = 0;

        public static string Sonsume_TableName = "ToolOut_Info", Borrow_TableName = "Borrow_Info";

        public static string GetTableRecGuidByID(int _ToolOutID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSingleToolOutByReader(_ToolOutID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static DataSet GetToolOutLstByDataSet(int _OrganID, string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.*"
                + ", b.OpName as ToolOutOpName, c.OrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from ToolOut_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.ToolOutUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " Where a.Status=0 And ToolOutType=" + ToolOutType_ToolOutValue.ToString() + sWhereSQL;
            if (_OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID=" + _OrganID.ToString();
            }
            sSQL = sSQL + " Order By a.ToolOutDate Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetToolOutLstByReader(int _OrganID, string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.*"
                + ", b.OpName as ToolOutOpName, c.OrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from ToolOut_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.ToolOutUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " Where a.Status=0 And ToolOutType=" + ToolOutType_ToolOutValue.ToString() + sWhereSQL;
            if (_OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID=" + _OrganID.ToString();
            }
            sSQL = sSQL + " Order By a.ToolOutDate Desc";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static DataSet GetToolOutDetailsLstByDataSet(int ToolOutID, string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName,"
                + " b.Specification, b.MaterialCode,b.Unit, IsNull(c.Quantity,0) As StockQuantity, a.Quantity As OldQuantity "
                + ", c.StorageLocation, d.ToolCode, d.TestCode"
                + " from ToolOutDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " left Join ToolStock_Info c on c.Status=0 And c.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + " and c.ToolID=a.ToolID"
                + " left join ToolStockDetail_Info d on d.Status=0 And a.ToolDetailID=d.ID"
                + " Where IsNull(a.ToolOutID,0)=" + ToolOutID.ToString() 
                + " And a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetToolOutDetailsLstByReader(int ToolOutID, string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification, b.MaterialCode,b.Unit  from ToolOutDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " Where a.Status=0" + WhereSQL;

            if (ToolOutID > 0)
            {
                sSQL += " And IsNull(a.ToolOutID,0)=" + ToolOutID.ToString() ;
            }

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetToolOutDetailsLstByReader(string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName"
                + " , b.Specification, b.MaterialCode,b.Unit, c.ToolOutDate"
                + ", (Case c.ToolOutType when 0 then '调拨' when 1 then '借用' end) As ToolOutTypeName"
                + ", d.ToolCode"
                + " from ToolOutDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " left join ToolOut_Info c on c.Status=0 And c.ID=a.ToolOutID"
                + " left join ToolStockDetail_Info d on d.ID=a.ToolDetailID"
                + " Where a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static Boolean CheckToolOutNoExists(int ToolOutID, string ToolOutNo)
        {
            string sSqlText = "Select 1 From ToolOut_Info Where ToolOutType=" + ToolOutType_ToolOutValue.ToString() 
                + " And ToolOutNo='" + ToolOutNo + "' And ID<>" + ToolOutID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //更新添加工具档案信息
        public static int UpdateSingleToolOut(int _ToolOutID, string[] FieldValues, string DetailsSQL)
        {
            string sSqlText = "begin";

            //先减去旧数据的库存
            //sSqlText += " Update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity + b.Quantity "
            //    + ", ToolStock_Info.ToolOutQuantity=ToolStock_Info.ToolOutQuantity - b.Quantity"
            //    + " From ( ToolOutDetails_Info b where b.ToolOutID In (Select ID From ToolOut_Info Where Status=0 "
            //    + " And TableRecGuid='" + FieldValues.GetValue(0) + "' and IsNull(ApprovalStatus,0)=1)) b and ToolStock_Info.ToolID=b.ToolID;";

            sSqlText += " Update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity + b.Quantity "
               + ", ToolStock_Info.ToolOutQuantity=ToolStock_Info.ToolOutQuantity - b.Quantity"
               + " From (Select ToolID, Sum(Quantity) As Quantity From ToolOutDetails_Info a, ToolOut_Info b where a.ToolOutID=b.Id  "
               + " And b.TableRecGuid='" + FieldValues.GetValue(0) + "' and b.ApprovalStatus=1 And b.Status=0 Group By ToolID) b "
               + " Where ToolStock_Info.ToolID=b.ToolID And ToolStock_Info.OrganID="+SysGlobal.GetCurrentUserOrganID().ToString()+";";

            if (_ToolOutID > 0)
            {
                sSqlText = sSqlText + " UPDATE ToolOut_Info SET ToolOutNo='" + FieldValues.GetValue(1) + "'"
                    + ",AcceptOrganID=" + FieldValues.GetValue(2) + ""
                     + ",ToolOutUserID=" + FieldValues.GetValue(3) + ""
                     + ",Description='" + FieldValues.GetValue(4) + "'"
                     + ",ApprovalStatus='" + FieldValues.GetValue(5) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _ToolOutID + "" + ";";

                sSqlText += " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into ToolOut_Info (TableRecGuid"
                    + ", ToolOutNo"
                    + ", ToolOutType"
                    + ", ToolOutDate"
                    + ", OrganID"
                    + ", AcceptOrganID"
                    + ", ToolOutUserID"
                    + ", CreateUserID"
                    + ", Description"
                    + ", ApprovalStatus)"
                    + " Values('" + FieldValues.GetValue(0) + "'"
                    + ",'" + FieldValues.GetValue(1) + "'"
                    + "," + ToolOutType_ToolOutValue.ToString()
                    + ",GetDate()"
                    + "," + SysGlobal.GetCurrentUserOrganID().ToString()
                    + "," + FieldValues.GetValue(2) + ""
                    + "," + FieldValues.GetValue(3) + ""
                    + "," + SysGlobal.GetCurrentUserID().ToString() + ""
                    + ",'" + FieldValues.GetValue(4) + "'"
                    + "," + FieldValues.GetValue(5) + ")";
                sSqlText = sSqlText + " ;";
            }
            sSqlText = sSqlText + DetailsSQL;

            sSqlText += " Insert Into ToolStock_Info (OrganID, ToolID, Quantity)"
                + " (Select distinct " + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + ", ToolID, 0 From ToolOutDetails_Info Where Status=0 "
                + " And ToolOutID In (Select ID From ToolOut_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "')"
                + " And ToolID not in (Select ToolID From ToolStock_Info Where OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + "));";

            //sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity-b.Quantity "
            //    + ", ToolStock_Info.ToolOutQuantity=ToolStock_Info.ToolOutQuantity + b.Quantity"
            //    + " From ToolOutDetails_Info b where b.ToolOutID In (Select ID From ToolOut_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "') and ToolStock_Info.ToolID=b.ToolID"
            //    + " And ToolStock_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID() + ";";

            sSqlText += " Update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity - b.Quantity "
               + ", ToolStock_Info.ToolOutQuantity=ToolStock_Info.ToolOutQuantity + b.Quantity"
               + " From (Select ToolID, Sum(Quantity) As Quantity From ToolOutDetails_Info a, ToolOut_Info b where a.ToolOutID=b.Id  "
               + " And b.TableRecGuid='" + FieldValues.GetValue(0) + "'and b.ApprovalStatus=1 And b.Status=0 Group By ToolID) b "
               + " Where ToolStock_Info.ToolID=b.ToolID And ToolStock_Info.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + ";";

            sSqlText += " Update ToolStockDetail_Info Set ToolStockDetail_Info.ToolStatus=8 From "
                + " ToolOutDetails_Info b where b.ToolOutID In (Select ID From ToolOut_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "') and ToolStockDetail_Info.ID=b.ToolDetailID"
                + " And ToolStockDetail_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID() + ";";

            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static SqlDataReader GetSingleToolOutByReader(int _ToolOutID)
        {
            string sSQL = "Select a.*"
                + ", b.OpCode as ToolOutOpCode,  b.OpName as ToolOutOpName, c.OrganName, d.OpName as CreateOpName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from ToolOut_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.ToolOutUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                + " Where a.Status=0 And a.ID=" + _ToolOutID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //删除工具申请单
        public static int DeleteSingleToolOut(string _ToolOutIDs)
        {
            string sSQL = "begin ";
            sSQL += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity+b.Quantity "
                + ", ToolStock_Info.ToolOutQuantity=ToolStock_Info.ToolOutQuantity - b.Quantity"
                    + " From "
                + " (select a.ToolID, Sum(a.Quantity) As Quantity From ToolOutDetails_Info a, ToolOut_Info b "
                + " where a.ToolOutID=b.ID And IsNull(b.ApprovalStatus,0)=1 And b.ID in (" + _ToolOutIDs.ToString() + ")"
                + " group by a.ToolID) b where  ToolStock_Info.ToolID=b.ToolID;";

            sSQL += " Delete from ToolOut_Info Where ID in (" + _ToolOutIDs.ToString() + "); ";
            sSQL = sSQL + " Delete from ToolOutDetails_Info Where ToolOutID in (" + _ToolOutIDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static int UpdateApprovalToolOut(string MainTableName, string IDs, int ApprovalStatus, string Description)
        {
            string sSqlText = "begin";

            sSqlText += " Insert Into ApprovalLog_Info (MainTableName, MainID, ApprovalStatus, ApprovalUserID, ApprovalTime, Description) "
                + " (Select '" + MainTableName + "', ID, " + ApprovalStatus.ToString() + "," + SysGlobal.GetCurrentUserID().ToString()
                + ", GetDate(), '" + Description + "' From ToolOut_info Where Status=0 And ID In (" + IDs + ")); ";

            sSqlText += " Update ToolOut_info Set ApprovalStatus=" + ApprovalStatus.ToString()
                + " Where Status=0  And ID In (" + IDs + ");";

            if (ApprovalStatus == ToolOut_ApprovalIsOK)
            {
                sSqlText += " Insert Into ToolStock_Info (OrganID, ToolID, Quantity)"
                + " (Select distinct " + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + ", ToolID, 0 From ToolOutDetails_Info Where Status=0 "
                + " And ToolOutID In (" + IDs.ToString() + ")"
                + " And ToolID not in (Select ToolID From ToolStock_Info Where OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + "));";

                sSqlText += " Update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity - b.Quantity "
                   + ", ToolStock_Info.ToolOutQuantity=ToolStock_Info.ToolOutQuantity + b.Quantity"
                   + " From (Select ToolID, Sum(Quantity) As Quantity From ToolOutDetails_Info where "
                   + " ToolOutID in (" + IDs.ToString() + ") Group By ToolID) b "
                   + " Where ToolStock_Info.ToolID=b.ToolID And ToolStock_Info.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + ";";

                sSqlText += " Update ToolStockDetail_Info Set ToolStockDetail_Info.ToolStatus=8 From "
                    + " ToolOutDetails_Info b where b.ToolOutID In (" + IDs.ToString() + ") and ToolStockDetail_Info.ID=b.ToolDetailID"
                    + " And ToolStockDetail_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID() + ";";

                //sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity-b.Quantity "
                //    + ", ToolStock_Info.ToolOutQuantity=ToolStock_Info.ToolOutQuantity + b.Quantity"
                //    + " From "
                //    + " (select a.ToolID, Sum(a.Quantity) As Quantity From ToolOutDetails_Info a, ToolOut_Info b "
                //    + " where a.ToolOutID=b.ID And IsNull(b.ApprovalStatus,0)=1 And b.ID in (" + IDs.ToString() + ")"
                //    + " group by a.ToolID) b where  ToolStock_Info.ToolID=b.ToolID";
            }

            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText); 
        }
    }
}
