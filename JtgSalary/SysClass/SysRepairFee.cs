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
    public class SysRepairFee
    {
        //获取单个车间信息
        public static int DeleteRepairFee(string _IDs)
        {
            string sSQL = "begin Delete from RepairFee_Info Where Status=0 And ID in (" + _IDs.ToString() + "); ";

            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static DataSet GetRepairFeeLstByDataSet(string sWhereSQL)
        {
            string sSQL = "Select a.*, b.OrganName, c.OpName as CreateOpName "
                + " from RepairFee_Info a "
                + " Left join SysOrgan_Info b on b.Status=0 and b.ID=a.OrganID"
                + " Left join SysUser_Info c on a.CreateUserID=c.ID"
                + " Where a.Status=0"
                + sWhereSQL;

            sSQL = sSQL + " Order By a.RepairDate Desc, a.OrganID";

            return DataCommon.GetDataByDataSet(sSQL);
        }
      
        public static SqlDataReader GetSingleRepairFeeByReader(int _ID)
        {
            string sSQL = "Select a.*, b.OrganName, c.OpName as CreateOpName "
                + " from RepairFee_Info a "
                + " Left join SysOrgan_Info b on b.Status=0 and b.ID=a.OrganID"
                + " Left join SysUser_Info c on a.CreateUserID=c.ID"
                + " Where a.Status=0 And a.ID=" + _ID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //更新添加工具档案信息
        public static int UpdateSingleRepairFee(int _ID, string[] FieldValues)
        {
            string sSqlText = " begin";

            sSqlText += " Update PurchaseCredit_Info Set UsedTotalCredit=UsedTotalCredit-b.RepairAmount"
                    + ",UsedCredit1=IsNull(UsedCredit1,0)+(Case Month(b.RepairDate) when 1 then b.RepairAmount else 0 end)"
                    + ",UsedCredit2=IsNull(UsedCredit2,0)+(Case Month(b.RepairDate) when 2 then b.RepairAmount else 0 end)"
                    + ",UsedCredit3=IsNull(UsedCredit3,0)+(Case Month(b.RepairDate) when 3 then b.RepairAmount else 0 end)"
                    + ",UsedCredit4=IsNull(UsedCredit4,0)+(Case Month(b.RepairDate) when 4 then b.RepairAmount else 0 end)"
                    + ",UsedCredit5=IsNull(UsedCredit5,0)+(Case Month(b.RepairDate) when 5 then b.RepairAmount else 0 end)"
                    + ",UsedCredit6=IsNull(UsedCredit6,0)+(Case Month(b.RepairDate) when 6 then b.RepairAmount else 0 end)"
                    + ",UsedCredit7=IsNull(UsedCredit7,0)+(Case Month(b.RepairDate) when 7 then b.RepairAmount else 0 end)"
                    + ",UsedCredit8=IsNull(UsedCredit8,0)+(Case Month(b.RepairDate) when 8 then b.RepairAmount else 0 end)"
                    + ",UsedCredit9=IsNull(UsedCredit9,0)+(Case Month(b.RepairDate) when 9 then b.RepairAmount else 0 end)"
                    + ",UsedCredit10=IsNull(UsedCredit10,0)+(Case Month(b.RepairDate) when 10 then b.RepairAmount else 0 end)"
                    + ",UsedCredit11=IsNull(UsedCredit11,0)+(Case Month(b.RepairDate) when 11 then b.RepairAmount else 0 end)"
                    + ",UsedCredit12=IsNull(UsedCredit12,0)+(Case Month(b.RepairDate) when 12 then b.RepairAmount else 0 end)"
                    + " From RepairFee_Info b Where b.Status=0 And b.ID = " + _ID.ToString()
                    + " And PurchaseCredit_Info.OrganID=b.OrganID And PurchaseCredit_Info.PurchaseYear=Year(b.RepairDate);";

            if (_ID > 0)
            {
                sSqlText += " UPDATE RepairFee_Info SET OrganID=" + FieldValues.GetValue(0) 
                    + ", RepairDate='" + FieldValues.GetValue(1) + "'"
                     + ",RepairAmount=" + FieldValues.GetValue(2) 
                     + ",Description='" + FieldValues.GetValue(3) + "'";
                sSqlText += sSqlText  + " WHERE ID=" + _ID + "" + ";";

            }
            else
            {
                sSqlText += " Insert Into RepairFee_Info ( OrganID, RepairDate, RepairAmount, CreateUserID, Description) Values("
                   + FieldValues.GetValue(0) + ",'" + FieldValues.GetValue(1) + "',"
                   + FieldValues.GetValue(2) + "," + SysGlobal.GetCurrentUserID().ToString() + ",'"
                   + FieldValues.GetValue(3) + "')" + ";";
            }

            sSqlText += " Update PurchaseCredit_Info Set UsedTotalCredit=UsedTotalCredit+b.RepairAmount"
                    + ",UsedCredit1=IsNull(UsedCredit1,0)+(Case Month(b.RepairDate) when 1 then b.RepairAmount else 0 end)"
                    + ",UsedCredit2=IsNull(UsedCredit2,0)+(Case Month(b.RepairDate) when 2 then b.RepairAmount else 0 end)"
                    + ",UsedCredit3=IsNull(UsedCredit3,0)+(Case Month(b.RepairDate) when 3 then b.RepairAmount else 0 end)"
                    + ",UsedCredit4=IsNull(UsedCredit4,0)+(Case Month(b.RepairDate) when 4 then b.RepairAmount else 0 end)"
                    + ",UsedCredit5=IsNull(UsedCredit5,0)+(Case Month(b.RepairDate) when 5 then b.RepairAmount else 0 end)"
                    + ",UsedCredit6=IsNull(UsedCredit6,0)+(Case Month(b.RepairDate) when 6 then b.RepairAmount else 0 end)"
                    + ",UsedCredit7=IsNull(UsedCredit7,0)+(Case Month(b.RepairDate) when 7 then b.RepairAmount else 0 end)"
                    + ",UsedCredit8=IsNull(UsedCredit8,0)+(Case Month(b.RepairDate) when 8 then b.RepairAmount else 0 end)"
                    + ",UsedCredit9=IsNull(UsedCredit9,0)+(Case Month(b.RepairDate) when 9 then b.RepairAmount else 0 end)"
                    + ",UsedCredit10=IsNull(UsedCredit10,0)+(Case Month(b.RepairDate) when 10 then b.RepairAmount else 0 end)"
                    + ",UsedCredit11=IsNull(UsedCredit11,0)+(Case Month(b.RepairDate) when 11 then b.RepairAmount else 0 end)"
                    + ",UsedCredit12=IsNull(UsedCredit12,0)+(Case Month(b.RepairDate) when 12 then b.RepairAmount else 0 end)"
                    + " From RepairFee_Info b Where b.Status=0 And b.ID = " + _ID.ToString()
                    + " And PurchaseCredit_Info.OrganID=b.OrganID And PurchaseCredit_Info.PurchaseYear=Year(b.RepairDate);";

            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }
    }
}
