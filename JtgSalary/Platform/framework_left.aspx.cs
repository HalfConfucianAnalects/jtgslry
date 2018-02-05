using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace JtgTMS.Platform
{
    public partial class framework_left : System.Web.UI.Page
    {
        public string _ModuleNo = "HomePage", _FuncNo = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["Module"] != null)
            {
                _ModuleNo = Request.Params["Module"];
            }

            if (Request.Params["Func"] != null)
            {
                _FuncNo = Request.Params["Func"];
            }
        
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            lbModuleTitle.Text = SysClass.SysSystem.GetSysModuleTitleByNo(_ModuleNo);            

            SqlDataReader sdr = SysClass.SysSystem.GetSysFuncLstByReader(_ModuleNo, _FuncNo);
            while (sdr.Read())
            {
                int _PurviewTag = int.Parse(sdr["PurviewTag"].ToString());
                if (_PurviewTag <= 0 || CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(_PurviewTag, ""))
                {
                    ltFunc.Text += "<tr style='display:' treeItemLevel='1'>"
                    + " <td nowrap>"
                    + "     <img align='absmiddle' style='cursor:pointer;' onClick='displayChildren(this);' isOpen='true' src='" + sdr["ImageUrl"].ToString() + "'/>"
                    +"      <img align='absmiddle' src='/sitefiles/bairong/Icons/menu/forum.gif'/>&nbsp;" + sdr["FuncTitle"]
                    + "	 </td>"
                    + "</tr>";
                    SqlDataReader sdr1 = SysClass.SysSystem.GetSysFuncLstByReader(_ModuleNo, sdr["FuncNo"].ToString());
                    while (sdr1.Read())
                    {
                        int _PurviewTag1 = int.Parse(sdr1["PurviewTag"].ToString());
                        if (_PurviewTag1 <= 0 || CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(_PurviewTag1, ""))
                        {
                            if (sdr1["NavigateUrl"].ToString().Length > 0)
                            {
                                ltFunc.Text += "<tr style='display: None1' treeitemlevel='2'>";

                                ltFunc.Text += " <td nowrap>"
                                    + " <img align='absmiddle' src='/sitefiles/bairong/icons/tree/empty.gif' />"
                                    + " <img align='absmiddle' src='/sitefiles/bairong/icons/tree/empty.gif' />"
                                    + " <img align='absmiddle' src='" + sdr1["ImageUrl"].ToString() + "' />&nbsp;"
                                    + " <a href='" + sdr1["NavigateUrl"].ToString() + "' target='right' onclick='openFolderByA(this);' istreelink='true'>" + sdr1["FuncTitle"].ToString() + "</a>"
                                    + " </td>"
                                    + " </tr>";

                            }
                            else
                            {
                                ltFunc.Text += "<tr style='display: ' treeitemlevel='2'>"
                                    + " <td nowrap>"
                                    + " <img align='absmiddle' src='/sitefiles/bairong/icons/tree/empty.gif' /><img align='absmiddle'"
                                    + " style='cursor: pointer;' onclick='displayChildren(this);' isopen='false' src='/sitefiles/bairong/icons/tree/plus.gif' /><img"
                                    + " align='absmiddle' src='" + sdr1["ImageUrl"].ToString() + "' />&nbsp;" + sdr1["FuncTitle"].ToString()
                                    + " </td>"
                                    + " </tr>";
                                SqlDataReader sdr2 = SysClass.SysSystem.GetSysFuncLstByReader(_ModuleNo, sdr1["FuncNo"].ToString());
                                while (sdr2.Read())
                                {
                                    int _PurviewTag2 = int.Parse(sdr2["PurviewTag"].ToString());
                                    if (_PurviewTag2 <= 0 || CyxPack.UserCommonOperation.UserCommonOperation.PurviewByID(_PurviewTag2, ""))
                                    {
                                        ltFunc.Text += "<tr style='display:None' treeitemlevel='3'>"
                                        + " <td nowrap>"
                                        + "     <img align='absmiddle' src='/sitefiles/bairong/icons/tree/empty.gif' /><img align='absmiddle'"
                                        + "     src='/sitefiles/bairong/icons/tree/empty.gif' /><img align='absmiddle' src='/sitefiles/bairong/icons/tree/empty.gif' /><img"
                                        + " align='absmiddle' src='" + sdr2["ImageUrl"].ToString() + "' />&nbsp;<a href='" + sdr2["NavigateUrl"].ToString() + "'"
                                        + " target='right' onclick='openFolderByA(this);' istreelink='true'>" + sdr2["FuncTitle"].ToString() + "</a>"
                                        + " </td>"
                                        + " </tr>";
                                    }
                                }
                                sdr2.Close();
                            }
                        }
                    }
                    sdr1.Close();
                }
            }
            sdr.Close();
        }
    }
}
