using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using CyxPack.UserCommonOperation;

namespace JtgTMS.Admin
{
    public partial class Notice_Edit : System.Web.UI.Page
    {
        public string _NoticeTitle = "";
        public int _NoticeID = 0;
        string _RecGuid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();
            if (Request.Params["NoticeID"] != null)
            {
                _NoticeID = int.Parse(Request.Params["NoticeID"]);
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
            SysClass.SysOrgan.FullToOrganLstByTreeList(ddlOrganID, false);

            if (_NoticeID > 0)
            {
                ltDownloadFiles.Text = SysClass.SysUploadFile.GetHyperByUploadFiles(_RecGuid);

                SqlDataReader sdr = SysClass.SysNotice.GetSingleToolsNoticeByReader(_NoticeID);
                if (sdr.Read())
                {
                    txtNoticeTitle.Text = sdr["NoticeTitle"].ToString();
                    txtContent.Value = sdr["NoticeBody"].ToString();
                    if (ddlOrganID.Items.FindByValue(sdr["OrganID"].ToString()) != null)
                    {
                        ddlOrganID.SelectedValue = sdr["OrganID"].ToString();
                    }
                }
                sdr.Close();
            }
            else
            {
               
            }
        }
        
        private bool SaveCheck()
        {
            bool bFlag = true;            
            if (txtNoticeTitle.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtNoticeTitle, "通知名称不能为空！");
            }
            return bFlag;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                if (_RecGuid.Length == 0)
                {
                    _RecGuid = SysClass.SysGlobal.GetCreateGUID();
                }

                string sContent = Request["txtContent"].ToString().Replace("'", "''");

                string[] FieldValues ={                                          
                                     ddlOrganID.SelectedValue,
                                     txtNoticeTitle.Text,                                                                          
                                     sContent,
                                     _RecGuid
                                     };

                if ((SysClass.SysNotice.UpdateSingleToolsNotice(_NoticeID, FieldValues) >0) && (UpLoadFiles(SysClass.SysUploadFile.CS_Notice_Type, SysClass.SysUploadFile.CS_NOTICE_SUBCATEGORY) >= 0))
                {
                    Dialog.OpenDialogInAjax(upForm, "恭喜您，保存信息成功……", "Notice_Lst.aspx?page=" + SysClass.SysNotice.Notice_PageNo);
                }
            }
        }

        private int UpLoadFiles(int TableType, string sSubCategory)
        {
            StringBuilder sb = new StringBuilder();
            int attCount = 0;
            string sFileName = "";
            for (int i = 0; i < Request.Files.Count; i++)
            {
                try
                {
                    if ((Request.Files[i].ContentLength > 0))
                    {
                        sFileName = Request.Files[i].FileName;
                        string sWJBH = SysClass.SysGlobal.GetCreateGUID();
                        char[] de = { '\\' };
                        string[] Afilename = sFileName.Split(de);
                        string strFileName = Afilename[Afilename.Length - 1];
                        string[] AExt = sFileName.Split('.');
                        string strExt = "";
                        if (AExt.Length > 1)
                        {
                            strExt = AExt[AExt.Length - 1];
                        }
                        string sUploadFile = sWJBH;
                        if (strExt.Length > 0)
                        {
                            sUploadFile = sUploadFile + '.' + strExt;
                        }
                        if (sFileName.Length > 0)
                        {
                            sb.Append("Files" + attCount++ + ": " + sFileName + "<br>");

                            string sPath = Server.MapPath("~/" + SysClass.SysUploadFile.UploadDirectory);

                            if (Directory.Exists(sPath) == false)
                            {
                                Directory.CreateDirectory(sPath);
                            }

                            sPath = Server.MapPath("~/" + SysClass.SysUploadFile.UploadDirectory + "/" + sSubCategory);

                            if (Directory.Exists(sPath) == false)
                            {
                                Directory.CreateDirectory(sPath);
                            }

                            sPath = Server.MapPath("../" + SysClass.SysUploadFile.UploadDirectory + "/" + sSubCategory + "/" + DateTime.Now.ToString("yyyy"));

                            if (Directory.Exists(sPath) == false)
                            {
                                Directory.CreateDirectory(sPath);
                            }

                            sPath = Server.MapPath("../" + SysClass.SysUploadFile.UploadDirectory + "/" + sSubCategory + "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("yyyy-MM"));

                            if (Directory.Exists(sPath) == false)
                            {
                                Directory.CreateDirectory(sPath);
                            }

                            sPath = Server.MapPath("../" + SysClass.SysUploadFile.UploadDirectory + "/" + sSubCategory + "/" + DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("yyyy-MM") + "/" + DateTime.Now.ToString("yyyy-MM-dd"));

                            if (Directory.Exists(sPath) == false)
                            {
                                Directory.CreateDirectory(sPath);
                            }

                            Request.Files[i].SaveAs(Server.MapPath("..") + "\\" + SysClass.SysUploadFile.UploadDirectory + "\\" + sSubCategory + "\\" + DateTime.Now.ToString("yyyy") + "\\" + DateTime.Now.ToString("yyyy-MM") + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\" + sUploadFile);

                            SysClass.SysUploadFile.AddUploadFiles(TableType, _RecGuid, sSubCategory, sUploadFile, strFileName, strExt, Request.Files[i].ContentLength.ToString(), "");
                        }
                        else
                        {
                            sUploadFile = "";
                        }
                    }
                }
                catch
                {
                    Response.Write("附件: \"" + Request.Files[i].FileName + "\"出错!");
                }
            }
            if (Session["FilesControls"] != null)
            {
                Session.Remove("FilesControls");
            }
            sb.Insert(0, "您 上传了 " + attCount + " 个文件.<br>");
            //Response.Write(sb.ToString());

            return attCount;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Notice_Lst.aspx");
        }   
    }
}
