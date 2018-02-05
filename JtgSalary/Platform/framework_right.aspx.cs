using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JtgTMS.Platform
{
    public partial class framework_right : System.Web.UI.Page
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
            ltlUserName.Text = SysClass.SysGlobal.GetCurrentOpName();
            ltlOrganName.Text = SysClass.SysGlobal.GetCurrentUserOrganName();
            ltlLastLoginDate.Text = SysClass.SysGlobal.GetLastDate();
            ltlLastLoginIP.Text = SysClass.SysGlobal.GetLastIp();
            LtlComputerName.Text = SysClass.SysGlobal.GetLastComputerName();

            string sWhereSQL = "";          
            sWhereSQL += " And c.ConsumeType = 0";

            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvLists, SysClass.SysToolReturn.GetNotReturnToolLstByReader(SysClass.SysGlobal.GetCurrentUserID(), sWhereSQL));

            sWhereSQL = "";
            sWhereSQL = " And c.ConsumeType = 1";
            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvBorrowLists, SysClass.SysToolReturn.GetNotReturnToolLstByReader(SysClass.SysGlobal.GetCurrentUserID(), sWhereSQL));

            sWhereSQL = " And ApprovalStatus=1";

            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvApprovaling, SysClass.SysDepotTool.GetToolOrderLstByDataSet(SysClass.SysGlobal.GetCurrentUserOrganID(), sWhereSQL));

            //采购申请权限
            trApprovaling.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(221, "");

            sWhereSQL = " And ShipingStatus=" + SysClass.SysDelivery.Delivery_Shipped.ToString(); ;

            sWhereSQL += " And a.DeliveryOrganID=" + SysClass.SysGlobal.GetCurrentUserOrganID().ToString();

            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvDeliveryNoStorage, SysClass.SysDelivery.GetDeliveryLstByDataSet(sWhereSQL));

            //待入库申请单权限
            gvDeliveryNoStorage.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(121, "");

        }
    }
}
