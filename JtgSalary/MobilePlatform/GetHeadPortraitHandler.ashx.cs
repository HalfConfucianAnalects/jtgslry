using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JtgSalary.MobilePlatform
{
    /// <summary>
    /// Summary description for GetHeadPortraitHandler
    /// </summary>
    public class GetHeadPortraitHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string result = "{}";
                string userID = context.Request.Form["userID"];
                if (GetData.MobileIsLogin(userID))
                {
                    result = GetData.GetUserHeadPortrait(userID);
                }
                context.Response.Write(result);
            }
            catch (Exception e)
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