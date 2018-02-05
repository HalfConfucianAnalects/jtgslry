using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JtgSalary.MobilePlatform
{
    /// <summary>
    /// Summary description for GetPhoneCodeHandler
    /// </summary>
    public class GetPhoneCodeHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string UserID = context.Request.Form["UserID"];
            string PhoneNum = context.Request.Form["PhoneNum"];
            string result = GetData.GetPhoneCode(UserID,PhoneNum);
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