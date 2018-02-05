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
using System.Data.SqlClient;
using CyxPack.CommonOperation;
using CyxPack.OperateSqlServer;
using CyxPack.UserCommonOperation;
using System.Text.RegularExpressions;
using System.Net;
using System.Web.SessionState;


namespace JtgTMS.SysClass
{
    public class InterPhoneInsation
    {
        public static string InterPhoneInspection_TableName = "InterPhoneInspection_Info";
        public static int Consume_Draft = 0, Consume_ApprovalIsOK = 1, ConsumeType_ConsumeValue = 0;
        //检验单单查询
        public static SqlDataReader GetSingleInspectionByReader(int _ConsumeID)
        {
            string sSQL = "Select a.*"
                + ", b.OpCode as ConsumeOpCode,  b.OpName as ConsumeOpName, c.OrganName, d.OpName as CreateOpName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from InterPhoneInspection_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.ConsumeUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                + " Where a.Status=0 And a.ID=" + _ConsumeID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }
        // 检验单明细
        public static DataSet GetInspectionInfoListBySer(int _ConsumeID)
        {
            string Ssql = "select  a.*,b.SerialNum,c.AliasesName as b from InterPhoneInsectionDetail_Info a left join Interphone_Info b on a.InterID=b.ID left join Tool_Info c on a.InterID=c.ID  where a.ApplyListID=" + _ConsumeID + " ";
            return DataCommon.GetDataByDataSet(Ssql);
        }
        public static Boolean CheckConsumeNoExists(int ConsumeID, string ConsumeNo)
        {
            string sSqlText = "Select 1 From InterPhoneInspection_Info Where ConsumeType=" + ConsumeType_ConsumeValue.ToString()
                + " And ConsumeNo='" + ConsumeNo + "' And ID<>" + ConsumeID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }
        //添加修改检验单
        public static int UpUserInspection(int _ID, string[] FieldValues, string Sql)
        {
            string sSqlText = "";
            if (_ID > 0)
            {
                sSqlText = sSqlText + " UPDATE InterPhoneInspection_Info SET ConsumeNo='" + FieldValues.GetValue(1) + "'"
                     + ",ConsumeUserID=" + FieldValues.GetValue(2) + ""
                     + ",Description='" + FieldValues.GetValue(3) + "'"
                     + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";

                sSqlText = sSqlText + Sql + " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into InterPhoneInspection_Info (TableRecGuid"
                    + ", ConsumeNo"
                    + ", ConsumeType"
                    + ", ConsumeDate"
                    + ", OrganID"
                    + ", ConsumeUserID"
                    + ", CreateUserID"
                    + ", Description"
                    + ", ApprovalStatus)"
                    + " Values('" + FieldValues.GetValue(0) + "'"
                    + ",'" + FieldValues.GetValue(1) + "'"
                    + "," + ConsumeType_ConsumeValue.ToString()
                    + ",GetDate()"
                    + "," + SysGlobal.GetCurrentUserOrganID().ToString()
                    + "," + FieldValues.GetValue(2) + ""
                    + "," + SysGlobal.GetCurrentUserID().ToString() + ""
                    + ",'" + FieldValues.GetValue(3) + "'"
                    + "," + FieldValues.GetValue(4) + ")";
                sSqlText = sSqlText + Sql + " ;";
            }

            return DataCommon.QueryData(sSqlText);
        }
        //工具借用单列表
        public static DataSet GetInspectionListByset(string sql)
        {
            string SSql = "select a.* ,b.OpName,case a.ApprovalStatus when 0 then '草稿' when 1 then '送检成功'end as a "
                + " from InterPhoneInspection_Info a "
                + " left join  SysUser_Info b on a.ConsumeUserID=b.ID where a.status=0 ";
            SSql = SSql + sql;
            SSql = SSql + "order by a.ConsumeDate desc";
            return DataCommon.GetDataByDataSet(SSql);
        }
        //删除检验单
        public static int DeleteInspection(string _IDs)
        {
            string sSQL = "begin Delete from InterPhoneInspection_Info Where ID in (" + _IDs.ToString() + "); ";
            sSQL += "update Interphone_Info set ConditionID =0 where ID in (select InterID from InterPhoneInsectionDetail_Info where ApplyListID IN(" + _IDs.ToString() + ") )";
            sSQL += " Delete From InterPhoneInsectionDetail_Info Where ApplyListID in (" + _IDs.ToString() + ");";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }
        public static SqlDataReader GetSingleInspectionreByReader(int _ConsumeID)
        {
            string sSQL = "Select a.*"
                + ", b.OpCode as ConsumeOpCode,  b.OpName as ConsumeOpName, c.OrganName, d.OpName as CreateOpName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from InterPhoneInsReturn_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.ConsumeUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                + " Where a.Status=0 And a.ID=" + _ConsumeID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }
        // 检验单明细
        public static DataSet GetInspectionInfoListreBySer(int _ConsumeID)
        {
            string Ssql = "select  a.*,b.SerialNum,c.AliasesName as b from InterphoneInsReturnDetail_info a left join Interphone_Info b on a.InterID=b.ID  left join WorkInterInspectionDetail_Info d on a.InterID=d.ID left join Interphone_Info e on d.InterID=e.ID left join Tool_Info c on e.ModeID=c.ID where a.ApplyListID=" + _ConsumeID + " ";
            return DataCommon.GetDataByDataSet(Ssql);
        }
        //添加修改退还检验单
        public static int UpUserInspectionRe(int _ID, string[] FieldValues, string Sql)
        {
            string sSqlText = "";
            if (_ID > 0)
            {
                sSqlText = sSqlText + " UPDATE InterPhoneInsReturn_Info SET ConsumeNo='" + FieldValues.GetValue(1) + "'"
                     + ",ConsumeUserID=" + FieldValues.GetValue(2) + ""
                     + ",Description='" + FieldValues.GetValue(3) + "'"
                     + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";

                sSqlText = sSqlText + Sql + " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into InterPhoneInsReturn_Info (TableRecGuid"
                    + ", ConsumeNo"
                    + ", ConsumeType"
                    + ", ConsumeDate"
                    + ", OrganID"
                    + ", ConsumeUserID"
                    + ", CreateUserID"
                    + ", Description"
                    + ", ApprovalStatus)"
                    + " Values('" + FieldValues.GetValue(0) + "'"
                    + ",'" + FieldValues.GetValue(1) + "'"
                    + "," + ConsumeType_ConsumeValue.ToString()
                    + ",GetDate()"
                    + "," + SysGlobal.GetCurrentUserOrganID().ToString()
                    + "," + FieldValues.GetValue(2) + ""
                    + "," + SysGlobal.GetCurrentUserID().ToString() + ""
                    + ",'" + FieldValues.GetValue(3) + "'"
                    + "," + FieldValues.GetValue(4) + ")";
                sSqlText = sSqlText + Sql + " ;";
            }

            return DataCommon.QueryData(sSqlText);
        }
        //工具借用单列表
        public static DataSet GetBorrowListByset(string sql)
        {
            string SSql = "select a.* ,b.OpName,case a.ApprovalStatus when 0 then '草稿' when 1 then '已退回'end as a from InterPhoneInsReturn_Info a left join  SysUser_Info b on a.ConsumeUserID=b.ID where a.status=0 ";
            SSql = SSql + sql;
            SSql = SSql + "order by a.ConsumeDate desc";
            return DataCommon.GetDataByDataSet(SSql);
        }
        public static int DeleteBorrows(string _IDs)
        {
            string sSQL = "begin Delete from InterPhoneInsReturn_Info Where ID in (" + _IDs.ToString() + "); ";
            sSQL += " Delete From InterphoneInsReturnDetail_info Where ApplyListID in (" + _IDs.ToString() + ");";

            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }
        //select 
        public static DataSet GetAllinter(int modeID, int OrganID)
        {
            string Ssql = "select a.*,b.ConsumeNo,c.SerialNum,d.Specification,d.AliasesName,e.OrganName from WorkInterCarryDetail_Info a left join WorkInterCarry_Info b on a.ApplyListID=b.ID left join Interphone_Info c on a.InterID=c.ID left join Tool_Info d on c.ModeID=D.ID left join SysOrgan_Info e on c.OrgainID=e.ID where b.ApprovalStatus=1  and a.IsReturn=1 ";
            if (modeID > 0)
            {
                Ssql = Ssql + " and c.ModeID=" + modeID + "";
            }
            Ssql = Ssql + Ssql;
            return DataCommon.GetDataByDataSet(Ssql);
        }

        public static DataSet GetInsAllinter(int modeID, int OrganID)
        {
            string Ssql = "select a.*,b.ConsumeNo,c.SerialNum,d.Specification,d.AliasesName,e.OrganName from InterPhoneInsectionDetail_Info a"
                + " left join InterPhoneInspection_Info b on a.ApplyListID=b.ID "
                + " left join Interphone_Info c on a.InterID=c.ID "
                + " left join Tool_Info d on c.ModeID=D.ID left "
                + " join SysOrgan_Info e on c.OrgainID=e.ID where b.ApprovalStatus=1  and a.IsReturn=1 ";

            //string Ssql = "select a.*, b.Toolname, b.AliasesName, c.OrganName, b.Specification from Interphone_Info a"
            //    + " left join Tool_Info b on b.SystemID=1 And a.ModeID=b.ID"
            //    + " left join SysOrgan_Info c on a.OrgainID=c.ID"
            //    + " where a.ConditionID=4 And a.OrgainID=0";
            if (modeID > 0)
            {
                Ssql = Ssql + " and c.ModeID=" + modeID + "";
            }
            Ssql = Ssql + Ssql;
            return DataCommon.GetDataByDataSet(Ssql);
        }
        public static SqlDataReader GetinterListInfoByreader(string a)
        {
            string ssql = "select a.*,b.SerialNum,c.AliasesName as BrandNames from InterPhoneInsectionDetail_Info a left join Interphone_Info b on a.InterID=b.ID left join Tool_Info c on b.ModeID=c.ID where b.Status=0";
            ssql = ssql + a;
            return DataCommon.GetDataByReader(ssql);
        }

        //查询送检库存
        public static DataSet GetInStockBySet(int CategoryID, string sWhereSQL)
        {
            string Ssql = "select a.*,b.ConsumeNo,c.SerialNum,d.Specification,d.AliasesName,e.OrganName from InterPhoneInsectionDetail_Info a "
                + " left join InterPhoneInspection_Info b on a.ApplyListID=b.ID "
                + " left join Interphone_Info c on a.InterID=c.ID left join Tool_Info d on c.ModeID=D.ID "
                + " left join SysOrgan_Info e on c.OrgainID=e.ID where b.ApprovalStatus=1 And a.IsReturn=1";

            if (CategoryID > 0)
            {
                Ssql = Ssql + " and a.ModeID=" + CategoryID + "";
            }
            Ssql = Ssql + sWhereSQL;
            return DataCommon.GetDataByDataSet(Ssql);
        }

        //查询送检库存
        public static SqlDataReader GetNotReturInsLstByReader(int UserID, string sWhereSQL)
        {
            string sSQL = "select a.*,b.ConsumeNo,c.SerialNum,d.Specification,d.AliasesName,e.OrganName, f.OpName,b.ConsumeDate from InterPhoneInsectionDetail_Info a "
                + " left join InterPhoneInspection_Info b on a.ApplyListID=b.ID "
                + " left join Interphone_Info c on a.InterID=c.ID left join Tool_Info d on c.ModeID=D.ID "
                + " left join SysOrgan_Info e on c.OrgainID=e.ID  "
                + " left join SysUser_Info f on b.ConsumeUserID=f.ID "
                + " where b.ApprovalStatus=1 And a.IsReturn=1";

            if (UserID > 0)
            {
                sSQL += " And IsNull(b.ConsumeUserID,0)=" + UserID.ToString();
            }

            sSQL += sWhereSQL;

            sSQL += " Order By b.ConsumeDate";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetReturnInsListInfo(string WhereSQL)
        {
            string sSQL = "select a.*,b.ConsumeNo,c.SerialNum,d.Specification,d.AliasesName,e.OrganName, f.OpName,b.ConsumeDate from InterPhoneInsectionDetail_Info a "
                + " left join InterPhoneInspection_Info b on a.ApplyListID=b.ID "
                + " left join Interphone_Info c on a.InterID=c.ID left join Tool_Info d on c.ModeID=D.ID "
                + " left join SysOrgan_Info e on c.OrgainID=e.ID  "
                + " left join SysUser_Info f on b.ConsumeUserID=f.ID "
                + " where b.ApprovalStatus=1 And a.IsReturn=1" + WhereSQL;
            return DataCommon.GetDataByReader(sSQL);
        }

        //无线电台领用归还操作
        public static int ReturnInterPhone(string interID)
        {
            string ssql = "update InterPhoneInsectionDetail_Info set IsReturn =0 where ID in(" + interID + ");";
            ssql = ssql + "update Interphone_Info set ConditionID=0 where ID in(select InterID from InterPhoneInsectionDetail_Info where ID in(" + interID + "));";
            return DataCommon.QueryData(ssql);
        }
       
    }
}
