using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JtgTMS.Admin
{
    public partial class Tream_Frame : System.Web.UI.Page
    {
        public int _POrganID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            _POrganID = SysClass.SysOrgan.GetTopOrganID(0);

            _POrganID = SysClass.SysOrgan.GetTopWorkShopID(_POrganID);

            _POrganID = SysClass.SysOrgan.GetTopDeptID(_POrganID);
        }
    }
}
