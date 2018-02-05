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
    public class WorkInterCarry
    {
        public static int Consume_Draft = 0, Consume_ApprovalIsOK = 1, ConsumeType_ConsumeValue = 0;
        //送修单查询
        public static SqlDataReader GetSingleInspectionByReader(int _ConsumeID)
        {
            string sSQL = "Select a.*"
                + ", b.OpCode as ConsumeOpCode,  b.OpName as ConsumeOpName, c.OrganName, d.OpName as CreateOpName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from WorkInterCarry_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.ConsumeUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                + " Where a.Status=0 And a.ID=" + _ConsumeID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }
        // 检验单明细
        public static DataSet GetInspectionInfoListBySer(int _ConsumeID)
        {
            string Ssql = "select  a.*,b.SerialNum,c.AliasesName as b from WorkInterCarryDetail_Info a left join Interphone_Info b on a.InterID=b.ID left join Tool_Info c on a.InterID=c.ID  where a.ApplyListID=" + _ConsumeID + " ";
            return DataCommon.GetDataByDataSet(Ssql);
        }
          public static Boolean CheckConsumeNoExists(int ConsumeID, string ConsumeNo)
        {
            string sSqlText = "Select 1 From WorkInterCarry_Info Where ConsumeType=" + ConsumeType_ConsumeValue.ToString() 
                + " And ConsumeNo='" + ConsumeNo + "' And ID<>" + ConsumeID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }
          //添加修改检验单
          public static int UpUserInspection(int _ID, string[] FieldValues, string Sql)
          {
              string sSqlText = "";
              if (_ID > 0)
              {
                  sSqlText = sSqlText + " UPDATE WorkInterCarry_Info SET ConsumeNo='" + FieldValues.GetValue(1) + "'"
                       + ",ConsumeUserID=" + FieldValues.GetValue(2) + ""
                       + ",Description='" + FieldValues.GetValue(3) + "'"
                         + ",ServiceID='" + FieldValues.GetValue(5) + "'"
                       + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                  sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";

                  sSqlText = sSqlText + Sql + " ;";
              }
              else
              {
                  sSqlText = sSqlText + " Insert Into WorkInterCarry_Info (TableRecGuid"
                      + ", ConsumeNo"
                      + ", ConsumeType"
                      + ", ConsumeDate"
                      + ", OrganID"
                      + ", ConsumeUserID"
                      + ", CreateUserID"
                      + ", ServiceID"
                      + ", Description"
                      + ", ApprovalStatus)"
                      + " Values('" + FieldValues.GetValue(0) + "'"
                      + ",'" + FieldValues.GetValue(1) + "'"
                      + "," + ConsumeType_ConsumeValue.ToString()
                      + ",GetDate()"
                      + "," + SysGlobal.GetCurrentUserOrganID().ToString()
                      + "," + FieldValues.GetValue(2) + ""
                      + "," + SysGlobal.GetCurrentUserID().ToString() + ""
                      + ",'" + FieldValues.GetValue(5) + "'"
                      + ",'" + FieldValues.GetValue(3) + "'"
                      + "," + FieldValues.GetValue(4) + ")";
                  sSqlText = sSqlText + Sql + " ;";
              }

              return DataCommon.QueryData(sSqlText);
          }
          //删除送修单
          public static int DeleteInspection(string _IDs)
          {
              string sSQL = "begin Delete from WorkInterCarry_Info Where ID in (" + _IDs.ToString() + "); ";
              sSQL += "update Interphone_Info set ConditionID =0 where ID in (select InterID from WorkInterInspectionDetail_Info where ApplyListID IN(" + _IDs.ToString() + ") )";
              sSQL += " Delete From WorkInterCarryDetail_Info Where ApplyListID in (" + _IDs.ToString() + ");";
              // sSQL += "update Interphone_Info set ConditionID =0 where ID in (select InterID from WorkInterInspectionDetail_Info where ApplyListID IN("+_IDs.ToString()+") )";
              sSQL = sSQL + " End;";
              return DataCommon.QueryData(sSQL);
          }
          //无线电台送修单列表
          public static DataSet GetInspectionListByset(string sql)
          {
              string SSql = "select a.* ,b.OpName,e.OrganName,case a.ApprovalStatus when 0 then '草稿' when 1 then '送修成功'end as a from WorkInterCarry_Info a left join  SysUser_Info b on a.ConsumeUserID=b.ID left join SysOrgan_Info e on a.ServiceID=e.ID where a.status=0 ";
              SSql = SSql + sql;
              SSql = SSql + "order by a.ConsumeDate desc";
              return DataCommon.GetDataByDataSet(SSql);
          }
          //查询送检库存
          public static DataSet GetInStockBySet(int CategoryID, string sWhereSQL)
          {
              string Ssql = "select a.*,b.ConsumeNo,c.SerialNum,d.Specification,d.AliasesName,e.OrganName from WorkInterCarryDetail_Info a left join WorkInterCarry_Info b on a.ApplyListID=b.ID left join Interphone_Info c on a.InterID=c.ID left join Tool_Info d on c.ModeID=D.ID left join SysOrgan_Info e on c.OrgainID=e.ID where b.ApprovalStatus=1 and a.IsReturn=1 ";
              if (CategoryID > 0)
              {
                  Ssql = Ssql + " and c.ModeID=" + CategoryID + "";
              }
              Ssql = Ssql + sWhereSQL;
              return DataCommon.GetDataByDataSet(Ssql);
          }
          //检验退还查询
          public static SqlDataReader GetSingleCRspectionByReader(int _ConsumeID)
          {
              string sSQL = "Select a.*"
                  + ", b.OpCode as ConsumeOpCode,  b.OpName as ConsumeOpName, c.OrganName, d.OpName as CreateOpName"
                  + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                  + " from InterCarryReturn_Info a "
                  + " left join SysUser_Info b on b.Status=0 And a.ConsumeUserID=b.ID"
                  + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                  + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                  + " Where a.Status=0 And a.ID=" + _ConsumeID.ToString();

              return DataCommon.GetDataByReader(sSQL);
          }
          // 检验单明细
          public static DataSet GetCRspectionInfoListBySer(int _ConsumeID)
          {
              string Ssql = "select  a.*,e.SerialNum,c.AliasesName as b from InterCarryReturnDetail_Info a left join Interphone_Info b on a.InterID=b.ID  left join WorkInterCarryDetail_Info d on a.InterID=d.ID left join Interphone_Info e on d.InterID=e.ID left join Tool_Info c on e.ModeID=c.ID where a.ApplyListID=" + _ConsumeID + " ";
              return DataCommon.GetDataByDataSet(Ssql);
          }
          public static Boolean CheckCRsumeNoExists(int ConsumeID, string ConsumeNo)
          {
              string sSqlText = "Select 1 From InterCarryReturn_Info Where ConsumeType=" + ConsumeType_ConsumeValue.ToString()
                  + " And ConsumeNo='" + ConsumeNo + "' And ID<>" + ConsumeID.ToString();

              return SysGlobal.GetExecSqlIsExist(sSqlText);
          }
          //添加修改退还检修
          public static int UpUserCRpection(int _ID, string[] FieldValues, string Sql)
          {
              string sSqlText = "";
              if (_ID > 0)
              {
                  sSqlText = sSqlText + " UPDATE InterCarryReturn_Info SET ConsumeNo='" + FieldValues.GetValue(1) + "'"
                       + ",ConsumeUserID=" + FieldValues.GetValue(2) + ""
                       + ",Description='" + FieldValues.GetValue(3) + "'"
                       + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                  sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";

                  sSqlText = sSqlText + Sql + " ;";
              }
              else
              {
                  sSqlText = sSqlText + " Insert Into InterCarryReturn_Info (TableRecGuid"
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
        //获取维修公司列表
          public static SqlDataReader GetOrganNameByread()
          {
              string ssql = "select * from SysOrgan_Info where OrganType=4 ";
                  return DataCommon.GetDataByReader(ssql);
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
          public static SqlDataReader GetinterListInfoByreader(string a)
          {
              string ssql = "select a.*,b.SerialNum,a.ApplyListID as ServiceTime ,a.ServiceID as ServieID ,c.AliasesName as BrandNames from WorkInterCarryDetail_Info a left join Interphone_Info b on a.InterID=b.ID left join Tool_Info c on b.ModeID=c.ID where b.Status=0 and a.IsReturn=1";
              ssql = ssql + a;
              return DataCommon.GetDataByReader(ssql);
          }
          //工具借用单列表
          public static DataSet GetCRListByset(string sql)
          {
              string SSql = "select a.* ,b.OpName,case a.ApprovalStatus when 0 then '草稿' when 1 then '已完成'end as a from InterCarryReturn_Info a left join  SysUser_Info b on a.ConsumeUserID=b.ID where a.status=0 ";
              SSql = SSql + sql;
              SSql = SSql + "order by a.ConsumeDate desc";
              return DataCommon.GetDataByDataSet(SSql);
          }
          public static int DeleteCarry(string _IDs)
          {
              string sSQL = "begin Delete from InterCarryReturn_Info Where ID in (" + _IDs.ToString() + "); ";
              sSQL += " Delete From InterCarryReturnDetail_Info Where ApplyListID in (" + _IDs.ToString() + ");";

              sSQL = sSQL + " End;";
              return DataCommon.QueryData(sSQL);
          }
          public static SqlDataReader GeteByread(int v)
          {
              string ssql = "select * from InterCarryReturnDetail_Info where ApplyListID="+v+" ";
              return DataCommon.GetDataByReader(ssql);
          }
          //对讲为评价列表
          public static DataSet GetListNotByset(string sql)
          {
              string SSql = "select  a.*,e.SerialNum,f.OrganName ,c.AliasesName as b from InterCarryReturnDetail_Info a "
                + " left join Interphone_Info b on a.InterID=b.ID  "
                + " left join WorkInterCarryDetail_Info d on a.InterID=d.ID "
                + " left join Interphone_Info e on d.InterID=e.ID"
                + " left join Tool_Info c on e.ModeID=c.ID left join SysOrgan_Info f on a.ServiceOrganID=f.ID where a.Evaluate=0";
              SSql = SSql + sql;
            
              return DataCommon.GetDataByDataSet(SSql);
          }
          public static SqlDataReader GetNotEv(string IDs)
          {
              string ssql = "select  a.*,e.SerialNum,f.OrganName ,c.AliasesName as b from InterCarryReturnDetail_Info a left join Interphone_Info b on a.InterID=b.ID  left join WorkInterCarryDetail_Info d on a.InterID=d.ID left join Interphone_Info e on d.InterID=e.ID left join Tool_Info c on e.ModeID=c.ID left join SysOrgan_Info f on a.ServiceOrganID=f.ID where a.Evaluate=0 and a.ID IN(" + IDs + ");";
              return DataCommon.GetDataByReader(ssql);
          }
        //评价无线电台维修
          public static int UpdateNotEv(string sql)
          {
              return DataCommon.QueryData(sql);
          }
        //查询二十天内为评价无线电台
          public static SqlDataReader Get()
          {
              string ssl = "select  a.*,e.SerialNum,f.OrganName ,c.AliasesName as b from InterCarryReturnDetail_Info a left join Interphone_Info b on a.InterID=b.ID  left join WorkInterCarryDetail_Info d on a.InterID=d.ID left join Interphone_Info e on d.InterID=e.ID left join Tool_Info c on e.ModeID=c.ID left join SysOrgan_Info f on a.ServiceOrganID=f.ID left join InterCarryReturn_Info g on a.ApplyListID=g.ID where a.Evaluate=0 and g.ConsumeDate+1>=getdate()";
              return DataCommon.GetDataByReader(ssl);
          }
    }
}
