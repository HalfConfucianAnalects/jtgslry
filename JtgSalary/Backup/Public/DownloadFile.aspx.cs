using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JtgTMS.Public
{
    public partial class DownloadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            DownloadFiles(Request.Params["FileName"].ToString(), Request.Params["SaveFileName"].ToString());
        }

        protected void DownloadFiles(string filename, string SaveFileName)
        {
            int intstart = filename.LastIndexOf("\\") + 1;
            if (SaveFileName.Length == 0)
                SaveFileName = filename.Substring(intstart, filename.Length - intstart);

            System.IO.FileInfo fi = new System.IO.FileInfo(filename);
            if (System.IO.File.Exists(Server.MapPath(filename)) == true)
            {
                string fileextname = fi.Extension;
                string default_content_type = "application/unknown";
                Microsoft.Win32.RegistryKey regkey, fileextkey;
                string filecontenttype;
                try
                {
                    regkey = Microsoft.Win32.Registry.ClassesRoot;
                    fileextkey = regkey.OpenSubKey(fileextname);
                    filecontenttype = fileextkey.GetValue("content type", default_content_type).ToString();
                }
                catch
                {
                    filecontenttype = default_content_type;
                }

                Response.Clear();
                //Response.Charset = "UTF-8";
                Response.Charset = "GB2312";
                Response.AddHeader("Content-Type", "text/html; charset=GB2312");

                Response.Buffer = true;
                this.EnableViewState = false;
                Response.ContentEncoding = System.Text.Encoding.Default;
                //Response.AppendHeader("content-disposition", "attachment;filename=" + Server.UrlEncode(SaveFileName.Trim()));
                Response.AppendHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(SaveFileName.Trim()));
                Response.ContentType = filecontenttype;

                Response.WriteFile(filename);
                Response.Flush();
                Response.Close();

                Response.End();

            }
            else
            {
                Response.Write("<script lanuage=javascript>alert('抱歉，文件不存在……！');location='javascript:history.go(-1)'</script>");
            }
        }
    }
}
