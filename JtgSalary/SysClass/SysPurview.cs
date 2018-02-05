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
    public class SysPurview
    {
        public static SqlDataReader GetRoleChildPurviewLst()
        {
            string sSQL = "Select * from SysPurviewTempate_Info Where Status=0 ";//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString();
            return DataCommon.GetDataByReader(sSQL);
        }

        public static DataTable GetRolePurviewLstByTable()
        {
            string sSQL = "Select * from SysPurviewTempate_Info Where Status=0";// And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString();
            SqlDataAdapter da = DataCommon.GetDataByAdapter(sSQL);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static SqlDataReader GetRoleChildPurviewLst(int PPurviewID)
        {
            string sSQL = "Select * from SysPurviewTempate_Info Where Status=0 "
                + " And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And IsNull(PPurviewID,0)=" + PPurviewID.ToString() + " Order By SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSingleRolePurview(int PurviewID)
        {
            string sSQL = "Select * from SysPurviewTempate_Info Where Status=0 And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString()
                + " And IsNull(PurviewID,0)=" + PurviewID.ToString() + " Order By SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetRoleChildPurvieLstByReader(int PPurviewID, string Purview)
        {
            string sSQL = "Select *,'" + Purview + "' as Purview from SysPurviewTempate_Info Where Status=0 "
                + " And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And IsNull(PPurviewID,0)=" + PPurviewID.ToString();
            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetRoleChildPurviewLst(int PPurviewID, int RoleID)
        {
            string sSQL = "Select *," + RoleID + " as RoleID from SysPurviewTempate_Info Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And IsNull(PPurviewID,0)=" + PPurviewID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetUserChildPurviewLst(int PPurviewID, int UserID, string UserRolePurview)
        {
            string sSQL = "Select *, " + UserID.ToString() + " as UserID,'" + UserRolePurview + "' as UserRolePurview from SysPurviewTempate_Info "
                + " Where Status=0 "//And IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And substring('" + UserRolePurview + "', PurviewID *5, 1)>0 And IsNull(PPurviewID,0)=" + PPurviewID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetChildPurviewByRole(int PPurviewID, int RoleID)
        {
            string sSQL = "exec sp_RolePurview " + SysParams.GetPurviewSystemID().ToString() + "," + PPurviewID.ToString() + "," + RoleID.ToString();

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetUserChildPurviewByRole(int PPurviewID, int UserID, string UserRolePurview)
        {
            string sSQL = "exec sp_UserRolePurview " + SysParams.GetPurviewSystemID().ToString() + "," + PPurviewID.ToString() + "," + UserID.ToString() + ",'" + UserRolePurview.ToString() + "'";

            return DataCommon.GetDataByReader(sSQL);
        }        
    }
  
}
