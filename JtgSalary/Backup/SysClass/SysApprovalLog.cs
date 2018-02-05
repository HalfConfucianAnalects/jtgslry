using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CyxPack.CommonOperation;
using CyxPack.OperateSqlServer;
using System.Data.SqlClient;

namespace JtgTMS.SysClass
{
    public class SysApprovalLog
    {
        //审批记录
        public static DataSet GetApprovalLogLstByDataSet(string MainTableName, int MainID)
        {
            string sSQL = "";
            sSQL = "Select a.*, b.OpName as ApprovalUserName "
                + ", (Case ApprovalStatus when 0 then '草稿' when 1 then '审批中' when 2 then '审批成功' when 3 then '审批退回' end) as ApprovalStatusName"
                + " from ApprovalLog_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.ApprovalUserID=b.ID"
                + " Where a.Status=0 And MainTableName='" + MainTableName + "'"
                + " And MainID=" + MainID.ToString();
            sSQL = sSQL + " Order By a.ApprovalTime desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }

        //审批记录
        public static DataSet GetDeliveryLogLstByDataSet(string MainTableName, int MainID)
        {
            string sSQL = "";
            sSQL = "Select a.*, b.OpName as ApprovalUserName "
                + ", (Case ApprovalStatus when 0 then '草稿' when 1 then '已发货' when 2 then '已入库' end) as ApprovalStatusName"
                + " from ApprovalLog_Info a "
                + " left join SysUser_Info b on b.Status=0 And a.ApprovalUserID=b.ID"
                + " Where a.Status=0 And MainTableName='" + MainTableName + "'"
                + " And MainID=" + MainID.ToString();
            sSQL = sSQL + " Order By a.ApprovalTime desc";

            return DataCommon.GetDataByDataSet(sSQL);
        }
    }
}
