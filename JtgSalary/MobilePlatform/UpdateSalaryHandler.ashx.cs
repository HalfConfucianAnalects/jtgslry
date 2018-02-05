using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JtgSalary.MobilePlatform
{
    /// <summary>
    /// Summary description for UpdateSalaryHandler
    /// </summary>
    public class UpdateSalaryHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string result = "{}";
            string userID = context.Request.Form["userID"];
            string UpdateID = context.Request.Form["UpdateID"];
            bool isToSign = Convert.ToInt32(context.Request.Form["isToSignNum"]) == 1;
            string Description = context.Request.Form["Description"];

            if (GetData.MobileIsLogin(userID))
            {
                result = GetData.UpdateSalary(UpdateID, isToSign, Description);
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