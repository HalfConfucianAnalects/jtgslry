using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace JtgTMS
{
    public partial class main : System.Web.UI.Page
    {
        public string _ModuleNo = "", _FuncNo = "", _DesktopHrf = "../Portal/IPDesktop.aspx", _LeftWidth = "0,7,*";

        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["Module"] != null)
            {
                _ModuleNo = Request.Params["Module"];
            }

            if (Request.Params["Func"] != null)
            {
                _FuncNo = Request.Params["Func"];
            }

            if (_ModuleNo.Length == 0)
            {
                string sWhereSQL = " And IsNull(IsVisible,0) = 1 And SystemID=" + SysClass.SysParams.GetPurviewSystemID().ToString();

                SqlDataReader sdr = SysClass.SysSystem.GetSysModuleLstByReader(sWhereSQL);
                if (sdr.Read())
                {
                    _ModuleNo = sdr["ModuleNo"].ToString();
                }
                sdr.Close();
            }
            else if ((_ModuleNo != "PersonalSalary") && (_ModuleNo != "SalaryHomePage"))
            {
                _LeftWidth = "180,7,*";
            }

            if (SysClass.SysParams.GetPurviewSystemID() == 1)
            {
                _DesktopHrf = "Portal/IPDesktop.aspx?Module=" + _ModuleNo;
            }
            else if (SysClass.SysParams.GetPurviewSystemID() == 2)
            {
                if (_FuncNo.Length > 0)
                {
                    SqlDataReader sdr = SysClass.SysSystem.GetSysFuncInfoByReader(_FuncNo);
                    if (sdr.Read())
                    {
                        _DesktopHrf = sdr["NavigateUrl"].ToString();
                    }
                    sdr.Close();
                }
                else
                {
                    _DesktopHrf = "Portal/SalaryDesktop.aspx?Module=" + _ModuleNo;
                }
            }
            else
            {
                _DesktopHrf = "Platform/framework_right.aspx?Module=" + _ModuleNo;
            }
        }
    }
}
