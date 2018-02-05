using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JtgSalary.MobilePlatform
{
    /// <summary>
    /// Summary description for UpdatePhoneNumHandler
    /// </summary>
    public class UpdatePhoneNumHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string result = "{}";
            string userID = context.Request.Form["userID"];
            string PhoneNum = context.Request.Form["PhoneNum"];
            string PhoneCode = context.Request.Form["PhoneCode"];

            if (GetData.MobileIsLogin(userID))
            {
                result = GetData.UpdateUserPhoneNum(userID, PhoneNum, PhoneCode);
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