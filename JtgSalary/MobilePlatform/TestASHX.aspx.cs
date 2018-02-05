using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JtgTMS.SysClass;
using System.Data.SqlClient;
using CyxPack.OperateSqlServer;

namespace JtgSalary.MobilePlatform
{
    public partial class TestASHX : System.Web.UI.Page
    {
        private string hostIP = "127.0.0.1";//"127.0.0.1";"106.14.155.147:8081";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void PostUrlMessage(string url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
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

        protected void Button9_Click(object sender, EventArgs e)
        {
            string url = "http://"+ hostIP + "/MobilePlatform/GetNameHandler.ashx";
            string postDataStr = "PhoneNum=437212";
            //string url = "http://localhost/MobilePlatform/GetPhoneCodeHandler.ashx";
            //string postDataStr = "PhoneNum=18268072407";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
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

        protected void Button10_Click(object sender, EventArgs e)
        {
            //string url = "http://localhost/MobilePlatform/GetSalaryDetailHandler.ashx";
            //string postDataStr = "SalaryID=94205&SalaryYears=201409";
            string url = "http://localhost/MobilePlatform/GetSalaryHandler.ashx";
            string postDataStr = "OpCode=admin&SalaryYears=201409&isReadSignNum=1";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
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

        protected void Button11_Click(object sender, EventArgs e)
        {
            string url = "http://" + hostIP + "/MobilePlatform/GetSalaryHandler.ashx";
            string postDataStr = "userID=63&OpCode=437212&SalaryYears=201407&isReadSignNum=0";
            PostUrlMessage(url, postDataStr);
        }

        protected void Button12_Click(object sender, EventArgs e)
        {
            string url = "http://" + hostIP + "/MobilePlatform/GetSalaryDetailHandler.ashx";
            string postDataStr = "userID=63&SalaryID=90291&SalaryYears=201407";
            PostUrlMessage(url, postDataStr);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //PushMessage pm = new PushMessage();
            //pm.message = "请及时签收工资信息。";
            //pm.SendAllAliasMessage();

            //PushDevice.Start();
            //PushMessageJobScheduler.GetTargetTime();
            PushMessageJobScheduler.Start();

            //PushMessage pm = new PushMessage();
            //pm.message = "Test Test Test";
            ////pm.SendMessage();
            //Response.Write("<script>alert('" + pm.SendMessage() + "')</script>");

            //SchedulePushMessage spm = new SchedulePushMessage();
            //spm.message = "Schedule Test Test Test";
            //Response.Write("<script>alert('" + spm.SendMessage() + "')</script>");
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            SysUserSalary.GetWarmingLstByDataSet(" OrganID = 37 And DateDiff(Day, SalaryDate, GetDate()) >= 10");
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            SqlDataReader sdr = GetUserRolePurviewByReader(3);
            while (sdr.Read())
            {
                Response.Write("<script>alert('" + sdr["ID"].ToString() + "')</script>");
            }
            sdr.Close();
        }

        public SqlDataReader GetUserRolePurviewByReader(int UserID)
        {
            string sSQL = "select * from SysRole_Info where Status=0 and IsNull(SystemID,0)=0"
                + " and ID In (Select RoleID From SysUserRoles_Info Where Status=0 "// IsNull(SystemID,0)=" + SysParams.GetPurviewSystemID().ToString() 
                + " And UserID=" + UserID.ToString() + ")";
            return DataCommon.GetDataByReader(sSQL);
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            string url = "http://" + hostIP + "/MobilePlatform/GetPhoneCodeHandler.ashx";
            string postDataStr = "UserID=3&PhoneNum=18358585620";//18358585620

            PostUrlMessage(url, postDataStr);
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            string url = "http://" + hostIP + "/MobilePlatform/LoginHandler.ashx";
            string postDataStr = "UserID=63&Password=1111112";

            PostUrlMessage(url, postDataStr);
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            string url = "http://" + hostIP + "/MobilePlatform/UpdateRegistrationIDHandler.ashx";
            string postDataStr = "UserID=3&RegistrationID=789654123";

            PostUrlMessage(url, postDataStr);
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            string url = "http://" + hostIP + "/MobilePlatform/UpdatePhoneNumHandler.ashx";
            string postDataStr = "UserID=3&PhoneNum=789654123&PhoneCode=196613";

            PostUrlMessage(url, postDataStr);
        }
    }
}