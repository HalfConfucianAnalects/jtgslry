using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JtgTMS.Portal
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ///清空Session的值，并停止Session
            //SysClass.SysGlobal.ClearPersonRefreshTime(Session);
            //CyxPack.UserCommonOperation.UserCommonOperation.ClearAndAbandon();
            Response.Redirect("~/Portal/Login.aspx");
        }
    }
}
