using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;
using System.Data;

namespace JtgTMS.PersonSalary
{
    public partial class UserSalary_Info : System.Web.UI.Page
    {
        public int _UserSalaryID = 0;
        public string _ReturnPage = "";
        private string _TableRecGuid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["UserSalaryID"] != null)
            {
                _UserSalaryID = int.Parse(Request.Params["UserSalaryID"]);
            }

            //返回页面
            int _ReturnPageType = 0;
            if (Request.Params["ReturnPageType"] != null)
            {
                _ReturnPageType = int.Parse(Request.Params["ReturnPageType"]);
            }

            if (_ReturnPageType == 0)
            {
                _ReturnPage = "UserSalary_Lst.aspx";
            }
            else if (_ReturnPageType == 1)
            {
                _ReturnPage = "MySignUserSalary_Lst.aspx";
            }
            else if (_ReturnPageType == 1)
            {
                _ReturnPage = "MySignUserSalary_Lst.aspx";
            }
            else if (_ReturnPageType == 1)
            {
                _ReturnPage = "UserNotSignSalary_Lst.aspx";
            }
            else if (_ReturnPageType == 1)
            {
                _ReturnPage = "UserSalary_Lst.aspx";
            }

            _TableRecGuid = SysClass.SysUserSalary.GetTableRecGuidByID(_UserSalaryID);

            if (!Page.IsPostBack)
            {
                AjaxPro.Utility.RegisterTypeForAjax(typeof(JtgTMS.PersonSalary.UserSalary_Edit));

                BindPageData();
            }
        }

        protected void BindPageData()
        {
            string _Years = "";
            if (_UserSalaryID > 0)
            {
                SqlDataReader sdr = SysClass.SysUserSalary.GetSingleUserSalaryByReader(_UserSalaryID);
                if (sdr.Read())
                {
                    _Years = sdr["SalaryYears"].ToString();
                    txtUserSalaryYears.Text = sdr["SalaryYears"].ToString();
                    txtUserSalaryOpCode.Text = sdr["OpCode"].ToString();
                    txtUserSalaryOpName.Text = sdr["OpName"].ToString();

                    string sSQLWhere = "";
                    CyxPack.CommonOperation.DataBinder.BindDataListData(dlList, SysClass.SysUserSalary.GetUserSalaryFieldsLstByReader(txtUserSalaryYears.Text, sdr["SalaryRecGuid"].ToString(), ""));

                    for (int i = 0; i < dlList.Items.Count; i++)
                    {
                        SalaryControl.SalaryInfo da = (SalaryControl.SalaryInfo)dlList.Items[i].FindControl("SalaryInfo1");
                        da.UserFieldValue = sdr[da.UserFieldName].ToString();

                        if (sdr[da.UserFieldName].ToString().Length == 0)
                        {
                            sSQLWhere += " And a.FieldName<>'" + da.UserFieldName + "'";
                        }
                        else if (sdr[da.UserFieldName].ToString() == "0.00")
                        {
                            sSQLWhere += " And  a.FieldName<>'" + da.UserFieldName + "'";
                        }
                    }

                    CyxPack.CommonOperation.DataBinder.BindDataListData(dlList, SysClass.SysUserSalary.GetUserSalaryFieldsLstByReader(txtUserSalaryYears.Text, sdr["SalaryRecGuid"].ToString(), sSQLWhere));

                    for (int i = 0; i < dlList.Items.Count; i++)
                    {
                        SalaryControl.SalaryInfo da = (SalaryControl.SalaryInfo)dlList.Items[i].FindControl("SalaryInfo1");
                        da.UserFieldValue = sdr[da.UserFieldName].ToString();

                        
                    }
                }
                sdr.Close();
            }
            else
            {
                
            }

            txtUserSalaryYears.Enabled = _UserSalaryID <= 0;

        } 
    }
}
