using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JtgTMS.BasicData
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
            trFunc61.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(61, "系统预警设置");
            trFunc611.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(611, "库存预警设置")
                && trFunc61.Visible;
            trFunc612.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(612, "采购额度设置")
                && trFunc61.Visible;
            trFunc613.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(613, "借用预警设置")
                && trFunc61.Visible;
            trFunc614.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(614, "收货提醒设置")
                && trFunc61.Visible;
        }
    }
}
