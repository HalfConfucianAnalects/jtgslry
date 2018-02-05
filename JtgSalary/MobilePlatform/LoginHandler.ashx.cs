using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace JtgSalary.MobilePlatform
{
    /// <summary>
    /// Summary description for LoginHandler
    /// </summary>
    public class LoginHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string userID = context.Request.Form["userID"];
            string Password = context.Request.Form["Password"];
            string result = "{}";
            if (GetData.IsNeedCode(userID))
            {
                string PhoneCode = context.Request.Form["PhoneCode"];
                result = GetData.UserLoginByPhoneCode(userID, Password, PhoneCode);
            }
            else
            {
                result = GetData.UserLogin(userID, Password);
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