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
    public partial class UserSalary_Edit : System.Web.UI.Page
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
                _ReturnPage = "UserSalaryApproval_Lst.aspx";
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
                    CyxPack.CommonOperation.DataBinder.BindDataListData(dlList, SysClass.SysUserSalary.GetUserSalaryFieldsLstByReader(txtUserSalaryYears.Text));

                    for (int i = 0; i < dlList.Items.Count; i++)
                    {
                        SalaryControl.SalaryEdit da = (SalaryControl.SalaryEdit)dlList.Items[i].FindControl("SalaryEdit2");
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
                        SalaryControl.SalaryEdit da = (SalaryControl.SalaryEdit)dlList.Items[i].FindControl("SalaryEdit2");
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

        protected void btnChoiceUser_Click(object sender, EventArgs e)
        {
            txtUserSalaryUserID.Text = SysClass.SysParams.UserInfo_ChoiceValues;
            txtUserSalaryOpCode.Text = SysClass.SysUser.GetOpCodeByUserID(int.Parse(SysClass.SysParams.UserInfo_ChoiceValues));
            txtUserSalaryOpName.Text = SysClass.SysUser.GetUserNameByUserID(int.Parse(SysClass.SysParams.UserInfo_ChoiceValues));

            SysClass.SysParams.UserInfo_ChoiceValues = "";
        }

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (txtUserSalaryOpCode.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtUserSalaryOpCode, "领用人不能为空！");
            }

            return bFlag;
        }

        protected void txtUserSalaryOpCode_TextChanged(object sender, EventArgs e)
        {
            txtUserSalaryOpCode.Text = CyxPack.CommonOperation.DealwithString.GetStringPrefix(txtUserSalaryOpCode.Text);

            txtUserSalaryUserID.Text = SysClass.SysUser.GetSelfUserIDByOpCode(txtUserSalaryOpCode.Text).ToString();
            txtUserSalaryOpName.Text = SysClass.SysUser.GetSelfUserNameByOpCode(txtUserSalaryOpCode.Text);
            if ((txtUserSalaryUserID.Text == "0") || (txtUserSalaryUserID.Text.Length == 0))
            {
                txtUserSalaryOpCode.Text = "";
                Dialog.OpenDialogInAjax(txtUserSalaryOpCode, "工号" + txtUserSalaryOpCode.Text + "不存在！");
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                if (_TableRecGuid.Length == 0)
                {
                    _TableRecGuid = SysClass.SysGlobal.GetCreateGUID();
                }

                int _UserSalaryUserID = 0;

                _UserSalaryUserID = int.Parse(txtUserSalaryUserID.Text);

                string _UserSalaryNo = "";

                if (_UserSalaryID > 0)
                {
                    _UserSalaryNo = txtUserSalaryOpCode.Text;
                }
                else
                {
                    _UserSalaryNo = SysClass.SysGlobal.GetTableOrderNo(SysClass.SysUserSalary.Sonsume_TableName);
                }

                string[] FieldValues ={
                                          _TableRecGuid,
                                         txtUserSalaryOpCode.Text,
                                         txtUserSalaryYears.Text
                                        };

                if (SysClass.SysUserSalary.UpdateSingleUserSalary(_UserSalaryID, FieldValues
                    , GetSavePackToolMemberSQL(_TableRecGuid)
                    , GetSaveNewFields(_TableRecGuid)
                    , GetSaveNewFieldValues(_TableRecGuid)) > 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "info", "<script>UpdateSuccess();</script>");
                    //Dialog.OpenDialogInAjax(upForm, "恭喜您，保存信息成功……", _ReturnPage);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "info", "<script>alert('数据保存失败.');</script>");
                }
            }

        }

        private string GetSaveNewFields(string BillTableRecGuid)
        {
            string sUpdateSQL = "";
            for (int i = 0; i < dlList.Items.Count; i++)
            {
                SalaryControl.SalaryEdit da = (SalaryControl.SalaryEdit)dlList.Items[i].FindControl("SalaryEdit2");
                sUpdateSQL += "," + da.UserFieldName;
            }
            return sUpdateSQL;
        }

        private string GetSaveNewFieldValues(string BillTableRecGuid)
        {
            string sUpdateSQL = "";
            for (int i = 0; i < dlList.Items.Count; i++)
            {
                SalaryControl.SalaryEdit da = (SalaryControl.SalaryEdit)dlList.Items[i].FindControl("SalaryEdit2");
                sUpdateSQL += "," + da.GetNewFieldValue;
            }
            return sUpdateSQL;
        }

        private string GetSavePackToolMemberSQL(string BillTableRecGuid)
        {
            string sUpdateSQL = "";
            for (int i = 0; i < dlList.Items.Count; i++)
            {
                SalaryControl.SalaryEdit da = (SalaryControl.SalaryEdit)dlList.Items[i].FindControl("SalaryEdit2");
                sUpdateSQL += da.GetUpdateSQL;
            }
            return sUpdateSQL;
        }

        protected void txtUserSalaryYears_TextChanged(object sender, EventArgs e)
        {
            CyxPack.CommonOperation.DataBinder.BindDataListData(dlList, SysClass.SysUserSalary.GetUserSalaryFieldsLstByReader(txtUserSalaryYears.Text));
        }             

    }
}
