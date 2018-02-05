using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CyxPack.CommonOperation;
using CyxPack.OperateSqlServer;
using System.Data.SqlClient;

namespace JtgTMS.SysClass
{
    public class SysSystem
    {
        public static string SysTopPModuleNo = "";

        public static string SysTopPFuncNo = "";

        public static SqlDataReader GetSysModuleLstByReader(string WhereSQL)
        {
            string sSQL = "Select * from SysModule_Info Where Status=0 " + WhereSQL + " Order By SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static string GetSysModuleTitleByNo(string ModuleNo)
        {
            string _ModuleTitle = "";
            string sSQL = "Select * from SysModule_Info Where ModuleNo='" + ModuleNo.ToString() + "' And SystemID=" + SysClass.SysParams.GetPurviewSystemID().ToString()
                + " And Status=0 Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            if (sdr.Read())
            {
                _ModuleTitle = sdr["ModuleTitle"].ToString();
            }
            sdr.Close();
            return _ModuleTitle;
        }

        public static string GetDefaultFuncByModuleNo(string ModuleNo)
        {
            string _FuncNo = "";
            string sSQL = "Select * from SysModule_Info Where ModuleNo='" + ModuleNo.ToString() + "' And Status=0 Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            if (sdr.Read())
            {
                _FuncNo = sdr["DefaultFuncNo"].ToString();
            }
            sdr.Close();
            return _FuncNo;
        }

        public static SqlDataReader GetSysFuncInfoByReader(string FuncNo)
        {
            string sSQL = "Select * from SysFunc_Info Where SystemID=" + SysClass.SysParams.GetPurviewSystemID().ToString()
                + " And IsNull(FuncNo,'')='" + FuncNo + "' And Status=0";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static SqlDataReader GetSysFuncLstByReader(string ModuleNo, string PFuncNo)
        {
            string sSQL = "Select * from SysFunc_Info Where PModuleNo='" + ModuleNo.ToString() + "' And SystemID=" + SysClass.SysParams.GetPurviewSystemID().ToString()
                + " And IsNull(PFuncNo,'')='" + PFuncNo + "' And Status=0 Order By SortID";

            return DataCommon.GetDataByReader(sSQL);
        }

        public static string GetSysFuncTitleByNo(string FuncNo)
        {
            string _ModuleTitle = "";
            string sSQL = "Select * from SysFunc_Info Where FuncNo='" + FuncNo.ToString() + "' And Status=0 Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            if (sdr.Read())
            {
                _ModuleTitle = sdr["FuncTitle"].ToString();
            }
            sdr.Close();
            return _ModuleTitle;
        }

        public static string GetNavigateUrlByFuncNo(string ModuleNo, string FuncNo)
        {
            string _NavigateUrl = "";
            string sSQL = "Select * from SysFunc_Info Where PModuleNo='" + ModuleNo + "' And FuncNo='" + FuncNo.ToString() + "' And Status=0 Order By SortID";
            SqlDataReader sdr = DataCommon.GetDataByReader(sSQL);
            if (sdr.Read())
            {
                _NavigateUrl = sdr["NavigateUrl"].ToString();
            }
            sdr.Close();

            if (_NavigateUrl.Length == 0)
            {
                sSQL = "Select * from SysFunc_Info Where PModuleNo='" + ModuleNo + "' And Status=0 Order By SortID";
                sdr = DataCommon.GetDataByReader(sSQL);
                if (sdr.Read())
                {
                    _NavigateUrl = sdr["NavigateUrl"].ToString();
                }
                sdr.Close();
            }
            return _NavigateUrl;
        }

    }
}
