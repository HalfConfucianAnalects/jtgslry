using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;

namespace JtgTMS.Admin
{
    public partial class Notice_Info : System.Web.UI.Page
    {
        public string _NoticeTitle = "", _RecGuid = "";
        public int _NoticeID = 0, _ViewType = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();
            if (Request.Params["NoticeID"] != null)
            {
                _NoticeID = int.Parse(Request.Params["NoticeID"]);
            }
            if (Request.Params["ViewType"] != null)
            {
                _ViewType = int.Parse(Request.Params["ViewType"]);
            }
             if (_NoticeID > 0)
            {
                _RecGuid = SysClass.SysNotice.GetRecGuidByNoticeID(_NoticeID);
            }
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            if (_NoticeID > 0)
            {
                ltDownloadFiles.Text = SysClass.SysUploadFile.GetHyperByUploadFiles(_RecGuid);

                SysClass.SysNotice.UpdateClickNum(_NoticeID);

                SqlDataReader sdr = SysClass.SysNotice.GetSingleToolsNoticeByReader(_NoticeID);
                if (sdr.Read())
                {
                    txtNoticeTitle.Text = sdr["NoticeTitle"].ToString();
                    txtContent.Text = sdr["NoticeBody"].ToString();
                }
                sdr.Close();
            }
            else
            {
               
            }
        }
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (_ViewType == 0)
            {
                Response.Redirect("MyNotice_Lst.aspx");
            }
            else if (_ViewType == 1)
            {
                Response.Redirect("Notice_Lst.aspx");
            }
            else if (_ViewType == 2)
            {
                Response.Redirect("../Portal/SalaryDesktop.aspx");
            }
        }   
    }
}
