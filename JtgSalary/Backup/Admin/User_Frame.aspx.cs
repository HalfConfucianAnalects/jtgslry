using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JtgTMS.Admin
{
    public partial class User_Frame : System.Web.UI.Page
    {
        public int _OrganID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            _OrganID = SysClass.SysOrgan.GetTopOrganID(0);
        }
    }
}
