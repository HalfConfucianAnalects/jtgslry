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
using CyxPack.UserCommonOperation;

namespace JtgTMS.SysClass
{
    public class SysLogs
    {
        //操作日志
        public static string GetOperatorLogSQL(string sLogText)
        {
            UserInfo us = CyxPack.UserCommonOperation.UserCommonOperation.GetUserInfo();

            string sSQL = "Insert into SysLogs_Info (LogType, OpCode, OpName, LogText,IP,ComputerName) "
                + " Values('用户操作','" + us.OpCode + "','" + us.OpName + "','" + sLogText + "','" + SysClass.SysGlobal.GetComputerIP() + "','" +SysClass.SysGlobal.GetComputerName() + "');";
            return sSQL;
        }
    

        //登录日志
        public static int CreateUserLogin(int UserID, string OpCode, string OpName, string LogText, string IP, string ComputerName)
        {
            string sSQL = "begin Insert into SysLogs_Info (LogType, OpCode, OpName, LogText,IP,ComputerName) "
                + " Values('登录系统','" + OpCode + "','" + OpName + "','" + LogText + "','" + IP + "','"+ComputerName+"');" ;
            sSQL = sSQL + " Update SysUser_Info Set IsError=1 Where ID="+UserID.ToString()+" And LastIp<>'" + IP + "';";
            sSQL = sSQL + " Update SysUser_Info Set IsError=0 Where ID=" + UserID.ToString() + " And LastIp='" + IP + "';";
            sSQL = sSQL + " Update SysUser_Info Set LastDate=GetDate(), LastIp='" + IP + "', LastComputerName='" + ComputerName + "' Where ID="+UserID.ToString()+";";
            sSQL = sSQL + " End;";
            return DataCommon.QueryData(sSQL);
        }

        //退出日志
        public static int CreateUserLogout(string OpCode, string OpName, string LogText, string IP, string ComputerName)
        {
            string sSQL = "Insert into SysLogs_Info (LogType, OpCode, OpName, LogText,IP,ComputerName) "
                + " Values('退出系统','" + OpCode + "','" + OpName + "','" + LogText + "','" + IP + "','" + ComputerName + "')" ;
           
            return DataCommon.QueryData(sSQL);
        }


        //查询登录日志
        public static DataSet GetUserSysLogsByDataSet(string txtTime1Search, string txtTime2Search, string txtNameSearch, string txtTypeSearch, string txtOpCodeSearch, string txtComputerSearch, string txtRemarkSearch)
        {         
            string sSQL = "Select Top 1000 * from SysLogs_Info where status=0";

            if (txtTime1Search.Length >= 0 && txtTime2Search.Length>=0)
            {
                sSQL = sSQL + "And CONVERT(VARCHAR(20), LogTime , 20) >= '" + txtTime1Search + " 0:00:00:000' and LogTime <= '" + txtTime2Search + " 23:59:59:999'";
            }
            if (txtNameSearch.Length > 0)//姓名
            {
                sSQL = sSQL + "And OpName LIKE '%" + txtNameSearch + "%'";
            }

            if (txtTypeSearch.Length > 0)//类型
            {
                sSQL = sSQL + "And LogType LIKE '%" + txtTypeSearch + "%'";
            }

            if (txtOpCodeSearch.Length > 0)//工号
            {
                sSQL = sSQL + "And OpCode LIKE '%" + txtOpCodeSearch + "%'";
            }

            if (txtComputerSearch.Length > 0)//计算机名
            {
                sSQL = sSQL + "And (ComputerName LIKE '%" + txtComputerSearch + "%')";
            }
            if (txtRemarkSearch.Length > 0)//备注
            {
                sSQL = sSQL + " And (LogText Like '%" + txtRemarkSearch + "%')";
            }
            
             sSQL = sSQL + " Order By LogTime Desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //查询登录日志
        public static DataSet GetCurrentUserSysLogsByDataSet()
        {
             UserInfo info = CyxPack.UserCommonOperation.UserCommonOperation.GetUserInfo();

             string sSQL = "Select Top 1000  * from SysLogs_Info where status=0";
            if (info != null)
            {                
               sSQL = sSQL + " And OpCode='" + info.OpCode + "'";
            }
             sSQL = sSQL + " Order By a.LogTime Desc";


            sSQL = sSQL + " Order By LogTime Desc";
            return DataCommon.GetDataByDataSet(sSQL);
        }  
        //=====
        public static SqlDataReader GetSingleOperatorByReader(int _ID)
        {
            string sSQL = "select * from SysLogs_Info where Status = 0 and ID = " + _ID.ToString();
            return DataCommon.GetDataByReader(sSQL);
        }
//-----------------
        public static SqlDataReader QueryDetailLst()
        {
            //int iRecCount = 1;
            //if (count > 0)
            //{
            //    iRecCount = count;
            //}
            //string sSQL = "Select Top " + iRecCount + " * from SysLogs_Info Where LogType Like '%" + prefixText + "%' and Status=0";
            string sSQL = "select LogType,count(id) from SysLogs_Info group by LogType";//分组，去除重复数据
            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader QueryOpNamelLst()
        {
            string sSQL = "select OpName,count(id) from SysLogs_Info group by OpName";//分组，去除重复数据
            return DataCommon.GetDataByReader(sSQL);
        }


        public static SqlDataReader QueryOpCodeLst()
        {
            string sSQL = "select OpCode,count(id) from SysLogs_Info group by OpCode";//分组，去除重复数据
            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader QueryComputerNameLst()
        {
            string sSQL = "select ComputerName,count(id) from SysLogs_Info group by ComputerName";//分组，去除重复数据
            return DataCommon.GetDataByReader(sSQL);
        }
    }
}
