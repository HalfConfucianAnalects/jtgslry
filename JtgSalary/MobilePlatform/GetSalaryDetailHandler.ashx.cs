using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JtgSalary.MobilePlatform
{
    /// <summary>
    /// Summary description for GetSalaryDetailHandler
    /// </summary>
    public class GetSalaryDetailHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string result = "{}";
            string userID = context.Request.Form["userID"];
            string SalaryID = context.Request.Form["SalaryID"];
            string SalaryYears = context.Request.Form["SalaryYears"];
            if (GetData.MobileIsLogin(userID))
            {
                result = GetData.GetSalaryDetail(SalaryID,  SalaryYears);
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