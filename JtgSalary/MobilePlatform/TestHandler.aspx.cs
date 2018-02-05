using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JtgSalary.MobilePlatform
{
    public partial class TestHandler : System.Web.UI.Page
    {
        private string hostIP = "127.0.0.1";//"127.0.0.1";"106.14.155.147:8081";
        private string urlHead = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            urlHead = "http://" + hostIP + "/MobilePlatform/";
        }

        protected void PostUrlMessage(string urlEnd, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlEnd);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postDataStr.Length;

            StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            writer.Write(postDataStr);
            writer.Flush();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码  
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string retString = reader.ReadToEnd();
            Response.Write("<script>alert('" + retString + "')</script>");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string urlEnd = urlHead+ "GetNameHandler.ashx";
            string postDataStr = "PhoneNum=441472";

            PostUrlMessage(urlEnd, postDataStr);
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            string urlEnd = urlHead + "UpdateRegistrationIDHandler.ashx";
            string postDataStr = "UserID=63&RegistrationID=789654123";

            PostUrlMessage(urlEnd, postDataStr);
        }
        protected void Button3_Click(object sender, EventArgs e)
        {
            string urlEnd = urlHead + "GetPhoneCodeHandler.ashx";
            string postDataStr = "UserID=63&PhoneNum=18268072407";

            PostUrlMessage(urlEnd, postDataStr);
        }
        protected void Button4_Click(object sender, EventArgs e)
        {
            string urlEnd = urlHead + "LoginHandler.ashx";
            string postDataStr = "UserID=70&Password=970222";

            PostUrlMessage(urlEnd, postDataStr);
        }
        protected void Button5_Click(object sender, EventArgs e)
        {
            string urlEnd = urlHead + "GetHeadPortraitHandler.ashx";
            string postDataStr = "UserID=70";

            PostUrlMessage(urlEnd, postDataStr);
        }
        protected void Button6_Click(object sender, EventArgs e)
        {
            string urlEnd = urlHead + "UpdateHeadPortraitHandler.ashx";
            string postDataStr = "UserID=70&HeadPortrait=123456789";

            PostUrlMessage(urlEnd, postDataStr);
            //GetData.UpdateHeadPortraitFile("aaa","nnn");
        }
        protected void Button7_Click(object sender, EventArgs e)
        {
            string urlEnd = urlHead + "GetSalaryHandler.ashx";
            string postDataStr = "userID=1748&OpCode=433127&SalaryYears=201710&isReadSignNum=0";

            PostUrlMessage(urlEnd, postDataStr);
        }
        protected void Button8_Click(object sender, EventArgs e)
        {
            string urlEnd = urlHead + "GetSalaryDetailHandler.ashx";
            string postDataStr = "userID=63&SalaryID=121266&SalaryYears=201409";

            PostUrlMessage(urlEnd, postDataStr);
        }
        protected void Button9_Click(object sender, EventArgs e)
        {
            string urlEnd = urlHead + "GetUserNoticeHandler.ashx";
            string postDataStr = "userID=3&userOrganID=37";

            PostUrlMessage(urlEnd, postDataStr);
        }
        protected void Button10_Click(object sender, EventArgs e)
        {
            string urlEnd = urlHead + "GetUserNoticeDetailHandler.ashx";
            string postDataStr = "UserID=3&noticeID=2";

            PostUrlMessage(urlEnd, postDataStr);
        }
        protected void Button11_Click(object sender, EventArgs e)
        {
            string urlEnd = urlHead + "GetUserProfileHandler.ashx";
            string postDataStr = "UserID=3";

            PostUrlMessage(urlEnd, postDataStr);
        }
        protected void Button12_Click(object sender, EventArgs e)
        {
            string urlEnd = urlHead + "UpdatePhoneNumHandler.ashx";
            string postDataStr = "UserID=3&PhoneNum=789654123&PhoneCode=196613";

            PostUrlMessage(urlEnd, postDataStr);
        }
        protected void Button13_Click(object sender, EventArgs e)
        {
            string urlEnd = urlHead + "UpdateSalaryHandler.ashx";
            string postDataStr = "userID=1748&UpdateID=612783&isToSignNum=1&Description=工资";

            PostUrlMessage(urlEnd, postDataStr);
        }

        protected void Button14_Click(object sender, EventArgs e)
        {
            Response.Redirect("DownLoadAndroidApp.aspx");
        }
    }
}