using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JtgSalary.MobilePlatform
{
    /// <summary>
    /// Summary description for GetUserNoticeDetailHandler
    /// </summary>
    public class GetUserNoticeDetailHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string result = "{}";
            string userID = context.Request.Form["userID"];
            string noticeID = context.Request.Form["noticeID"];

            if (GetData.MobileIsLogin(userID))
            {
                result = GetData.GetUserNoticeDetail(userID, Convert.ToInt32(noticeID));
            }
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}