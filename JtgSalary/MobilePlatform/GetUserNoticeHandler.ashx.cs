using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JtgSalary.MobilePlatform
{
    /// <summary>
    /// Summary description for GetUserNoticeHandler
    /// </summary>
    public class GetUserNoticeHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string result = "{}";
            string userID = context.Request.Form["userID"];
            string userOrganID = context.Request.Form["userOrganID"];

            if (GetData.MobileIsLogin(userID))
            {
                result = GetData.GetUserNotice(userID, userOrganID);
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