using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JtgSalary.MobilePlatform
{
    public partial class DownLoadAndroidApp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string fileName = HttpContext.Current.Server.UrlEncode("JtgSalaryApp.apk");
            string filePath = HttpContext.Current.Server.MapPath("JtgSalaryApp.apk");

            FileInfo info = new FileInfo(filePath);
            long fileSize = info.Length;
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachement;filename=" + fileName);
            //指定文件大小    
            Response.AddHeader("Content-Length", fileSize.ToString());
            Response.WriteFile(filePath, 0, fileSize);
            Response.Flush();
            Response.Close();
        }
    }
}