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
    public class WorksShopManager
    {
        public static int Consume_Draft = 0, Consume_ApprovalIsOK = 1, ConsumeType_ConsumeValue = 0;
        //-----------------------工具借用
        public static int UpUserBorrow(int _ID, string[] FieldValues, string Sql)
        {
            string sSqlText = "";
            if (_ID > 0)
            {
                sSqlText = sSqlText + " UPDATE WorkBorrowUser_Info SET ConsumeNo='" + FieldValues.GetValue(1) + "'"
                     + ",ConsumeUserID=" + FieldValues.GetValue(2) + ""
                     + ",Description='" + FieldValues.GetValue(3) + "'"
                     + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";

                sSqlText = sSqlText + Sql + " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into WorkBorrowUser_Info (TableRecGuid"
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

         public static DataSet GetAllinter(int modeID,int OrganID)
        {
            string ssql = "select a.*,b.AliasesName as BrandNames from Interphone_Info a "
             + " left join Tool_Info b on a.ModeID=b.ID  "
             + " where a.ModeID=" + modeID + " and OrgainID=" + OrganID + "  and ConditionID=0 and Isfinish=0";
           return DataCommon.GetDataByDataSet(ssql);
        }
         //借用
         public static DataSet GetBorrowInfoListBySer(int _ConsumeID)
         {
             string Ssql = "select  a.*,b.SerialNum,c.AliasesName as b from WorkBorrowuserDetail_Info a left join Interphone_Info b on a.InterID=b.ID left join Tool_Info c on a.InterID=c.ID  where a.ApplyListID=" + _ConsumeID + " ";
             return DataCommon.GetDataByDataSet(Ssql);
         }
         //借用单查询
         public static SqlDataReader GetSingleBorrowByReader(int _ConsumeID)
         {
             string sSQL = "Select a.*"
                 + ", b.OpCode as ConsumeOpCode,  b.OpName as ConsumeOpName, c.OrganName, d.OpName as CreateOpName"
                 + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                 + " from WorkBorrowUser_Info a "
                 + " left join SysUser_Info b on b.Status=0 And a.ConsumeUserID=b.ID"
                 + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                 + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                 + " Where a.Status=0 And a.ID=" + _ConsumeID.ToString();

             return DataCommon.GetDataByReader(sSQL);
         }
         //工具借用单列表
         public static DataSet GetBorrowListByset(string sql)
         {
             string SSql = "select a.* ,b.OpName,case a.ApprovalStatus when 0 then '草稿' when 1 then '领用成功'end as a from WorkBorrowUser_Info a left join  SysUser_Info b on a.ConsumeUserID=b.ID where a.status=0 ";
             SSql = SSql + sql;
             SSql = SSql + "order by a.ConsumeDate desc";
             return DataCommon.GetDataByDataSet(SSql);
         }
         //删除借用单
         public static int DeleteBorrows(string _IDs)
         {
             string sSQL = "begin Delete from WorkBorrowUser_Info Where ID in (" + _IDs.ToString() + "); ";
             sSQL += " Delete From WorkBorrowuserDetail_Info Where ApplyListID in (" + _IDs.ToString() + ");";

             sSQL = sSQL + " End;";
             return DataCommon.QueryData(sSQL);
         }

         //编辑申请单查询
         public static SqlDataReader GetSingleConsumeByReader(int _ConsumeID)
         {
             string sSQL = "Select a.*"
                 + ", b.OpCode as ConsumeOpCode,  b.OpName as ConsumeOpName, c.OrganName, d.OpName as CreateOpName"
                 + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                 + " from WorkApplyUser_Info a "
                 + " left join SysUser_Info b on b.Status=0 And a.ConsumeUserID=b.ID"
                 + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                 + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                 + " Where a.Status=0 And a.ID=" + _ConsumeID.ToString();

             return DataCommon.GetDataByReader(sSQL);
         }
         public static DataSet GetApplyInfoListBySer(int _ConsumeID)
         {
             string Ssql = "select  a.*,b.SerialNum,c.AliasesName as b from WorkApplyUserDetail_Info a left join Interphone_Info b on a.InterID=b.ID left join Tool_Info c on a.InterID=c.ID  where a.ApplyListID=" + _ConsumeID + " ";
             return DataCommon.GetDataByDataSet(Ssql);
         }

         public static int UpUserApply(int _ID, string[] FieldValues, string Sql)
         {
             string sSqlText = "";
             if (_ID > 0)
             {
                 sSqlText = sSqlText + " UPDATE WorkApplyUser_Info SET ConsumeNo='" + FieldValues.GetValue(1) + "'"
                      + ",ConsumeUserID=" + FieldValues.GetValue(2) + ""
                      + ",Description='" + FieldValues.GetValue(3) + "'"
                      + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                 sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";

                 sSqlText = sSqlText + Sql + " ;";
             }
             else
             {
                 sSqlText = sSqlText + " Insert Into WorkApplyUser_Info (TableRecGuid"
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
         //删除领用单
         public static int DeleteApplys(string _IDs)
         {
             string sSQL = "begin Delete from WorkApplyUser_Info Where ID in (" + _IDs.ToString() + "); ";
             sSQL += " Delete From WorkApplyUserDetail_Info Where ApplyListID in (" + _IDs.ToString() + ");";

             sSQL = sSQL + " End;";
             return DataCommon.QueryData(sSQL);
         }
         //工具领用单列表
         public static DataSet GetApplyListByset(string sql)
         {
             string SSql = "select a.* ,b.OpName,case a.ApprovalStatus when 0 then '草稿' when 1 then '领用成功'end as a from WorkApplyUser_Info a left join  SysUser_Info b on a.ConsumeUserID=b.ID  where a.status=0 ";
             SSql = SSql + sql;
             SSql = SSql + "order by a.ConsumeDate desc";
             return DataCommon.GetDataByDataSet(SSql);
         }
         //无线电台借用归还查询
         public static SqlDataReader GetNotReturnBorrowPhoneLstByReader(int UserID, string WhereSQL)
         {
             string sSQL = "select a.ID,a.TableRecGuid,a.InterID, a.BrandNames,b.ConsumeDate,c.OpName,d.OrganName,e.ModeID,e.SerialNum,f.Specification from  WorkBorrowuserDetail_Info a "
                 + "left join WorkBorrowUser_Info b on a.ApplyListID=b.ID left join SysUser_Info c on b.ConsumeUserID=c.ID "
                 + "left join  SysOrgan_Info d on d.ID=c.OrganID "
                 + "left join Interphone_Info e on a.InterID=e.ID "
                 + "left join Tool_Info f on e.ModeID=f.ID"
                 + " where  a.Status=0 and a.IsReturn=1 " + WhereSQL;

             if (UserID > 0)
             {
                 sSQL += " And IsNull(b.ConsumeUserID,0)=" + UserID.ToString();
             }

             sSQL += " and b.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString();

             sSQL = sSQL + " Order By b.ConsumeDate";

             return DataCommon.GetDataByReader(sSQL);
         }
         public static SqlDataReader GetReturnListinfoBorrow(string WhereSQL)
         {
             string sSQL = "select  a.BrandNames,b.ConsumeDate,e.SerialNum,f.Specification from  WorkBorrowuserDetail_Info a "
                + "left join WorkBorrowUser_Info b on a.ApplyListID=b.ID left join SysUser_Info c on b.ConsumeUserID=c.ID "
                + "left join  SysOrgan_Info d on d.ID=c.OrganID "
                + "left join Interphone_Info e on a.InterID=e.ID "
                + "left join Tool_Info f on e.ModeID=f.ID"
                + " where  a.Status=0 and a.IsReturn=1 " + WhereSQL;
             return DataCommon.GetDataByReader(sSQL);
         }
         //无线电台借用归还操作
         public static int ReturnInterPhoneBorrow(string interID)
         {
             string ssql = "update WorkBorrowuserDetail_Info set IsReturn =0 where ID in(" + interID + ");";
             ssql = ssql + "update Interphone_Info set ConditionID=0 where ID in(select InterID from WorkBorrowuserDetail_Info where ID in(" + interID + "));";
             return DataCommon.QueryData(ssql);
         }
         public static SqlDataReader GetReturnListinfo(string WhereSQL)
         {
             string sSQL = "select  a.BrandNames,b.ConsumeDate,e.SerialNum,f.Specification from  WorkApplyUserDetail_Info a "
                + "left join WorkApplyUser_Info b on a.ApplyListID=b.ID left join SysUser_Info c on b.ConsumeUserID=c.ID "
                + "left join  SysOrgan_Info d on d.ID=c.OrganID "
                + "left join Interphone_Info e on a.InterID=e.ID "
                + "left join Tool_Info f on e.ModeID=f.ID"
                + " where  a.Status=0 and a.IsReturn=1 " + WhereSQL;
             return DataCommon.GetDataByReader(sSQL);
         }
         //无线电台领用归还操作
         public static int ReturnInterPhone(string interID)
         {
             string ssql = "update WorkApplyUserDetail_Info set IsReturn =0 where ID in(" + interID + ");";
             ssql = ssql + "update Interphone_Info set ConditionID=0 where ID in(select InterID from WorkApplyUserDetail_Info where ID in(" + interID + "));";
             return DataCommon.QueryData(ssql);
         }
         //无线电台领用归还查询
         public static SqlDataReader GetNotReturnInterPhoneLstByReader(int UserID, string WhereSQL)
         {
             string sSQL = "select a.ID,a.TableRecGuid,a.InterID, a.BrandNames,b.ConsumeDate,c.OpName,d.OrganName,e.ModeID,e.SerialNum,f.Specification from  WorkApplyUserDetail_Info a "
                 + "left join WorkApplyUser_Info b on a.ApplyListID=b.ID left join SysUser_Info c on b.ConsumeUserID=c.ID "
                 + "left join  SysOrgan_Info d on d.ID=c.OrganID "
                 + "left join Interphone_Info e on a.InterID=e.ID "
                 + "left join Tool_Info f on e.ModeID=f.ID"
                 + " where  a.Status=0 and a.IsReturn=1 " + WhereSQL;

             if (UserID > 0)
             {
                 sSQL += " And IsNull(b.ConsumeUserID,0)=" + UserID.ToString();
             }

             sSQL += " and b.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString();

             sSQL = sSQL + " Order By b.ConsumeDate";

             return DataCommon.GetDataByReader(sSQL);
         }
         public static DataSet GetStockBySet(int CategoryID, string sWhereSQL)
         {
             string Ssql = "select a.*,b.OrganName,c.Specification,c.AliasesName,case ConditionID when 0 then '可用' when 1 then '已领用' when 2 then '已借用'  when 3 then '送修' when 4 then '送检' when 5 then '已注销' end as n from Interphone_Info a left join SysOrgan_Info b on a.OrgainID=b.ID left join Tool_Info c on a.ModeID=c.ID where a.status=0 ";
             if (CategoryID > 0)
             {
                 Ssql = Ssql + " and a.ModeID=" + CategoryID + "";
             }
             Ssql = Ssql + sWhereSQL;
             return DataCommon.GetDataByDataSet(Ssql);
         }
         public static DataSet GetStockBySet(int CategoryID, string sWhereSQL,int _OrganID)
         {
             string Ssql = "select a.*,b.OrganName,c.Specification,c.AliasesName,case ConditionID when 0 then '可用' when 1 then '已领用' when 2 then '已借用'  when 3 then '送修' when 4 then '送检' when 5 then '已注销' end as n from Interphone_Info a left join SysOrgan_Info b on a.OrgainID=b.ID left join Tool_Info c on a.ModeID=c.ID where a.status=0 and a.OrgainID=" + _OrganID + "  ";
             if (CategoryID > 0)
             {
                 Ssql = Ssql + " and a.ModeID=" + CategoryID + "";
             }
             Ssql = Ssql + sWhereSQL;
             return DataCommon.GetDataByDataSet(Ssql);
         }
         public static SqlDataReader GetworkInspectionInfo(int OrderID)
         {
             string sql = "select a.*,b.OpName,c.OrganName from WorkInterInspection_Info a left join SysUser_Info b on a.OrderUserID=b.ID left join SysOrgan_Info c on a.OrganID=c.ID where a.ID=" + OrderID + "";
             return DataCommon.GetDataByReader(sql);
         }
         //车间订单明细
         public static SqlDataReader GetworkInspectionInfoDa(int OrderID)
         {
             string sql = "select a.*,b.*,b.ID as ToolID from WorkInterInspectionDetail_Info a left join Tool_Info b on a.ModeID=b.ID where a.OrderID=" + OrderID + "";
             return DataCommon.GetDataByReader(sql);
         }
         //检验单单查询
         public static SqlDataReader GetSingleInspectionByReader(int _ConsumeID)
         {
             string sSQL = "Select a.*"
                 + ", b.OpCode as ConsumeOpCode,  b.OpName as ConsumeOpName, c.OrganName, d.OpName as CreateOpName"
                 + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                 + " from WorkInterInspection_Info a "
                 + " left join SysUser_Info b on b.Status=0 And a.ConsumeUserID=b.ID"
                 + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                 + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                 + " Where a.Status=0 And a.ID=" + _ConsumeID.ToString();

             return DataCommon.GetDataByReader(sSQL);
         }
        // 检验单明细
         public static DataSet GetInspectionInfoListBySer(int _ConsumeID)
         {
             string Ssql = "select  a.*,b.SerialNum,c.AliasesName as b from WorkInterInspectionDetail_Info a left join Interphone_Info b on a.InterID=b.ID left join Tool_Info c on a.InterID=c.ID  where a.ApplyListID=" + _ConsumeID + " ";
             return DataCommon.GetDataByDataSet(Ssql);
         }
        //添加修改检验单
         public static int UpUserInspection(int _ID, string[] FieldValues, string Sql)
         {
             string sSqlText = "";
             if (_ID > 0)
             {
                 sSqlText = sSqlText + " UPDATE WorkInterInspection_Info SET ConsumeNo='" + FieldValues.GetValue(1) + "'"
                      + ",ConsumeUserID=" + FieldValues.GetValue(2) + ""
                      + ",Description='" + FieldValues.GetValue(3) + "'"
                      + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                 sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";

                 sSqlText = sSqlText + Sql + " ;";
             }
             else
             {
                 sSqlText = sSqlText + " Insert Into WorkInterInspection_Info (TableRecGuid"
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
             string SSql = "select a.* ,b.OpName,case a.ApprovalStatus when 0 then '草稿' when 1 then '送检成功'end as a from WorkInterInspection_Info a left join  SysUser_Info b on a.ConsumeUserID=b.ID where a.status=0 ";
             SSql = SSql + sql;
             SSql = SSql + "order by a.ConsumeDate desc";
             return DataCommon.GetDataByDataSet(SSql);
         }
         //删除检验单
         public static int DeleteInspection(string _IDs)
         {
             string sSQL = "begin Delete from WorkInterInspection_Info Where ID in (" + _IDs.ToString() + "); ";
             sSQL += "update Interphone_Info set ConditionID =0 where ID in (select InterID from WorkInterInspectionDetail_Info where ApplyListID IN(" + _IDs.ToString() + ") )";
             sSQL += " Delete From WorkInterInspectionDetail_Info Where ApplyListID in (" + _IDs.ToString() + ");";
            // sSQL += "update Interphone_Info set ConditionID =0 where ID in (select InterID from WorkInterInspectionDetail_Info where ApplyListID IN("+_IDs.ToString()+") )";
             sSQL = sSQL + " End;";
             return DataCommon.QueryData(sSQL);
         }
        //查询送检库存
         public static DataSet GetInStockBySet(int CategoryID, string sWhereSQL)
         {
             string Ssql = "select a.*,b.ConsumeNo,c.SerialNum,d.Specification,d.AliasesName,e.OrganName from WorkInterInspectionDetail_Info a left join WorkInterInspection_Info b on a.ApplyListID=b.ID left join Interphone_Info c on a.InterID=c.ID left join Tool_Info d on c.ModeID=D.ID left join SysOrgan_Info e on c.OrgainID=e.ID where b.ApprovalStatus=1 ";
             if (CategoryID > 0)
             {
                 Ssql = Ssql + " and c.ModeID=" + CategoryID + "";
             }
             Ssql = Ssql + sWhereSQL;
             return DataCommon.GetDataByDataSet(Ssql);
         }
    }
}
