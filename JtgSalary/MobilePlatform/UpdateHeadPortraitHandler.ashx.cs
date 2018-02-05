using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace JtgSalary.MobilePlatform
{
    /// <summary>
    /// Summary description for SaveHeadPortraitHandler
    /// </summary>
    public class UpdateHeadPortraitHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string result = "{}";
                string userID = context.Request.Form["userID"];
                string HeadPortrait = context.Request.Form["HeadPortrait"];

                if (GetData.MobileIsLogin(userID))
                {
                    result = GetData.UpdateUserHeadPortrait(userID, HeadPortrait);
                }
                context.Response.Write(result);
            }
            catch (Exception ex)
            {
                //
            }
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
 