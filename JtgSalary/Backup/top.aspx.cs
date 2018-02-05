using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace JtgTMS
{
    public partial class top : System.Web.UI.Page
    {
        public string _ModuleNo = "HomePage";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["Module"] != null)
            {
                _ModuleNo = Request.Params["Module"];

            }
            SysClass.SysGlobal.CheckSysIsLogined();

            lblTime.Text = DateTime.Now.ToString("HH:mm:ss");
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            lblTitle.Text = SysClass.SysParams.GetPurviewSystemTitle();

            lblUserName.Text = SysClass.SysGlobal.GetCurrentOpName();
            lblOrganName.Text = SysClass.SysGlobal.GetCurrentUserOrganName();

            int i = 0;

            string sWhereSQL = " And IsNull(IsVisible,0) = 1 And SystemID=" + SysClass.SysParams.GetPurviewSystemID().ToString();

            string sOldInnerHtml = navigation.InnerHtml;

            navigation.InnerHtml = "";

            SqlDataReader sdr = SysClass.SysSystem.GetSysModuleLstByReader(sWhereSQL);
            while (sdr.Read())
            {
                int _PurviewTag = int.Parse(sdr["PurviewTag"].ToString());
                if (_PurviewTag <= 0 || CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(_PurviewTag, ""))
                {
                    if (i > 0)
                    {
                        navigation.InnerHtml += "<label style='color:white' runat='server'>|</label>";
                    }
                    //navigation.InnerHtml += "<a onclick='clickLink(this);' href='" + sdr["NavigateUrl"].ToString() + "' target='left'>" + sdr["ModuleTitle"].ToString() + "</a>";

                    navigation.InnerHtml += "<a onclick='clickLink(this);' href='" + sdr["NavigateUrl"].ToString() + "'";

                    if (_ModuleNo.ToLower() == sdr["ModuleNo"].ToString().ToLower())
                    {
                        navigation.InnerHtml += "style='font-weight:bold;color:yellow'";
                    }
                    else
                    {
                        navigation.InnerHtml += "style='font-weight:normal'";
                    }

                    navigation.InnerHtml += " target='_top'>" + sdr["ModuleTitle"].ToString() + "</a>";
                    i++;
                }
            }
            sdr.Close();

            navigation.InnerHtml += sOldInnerHtml;

            //hyToolMsg.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(1, "车间工具");
            //lblSeparator1.Visible = hyToolMsg.Visible;

            //hyDepotTool.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(2, "段工资签收");
            //lblSeparator2.Visible = hyDepotTool.Visible;

            //hyDelivery.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(3, "采购配送");
            //lblSeparator3.Visible = hyDelivery.Visible;

            //hyWarning.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(4, "系统预警");
            //lblSeparator4.Visible = hyWarning.Visible;

            //hyReport.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(5, "统计查询");
            //lblSeparator5.Visible = hyReport.Visible;

            //hyBasicData.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(6, "基础数据");
            //lblSeparator6.Visible = hyBasicData.Visible;

            //hyAdmin.Visible = CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(7, "系统管理");
            //lblSeparator7.Visible = hyAdmin.Visible;
        }
    }
}
