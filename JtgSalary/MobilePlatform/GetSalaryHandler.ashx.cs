using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JtgSalary.MobilePlatform
{
    /// <summary>
    /// Summary description for GetSalaryHandler
    /// </summary>
    public class GetSalaryHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string result = "{}";
            string userID = context.Request.Form["userID"];
            string OpCode = context.Request.Form["OpCode"];
            string SalaryYears = context.Request.Form["SalaryYears"];
            bool isReadSign = Convert.ToInt32(context.Request.Form["isReadSignNum"]) == 1;
            if (GetData.MobileIsLogin(userID))
            {
                result = GetData.GetSalary(OpCode, SalaryYears, isReadSign);
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