using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;

namespace JtgTMS.WebService
{
    /// <summary>
    /// GetSelfUserLst 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    [System.Web.Script.Services.ScriptService]
    public class GetSelfUserLst : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] GetCompleteList(string prefixText, int count)
        {
            if (count == 0)
                count = 12;
            List<String> list = new List<string>(count);
            list.Clear();
            SqlDataReader dr = SysClass.SysUser.QuerySelfOpNameLst(prefixText.Trim(), count);
            if (dr != null)
            {
                while (dr.Read())
                {
                    list.Add(dr["OpCode"].ToString() + "|" + dr["OpName"].ToString());
                }
                dr.Close();
            }
            return list.ToArray();
        }
    }
}
