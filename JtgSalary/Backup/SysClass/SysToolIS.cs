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
    public class SysToolIS
    {
        //历史损益点
        public static string ToolIS_SearchText = "";
        public static string ToolIS_ApprovalStatus = "";

        public static string ToolIS_TableName = "ToolIS_Info";
        public static int ToolIS_Draft = 0, ToolIS_ApprovalIsOK = 1;
        public static string GetTableRecGuidByID(int _ToolISID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSingleToolISByReader(_ToolISID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static DataSet GetToolISLstByDataSet(int _OrganID, string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.*"
                + ", b.OpName as ToolISOpName, c.OrganName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from ToolIS_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.ToolISUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " Where a.Status=0 " + sWhereSQL;
            if (_OrganID > 0)
            {
                sSQL = sSQL + " And a.OrganID=" + _OrganID.ToString();
            }
            sSQL = sSQL + " Order By a.ToolISDate Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetToolISDetailsLstByDataSet(int ToolISID, string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName"
                + " , b.Specification, b.MaterialCode,b.Unit, IsNull(c.Quantity,0) As StockQuantity, a.Quantity As OldQuantity"
                + ", c.StorageLocation, d.ToolCode, d.TestCode"
                + " from ToolISDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " left Join ToolStock_Info c on c.Status=0 And c.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + " and c.ToolID=a.ToolID"
                + " left join ToolStockDetail_Info d on d.Status=0 And a.ToolDetailID=d.ID"
                + " Where IsNull(a.ToolISID,0)=" + ToolISID.ToString() 
                + " And a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetToolISDetailsLstByReader(int ToolISID, string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification, b.MaterialCode,b.Unit  from ToolISDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " Where a.Status=0" + WhereSQL;

            if (ToolISID > 0)
            {
                sSQL += " And IsNull(a.ToolISID,0)=" + ToolISID.ToString() ;
            }

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetToolISDetailsLstByReader(string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName"
                + " , b.Specification, b.MaterialCode,b.Unit, c.ToolISDate"
                + " from ToolISDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " left join ToolIS_Info c on c.Status=0 And c.ID=a.ToolISID"
                + " Where a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static Boolean CheckToolISNoExists(int ToolISID, string ToolISNo)
        {
            string sSqlText = "Select 1 From ToolIS_Info Where "
                + " ToolISNo='" + ToolISNo + "' And ID<>" + ToolISID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //更新添加工具档案信息
        public static int UpdateSingleToolIS(int _ToolISID, string[] FieldValues, string DetailsSQL)
        {
            string sSqlText = "begin";

            //sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity+b.Quantity "
            //    + " From ToolISDetails_Info b where b.ToolISID In (Select ID From ToolIS_Info "
            //    + " Where Status=0 And TableRecGuid='" + FieldValues.GetValue(0) + "' and IsNull(ApprovalStatus,0)=1) and ToolStock_Info.ToolID=b.ToolID;";

            sSqlText += " Update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity + b.Quantity "
               + ", ToolStock_Info.ISQuantity=ToolStock_Info.ISQuantity - b.Quantity"
               + " From (Select ToolID, Sum(Quantity) As Quantity From ToolISDetails_Info a, ToolIS_Info b where a.ToolISID=b.Id  "
               + " And b.TableRecGuid='" + FieldValues.GetValue(0) + "' and b.ApprovalStatus=1 And b.Status=0 Group By ToolID) b "
               + " Where ToolStock_Info.ToolID=b.ToolID And ToolStock_Info.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + ";";
            
            if (_ToolISID > 0)
            {
                sSqlText = sSqlText + " UPDATE ToolIS_Info SET ToolISNo='" + FieldValues.GetValue(1) + "'"
                     + ",ToolISUserID=" + FieldValues.GetValue(2) + ""
                     + ",Description='" + FieldValues.GetValue(3) + "'"
                     + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _ToolISID + "" + ";";

                sSqlText += " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into ToolIS_Info (TableRecGuid"
                    + ", ToolISNo"
                    + ", ToolISDate"
                    + ", OrganID"
                    + ", ToolISUserID"
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
                + " (Select distinct " + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + ", ToolID, 0 From ToolISDetails_Info Where Status=0 "
                + " And ToolISID In (Select ID From ToolIS_Info Where Status=0 and IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "')"
                + " And ToolID not in (Select ToolID From ToolStock_Info Where OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + "));";

            //sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity-b.Quantity "
            //    + " From ToolISDetails_Info b where b.ToolISID In (Select ID From ToolIS_Info Where Status=0 and IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "') and ToolStock_Info.ToolID=b.ToolID"
            //    + " And ToolStock_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID() + ";";

            sSqlText += " Update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity - b.Quantity "
               + ", ToolStock_Info.ISQuantity=ToolStock_Info.ISQuantity + b.Quantity"
               + " From (Select ToolID, Sum(Quantity) As Quantity From ToolISDetails_Info a, ToolIS_Info b where a.ToolISID=b.Id  "
               + " And b.TableRecGuid='" + FieldValues.GetValue(0) + "'and b.ApprovalStatus=1 And b.Status=0 Group By ToolID) b "
               + " Where ToolStock_Info.ToolID=b.ToolID And ToolStock_Info.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + ";";

            sSqlText += " Update ToolStockDetail_Info Set ToolStockDetail_Info.ToolStatus=6 From "
                + " ToolISDetails_Info b where b.ToolISID In (Select ID From ToolIS_Info Where Status=0 And IsNull(ApprovalStatus,0)=1 And TableRecGuid='" + FieldValues.GetValue(0) + "') and ToolStockDetail_Info.ID=b.ToolDetailID"
                + " And ToolStockDetail_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID() + ";";

            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static SqlDataReader GetSingleToolISByReader(int _ToolISID)
        {
            string sSQL = "Select a.*"
                + ", b.OpCode as ToolISOpCode,  b.OpName as ToolISOpName, c.OrganName, d.OpName as CreateOpName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from ToolIS_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.ToolISUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                + " Where a.Status=0 And a.ID=" + _ToolISID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //删除工具申请单
        public static int DeleteSingleToolIS(string _ToolISIDs)
        {
            string sSQL = "begin Delete from ToolIS_Info Where ID in (" + _ToolISIDs.ToString() + "); ";
            sSQL = sSQL + " Delete from ToolISDetails_Info Where ToolISID in (" + _ToolISIDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }
    }
}
