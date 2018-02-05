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
    public class SysWarning
    {       
        public static string ReceiptItem_ParamName = "ReceiptItem";

        public static string BorrowItem_ParamName = "BorrowItem";

        public static string SalaryItem_ParamName = "SalaryItem";

        //设置预警
        public static int UpdateSingleWarning(int _ToolID, int _OrganID, double WarningQuantity, double LowerQuantity)
        {

            string sSql = "if Exists(Select 1 from ToolStock_Info Where ToolID=" + _ToolID.ToString() + " And OrganID=" + _OrganID.ToString() + ")";
            sSql = sSql + "  begin update ToolStock_Info Set WarningQuantity='" + WarningQuantity.ToString() + "', LowerQuantity=" + LowerQuantity.ToString()
                + "  Where ToolID=" + _ToolID.ToString() + " And OrganID=" + _OrganID.ToString() + " ";
            sSql = sSql + "end "
                + " else "
                + " begin "
                + " Insert into ToolStock_Info (ToolID,OrganID, WarningQuantity, LowerQuantity) Values(" + _ToolID.ToString()
                + ", " + _OrganID.ToString() + ", " + WarningQuantity.ToString()  + ", " + LowerQuantity.ToString() + ") "
                + " end; ";

            return DataCommon.QueryData(sSql);
        }

        public static int GetReceiptValueByOrganID(int _OrganID)
        {
            int _Value = 0;
            string sSQL = "Select * from SysParmaeters_Info Where Status=0 And OrganID=" + _OrganID.ToString() + " And ParamNo='" + ReceiptItem_ParamName + "'";
            SqlDataReader sdr = CyxPack.OperateSqlServer.DataCommon.GetDataByReader(sSQL);
            if (sdr.Read())
            {
                _Value = int.Parse(sdr["ParamValue"].ToString());
            }
            sdr.Close();
            return _Value;
        }

        public static int UpdateSingleReceiptValue(int _OrganID, double Value)
        {

            string sSql = "if Exists(Select 1 from SysParmaeters_Info Where Status=0 And OrganID=" + _OrganID.ToString() + " And ParamNo='" + ReceiptItem_ParamName + "')";
            sSql = sSql + "  begin update SysParmaeters_Info Set ParamValue='" + Value.ToString() + "'"
                + "  Where Status=0 And OrganID=" + _OrganID.ToString() + " And ParamNo='" + ReceiptItem_ParamName + "'";
            sSql = sSql + "end "
                + " else "
                + " begin "
                + " Insert into SysParmaeters_Info (OrganID, ParamNo, ParamValue) Values(" + _OrganID.ToString()
                + ", '" + ReceiptItem_ParamName + "', '" + Value.ToString() + "') "
                + " end; ";

            return DataCommon.QueryData(sSql);
        }

        public static double GetBorrowValueByOrganID(int _OrganID)
        {
            double _Value = 0;
            string sSQL = "Select * from SysParmaeters_Info Where Status=0 And OrganID=" + _OrganID.ToString() + " And ParamNo='" + BorrowItem_ParamName + "'";
            SqlDataReader sdr = CyxPack.OperateSqlServer.DataCommon.GetDataByReader(sSQL);
            if (sdr.Read())
            {
                _Value = double.Parse(sdr["ParamValue"].ToString());
            }
            sdr.Close();
            return _Value;
        }

        public static int UpdateSingleBorrowValue(int _OrganID, double Value)
        {

            string sSql = "if Exists(Select 1 from SysParmaeters_Info Where Status=0 And OrganID=" + _OrganID.ToString() + " And ParamNo='" + BorrowItem_ParamName + "')";
            sSql = sSql + "  begin update SysParmaeters_Info Set ParamValue='" + Value.ToString() + "'"
                + "  Where Status=0 And OrganID=" + _OrganID.ToString() + " And ParamNo='" + BorrowItem_ParamName + "'";
            sSql = sSql + "end "
                + " else "
                + " begin "
                + " Insert into SysParmaeters_Info (OrganID, ParamNo, ParamValue) Values(" + _OrganID.ToString()
                + ", '" + BorrowItem_ParamName + "', '" + Value.ToString() + "') "
                + " end; ";

            return DataCommon.QueryData(sSql);
        }


        public static DataSet GetBorrowDetailsLstByDataSet(int OrganID, string WhereSQL)
        {
            string sSQL = "Select a.* , DateDiff(Day, ConsumeDate, GetDate()) As Day, b.ToolID, b.Quantity, c.ToolNo, c.ToolName, c.AliasesName, c.Specification, c.MaterialCode,c.Unit "
                + ", IsNull(d.OrganName,'') as OrganName, IsNull(e.OpName, '') as ConsumeOpName"
                + " from Consume_Info a"
                + " left join SysOrgan_Info d on d.Status=0 And d.ID=a.OrganID"
                + " left join SysUser_Info e on e.Status=0 And e.ID=a.ConsumeUserID"
                + ", ConsumeDetails_Info b "
                + " left join Tool_Info c on c.Status=0 And c.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and b.ToolID=c.ID"
                + " Where IsNull(a.OrganID,0)=" + OrganID.ToString() + " And a.ID=b.ConsumeID And a.ConsumeType=" + SysClass.SysBorrow.ConsumeType_BorrowValue.ToString()
                + " And a.Status=0 And DateDiff(Day, ConsumeDate, GetDate())>" + GetBorrowValueByOrganID(SysGlobal.GetCurrentUserOrganID()).ToString()
                + WhereSQL;

            sSQL = sSQL + " Order By a.ConsumeDate";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetDeliveryLstByDataSet(string sWhereSQL)
        {
            string sSQL = "Select a.*, DateDiff(Day, a. DeliveryDate, GetDate()) as Day"
                  + ", b.OpName as DeliveryOpName, c.OrganName as SupplierName, d.OrganName as DeliveryOrganName"
                  + ", (Case IsNull(ShipingStatus,0) when 0 then '草稿' when 1 then '已发货' when 2 then '已入库' end) As ShipingStatusName"
                  + " from Delivery_Info a "
                  + " left join SysUser_Info b on b.Status=0 And a.DeliveryUserID=b.ID"
                  + " left join SysOrgan_Info c on c.Status=0 And a.SupplierID=c.ID"
                  + " left join SysOrgan_Info d on d.Status=0 And a.DeliveryOrganID=d.ID"
                  + " Where a.Status=0 " + sWhereSQL;

            sSQL = sSQL + " Order By a.DeliveryDate Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //获取单个车间信息
        public static int DeletePurchaseCredit(string _IDs)
        {
            string sSQL = "begin Delete from PurchaseCredit_Info Where Status=0 And ID in (" + _IDs.ToString() + "); ";

            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static double GetTotalCredit(int _OrganID, int _PurchaseYear)
        {
            double _TotalCredit = 0;
            SqlDataReader sdr = GetSinglePurchaseCreditByReader(_OrganID, _PurchaseYear);
            if (sdr.Read())
            {
                _TotalCredit = double.Parse(sdr["TotalCredit"].ToString());
            }
            sdr.Close();
            return _TotalCredit;
        }

        public static double GetUsedTotalCredit(int _OrganID, int _PurchaseYear)
        {
            double _TotalCredit = 0;
            SqlDataReader sdr = GetSinglePurchaseCreditByReader(_OrganID, _PurchaseYear);
            if (sdr.Read())
            {
                _TotalCredit = double.Parse(sdr["UsedTotalCredit"].ToString());
            }
            sdr.Close();
            return _TotalCredit;
        }

        public static double GetMonthCredit(int _OrganID, int _PurchaseYear, int _PurchaseMonth)
        {
            double _TotalCredit = 0;
            SqlDataReader sdr = GetSinglePurchaseCreditByReader(_OrganID, _PurchaseYear);
            if (sdr.Read())
            {
                _TotalCredit = double.Parse(sdr["Credit" + _PurchaseMonth.ToString()].ToString());
            }
            sdr.Close();
            return _TotalCredit;
        }

        public static double GetUsedMonthCredit(int _OrganID, int _PurchaseYear, int _PurchaseMonth)
        {
            double _TotalCredit = 0;
            SqlDataReader sdr = GetSinglePurchaseCreditByReader(_OrganID, _PurchaseYear);
            if (sdr.Read())
            {
                _TotalCredit = double.Parse(sdr["UsedCredit" + _PurchaseMonth.ToString()].ToString());
            }
            sdr.Close();
            return _TotalCredit;
        }

        public static DataSet GetPurchaseCreditLstByDataSet(string sWhereSQL)
        {
            string sSQL = "Select a.*, b.OrganName  from PurchaseCredit_Info a "
                + " Left join SysOrgan_Info b on b.Status=0 and b.ID=a.OrganID"
                + " Where a.Status=0" 
                + sWhereSQL;

            sSQL = sSQL + " Order By a.PurchaseYear Desc, a.OrganID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetSinglePurchaseCreditByReader(int _OrganID, int _PurchaseYear)
        {
            string sSQL = "Select * from PurchaseCredit_Info Where Status=0 And OrganID=" + _OrganID.ToString() + " And PurchaseYear=" + _PurchaseYear.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSinglePurchaseCreditByReader(int _ID)
        {
            string sSQL = "Select * from PurchaseCredit_Info Where Status=0 And ID=" + _ID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //更新添加工具档案信息
        public static int UpdateSinglePurchaseCredit(int _ID, string[] FieldValues)
        {
            string sSqlText = "";
            if (_ID > 0)
            {
                sSqlText = "begin UPDATE PurchaseCredit_Info SET OrganID=" + FieldValues.GetValue(0) + ", PurchaseYear=" + FieldValues.GetValue(1)
                     + ",TotalCredit=" + FieldValues.GetValue(2) + ",Credit1=" + FieldValues.GetValue(3)
                     + ",Credit2=" + FieldValues.GetValue(4) + ",Credit3=" + FieldValues.GetValue(5)
                     + ",Credit4=" + FieldValues.GetValue(6) + ",Credit5=" + FieldValues.GetValue(7)
                     + ",Credit6=" + FieldValues.GetValue(8) + ",Credit7=" + FieldValues.GetValue(9)
                     + ",Credit8=" + FieldValues.GetValue(10) + ",Credit9=" + FieldValues.GetValue(11)
                     + ",Credit10=" + FieldValues.GetValue(12) + ",Credit11=" + FieldValues.GetValue(13)
                     + ",Credit12=" + FieldValues.GetValue(14) + ",Description='" + FieldValues.GetValue(15) + "'";
                sSqlText = sSqlText + ",SortID=" + FieldValues.GetValue(10) + " WHERE ID=" + _ID + "" + ";";
                string sLogText = "更新 人员管理>机构部门：" + FieldValues.GetValue(2) + "记录。";

                sSqlText += SysLogs.GetOperatorLogSQL(sLogText) + " end;";
            }
            else
            {
                sSqlText = "begin Insert Into PurchaseCredit_Info ( OrganID, PurchaseYear, TotalCredit, Credit1, Credit2, Credit3, Credit4,"
                    + " Credit5, Credit6, Credit7, Credit8, Credit9, Credit10, Credit11, Credit12, Description) Values("
                   + FieldValues.GetValue(0) + "," + FieldValues.GetValue(1) + ","
                   + FieldValues.GetValue(2) + "," + FieldValues.GetValue(3) + "," 
                   + FieldValues.GetValue(4) + "," + FieldValues.GetValue(5) + "," 
                   + FieldValues.GetValue(6) + "," + FieldValues.GetValue(7) + "," 
                   + FieldValues.GetValue(8) + "," + FieldValues.GetValue(9) + ","
                   + FieldValues.GetValue(10) + "," + FieldValues.GetValue(11) + ","
                   + FieldValues.GetValue(12) + "," + FieldValues.GetValue(13) + ","
                   + FieldValues.GetValue(14) + ",'" + FieldValues.GetValue(15) + "')" + ";";
                string sLogText = "新增 人员管理>机构部门：" + FieldValues.GetValue(2) + "记录。";
                sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            }
            return DataCommon.QueryData(sSqlText);
        }

        //判断工具编号是否重复
        public static Boolean CheckCreditExists(int CreditID, int OrganID, int PurchaseYear)
        {
            string sSqlText = "Select 1 From PurchaseCredit_Info Where OrganID=" + OrganID + " And PurchaseYear=" + PurchaseYear + " And ID<>" + CreditID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        public static int UpdateSingleSalaryValue(int _OrganID, double Value)
        {

            string sSql = "if Exists(Select 1 from SysParmaeters_Info Where Status=0 And OrganID=" + _OrganID.ToString() + " And ParamNo='" + SalaryItem_ParamName + "')";
            sSql = sSql + "  begin update SysParmaeters_Info Set ParamValue='" + Value.ToString() + "'"
                + "  Where Status=0 And OrganID=" + _OrganID.ToString() + " And ParamNo='" + SalaryItem_ParamName + "'";
            sSql = sSql + "end "
                + " else "
                + " begin "
                + " Insert into SysParmaeters_Info (OrganID, ParamNo, ParamValue) Values(" + _OrganID.ToString()
                + ", '" + SalaryItem_ParamName + "', '" + Value.ToString() + "') "
                + " end; ";

            return DataCommon.QueryData(sSql);
        }


        public static int GetSalaryValueByOrganID(int _OrganID)
        {
            int _Value = 0;
            string sSQL = "Select * from SysParmaeters_Info Where Status=0 And OrganID=" + _OrganID.ToString() + " And ParamNo='" + SalaryItem_ParamName + "'";
            SqlDataReader sdr = CyxPack.OperateSqlServer.DataCommon.GetDataByReader(sSQL);
            if (sdr.Read())
            {
                _Value = int.Parse(sdr["ParamValue"].ToString());
            }
            sdr.Close();
            return _Value;
        }
    }
}
