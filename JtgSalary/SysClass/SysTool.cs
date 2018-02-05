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
    public class SysTool
    {
        public static string ConsumeDetails_TableName = "ConsumeDetails_Info";
        public static int _NomalToolType = 0;

        public static int ToolStockDetailSelect_selCategoryID = 0;
        public static int ToolStockSelect_selCategoryID = 0;

        //工具档案参数
        public static bool Tool_ViewChildren = false;
        public static string Tool_SearchText = "";
        public static int Tool_PageNo = 1;

        //工具初始化
        public static bool Location_ShowOnlyWarning_Flag = false;
        public static bool Location_HideZero_Flag = true;
        public static bool Location_ViewChildren = false;
        public static string Location_SearchText = "";

        //工具库存
        public static bool Stock_ShowOnlyWarning_Flag = false;
        public static bool Stock_HideZero_Flag = false;
        public static bool Stock_HasChildren_Flag = false;
        public static string Stock_SearchText = "";

        //工具检验
        public static bool Test_HasChildren_Flag = false;
        public static string Test_SearchText = "";

        //送检库存
        public static bool SendTestStock_HideZero_Flag = true;
        public static bool SendTestStock_HasChildren_Flag = false;
        public static string SendTestStock_SearchText = "";

        public static bool SendTestStockDetail_HasChildren_Flag = true;
        public static string SendTestStockDetail_SearchText = "";

        //送修库存
        public static bool SendServiceStock_HideZero_Flag = true;
        public static bool SendServiceStock_HasChildren_Flag = false;
        public static string SendServiceStock_SearchText = "";

        public static bool SendServiceStockDetail_HasChildren_Flag = true;
        public static string SendServiceStockDetail_SearchText = "";

        //工具明细选择
        public static string ToolStockDetailSelect_SearchName = "", ToolStockDetailSelect_SearchText = "";
        public static bool ToolStockDetailSelect_HasChildren_Flag = false;

        //工具库存选择
        public static string ToolStockSelect_SearchName = "", ToolStockSelect_SearchText = "";
        public static bool ToolStockSelect_HasChildren_Flag = false;

        //工具选择
        public static string ToolSelect_SearchName = "", ToolSelect_SearchText = "";
        public static bool ToolSelect_HasChildren_Flag = false;

        //工具库存
        public static string ToolStock_SearchName = "", ToolStock_SearchText = "";
        public static bool ToolStock_HasChildren_Flag = false;

        public static string GetTableRecGuidByID(int _ID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSingleToolsByReader(_ID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static DataSet GetToolsLstByDataSet(int CategoryID, string sWhereSQL)
        {
            string sSQL = "Select *  from Tool_Info a Where ToolType='" + 0 + "'and Status=0 " + sWhereSQL;            

            sSQL = sSQL + " Order By SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static bool GetIsCodeByToolID(int ToolID)
        {
            bool _IsCode = false;
            SqlDataReader sdr = GetSingleToolsByReader(ToolID);
            if (sdr.Read())
            {
                _IsCode = sdr["IsCode"].ToString() == "1";
            }
            sdr.Close();
            return _IsCode;
        }       

        public static string GetToolNoByToolID(int ToolID)
        {
            string _ToolNo = "";
            SqlDataReader sdr = GetSingleToolsByReader(ToolID);
            if (sdr.Read())
            {
                _ToolNo = sdr["ToolNo"].ToString();
            }
            sdr.Close();
            return _ToolNo;
        }

        public static string GetToolCodeByToolNo(string ToolNo, int CusNum)
        {
            string _ToolCode = ToolNo;

            string _CusNo = CusNum.ToString();

            for (int i = _CusNo.Length; i <= 4; i++)
            {
                _CusNo = "0" + _CusNo;
            }

            _ToolCode += _CusNo;

            return _ToolCode;
        }

        public static int GetMaxNumByToolID(int ToolID)
        {
            int _MaxNum = 0;
            SqlDataReader sdr = GetSingleToolsByReader(ToolID);
            if (sdr.Read())
            {
                _MaxNum = int.Parse(sdr["MaxNum"].ToString());
            }
            sdr.Close();
            return _MaxNum;
        }

        public static int UpdateAddNumByToolID(int ToolID, int AddNum)
        {
            string sSQL = "Update Tool_Info Set MaxNum=IsNull(MaxNum,0)+" + AddNum.ToString() + " Where ID=" + ToolID.ToString();

            return DataCommon.QueryData(sSQL);
        }

        public static string GetUpdateAddNumSQL(int ToolID, int AddNum)
        {
            string sSQL = " Update Tool_Info Set MaxNum=IsNull(MaxNum,0)+" + AddNum.ToString() + " Where ID=" + ToolID.ToString() + ";";

            return sSQL ;
        }

        public static DataSet GetToolsLstByDataSet(string WhereSQL)
        {
            string sSQL = "Select *  from Tool_Info Where ToolType='" + 0 + "'and Status=0" + WhereSQL;            

            sSQL = sSQL + " Order By ToolNo, SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }


        public static SqlDataReader GetToolsLstByReader(int OrganID, string WhereSQL)
        {
            string sSQL = " select a.ID as ToolDetailID, a.ToolID, a.ToolCode, a.TestCode, c.StorageLocation, a.ToolDate"
                + ", (Case a.ToolStatus when 0 then '库存' when 1 then '领用' when 2 then '借用'"
                + " when 3 then '送检' when 4 then '送修' when 5 then '注销' when 5 then '注销' when 6 then '损益' end) as ToolStatusName"
                + ", (Case a.TestStatus when 0 then '待检' when 1 then '合格' when 2 then '不合格' end) as TestStatusName, a.TestDate"
                + " ,b.* from ToolStockDetail_Info a"
                + " left join Tool_Info b on a.ToolID=b.ID"
                + " left join ToolStock_Info c on a.ToolID=c.ToolID And c.OrganID=" + OrganID.ToString()
                + " Where a.OrganID=" + OrganID.ToString() + WhereSQL;

            //string sSQL = "Select *  from Tool_Info a Where ToolType='" + 0 + "'and Status=0"
                //+ WhereSQL;

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetToolDetailLstByReader(string WhereSQL)
        {
            string sSQL = "Select *  from Tool_Info a Where ToolType='" + 0 + "'and Status=0"
                + WhereSQL;

            sSQL = sSQL + " Order By SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetWhereToolLstByReader(int CategoryID, string WhereSQL)
        {
            string sSQL = "Select a.* from Tool_Info a Where a.ToolType=" + _NomalToolType.ToString() + " And a.Status=0" + WhereSQL;
            if (CategoryID > 0)
            {
                sSQL = sSQL + " And a.CategoryID=" + CategoryID.ToString();
            }
            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetWhereStockToolLstByReader(int CategoryID, string WhereSQL)
        {
            string sSQL = "Select a.*, IsNull(b.Quantity, 0) As StockQuantity, 0 As OldQuantity, b.StorageLocation from Tool_Info a "
                + " left join ToolStock_Info b on b.Status=0 And a.ID=b.ToolID And b.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString()
                + " Where a.ToolType=" + _NomalToolType.ToString() + " And a.Status=0" + WhereSQL;
            if (CategoryID > 0)
            {
                sSQL = sSQL + " And a.CategoryID=" + CategoryID.ToString();
            }
            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetWhereTestStockToolLstByReader(int CategoryID, string WhereSQL)
        {
            string sSQL = "Select a.*, IsNull(b.TestQuantity, 0) As StockQuantity, 0 As OldQuantity, b.StorageLocation from Tool_Info a "
                + " left join ToolStock_Info b on b.Status=0 And a.ID=b.ToolID And b.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString()
                + " Where a.ToolType=" + _NomalToolType.ToString() + " And a.Status=0" + WhereSQL;
            if (CategoryID > 0)
            {
                sSQL = sSQL + " And a.CategoryID=" + CategoryID.ToString();
            }
            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetWhereServiceStockToolLstByReader(int CategoryID, string WhereSQL)
        {
            string sSQL = "Select a.*, IsNull(b.ServiceQuantity, 0) As StockQuantity, 0 As OldQuantity, b.StorageLocation from Tool_Info a "
                + " left join ToolStock_Info b on b.Status=0 And a.ID=b.ToolID And b.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString()
                + " Where a.ToolType=" + _NomalToolType.ToString() + " And a.Status=0" + WhereSQL;
            if (CategoryID > 0)
            {
                sSQL = sSQL + " And a.CategoryID=" + CategoryID.ToString();
            }
            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetWhereStockDetailToolLstByReader(int CategoryID, string WhereSQL)
        {
            string sSQL = "Select c.*, a.ID as ToolDetailID, a.ToolCode, a.TestCode"
                + ", IsNull(b.Quantity, 0) As StockQuantity, 0 As OldQuantity, b.StorageLocation from ToolStockDetail_Info a "
                + " left join ToolStock_Info b on b.Status=0 And a.ToolID=b.ToolID And b.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString()
                + " left join Tool_Info c on c.Status=0 And a.ToolID=c.ID "
                + " Where c.ToolType=" + _NomalToolType.ToString() 
                + " And a.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + " And a.Status=0" + WhereSQL;
            if (CategoryID > 0)
            {
                sSQL = sSQL + " And c.CategoryID=" + CategoryID.ToString();
            }
            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetWhereTestStockDetailToolLstByReader(int CategoryID, string WhereSQL)
        {
            string sSQL = "Select c.*, a.ID as ToolDetailID, a.ToolCode, a.TestCode, IsNull(b.TestQuantity, 0) As StockQuantity, 0 As OldQuantity, b.StorageLocation from ToolStockDetail_Info a "
                + " left join ToolStock_Info b on b.Status=0 And a.ToolID=b.ToolID And b.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString()
                + " left join Tool_Info c on c.Status=0 And a.ToolID=c.ID "
                + " Where c.ToolType=" + _NomalToolType.ToString()
                + " And a.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + " And a.Status=0" + WhereSQL;
            if (CategoryID > 0)
            {
                sSQL = sSQL + " And c.CategoryID=" + CategoryID.ToString();
            }
            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetWhereServiceStockDetailToolLstByReader(int CategoryID, string WhereSQL)
        {
            string sSQL = "Select c.*, a.ID as ToolDetailID, a.ToolCode, a.TestCode, IsNull(b.ServiceQuantity, 0) As StockQuantity, 0 As OldQuantity, b.StorageLocation from ToolStockDetail_Info a "
                + " left join ToolStock_Info b on b.Status=0 And a.ToolID=b.ToolID And b.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString()
                + " left join Tool_Info c on c.Status=0 And a.ToolID=c.ID "
                + " Where c.ToolType=" + _NomalToolType.ToString()
                + " And a.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + " And a.Status=0" + WhereSQL;
            if (CategoryID > 0)
            {
                sSQL = sSQL + " And c.CategoryID=" + CategoryID.ToString();
            }
            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        } 
     
      
        public static SqlDataReader GetSingleToolsByReader(int _ID)
        {
            string sSQL = "Select * from Tool_Info Where Status=0 And ID=" + _ID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //更新添加工具档案信息
        public static int UpdateSingleTools(int _ID, string[] FieldValues)
        {
            string sSqlText = "";
            if (_ID > 0)
            {
                sSqlText = "begin UPDATE Tool_Info SET CategoryID=" + FieldValues.GetValue(0) + ", ToolNo='" + FieldValues.GetValue(1)
                     + "',ToolName='" + FieldValues.GetValue(2) + "',AliasesName='" + FieldValues.GetValue(3)
                     + "',Specification='" + FieldValues.GetValue(4) + "',MaterialCode='" 
                     + FieldValues.GetValue(5) + "',Unit='" + FieldValues.GetValue(6)
                     + "',PurchasePrice=" + FieldValues.GetValue(7) + ",SalesPrice=" + FieldValues.GetValue(8) 
                     + ",IsCode="+FieldValues.GetValue(9)
                     + ",Description='" + FieldValues.GetValue(10) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";
                string sLogText = "更新 人员管理>机构部门：" + FieldValues.GetValue(2) + "记录。";

                sSqlText += SysLogs.GetOperatorLogSQL(sLogText) + " end;";
            }
            else
            {
                sSqlText = "begin Insert Into Tool_Info ( CategoryID, ToolNo, ToolName, AliasesName, Specification,MaterialCode,Unit,PurchasePrice,SalesPrice,IsCode, Description) Values("
                   + FieldValues.GetValue(0) + ",'" + FieldValues.GetValue(1) + "','"
                   + FieldValues.GetValue(2) + "','" + FieldValues.GetValue(3) + "','" + FieldValues.GetValue(4) + "','" + FieldValues.GetValue(5) + "','" + FieldValues.GetValue(6) + "'," + FieldValues.GetValue(7) + "," + FieldValues.GetValue(8) + "," + FieldValues.GetValue(9) + ",'" + FieldValues.GetValue(10) + "')" + ";";
                string sLogText = "新增 人员管理>机构部门：" + FieldValues.GetValue(2) + "记录。";
                sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            }

            sSqlText += "Insert Into ToolStock_Info (OrganID, ToolID)"
                + " select "
                + " a.ID As OrganID, b.ID As ToolID from SysOrgan_Info a, Tool_Info b "
                + " Where a.OrganType in (0, 1) and b.ToolNo='" + FieldValues.GetValue(1) + "'"
                + " And Convert(varchar(10),a.ID) + '-' + Convert(varchar(10),b.ID) not in ("
                + " Select Convert(varchar(10),a.OrganID) + '-' + Convert(varchar(10),a.ToolID) From ToolStock_Info a, Tool_Info b "
                + " where a.ToolID=b.ID and b.ToolNo='" + FieldValues.GetValue(1) + "')";

            return DataCommon.QueryData(sSqlText);
        }

        //判断工具编号是否重复
        public static Boolean CheckToolNoExists(int ToolID, string ToolNo)
        {
            string sSqlText = "Select 1 From Tool_Info Where ToolNo='" + ToolNo + "' And ID<>" + ToolID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //获取单个车间信息
        public static int DeleteSingleTools(string _IDs)
        {
            string sSQL = "begin Delete from Tool_Info Where Status=0 And ID in (" + _IDs.ToString() + "); ";
            sSQL = sSQL + " Delete from ToolMember_Info Where Status=0 And PackToolID in (" + _IDs.ToString() + ");";
            sSQL += " Delete From ToolStock_Info Where ToolID in (" + _IDs.ToString() + ");";
            
            sSQL = sSQL  + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static int UpdateConsumeReturnStatus(string MainTableName, string ConsumeDetailIDs, int ApprovalStatus, string Description)
        {
            string sSqlText = "begin";

            sSqlText += " Insert Into ApprovalLog_Info (MainTableName, MainID, ApprovalStatus, ApprovalUserID, ApprovalTime, Description) "
                + " (Select '" + MainTableName + "', ID, " + ApprovalStatus.ToString() + "," + SysGlobal.GetCurrentUserID().ToString()
                + ", GetDate(), '" + Description + "' From ConsumeDetails_Info Where Status=0 And ReturnStatus=0 And ID In (" + ConsumeDetailIDs + ")); ";

            sSqlText += " Update ConsumeDetails_Info Set ReturnStatus=" + ApprovalStatus.ToString()
                + " Where Status=0 And ReturnStatus=0 And ID In (" + ConsumeDetailIDs + ");";

            sSqlText += " Update ToolStockDetail_Info Set ToolStatus=0 Where ID in (Select ToolDetailID From ConsumeDetails_Info Where Status=0 And ID In (" + ConsumeDetailIDs + "));";

            sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity+b.Quantity "
                + ", ToolStock_Info.ConsumeQuantity=ToolStock_Info.ConsumeQuantity - b.Quantity "
                + " From (select ToolID, Sum(Quantity) As Quantity  from ConsumeDetails_Info Where ID In (" + ConsumeDetailIDs + ")"
                + " Group By ToolID) b where ToolStock_Info.ToolID=b.ToolID"
                + " And ToolStock_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID();


            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static int UpdateBorrowReturnStatus(string MainTableName, string ConsumeDetailIDs, int ApprovalStatus, string Description)
        {
            string sSqlText = "begin";

            sSqlText += " Insert Into ApprovalLog_Info (MainTableName, MainID, ApprovalStatus, ApprovalUserID, ApprovalTime, Description) "
                + " (Select '" + MainTableName + "', ID, " + ApprovalStatus.ToString() + "," + SysGlobal.GetCurrentUserID().ToString()
                + ", GetDate(), '" + Description + "' From ConsumeDetails_Info Where Status=0 And ReturnStatus=0 And ID In (" + ConsumeDetailIDs + ")); ";

            sSqlText += " Update ConsumeDetails_Info Set ReturnStatus=" + ApprovalStatus.ToString()
                + " Where Status=0 And ReturnStatus=0 And ID In (" + ConsumeDetailIDs + ");";

            sSqlText += " Update ToolStockDetail_Info Set ToolStatus=0 Where ID in (Select ToolDetailID From ConsumeDetails_Info Where Status=0 And ID In (" + ConsumeDetailIDs + "));";

            sSqlText += " update ToolStock_Info Set ToolStock_Info.Quantity=ToolStock_Info.Quantity+b.Quantity "
                + ", ToolStock_Info.BorrowQuantity=ToolStock_Info.BorrowQuantity - b.Quantity "
                + " From (select ToolID, Sum(Quantity) As Quantity  from ConsumeDetails_Info Where ID In (" + ConsumeDetailIDs + ")"
                + " Group By ToolID) b where ToolStock_Info.ToolID=b.ToolID"
                + " And ToolStock_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID();


            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static int UpdateCancelStatus(string MainTableName, string ConsumeDetailIDs, int CancelStatus, string Description)
        {
            string sSqlText = "begin";

            sSqlText += " Insert Into ApprovalLog_Info (MainTableName, MainID, ApprovalStatus, ApprovalUserID, ApprovalTime, Description) "
                + " (Select '" + MainTableName + "', ID, " + CancelStatus.ToString() + "," + SysGlobal.GetCurrentUserID().ToString()
                + ", GetDate(), '" + Description + "' From ConsumeDetails_Info Where Status=0 And ReturnStatus=0 And ID In (" + ConsumeDetailIDs + ")); ";

            sSqlText += " Update ConsumeDetails_Info Set ReturnStatus=" + CancelStatus.ToString()
                + " Where Status=0 And ReturnStatus=0 And ID In (" + ConsumeDetailIDs + ");";

            sSqlText += " Update ToolStockDetail_Info Set ToolStatus=5 Where ID in (Select ToolDetailID From ConsumeDetails_Info Where Status=0  And ID In (" + ConsumeDetailIDs + "));";

            sSqlText += " update ToolStock_Info Set ToolStock_Info.CancelQuantity=ToolStock_Info.CancelQuantity+b.Quantity "
                + ", ToolStock_Info.ConsumeQuantity=ToolStock_Info.ConsumeQuantity - b.Quantity "
                + " From (select a.ToolID, Sum(a.Quantity) As Quantity  from ConsumeDetails_Info a, Consume_Info b Where a.ConsumeID=b.ID And ConsumeType=" + SysClass.SysConsume.ConsumeType_ConsumeValue.ToString() + " And a.ID In (" + ConsumeDetailIDs + ")"
                + " Group By a.ToolID) b where ToolStock_Info.ToolID=b.ToolID"
                + " And ToolStock_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID();

            sSqlText += " update ToolStock_Info Set ToolStock_Info.CancelQuantity=ToolStock_Info.CancelQuantity+b.Quantity "
                + ", ToolStock_Info.BorrowQuantity=ToolStock_Info.BorrowQuantity - b.Quantity "
                + " From (select a.ToolID, Sum(a.Quantity) As Quantity  from ConsumeDetails_Info a, Consume_Info b Where a.ConsumeID=b.ID And ConsumeType=" + SysClass.SysBorrow.ConsumeType_BorrowValue.ToString() + " And a.ID In (" + ConsumeDetailIDs + ")"
                + " Group By a.ToolID) b where ToolStock_Info.ToolID=b.ToolID"
                + " And ToolStock_Info.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID();

            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static DataSet GetToolStockDetailByDataSet(int OrganID, int ToolID, string sWhereSQL)
        {
            string sSQL = " select a.ToolID, a.ToolCode, a.TestCode, a.NextTestDate, c.StorageLocation, a.ToolDate"
                + ", (Case a.ToolStatus when 0 then '库存' when 1 then '领用' when 2 then '借用'"
                + " when 3 then '送检' when 4 then '送修' when 5 then '注销' when 5 then '注销' when 6 then '损益' when 7 then '发货' when 9 then '调出' end) as ToolStatusName"
	            + " ,b.*, a.ID as ToolDetailID from ToolStockDetail_Info a"
	            + " left join Tool_Info b on a.ToolID=b.ID"
                + " left join ToolStock_Info c on a.ToolID=c.ToolID And c.OrganID=" + OrganID.ToString()
                + " Where a.OrganID=" + OrganID.ToString() + " And a.ToolID=" + ToolID + sWhereSQL;
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetSingleToolStockDetailByReader(int ID)
        {
            string sSQL = " select a.ToolID, a.ToolCode, a.TestCode, a.ToolDate, a.NextTestDate"
                + ", (Case a.ToolStatus when 0 then '库存' when 1 then '领用' when 2 then '借用'"
                + " when 3 then '送检' when 4 then '送修' when 5 then '注销' when 5 then '注销' when 6 then '损益' when 7 then '发货' when 9 then '调出' end) as ToolStatusName"
                + " from ToolStockDetail_Info a"
                + " Where ID=" + ID.ToString();
            return DataCommon.GetDataByReader(sSQL);
        }

        public static int UpdateTestCodeByDetailID(int ID, string TestCode, string NextTestDate)
        {
            string sSQL = " Update ToolStockDetail_Info Set TestCode='" + TestCode + "'";
            if (NextTestDate.Length > 0)
            {
                sSQL += ", NextTestDate='" + NextTestDate + "'";
            }
            else
            {
                sSQL += ", NextTestDate=null";
            }
            sSQL += " Where ID=" + ID.ToString();
            return DataCommon.QueryData(sSQL);
        }

        public static DataSet GetToolStockDetailByCateory(int OrganID, int CategoryID, string sWhereSQL)
        {
            string sSQL = " select a.ID as ToolDetailID, a.ToolID, a.ToolCode, a.TestCode, c.StorageLocation, a.ToolDate"
                + ", (Case a.ToolStatus when 0 then '库存' when 1 then '领用' when 2 then '借用'"
                + " when 3 then '送检' when 4 then '送修' when 5 then '注销' when 5 then '注销' when 6 then '损益' when 7 then '发货' when 9 then '调出' end) as ToolStatusName"
                + ", (Case a.TestStatus when 0 then '待检' when 1 then '合格' when 2 then '不合格' end) as TestStatusName, a.TestDate"
                + ", a.TestStatus"
                + " ,b.* from ToolStockDetail_Info a"
                + " left join Tool_Info b on a.ToolID=b.ID"
                + " left join ToolStock_Info c on a.ToolID=c.ToolID And c.OrganID=" + OrganID.ToString()
                + " Where a.OrganID=" + OrganID.ToString()  + sWhereSQL;

            if (CategoryID > 0)
            {
                sSQL += " And b.CategoryID=" + CategoryID;
            }
            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetWorkShopToolStockByDataSet(int OrganID, int CategoryID, string sWhereSQL)
        {
            string sSQL = "Select a.*, IsNull(b.ToolID, 0) As ToolID"
                + ", IsNull(b.InitQuantity, 0) As InitQuantity"
                + ", IsNull(b.Quantity, 0) As Quantity"
                + ", IsNull(b.ConsumeQuantity, 0) As ConsumeQuantity"
                + ", IsNull(b.BorrowQuantity, 0) As BorrowQuantity"
                + ", IsNull(b.ServiceQuantity, 0) As ServiceQuantity"
                + ", IsNull(b.TestQuantity, 0) As TestQuantity"
                + ", IsNull(b.CancelQuantity, 0) As CancelQuantity"
                + ", IsNull(b.ISQuantity, 0) As ISQuantity"
                + ", IsNull(b.SendOutQuantity, 0) As SendOutQuantity"
                + ", IsNull(b.ToolOutQuantity, 0) As ToolOutQuantity"
                + ", IsNull(b.Amount,0) As Amount "
                + ", IsNull(b.StorageLocation,'') As StorageLocation "
                + ", IsNull(b.WarningQuantity,0) as WarningQuantity, IsNull(b.LowerQuantity,0) as LowerQuantity"
                + " from Tool_Info a "
                + " Left Join ToolStock_Info b on b.Status=0 And a.ID=b.ToolID";
            
        
            if (OrganID > 0)
            {
                sSQL += " And b.OrganID=" + OrganID.ToString();
            }

            sSQL += " Where  a.Status=0" + sWhereSQL;


            if (CategoryID > 0)
            {
                sSQL = sSQL + " And a.ToolType=" + _NomalToolType.ToString() + " And a.CategoryID=" + CategoryID.ToString();

                //if (OrganID > 0)
                //{
                //    sSQL += " And b.OrganID=" + OrganID.ToString();
                //}
            }
            else if (CategoryID == -99)
            {
                sSQL = sSQL + " And a.ToolType=" + SysPackTool._PackToolType.ToString() + " And a.OrganID in (" + SysOrgan._TopOrganID.ToString() + "," + OrganID.ToString() + ")";
                //if (OrganID > 0)
                //{
                //    sSQL += " And b.OrganID=" + OrganID.ToString();
                //}
            }

            sSQL = sSQL + " Order By a.ToolNo, a.ToolName";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetSingleToolStockByReader(int OrganID, int ToolID)
        {
            string sSQL = "Select a.*, IsNull(b.ToolID, 0) As ToolID, IsNull(b.Quantity, 0) As Quantity, IsNull(b.Amount,0) As Amount "
                + ", IsNull(b.InitQuantity,0) as InitQuantity"
                + ", IsNull(b.StorageLocation,'') as StorageLocation"
                + ", IsNull(b.WarningQuantity,0) as WarningQuantity, IsNull(b.LowerQuantity,0) as LowerQuantity, b.Description as StockDescription from Tool_Info a "
                + " Left Join ToolStock_Info b on b.Status=0 And a.ID=b.ToolID";
            if (OrganID > 0)
            {
                sSQL += " And b.OrganID=" + OrganID.ToString();
            }
            sSQL += " Where a.ToolType='" + 0 + "' And a.Status=0";
            if (ToolID > 0)
            {
                sSQL = sSQL + " And a.ID=" + ToolID.ToString();
            }

            sSQL = sSQL + " Order By a.SortID, a.ToolNo, a.ToolName";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static int GetInitQuantityByReader(int OrganID, int ToolID)
        {
            int _InitQuantity = 0;
            SqlDataReader sdr = GetSingleToolStockByReader(OrganID, ToolID);
            if (sdr.Read())
            {
                _InitQuantity = int.Parse(sdr["InitQuantity"].ToString());
            }
            sdr.Close();

            return _InitQuantity;
        }

        //设置预警
        public static int UpdateToolInitInfo(int _ToolID, int _OrganID, int InitQuantity, string StorageLocation, string Description)
        {

            string sSql = "if Exists(Select 1 from ToolStock_Info Where ToolID=" + _ToolID.ToString() + " And OrganID=" + _OrganID.ToString() + ")";
            sSql += "  begin";

            sSql += " update ToolStock_Info Set Quantity=IsNull(Quantity,0) -InitQuantity Where ToolID=" + _ToolID.ToString() + " And OrganID=" + _OrganID.ToString() + " ";

            sSql += " update ToolStock_Info Set InitQuantity='" + InitQuantity.ToString() + "', StorageLocation='" + StorageLocation.ToString()
                 + "'  ,Quantity=IsNull(Quantity,0) + " + InitQuantity.ToString() + ", Description='" + Description + "' Where ToolID=" + _ToolID.ToString() + " And OrganID=" + _OrganID.ToString() + " ";

            sSql += "end "
                + " else "
                + " begin ";

            sSql += " Insert into ToolStock_Info (ToolID,OrganID, InitQuantity, Quantity, StorageLocation, Description) Values(" + _ToolID.ToString()
                + ", " + _OrganID.ToString() + ", " + InitQuantity.ToString() 
                + ", " + InitQuantity.ToString() + ", '" + StorageLocation.ToString()
                + "', '" + Description.ToString() + "') "
                + " end; ";

            if (GetIsCodeByToolID(_ToolID))
            {
                int _OldInitQuantity = GetInitQuantityByReader(_OrganID, _ToolID);
                string _ToolNo = GetToolNoByToolID(_ToolID);
                int _ToolMax = GetMaxNumByToolID(_ToolID);
                for (int i = 1; i <= InitQuantity - _OldInitQuantity; i++)
                {
                    sSql += " Insert Into ToolStockDetail_Info (OrganID, ToolID, ToolCode) Values(" + SysGlobal.GetCurrentUserOrganID().ToString()
                        + "," + _ToolID.ToString() + ",'"+ GetToolCodeByToolNo(_ToolNo,_ToolMax+i)   +"');";
                }
                sSql += GetUpdateAddNumSQL(_ToolID, InitQuantity);
            }

            return DataCommon.QueryData(sSql);
        }

        public static int InitStockDetail()
        {
            string sSql = "select b.ToolID, b.OrganID, b.InitQuantity, IsNull(c.ACount, 0) OldInitQuantity  from tool_Info a, ToolStock_Info b "
                + " left join (Select OrganID, ToolID,Count(1) as ACount From ToolStockDetail_Info group by OrganID, ToolID) c on b.OrganID=c.OrganID and b.ToolID=c.ToolID"
                + " Where a.IsCode=1 And a.ID=b.ToolID And b.InitQuantity>IsNull(c.ACount, 0) And b.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString();
            SqlDataReader sdr = DataCommon.GetDataByReader(sSql);
            while (sdr.Read())
            {                
                int InitQuantity = int.Parse(sdr["InitQuantity"].ToString());
                int _OldInitQuantity = int.Parse(sdr["OldInitQuantity"].ToString());

                string _ToolNo = GetToolNoByToolID(int.Parse(sdr["ToolID"].ToString()));
                int _ToolMax = GetMaxNumByToolID(int.Parse(sdr["ToolID"].ToString()));

                for (int i = 1; i <= InitQuantity - _OldInitQuantity; i++)
                {
                   string sSql1 = " Insert Into ToolStockDetail_Info (OrganID, ToolID, ToolCode) Values(" + sdr["OrganID"].ToString()
                        + "," + sdr["ToolID"].ToString() + ",'" + GetToolCodeByToolNo(_ToolNo, _ToolMax + i) + "');";

                   DataCommon.QueryData(sSql1);
                }
            }
            sdr.Close();

            return 1;
        }
    }
}
