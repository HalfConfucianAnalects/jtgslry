using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JtgSalary.MobilePlatform
{
    /// <summary>
    /// Summary description for UpdateRegistrationIDHandler
    /// </summary>
    public class UpdateRegistrationIDHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string userID = context.Request.Form["userID"];
            string RegistrationID = context.Request.Form["RegistrationID"];
            string result = GetData.UpdateRegistrationID(userID, RegistrationID);

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