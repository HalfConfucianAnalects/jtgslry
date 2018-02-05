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
    public class SysInterPhone
    {
        public static string ToolOrder_TableName = "WorksShopInterOrder_Info";
        public static string InterPhoneOrder_Name = "InterPhoneOrder_Info", WorksShopInterOrder_Name = "WorksShopInterOrder_Info";
        public static string InterphoneShip_TableName = "InterphoneShip_Info", ApplyUser_TableName = "ApplyUser_Info";
        public static string BorrowUser_TableName = "BorrowUser_Info";
        public static int Consume_Draft = 0, Consume_ApprovalIsOK = 1, ConsumeType_ConsumeValue = 0;

        public static bool _Sel0 = true, _Sel1 = false, _Sel2 = false, _Sel3 = false, _Sel4 = false, _Sel5 = false;

        public static SqlDataReader GetInterByRead(int _ID)
        {
            string ssql = "select * from Tool_Info where ID="+_ID+"";
            return DataCommon.GetDataByReader(ssql);
        }
        //绑定dropdownlist数据
        public static SqlDataReader GetToolInfoByRead()
        {
            string ssql = "select * from Tool_Info";
            return DataCommon.GetDataByReader(ssql);
        }
        public static SqlDataReader GetToolInfoByReadlist(int i)
        {
            string ssql = "select * from Tool_Info where SystemID=1 And CategoryID="+i+"";
            return DataCommon.GetDataByReader(ssql);
        }
        //更新添加无线电台档案信息
        public static int UpdateSingleTools(int _ID, string[] FieldValues)
        {
            string sSqlText = "";
            if (_ID > 0)
            {
                sSqlText = "begin UPDATE Tool_Info SET ToolNo='" + FieldValues.GetValue(0)
                     + "',ToolName='" + FieldValues.GetValue(1) + "',Specification='" + FieldValues.GetValue(2)
                     + "',MaterialCode='" + FieldValues.GetValue(3) + "',Unit='" + FieldValues.GetValue(4)
                     + "',AliasesName='" + FieldValues.GetValue(5)
                     + "',Description='" + FieldValues.GetValue(6) + "'";
                sSqlText = sSqlText + ",SystemID=" + FieldValues.GetValue(7) + " WHERE ID=" + _ID + "" + ";";
                string sLogText = "更新 人员管理>机构部门：" + FieldValues.GetValue(2) + "记录。";

                sSqlText += SysLogs.GetOperatorLogSQL(sLogText) + " end;";
            }
            else
            {
                sSqlText = "begin Insert Into Tool_Info (  ToolNo, ToolName, AliasesName, Specification,MaterialCode,Unit,Description,SystemID) Values('"
                   + FieldValues.GetValue(0) + "','" + FieldValues.GetValue(1) + "','"
                   + FieldValues.GetValue(5) + "','" + FieldValues.GetValue(2) + "','" + FieldValues.GetValue(3) + "','" + FieldValues.GetValue(4) + "','" + FieldValues.GetValue(6) + "'," + FieldValues.GetValue(7) + ")" + ";";
                string sLogText = "新增 人员管理>机构部门：" + FieldValues.GetValue(2) + "记录。";
                sSqlText = sSqlText + SysLogs.GetOperatorLogSQL(sLogText) + " End;";
            }
            return DataCommon.QueryData(sSqlText);
        }
        public static DataSet GetToolsLstByDataSet(string WhereSQL)
        {
            string sSQL = "Select *  from Tool_Info Where ToolType='" + 0 + "'and SystemID=1" + WhereSQL;

            sSQL = sSQL + " Order By SortID";

            return DataCommon.GetDataByDataSet(sSQL);
        }
        //----------------------------------------------
        public static DataSet GetToolsLstByDataSet2(string WhereSQL)
        {
            string sSQL = "select *,case Isfinish when 0 then '草稿' when 1 then '正式' end as a from InList_Info Where Status=" + 0 + "" + WhereSQL;

            sSQL = sSQL + " Order By Indatetime desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }
        public static DataSet GetCertificateBydataset(string _GuID)
        {
            string sSql = "select a.*,b.*, b.Specification,b.ID as BID from Interphone_Info a left join Tool_Info b on a.ModeID=b.ID where a.ID='" + _GuID + "'";
            return DataCommon.GetDataByDataSet(sSql);
        }
        //----修改订单明细
        public static DataSet GetOrderInfoBydataset(string _GuID)
        {
            string sSql = "select a.*,b.*, b.Specification,b.ID as BID from InListdetails_Info a left join Tool_Info b on a.ModeID=b.ID  where a.ID='" + _GuID + "'";
            return DataCommon.GetDataByDataSet(sSql);
        }

        //---
        public static DataSet GetInlistInfoBydataset(string _GuID)
        {
            string sSql = "select a.*,b.*, b.Specification,b.ID as BID from InListdetails_Info a left join Tool_Info b on a.ModeID=b.ID  where a.InListID='" + _GuID + "'";
            return DataCommon.GetDataByDataSet(sSql);
        }
        //执行sql语句
        public static int UpdateCertificate(int _ID,string[] FieldValues)
        {
            string sSql="";
            if (_ID==0)
            {
                sSql = "insert into InList_Info(TableRecGuid,InLisrNO,Quantity,Isfinish,Description,ActionUser,Indatetime,OrderID)values('" + FieldValues.GetValue(0) + "','" + FieldValues.GetValue(1) + "','" + FieldValues.GetValue(2) + "'," + FieldValues.GetValue(6) + ",'" + FieldValues.GetValue(3) + "','" + FieldValues.GetValue(4) + "','" + FieldValues.GetValue(5) + "','" + FieldValues.GetValue(7) + "')";
            }
            else
            {
                sSql = "update InList_Info set InLisrNO='" + FieldValues.GetValue(1) + "',Quantity='" + FieldValues.GetValue(2) + "',Description='" + FieldValues.GetValue(3) + "',ActionUser='" + FieldValues.GetValue(4) + "',Indatetime='" + FieldValues.GetValue(5) + "',Isfinish=" + FieldValues.GetValue(6) + " where ID=" + _ID + "";
            }
           
            return DataCommon.QueryData(sSql);
        }
        public static int Updateinter(string sql)
        {
            return DataCommon.QueryData(sql);
        }
        public static SqlDataReader GetInlistByReader(int _id)
        {
            string ssql = "select * from InList_Info where ID="+_id+"";
            return DataCommon.GetDataByReader(ssql);
        }
        public static SqlDataReader GetListInfoByreader(int _ID)
        {
            string ssql = "select * from Interphone_Info where InListID="+_ID+"";
            return DataCommon.GetDataByReader(ssql);
        }
        public static DataSet GetAllinter(int modeID, string sWhereSQL)
        {
            string ssql = "select a.*,b.AliasesName as BrandNames, b.Specification from Interphone_Info a left join Tool_Info b on a.ModeID=b.ID  where a.ModeID=" + modeID
                + " and OrgainID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() + "  and ConditionID=0 and Isfinish=0" + sWhereSQL;
           return DataCommon.GetDataByDataSet(ssql);
        }
        public static SqlDataReader GetinterListInfoByreader( string a)
        {
            string ssql = "select  a.*,b.AliasesName as BrandNames from Interphone_Info a left join Tool_Info b on a.ModeID=b.ID where a.Status=0";
            ssql = ssql + a;
            return DataCommon.GetDataByReader(ssql);
        }
        public static DataSet GetBydataset()
        {
            string ssql = "select a.*,b.AliasesName as BrandNames,a.ModeID as ServiecID,a.ModeID as ServieID,a.ModeID as ServiceTime,a.ModeID as Evaluate,a.ID as InterID from Interphone_Info a left join Tool_Info b on a.ModeID=b.ID where a.ID=0";
            return DataCommon.GetDataByDataSet(ssql);
        }

        public static int UpUserApply(int _ID, string[] FieldValues, string Sql)
        {
            string sSqlText = "";
            if (_ID > 0)
            {
                sSqlText = sSqlText + " UPDATE ApplyUser_Info SET ConsumeNo='" + FieldValues.GetValue(1) + "'"
                     + ",ConsumeUserID=" + FieldValues.GetValue(2) + ""
                     + ",Description='" + FieldValues.GetValue(3) + "'"
                     + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";

                sSqlText =sSqlText+Sql+ " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into ApplyUser_Info (TableRecGuid"
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
                sSqlText = sSqlText + Sql+" ;";
            }
           
            return DataCommon.QueryData(sSqlText);
        }
        //-----------------------工具借用
        public static int UpUserBorrow(int _ID, string[] FieldValues, string Sql)
        {
            string sSqlText = "";
            if (_ID > 0)
            {
                sSqlText = sSqlText + " UPDATE BorrowUser_Info SET ConsumeNo='" + FieldValues.GetValue(1) + "'"
                     + ",ConsumeUserID=" + FieldValues.GetValue(2) + ""
                     + ",Description='" + FieldValues.GetValue(3) + "'"
                     + ",ApprovalStatus='" + FieldValues.GetValue(4) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";

                sSqlText = sSqlText + Sql + " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into BorrowUser_Info (TableRecGuid"
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
        //工具领用单列表
        public static DataSet GetApplyListByset(string sql)
        {
            string SSql = "select a.* ,b.OpName,case a.ApprovalStatus when 0 then '草稿' when 1 then '借用成功'end as a from ApplyUser_Info a "
                + " left join  SysUser_Info b on a.ConsumeUserID=b.ID  where a.status=0 ";
            SSql = SSql + sql;
            SSql = SSql + "order by a.ConsumeDate desc";
            return DataCommon.GetDataByDataSet(SSql);
        }
        //工具借用单列表
        public static DataSet GetBorrowListByset(string sql)
        {
            string SSql = "select a.* ,b.OpName,case a.ApprovalStatus when 0 then '草稿' when 1 then '借用成功'end as a from BorrowUser_Info a left join  SysUser_Info b on a.ConsumeUserID=b.ID where a.status=0 ";
            SSql = SSql + sql;
            SSql = SSql + "order by a.ConsumeDate desc";
            return DataCommon.GetDataByDataSet(SSql);
        }
        //编辑申请单查询
        public static SqlDataReader GetSingleConsumeByReader(int _ConsumeID)
        {
            string sSQL = "Select a.*"
                + ", b.OpCode as ConsumeOpCode,  b.OpName as ConsumeOpName, c.OrganName, d.OpName as CreateOpName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from ApplyUser_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.ConsumeUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                + " Where a.Status=0 And a.ID=" + _ConsumeID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }
        //借用单查询
        public static SqlDataReader GetSingleBorrowByReader(int _ConsumeID)
        {
            string sSQL = "Select a.*"
                + ", b.OpCode as ConsumeOpCode,  b.OpName as ConsumeOpName, c.OrganName, d.OpName as CreateOpName"
                + ", (Case IsNull(ApprovalStatus,0) when 0 then '草稿' when 1 then '正式' end) As ApprovalStatusName"
                + " from BorrowUser_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.ConsumeUserID=b.ID"
                + " left join SysOrgan_Info c on c.Status=0 And a.OrganID=c.ID"
                + " left join SysUser_Info d on d.Status=0 And a.CreateUserID=d.ID"
                + " Where a.Status=0 And a.ID=" + _ConsumeID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }
        public static DataSet GetApplyInfoListBySer(int _ConsumeID)
        {
            string Ssql = "select  a.*,b.SerialNum,c.AliasesName as b from ApplyUserDetails_Info a "
                + " left join Interphone_Info b on a.InterID=b.ID "
                + " left join Tool_Info c on a.InterID=c.ID  where a.ApplyListID=" + _ConsumeID + " ";
            return DataCommon.GetDataByDataSet(Ssql);
        }
        //借用
        public static DataSet GetBorrowInfoListBySer(int _ConsumeID)
        {
            string Ssql = "select  a.*,b.SerialNum,c.AliasesName as b from BorrowUserDetail_Info a left join Interphone_Info b on a.InterID=b.ID left join Tool_Info c on a.InterID=c.ID  where a.ApplyListID=" + _ConsumeID + " ";
            return DataCommon.GetDataByDataSet(Ssql);
        }
        //删除领用单
        public static int DeleteApplys(string _IDs)
        {
            string sSQL = "begin Delete from ApplyUser_Info Where ID in (" + _IDs.ToString() + "); ";
            sSQL += " Delete From ApplyUserDetails_Info Where ApplyListID in (" + _IDs.ToString() + ");";

            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }
        //删除借用单
        public static int DeleteBorrows(string _IDs)
        {
            string sSQL = "begin Delete from BorrowUser_Info Where ID in (" + _IDs.ToString() + "); ";
            sSQL += " Delete From BorrowUserDetail_Info Where ApplyListID in (" + _IDs.ToString() + ");";

            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }
        //无线电台领用归还查询
        public static SqlDataReader GetNotReturnInterPhoneLstByReader(int UserID, string WhereSQL)
        {
            string sSQL = "select a.ID,a.TableRecGuid,a.InterID, a.BrandNames,b.ConsumeDate,c.OpName,d.OrganName,e.ModeID,e.SerialNum,f.Specification from  ApplyUserDetails_Info a "
                +" left join ApplyUser_Info b on a.ApplyListID=b.ID "
                + " left join SysUser_Info c on b.ConsumeUserID=c.ID "
                +" left join  SysOrgan_Info d on d.ID=c.OrganID "
                +" left join Interphone_Info e on a.InterID=e.ID "
                +" left join Tool_Info f on e.ModeID=f.ID"
                + " where  a.Status=0 and a.ReturnStatus=1 and b.ApprovalStatus=1 " + WhereSQL;

            if (UserID > 0)
            {
                sSQL += " And IsNull(b.ConsumeUserID,0)=" + UserID.ToString();
            }

            sSQL += " and b.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString();

            sSQL = sSQL + " Order By b.ConsumeDate";

            return DataCommon.GetDataByReader(sSQL);
        }

        //无线电台借用归还查询
        public static SqlDataReader GetNotReturnBorrowPhoneLstByReader(int UserID, string WhereSQL)
        {
            string sSQL = "select a.ID,a.TableRecGuid,a.InterID, a.BrandNames,b.ConsumeDate,c.OpName,d.OrganName,e.ModeID,e.SerialNum,f.Specification from  BorrowUserDetail_Info a "
                + "left join BorrowUser_Info b on a.ApplyListID=b.ID left join SysUser_Info c on b.ConsumeUserID=c.ID "
                + "left join  SysOrgan_Info d on d.ID=c.OrganID "
                + "left join Interphone_Info e on a.InterID=e.ID "
                + "left join Tool_Info f on e.ModeID=f.ID"
                + " where  a.Status=0 and a.ReturnStatus=1 and b.ApprovalStatus=1 " + WhereSQL;

            if (UserID > 0)
            {
                sSQL += " And IsNull(b.ConsumeUserID,0)=" + UserID.ToString();
            }

            sSQL += " and b.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString();

            sSQL = sSQL + " Order By b.ConsumeDate";

            return DataCommon.GetDataByReader(sSQL);
        }
        //无线电台领用归还操作
        public static int ReturnInterPhone(string interID)
        {
            string ssql = "update ApplyUserDetails_Info set ReturnStatus =0,ReturnQuantity=0 where ID in(" + interID + ");";
            ssql = ssql + "update Interphone_Info set ConditionID=0 where ID in(select InterID from ApplyUserDetails_Info where ID in(" + interID + "));";
            return DataCommon.QueryData(ssql);
        }

        //无线电台领用注销操作
        public static int CancelInterPhone(string interID)
        {
            string ssql = "update ApplyUserDetails_Info set ReturnStatus=5 where ID in(" + interID + ");";
            ssql = ssql + "update Interphone_Info set ConditionID=5 where ID in(select InterID from ApplyUserDetails_Info where ID in(" + interID + "));";
            return DataCommon.QueryData(ssql);
        }

        //无线电台借用归还操作
        public static int ReturnInterPhoneBorrow(string interID)
        {
            string ssql = "update BorrowUserDetail_Info set ReturnStatus =0,ReturnQuantity=0 where ID in(" + interID + ");";
            ssql = ssql + "update Interphone_Info set ConditionID=0 where ID in(select InterID from BorrowUserDetail_Info where ID in(" + interID + "));";
            return DataCommon.QueryData(ssql);
        }

        //无线电台借用注销操作
        public static int CancelInterPhoneBorrow(string interID)
        {
            string ssql = "update BorrowUserDetail_Info set ReturnStatus =5 where ID in(" + interID + ");";
            ssql = ssql + "update Interphone_Info set ConditionID=5 where ID in(select InterID from BorrowUserDetail_Info where ID in(" + interID + "));";
            return DataCommon.QueryData(ssql);
        }

        public static SqlDataReader GetReturnListinfo(string WhereSQL)
        {
            string sSQL = "select  a.BrandNames,b.ConsumeDate,e.SerialNum,f.Specification from  ApplyUserDetails_Info a "
               + "left join ApplyUser_Info b on a.ApplyListID=b.ID left join SysUser_Info c on b.ConsumeUserID=c.ID "
               + "left join  SysOrgan_Info d on d.ID=c.OrganID "
               + "left join Interphone_Info e on a.InterID=e.ID "
               + "left join Tool_Info f on e.ModeID=f.ID"
               + " where  a.Status=0 and a.IsReturn=1 " + WhereSQL;
            return DataCommon.GetDataByReader(sSQL);
        }
        public static SqlDataReader GetReturnListinfoBorrow(string WhereSQL)
        {
            string sSQL = "select  a.BrandNames,b.ConsumeDate,e.SerialNum,f.Specification from  BorrowUserDetail_Info a "
               + "left join BorrowUser_Info b on a.ApplyListID=b.ID left join SysUser_Info c on b.ConsumeUserID=c.ID "
               + "left join  SysOrgan_Info d on d.ID=c.OrganID "
               + "left join Interphone_Info e on a.InterID=e.ID "
               + "left join Tool_Info f on e.ModeID=f.ID"
               + " where  a.Status=0 and a.IsReturn=1 " + WhereSQL;
            return DataCommon.GetDataByReader(sSQL);
        }
        //订单--------------------------------------------
        public static SqlDataReader GetiNTER(string sWhereSQL)
        {
            string ssql = "select * from Tool_Info where SystemID=1 " + sWhereSQL;
            return DataCommon.GetDataByReader(ssql);
        }
        //---新增、修改订单
        public static int UpNewOrder(int _ID,string[] FieldValues, string DetailsSQL)
        {
            string Sql = "";
            if (_ID>0)
            {
                Sql = Sql + " UPDATE InterPhoneOrder_Info SET OrderNo='" + FieldValues.GetValue(1)
                    + "',Description='" + FieldValues.GetValue(3) + "'"
                    + ",Isfinish=" + FieldValues.GetValue(4);
                Sql = Sql + " WHERE ID=" + _ID + "" + ";";

                Sql += " ;";
            }
            else
            {
                Sql = Sql + " Insert Into InterPhoneOrder_Info (TableRecGuid"
                   + ", OrderNo"
                   + ", OrderDate"
                   + ", OrganID"
                   + ", OrderUserID"
                   + ", Description"
                   + ", Isfinish)"
                   + " Values('" + FieldValues.GetValue(0) + "'"
                   + ",'" + FieldValues.GetValue(1) + "'"
                   + ",GetDate()"
                   + "," + SysGlobal.GetCurrentUserOrganID().ToString()
                   + ",'" + SysGlobal.GetCurrentUserID().ToString() + "'"
                   + ",'" + FieldValues.GetValue(3) + "'"
                
                   + "," + FieldValues.GetValue(4) + ")";
                Sql = Sql + " ;";
            }
            Sql = Sql + DetailsSQL;
            return DataCommon.QueryData(Sql);
        }
        //无线电台订单信息
        public static DataSet GetOrderByset( string  wsql)
        {
            string ssql = "select a.*,b.OrganName,c.OpName,case a.Isfinish when 0 then '草稿' when 1 then '正式'end as n   from InterPhoneOrder_Info a left join SysOrgan_Info b on a.OrganID=b.ID left join SysUser_Info c on a.OrderUserID=c.ID where a.status=0";
            ssql = ssql + wsql;
            return DataCommon.GetDataByDataSet(ssql);
        }
        public static DataSet GetStockBySet(int CategoryID, string sWhereSQL)
        {
            string Ssql = "select a.*,b.OrganName,c.Specification,c.AliasesName,case ConditionID when 0 then '可用' when 1 then '已领用' when 2 then '已借用'  when 3 then '送修' when 4 then '送检' when 5 then '已注销' end as n "
                + " from Interphone_Info a "
                + " left join SysOrgan_Info b on a.OrgainID=b.ID left "
                + " join Tool_Info c on a.ModeID=c.ID where a.status=0 ";
            if (CategoryID>0)
            {
                Ssql = Ssql + " and a.ModeID="+CategoryID+"";
            }
            Ssql = Ssql + sWhereSQL;
            return DataCommon.GetDataByDataSet(Ssql);
        }
        public static SqlDataReader GetModeByRead(int _ID)
        {
            string sql = "select * from Tool_Info where ID=" + _ID + "";
            return DataCommon.GetDataByReader(sql);

        }
        public static string GetCategoryNameByID(int CategoryID)
        {
            string _CategoryName = "";
            SqlDataReader sdr = GetModeByRead(CategoryID);
            if (sdr.Read())
            {
                _CategoryName =  sdr["AliasesName"].ToString() + sdr["Specification"].ToString();
            }
            sdr.Close();
            return _CategoryName;
        }
        //删除订单申请单
        public static int DeleteSingleDepotInterOrder(string _OrderIDs)
        {
            string sSQL = "begin Delete from InterPhoneOrder_Info Where ID in (" + _OrderIDs.ToString() + "); ";
            sSQL = sSQL + " Delete from InterPhoneOrderDetail_Info Where OrderID in (" + _OrderIDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }
        //删除入库单草稿
        public static int DeleteSingleStockInterOrder(string _OrderIDs,int orID)
        {
            string sSQL = "begin Delete from InList_Info Where ID in (" + _OrderIDs.ToString() + "); ";
            sSQL = sSQL + " Delete from InListdetails_Info Where InListID in (" + _OrderIDs.ToString() + "); ";
            sSQL = sSQL + " update InterPhoneOrderDetail_Info set OrderStatus=0 where ID=" + orID + " ; ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }
        //查询订单
        public static SqlDataReader GetOrderInfo(int OrderID)
        {
            string sql = "select a.*,b.OpName,c.OrganName from InterPhoneOrder_Info a left join SysUser_Info b on a.OrderUserID=b.ID left join SysOrgan_Info c on a.OrganID=c.ID where a.ID="+OrderID+"";
            return DataCommon.GetDataByReader(sql);
        }
        //订单明细
        public static SqlDataReader GetOrderInfoDa(int OrderID)
        {
            string sql = "select a.*,b.*,b.ID as ToolID from InterPhoneOrderDetail_Info a left join Tool_Info b on a.ModeID=b.ID where a.OrderID=" + OrderID + "";
            return DataCommon.GetDataByReader(sql);
        }
        //--------------订单明细
        public static DataSet GetOrderInfoByset()
        {
            string sql = "select a.*,b.AliasesName,b.Specification,d.OpName,c.OrderNo,c.OrderDate "
                + " from InterPhoneOrderDetail_Info a left join Tool_Info b on a.ModeID=b.ID "
                + " left join InterPhoneOrder_Info c on a.OrderID= c.ID"
                + " left join SysUser_Info d on c.OrderUserID=d.ID where a.OrderStatus=0 And a.Isfinish=1";
            return DataCommon.GetDataByDataSet(sql);
        }
        public static SqlDataReader GetOrderInfoByread( int _ID)
        {
            string sql = "select a.*,b.AliasesName,b.Specification,d.OpName,c.OrderNo,c.OrderDate  from InterPhoneOrderDetail_Info a left join Tool_Info b on a.ModeID=b.ID left join InterPhoneOrder_Info c on a.OrderID= c.ID left join SysUser_Info d on c.OrderUserID=d.ID where a.ID=" + _ID + "";
            return DataCommon.GetDataByReader(sql);
        }
        public static SqlDataReader GetInlistInfoByread(int _ID)
        {
            string sql = "select a.*,a.InLisrNO as Specification,b.AliasesName,b.Specification from InList_Info a  left join InterPhoneOrderDetail_Info c on a.OrderID=c.ID left join Tool_Info b on c.ModeID=b.ID   where a.ID=" + _ID + "";
            return DataCommon.GetDataByReader(sql);
        }
        public static SqlDataReader getIDByreda(int _ID)
        {
            string ssql = "select b.ID from InList_Info a left join InterPhoneOrderDetail_Info b on a.OrderID=b.ID where a.ID="+_ID+"";
            return DataCommon.GetDataByReader(ssql);
        }
        //获取订单明细的ID
        public static string GetOtderInfoA(int _ID)
        {
            string _id = "";
            SqlDataReader sdr = getIDByreda(_ID);
            while (sdr.Read())
            {
                _id = sdr["ID"].ToString();
            }
            sdr.Close();
            return _id;
        }
        //判断单号是否重复
        public static Boolean CheckInterlOrderNoExists(int OrderID, string OrderNo)
        {
            string sSqlText = "Select 1 From InterPhoneOrder_Info Where  OrderNo='" + OrderNo + "' And ID<>" + OrderID.ToString();

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }
        //查询无线电台编号是否重复
        public static Boolean CheckInterlOrderNoExistsOrder(int _OrderID, string OrderNo)
        {
            string sSqlText = "Select 1 From InListdetails_Info Where InListID<>" + _OrderID.ToString() + " And InterID='" + OrderNo + "'";

            return SysGlobal.GetExecSqlIsExist(sSqlText);
        }
       //==车间添加修该订单
        public static int UpNewworkOrder(int _ID, string[] FieldValues, string DetailsSQL)
        {
            string Sql = "";
            if (_ID > 0)
            {
                Sql = Sql + " UPDATE WorksShopInterOrder_Info SET OrderNo='" + FieldValues.GetValue(1)
                    + "',Class='" + FieldValues.GetValue(5) + ""
                    + "',Description='" + FieldValues.GetValue(3) + "'"
                    + ",Isfinish=" + FieldValues.GetValue(4);
                Sql = Sql + " WHERE ID=" + _ID + "" + ";";

                Sql += " ;";
            }
            else
            {
                Sql = Sql + " Insert Into WorksShopInterOrder_Info (TableRecGuid"
                   + ", OrderNo"
                   + ", OrderDate"
                   + ", OrganID"
                   + ", OrderUserID"
                   + ", Class"
                   + ", Description"
                   + ", Isfinish)"
                   + " Values('" + FieldValues.GetValue(0) + "'"
                   + ",'" + FieldValues.GetValue(1) + "'"
                   + ",GetDate()"
                   + "," + SysGlobal.GetCurrentUserOrganID().ToString()
                   + ",'" + SysGlobal.GetCurrentUserID().ToString() + "'"
                   + ",'" + FieldValues.GetValue(5) + "'"
                   + ",'" + FieldValues.GetValue(3) + "'"

                   + "," + FieldValues.GetValue(4) + ")";
                Sql = Sql + " ;";
            }
            Sql = Sql + DetailsSQL;
            return DataCommon.QueryData(Sql);
        }
        //查询车间订单
        //查询订单
        public static SqlDataReader GetworkOrderInfo(int OrderID)
        {
            string sql = "select a.*,b.OpName,c.OrganName from WorksShopInterOrder_Info a left join SysUser_Info b on a.OrderUserID=b.ID left join SysOrgan_Info c on a.OrganID=c.ID where a.ID=" + OrderID + "";
            return DataCommon.GetDataByReader(sql);
        }


        //车将无线电台订单
        public static DataSet GetworkOrderByset(string wsql)
        {
            string ssql = "select a.*,b.OrganName,c.OpName,case a.Isfinish when 0 then '草稿' when 1 then '待审批'  when 2 then '审批通过'  when 3 then '审批退回'end as n   from WorksShopInterOrder_Info a left join SysOrgan_Info b on a.OrganID=b.ID left join SysUser_Info c on a.OrderUserID=c.ID where a.status=0";
            ssql = ssql + wsql;
            return DataCommon.GetDataByDataSet(ssql);
        }

        //审批列表
        //车将无线电台订单
        public static DataSet GetworkOrderoverByset(string wsql)
        {
            string ssql = "select a.*,b.OrganName,c.OpName,case a.Isfinish when 0 then '草稿' when 1 then '待审批'  when 2 then '审批通过'  when 3 then '审批退回'end as n   from WorksShopInterOrder_Info a left join SysOrgan_Info b on a.OrganID=b.ID left join SysUser_Info c on a.OrderUserID=c.ID where a.status=0  and(  isfinish=2 or isfinish=3)";
            ssql = ssql + wsql;
            return DataCommon.GetDataByDataSet(ssql);
        }

        //删除车间订单申请单
        public static int DeleteSingleDepotInterworkOrder(string _OrderIDs)
        {
            string sSQL = "begin Delete from WorksShopInterOrder_Info Where ID in (" + _OrderIDs.ToString() + "); ";
            sSQL = sSQL + " Delete from WorksShopInterDetail_Info Where OrderID in (" + _OrderIDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }
        //车间订单明细
        public static SqlDataReader GetworkOrderInfoDa(int OrderID)
        {
            string sql = "select a.*,b.*,b.ID as ToolID from WorksShopInterDetail_Info a left join Tool_Info b on a.ModeID=b.ID where a.OrderID=" + OrderID + "";
            return DataCommon.GetDataByReader(sql);
        }
        public static DataSet GetOrderBDset(string ssq)
        {
            string ssql = "select a.*,b.OpName,c.OrganName,case a.Isfinish when 0 then '草稿' when 1 then '待审批'  when 2 then '审批通过'  when 3 then '审批退回'end as n "
                + " from WorksShopInterOrder_Info a left join SysUser_Info b on a.OrderUserID=b.ID left join SysOrgan_Info c on a.OrganID=c.ID where a.Status=0 ";
            ssql = ssql + ssq;
            return DataCommon.GetDataByDataSet(ssql);
        }
        //审批中
        public static SqlDataReader GetapplrByRead(string _ID)
        {
            string sspl = "select a.*, IsNull(b.ACount,0) As ACount from WorksShopInterOrder_Info a left join (select OrderID, sum(Quantity) as ACount from WorksShopInterDetail_Info Where Status=0 Group By OrderID) b on a.ID=b.OrderID where a.ID in("+_ID+")";
            return DataCommon.GetDataByReader(sspl);
        }

        //审批保存
        public static int UpdateApprovalToolOrder(string MainTableName, string OrderIDs, int ApprovalStatus, string Description)
        {
            string sSqlText = "begin";

            sSqlText += " Insert Into ApprovalLog_Info (MainTableName, MainID, ApprovalStatus, ApprovalUserID, ApprovalTime, Description) "
                + " (Select '" + MainTableName + "', ID, " + ApprovalStatus.ToString() + "," + SysGlobal.GetCurrentUserID().ToString()
                + ", GetDate(), '" + Description + "' From WorksShopInterOrder_Info Where Status=0 And ID In (" + OrderIDs + ")); ";

            sSqlText += " Update WorksShopInterOrder_Info Set Isfinish=" + ApprovalStatus.ToString()
                + ", ApprovalUserID=" + SysGlobal.GetCurrentUserID().ToString() + ", ApprovalTime=GetDate() "
                + " Where Status=0  And ID In (" + OrderIDs + ");";
            sSqlText += " end;";
            return DataCommon.QueryData(sSqlText);
        }
        //发货单明细
        public static DataSet GetShipOrderInfoByset()
        {
            string sql = "select a.*,b.OrderNo,b.organID,b.OrderDate,c.organName,d.Specification,d.AliasesName from WorksShopInterDetail_Info a left join WorksShopInterOrder_Info b on a.OrderID=b.ID left join SysOrgan_Info c on b.organID=c.ID left join Tool_Info d on a.ModeID=D.ID where b.Isfinish=2 and a.Isfinish=1";
            return DataCommon.GetDataByDataSet(sql);
        }
        //发货单明细
        public static SqlDataReader GetShipOrderInfoByset(int _ID)
        {
            string sql = "select a.*,b.OrderNo,b.organID,b.OrderDate,c.organName,d.Specification,d.AliasesName from WorksShopInterDetail_Info a left join WorksShopInterOrder_Info b on a.OrderID=b.ID left join SysOrgan_Info c on b.organID=c.ID left join Tool_Info d on a.ModeID=D.ID where a.ID=" + _ID + "";
            return DataCommon.GetDataByReader(sql);
        }
        //发货单保存修改
        public static int UpUserShiply(int _ID, string[] FieldValues, string Sql)
        {
            string sSqlText = "";
            if (_ID > 0)
            {
                sSqlText = sSqlText + " UPDATE InterphoneShip_Info SET ShipNo='" + FieldValues.GetValue(1) + "'"
                     + ", Isfinish=" + FieldValues.GetValue(5)
                     + ", Description='" + FieldValues.GetValue(4) + "'";
                sSqlText = sSqlText + " WHERE ID=" + _ID + "" + ";";

                sSqlText = sSqlText + Sql + " ;";
            }
            else
            {
                sSqlText = sSqlText + " Insert Into InterphoneShip_Info (TableRecGuid"
                    + ", ShipNo"
                    + ", Quantity"
                    + ", ActionUserID"
                    + ", Shipdate"
                    + ", ShipOrganID"
                    + ", Isfinish"
                    + ", OrderInfoID"
                    + ", Mode"
                    + ", Description)"
                    + " Values('" + FieldValues.GetValue(0) + "'"
                    + ",'" + FieldValues.GetValue(1) + "'"
                    + ",'" + FieldValues.GetValue(2) + "'"
                    + "," + SysGlobal.GetCurrentUserID().ToString() + ""
                    + ",GetDate()"
                    + "," + FieldValues.GetValue(3) + ""
                    + "," + FieldValues.GetValue(5) + ""
                    + "," + FieldValues.GetValue(6) + ""
                    + ",'" + FieldValues.GetValue(7) + "'"
                    + ",'" + FieldValues.GetValue(4) + "')";
                sSqlText = sSqlText + Sql + " ;";
            }

            return DataCommon.QueryData(sSqlText);
        }
        public static DataSet GetSgipListByset( string sql)
        {
            string ssql = "select a.*,b.Opname,c.organname,Isfinish,case a.Isfinish when 0 then '草稿' when 1 then '正式' end as n  from InterphoneShip_Info a left join  SysUser_Info b on a.ActionUserID=b.ID left join SysOrgan_Info c on a.ShipOrganID=c.ID where a.Status=0";
            ssql = ssql + sql;
            return DataCommon.GetDataByDataSet(ssql);
        }

        //删除发货单
        public static int DeleteShip(string _IDs)
        {
            string sSQL = "begin update Interphone_Info set OrgainID=0 where ID in (select InterID from ShipInterPhoneDetail_Info where ShipNOID in(" + _IDs + ") )";
            sSQL += " update WorksShopInterDetail_Info set IsFinish=1 where ID in (select OrderInfoID from InterphoneShip_Info where ID in (" + _IDs + "))";
            sSQL += " Delete From ShipInterPhoneDetail_Info Where ShipNOID in (" + _IDs.ToString() + ");";
             sSQL += " Delete from InterphoneShip_Info Where ID in (" + _IDs.ToString() + "); ";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        public static SqlDataReader GetshipInfolist(int _ID)
        {
            string ssql = "select a.*,b.opName,c.OrganName from InterphoneShip_Info a left join SysUser_Info b on a.ActionUserID=b.ID left join SysOrgan_Info c on a.ShipOrganID=c.ID where a.ID=" + _ID + "";
            return DataCommon.GetDataByReader(ssql);
        }
        public static DataSet GetshipInfoListBySer(int _ConsumeID)
        {
            string Ssql = "select a.* ,b.SerialNum,c.AliasesName as BrandNames from ShipInterPhoneDetail_Info a left join Interphone_Info b on a.InterID=b.ID left join Tool_Info c on b.ModeID=c.ID  where a.ShipNOID=" + _ConsumeID + " ";
            return DataCommon.GetDataByDataSet(Ssql);
        }
    }
}
