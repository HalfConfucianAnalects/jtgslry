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
    public class SysToolReturn
    {
        public static SqlDataReader GetNotReturnToolLstByReader(int UserID, string WhereSQL)
        {
            string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification"
                + " , b.MaterialCode,b.Unit, d.OrganName, c.ConsumeDate, e.OpName as ConsumeOpName "
                + ", (Case c.ConsumeType when 0 then '领用' when 1 then '借用' end) As ConsumeTypeName, f.ToolCode, f.TestCode"
                + " from ConsumeDetails_Info a "
                + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
                + " Left join Consume_Info c on c.Status=0 And c.ID=a.ConsumeID"
                + " Left join SysOrgan_Info d on d.ID=c.OrganID"
                + " Left join SysUser_Info e on e.ID=c.ConsumeUserID"
                + " Left join ToolStockDetail_Info f on f.ID=a.ToolDetailID"
                + " Where a.ReturnStatus=0 And a.Status=0 " + WhereSQL;

            if (UserID > 0)
            {
                sSQL += " And IsNull(c.ConsumeUserID,0)=" + UserID.ToString();
            }

            sSQL += " and c.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString() ;

            sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetNotReturnInterPhoneLstByReader(int UserID, string WhereSQL)
        {
            string sSQL = "select a.ID,a.TableRecGuid,a.InterID, a.BrandNames,b.ConsumeDate,c.OpName,d.OrganName"
                + ",e.ModeID,e.SerialNum,f.Specification "
                + ", (Case b.ConsumeType when 0 then '领用' when 1 then '借用' end) As ConsumeTypeName from  ApplyUserDetails_Info a "
                + " left join ApplyUser_Info b on a.ApplyListID=b.ID "
                + " left join SysUser_Info c on b.ConsumeUserID=c.ID "
                + " left join  SysOrgan_Info d on d.ID=c.OrganID "
                + " left join Interphone_Info e on a.InterID=e.ID "
                + " left join Tool_Info f on e.ModeID=f.ID"
                + " where  a.Status=0 and a.ReturnStatus=1 and b.ApprovalStatus=1" + WhereSQL;

            if (UserID > 0)
            {
                sSQL += " And IsNull(b.ConsumeUserID,0)=" + UserID.ToString();
            }

            sSQL += " and b.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString();

            sSQL = sSQL + " Order By b.ConsumeDate";

            //string sSQL = "Select a.*, b.ToolNo, b.ToolName, b.AliasesName, b.Specification"
            //    + " , b.MaterialCode,b.Unit, d.OrganName, c.ConsumeDate, e.OpName as ConsumeOpName "
            //    + ", (Case c.ConsumeType when 0 then '领用' when 1 then '借用' end) As ConsumeTypeName, f.ToolCode, f.TestCode"
            //    + " from ConsumeDetails_Info a "
            //    + " left join Tool_Info b on b.Status=0 And b.ToolType=" + SysClass.SysTool._NomalToolType.ToString() + " and a.ToolID=b.ID"
            //    + " Left join Consume_Info c on c.Status=0 And c.ID=a.ConsumeID"
            //    + " Left join SysOrgan_Info d on d.ID=c.OrganID"
            //    + " Left join SysUser_Info e on e.ID=c.ConsumeUserID"
            //    + " Left join ToolStockDetail_Info f on f.ID=a.ToolDetailID"
            //    + " Where a.ReturnStatus=0 And a.Status=0 " + WhereSQL;

            //if (UserID > 0)
            //{
            //    sSQL += " And IsNull(c.ConsumeUserID,0)=" + UserID.ToString();
            //}

            //sSQL += " and c.OrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString();

            //sSQL = sSQL + " Order By b.ToolNo";

            return DataCommon.GetDataByReader(sSQL);
        }
    }
}
