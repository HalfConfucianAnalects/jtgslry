using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JtgTMS.Admin
{
    public partial class Left : System.Web.UI.Page
    {
         protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

         private void BindPageData()
         {
             trFunc71.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(71, "机构部门管理");
             trFunc711.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(711, "车间管理")
                 && trFunc71.Visible;
             trFunc712.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(712, "部门管理")
                 && trFunc71.Visible;
             trFunc713.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(713, "班组管理")
                 && trFunc71.Visible;
             trFunc714.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(714, "供应商管理")
                 && trFunc71.Visible;

             trFunc72.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(72, "用户管理");
             trFunc721.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(721, "用户")
                 && trFunc72.Visible;
             trFunc722.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(722, "添加角色")
                 && trFunc72.Visible;
             trFunc723.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(723, "角色管理")
                 && trFunc72.Visible;
             trFunc724.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(724, "操作日志")
                 && trFunc72.Visible;

         }
    }
}
