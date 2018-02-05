using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CyxPack.CommonOperation;
using CyxPack.OperateSqlServer;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;

namespace JtgTMS.SysClass
{
    public class SysUserSalary
    {

        //工资单查询
        public static string UserSalary_UserSalaryOpCode = "", UserSalary_UserSalaryOpName = "", UserSalary_UserSalaryUserID = "";
        public static string UserSalary_SearchText = "";
        public static string UserSalary_ApprovalStatus = "";
        //Add by lk 20151214 start
        public static int UserSalary_ImportRecIndex = 0;
        public static string UserSalary_ImportRecValue = "";
        public static string UserSalary_ImportRecText = "";
        public static int UserSalary_ImportRecIndex2 = 0;
        public static string UserSalary_ImportRecValue2 = "";
        public static string UserSalary_ImportRecText2 = "";
        public static string UserSalary_SearchText2 = "";
        public static string UserSalary_SignStatus = "";
        //Add by lk 20151214 end

        //Add by lk 20160118 start
        public static int UserSalary_FlgValue = 0;
        //Add by lk 20160118 end

        //工具领用确认
        public static string UserSalaryApproval_SearchText = "";

        public static int UserSalary_Draft = 0, UserSalary_ApprovalIsOK = 1, UserSalaryType_UserSalaryValue = 0;

        public static string Sonsume_TableName = "UserSalary_Info", Borrow_TableName = "Borrow_Info";

        public static string GetTableRecGuidByID(int _UserSalaryID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSingleUserSalaryByReader(_UserSalaryID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static string GetTableRecGuidBySetID(int _UserSalaryID)
        {
            string _TableRecGuid = "";
            SqlDataReader sdr = GetSingleUserSalarySetByReader(_UserSalaryID);
            if (sdr.Read())
            {
                _TableRecGuid = sdr["TableRecGuid"].ToString();
            }
            sdr.Close();
            return _TableRecGuid;
        }

        public static DataSet GetSalaryNotSignLstByDataSet(string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "select b.OpName, b.Sex, b.IdNumber, a.*, DateDiff(Day, a.SalaryDate, GetDate()) As NotSingDay, c.OrganName"
                + " from UserSalary_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OpCode=b.OpCode"
                + " left join SysOrgan_Info c on c.Status=0 And b.OrganID=c.ID"
                + " Where a.Status=0 " + sWhereSQL;

            //Upd by lk 20151214 start
            //sSQL = sSQL + " Order By DateDiff(Day, a.SalaryDate, GetDate()) Desc";
            sSQL = sSQL + " Order By DateDiff(Day, a.SalaryDate, GetDate()) Desc,c.OrganName,b.opName,a.TotalSalary";
            //Upd by lk 20151214 end

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetUserSalaryLstByDataSet(string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "select a.OpCode, b.OpName, b.Sex, b.IdNumber, a.SalaryYears,a.SalaryDate,a.TotalSalary,a.SignStatus,a.SignDate,a.ID, c.Description"
                + " from UserSalary_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OpCode=b.OpCode"
                + " left join UserUserImportRec_Info c on c.SalaryRecGuid=a.SalaryRecGuid"
                + " Where a.Status=0 " + sWhereSQL;
            
            sSQL = sSQL + " Order By a.SalaryYears Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static DataSet GetTopUserSalaryLstByDataSet(string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "select Top 6 b.OpName, b.Sex, b.IdNumber, a.*"
                + " from UserSalary_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OpCode=b.OpCode"
                + " Where a.Status=0 " + sWhereSQL;

            sSQL = sSQL + " Order By a.SalaryYears Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetUserSalaryLstByReader(string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "select b.OpName, b.Sex, b.IdNumber, a.*"
                + " from UserSalary_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OpCode=b.OpCode"
                + " Where a.Status=0 " + sWhereSQL;

            sSQL = sSQL + " Order By a.SalaryYears Desc";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSalaryFieldsLstByReader()
        {
            string sSQL = "";
            sSQL = "Select a.* From UserSalaryFields_Info a"                
                + " Where Status=0 ";
            sSQL = sSQL + " Order By a.SortID, a.ID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetUserSalaryFieldsLstByReader(string sYears)
        {
            string sSQL = "";
            sSQL = "Select a.*, b.UserFieldTitle From UserSalaryFields_Info a"
                + " Left Join (select b.* from UserSalarySet_Info a, UserSalarySet_Fields_Info b "
                + " Where a.ID=b.MasterID And a.BeginYears<='" + sYears + "' And a.EndYears>='" + sYears + "' ) b"
                + " on a.FieldName=b.FieldName"
                + " Where IsNull(b.UserIsVisible,2) = 1 OR ((IsNull(b.UserIsVisible,2) = 2) And (IsNull(a.IsVisible,0) = 1))";
            sSQL = sSQL + " Order By b.SortID, a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetUserSalaryFieldsLstByReader(string sYears, string SalaryRecGuid, string WhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.*, b.UserFieldTitle From UserSalaryFields_Info a"
                + " Left Join (select b.* from UserSalarySet_Info a, UserSalarySet_Fields_Info b "
                + " Where a.ID=b.MasterID And a.BeginYears<='" + sYears + "' And a.EndYears>='" + sYears + "' ) b"
                + " on a.FieldName=b.FieldName"
                + " left join UserUserImportRecFields_Info c on c.SalaryRecGuid='" + SalaryRecGuid + "' and a.FieldName=c.FieldName"
                + " Where (IsNull(b.UserIsVisible,2) = 1 OR ((IsNull(b.UserIsVisible,2) = 2) And (IsNull(a.IsVisible,0) = 1))) And len(c.FieldName)>0 " + WhereSQL;
            sSQL = sSQL + " Order By b.SortID, a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static DataSet GetUserSalaryDetailsLstByDataSet(int UserSalaryID, string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName,"
                + " b.Specification, b.MaterialCode,b.Unit, IsNull(c.Quantity,0) As StockQuantity, a.Quantity As OldQuantity "
                + ", c.StorageLocation, d.ToolCode, d.TestCode"
                + " from UserSalaryDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " left Join ToolStock_Info c on c.Status=0 And c.OrganID=" + SysGlobal.GetCurrentUserOrganID().ToString() + " and c.ToolID=a.ToolID"
                + " left join ToolStockDetail_Info d on d.Status=0 And a.ToolDetailID=d.ID"
                + " Where IsNull(a.UserSalaryID,0)=" + UserSalaryID.ToString() 
                + " And a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        public static SqlDataReader GetUserSalaryDetailsLstByReader(int UserSalaryID, string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification, b.MaterialCode,b.Unit  from UserSalaryDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " Where a.Status=0" + WhereSQL;

            if (UserSalaryID > 0)
            {
                sSQL += " And IsNull(a.UserSalaryID,0)=" + UserSalaryID.ToString() ;
            }

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetUserSalaryDetailsLstByReader(string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName"
                + " , b.Specification, b.MaterialCode,b.Unit, c.UserSalaryDate"
                + ", (Case c.UserSalaryType when 0 then '领用' when 1 then '借用' end) As UserSalaryTypeName"
                + ", d.ToolCode"
                + " from UserSalaryDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " left join UserSalary_Info c on c.Status=0 And c.ID=a.UserSalaryID"
                + " left join ToolStockDetail_Info d on d.ID=a.ToolDetailID"
                + " Where a.Status=0" + WhereSQL;

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static Boolean CheckUserSalaryNoExists(int UserSalaryID, string UserSalaryNo)
        {
            string sSqlText = "Select 1 From UserSalary_Info Where UserSalaryType=" + UserSalaryType_UserSalaryValue.ToString() 
                + " And UserSalaryNo='" + UserSalaryNo + "' And ID<>" + UserSalaryID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }

        //更新添加工具档案信息
        public static int UpdateSingleUserSalary(int _UserSalaryID, string[] FieldValues, string DetailsSQL, string NewFields, string NewFieldValues)
        {
            string sSqlText = "begin";
        
            if (_UserSalaryID > 0)
            {
                sSqlText = sSqlText + " UPDATE UserSalary_Info SET TableRecGuid='" + FieldValues.GetValue(0) + "'"
                     +",  OpCode='" + FieldValues.GetValue(1) + "'"
                     + ",SalaryYears=" + FieldValues.GetValue(2) + "" + DetailsSQL;                     
                sSqlText = sSqlText + " WHERE ID=" + _UserSalaryID + "" + ";";

                sSqlText += " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into UserSalary_Info (TableRecGuid"
                    + ", OpCode"
                    + ", SalaryYears"
                    + NewFields
                    + " )"
                    + " Values('" + FieldValues.GetValue(0) + "'"
                    + ",'" + FieldValues.GetValue(1) + "'"
                    + ",'" + FieldValues.GetValue(2) + "'"
                    + NewFieldValues
                    + ")";
                sSqlText = sSqlText + " ;";
            }

            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static SqlDataReader GetSingleUserSalaryByReader(int _UserSalaryID)
        {
            string sSQL = "";
            sSQL = "select b.OpName, b.Sex, b.IdNumber, a.*"
                + " from UserSalary_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.OpCode=b.OpCode"
                + " Where a.Status=0 And a.ID=" + _UserSalaryID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //删除工具申请单
        public static int DeleteSingleUserSalary(string _UserSalaryIDs)
        {
            string sSQL = "begin ";
            
            sSQL += " Delete from UserSalary_Info Where ID in (" + _UserSalaryIDs.ToString() + "); ";

            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        //删除工具申请单
        public static int CancelSingleUserSalary(string _UserSalaryIDs)
        {
            string sSQL = "begin ";

            sSQL += " Update UserSalary_Info Set SignStatus=0 Where ID in (" + _UserSalaryIDs.ToString() + "); ";

            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static SqlDataReader GetSingleUserSalarySetByReader(int _UserSalaryID)
        {
            string sSQL = "";
            sSQL = "select a.*"
                + " from UserSalarySet_Info a "
                + " Where a.Status=0 And a.ID=" + _UserSalaryID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        //删除工具申请单
        public static int DeleteSingleUserSalarySet(string _UserSalaryIDs)
        {
            string sSQL = "begin ";


            sSQL += " Delete from UserSalarySet_Info Where ID in (" + _UserSalaryIDs.ToString() + "); ";
            sSQL = sSQL + " Delete from UserSalarySet_Fields_Info Where MasterID in (" + _UserSalaryIDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static DataSet GetUserSalarySetLstByDataSet(string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "select a.*"
                + " from UserSalarySet_Info a "
                + " Where a.Status=0 " + sWhereSQL;

            sSQL = sSQL + " Order By a.BeginYears Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //更新添加工具档案信息
        public static int UpdateSingleUserSalarySet(int _UserSalaryID, string[] FieldValues, string DetailsSQL)
        {
            string sSqlText = "begin";

            if (_UserSalaryID > 0)
            {
                sSqlText = sSqlText + " UPDATE UserSalarySet_Info SET TableRecGuid='" + FieldValues.GetValue(0) + "'"
                     + ", BeginYears='" + FieldValues.GetValue(1) + "'"
                     + ", EndYears='" + FieldValues.GetValue(2) + "'"
                     + ", Description='" + FieldValues.GetValue(3) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _UserSalaryID + "" + ";";

                sSqlText += " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into UserSalarySet_Info (TableRecGuid"
                    + ", BeginYears"
                    + ", EndYears"
                    + ", Description"
                    + " )"
                    + " Values('" + FieldValues.GetValue(0) + "'"
                    + ",'" + FieldValues.GetValue(1) + "'"
                    + ",'" + FieldValues.GetValue(2) + "'"
                    + ",'" + FieldValues.GetValue(3) + "'"
                    + ")";
                sSqlText = sSqlText + " ;";
            }

            sSqlText += DetailsSQL + " end;";
            return DataCommon.QueryData(sSqlText);
        }

        public static DataSet GetUserSalarySetFieldsLstByDataSet(int MasterID, string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "select a.*, b.FieldType"
                + " from UserSalarySet_Fields_Info a "
                + " Left join UserSalaryFields_Info b on a.FieldName=b.FieldName"
                + " Where a.Status=0 And MasterID=" + MasterID.ToString() + sWhereSQL;

            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //获取车站列表至控件
        public static void FullToSalaryFieldsLst(DropDownList ddlList,string SalaryYears)
        {
            ddlList.Items.Clear();

            ListItem liItem = new ListItem();
            liItem.Value = "";
            liItem.Text = "无";
            ddlList.Items.Add(liItem);

            liItem = new ListItem();
            liItem.Value = "OpCode";
            liItem.Text = "工资号";
            ddlList.Items.Add(liItem);

            SqlDataReader sdr = GetUserSalaryFieldsLstByReader(SalaryYears);
            while (sdr.Read())
            {
                liItem = new ListItem();
                liItem.Value = sdr["FieldName"].ToString();
                liItem.Text = sdr["UserFieldTitle"].ToString();
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }

        public static DataSet GetUserSalaryFieldsImportLstByDataSet(string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.* From UserSalaryImport_Info a Where 1=1 "
                + sWhereSQL;
            return DataCommon.GetDataByDataSet(sSQL);
        }

        //更新添加工具档案信息
        public static int UpdateSignByID(string _UserSalaryIDs, string _Description)
        {
            string sSqlText = "begin";

          
                sSqlText = sSqlText + " UPDATE UserSalary_Info SET "
                     + "  SignStatus=1"
                     + " ,SignDate=GetDate()"
                     + " , Description='" + _Description + "'";
                sSqlText = sSqlText + " WHERE ID in (" + _UserSalaryIDs + ")" + ";";

                sSqlText += " end;";

            return DataCommon.QueryData(sSqlText);
        }

        //获取车站列表至控件
        public static void FullToSalaryImportRecLst(DropDownList ddlList, string SalaryYears)
        {
            ddlList.Items.Clear();

            ListItem liItem = new ListItem();
            liItem.Value = "";
            liItem.Text = "新导入";
            ddlList.Items.Add(liItem);

            SqlDataReader sdr = GetUserImportRecLstByReader(SalaryYears);
            while (sdr.Read())
            {
                liItem = new ListItem();
                liItem.Value = sdr["SalaryRecGuid"].ToString();
                liItem.Text = "覆盖 " + sdr["SalaryDate"].ToString();
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }

        //获取车站列表至控件
        public static void FullToSalaryImportRecLst2(DropDownList ddlList, string SalaryYears)
        {
            ddlList.Items.Clear();

            ListItem liItem = new ListItem();
            liItem.Value = "";
            liItem.Text = "批次";
            ddlList.Items.Add(liItem);

            SqlDataReader sdr = GetUserImportRecLstByReader(SalaryYears);
            while (sdr.Read())
            {
                liItem = new ListItem();
                liItem.Value = sdr["SalaryRecGuid"].ToString();
                liItem.Text = sdr["SalaryDate"].ToString();
                ddlList.Items.Add(liItem);
            }
            sdr.Close();
        }

        public static SqlDataReader GetUserImportRecLstByReader(string sYears)
        {
            string sSQL = "";
            sSQL = "Select a.* From UserUserImportRec_Info a"
                + " Where SalaryYears='"+ sYears + "'";
            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetUserImportRecLstByWhere(string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "Select a.* From UserUserImportRec_Info a"
                + " Where 1=1 " + sWhereSQL ;
            sSQL = sSQL + " Order By a.SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        //删除工具申请单
        public static int DeleteSingleUserImportRec(string _UserSalaryIDs)
        {
            string sSQL = "begin ";

            sSQL += " Delete from UserUserImportRec_Info Where ID in (" + _UserSalaryIDs.ToString() + "); ";


            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        //删除工具申请单
        public static int DeleteSingleUserImportRec(int _UserSalaryID)
        {
            string sSQL = "begin ";

            string _RecGuid = GetSalaryRecGuidByIMportRecID(_UserSalaryID);

            sSQL += " Delete from UserSalary_Info Where SalaryRecGuid='" + _RecGuid + "'; ";

            sSQL += " Delete from UserUserImportRec_Info Where ID = " + _UserSalaryID.ToString() + "; ";


            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static string GetSalaryRecGuidByIMportRecID(int _UserSalaryID)
        {
            string _RecGuid = "";

            SqlDataReader sdr = GetSingUserImportRecByID(_UserSalaryID);
            if (sdr.Read())
            {
                _RecGuid = sdr["SalaryRecGuid"].ToString();
            }
            sdr.Close();

            return _RecGuid;
        }

        public static SqlDataReader GetSingUserImportRecByID(int _UserSalaryID)
        {
            string sSQL = "";
            sSQL = "select a.*"
                + " from UserUserImportRec_Info a "
                + " Where a.Status=0 And a.ID=" + _UserSalaryID;

            sSQL = sSQL + " Order By SalaryDate Desc";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static DataSet GetUserImportRecLstByDataSet(string sWhereSQL)
        {
            string sSQL = "";
            sSQL = "select a.*"
                + " from UserUserImportRec_Info a "
                + " Where a.Status=0 " + sWhereSQL;

            sSQL = sSQL + " Order By SalaryDate Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //更新添加工具档案信息
        public static int UpdateUserImportRecAuditByID(string _UserSalaryIDs)
        {
            string sSqlText = "begin";


            sSqlText = sSqlText + " UPDATE UserUserImportRec_Info SET "
                 + "  ApprovalStatus=1"
                 + " ,ApprovalDate=GetDate()";
            sSqlText = sSqlText + " WHERE ID in (" + _UserSalaryIDs + ")" + ";";

            sSqlText += " end;";

            return DataCommon.QueryData(sSqlText);
        }

        //更新添加导入备注
        public static int UpdateUserImportRecDescriptionByID(string _UserSalaryIDs, string _Description)
        {
            string sSqlText = "begin";


            sSqlText = sSqlText + " UPDATE UserUserImportRec_Info SET "
                 + "  Description='" + _Description + "'";
            sSqlText = sSqlText + " WHERE ID in (" + _UserSalaryIDs + ")" + ";";

            sSqlText += " end;";

            return DataCommon.QueryData(sSqlText);
        }

        //Add by lk 20151214 start
        //更新添加工具档案信息
        public static int UpdateUserSalaryFieldsInfo(string strSQL)
        {
            string sSqlText = "begin ";

            sSqlText += strSQL;

            sSqlText += " end;";

            return DataCommon.QueryData(sSqlText);
        }
        //Add by lk 20151214 end
    }
}
