using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JtgTMS.Platform
{
    public partial class IPDesktop : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvLists, SysClass.SysInterPhone.GetNotReturnInterPhoneLstByReader(SysClass.SysGlobal.GetCurrentUserID(), ""));

            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvBorrowLists, SysClass.SysInterPhone.GetNotReturnBorrowPhoneLstByReader(SysClass.SysGlobal.GetCurrentUserID(), ""));

            string sWhereSQL = " And a.Isfinish=1 ";


            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvApprovaling, SysClass.SysInterPhone.GetOrderBDset(sWhereSQL));


            sWhereSQL = "";

            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvNotEvaluateList, SysClass.InterPhoneCarry.GetListNotByset(sWhereSQL));

            //无线电台评价
            trNotEvaluateList.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(1557, "") || CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(2256, "");

            ltlUserName.Text = SysClass.SysGlobal.GetCurrentOpName();
            ltlOrganName.Text = SysClass.SysGlobal.GetCurrentUserOrganName();
            ltlLastLoginDate.Text = SysClass.SysGlobal.GetLastDate();
            ltlLastLoginIP.Text = SysClass.SysGlobal.GetLastIp();
            LtlComputerName.Text = SysClass.SysGlobal.GetLastComputerName();
           
        }
    }
}
