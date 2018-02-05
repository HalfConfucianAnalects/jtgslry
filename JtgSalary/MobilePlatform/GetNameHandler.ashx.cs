using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml.Schema;

namespace JtgSalary.MobilePlatform
{
    /// <summary>
    /// Summary description for GetNameHandler
    /// </summary>
    public class GetNameHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string PhoneNum = context.Request.Form["PhoneNum"];
            string result = GetData.GetName(PhoneNum);
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