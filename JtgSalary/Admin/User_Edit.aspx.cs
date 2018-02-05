using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;
using CyxPack.UserCommonOperation;
using CyxPack.OperateSqlServer;

namespace JtgTMS.Admin
{
    public partial class User_Edit : System.Web.UI.Page
    {
        public string _OrganName = "", _RecGuid = "";
        public int _UserID = 0, _OrganID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();
            if (Request.Params["OrganID"] != null)
            {
                _OrganID = int.Parse(Request.Params["OrganID"]);
            }
            if (Request.Params["UserID"] != null)
            {
                _UserID = int.Parse(Request.Params["UserID"]);
            }
            _RecGuid = SysClass.SysUser.GetRecGuidByUserID(_UserID);
            _OrganName = SysClass.SysOrgan.GetOrganNameByID(_OrganID);
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            //自定义字段
            CyxPack.CommonOperation.DataBinder.BindDataListData(dlList, SysClass.SysCustomField.GetCustomFieldLstByReader(" And TableNo='UserInfo'"));

            //
            LoadChildNodes(0, 0);

            SysClass.SysRole.FullToRoleLst(ddcRole);

            if (_UserID > 0)
            {
                rvfPassword.Enabled = false;
                rfvConfirmPassword.Enabled = false;
                cvNewPasswordCompare.Enabled = false;
                trPassword.Style.Add("display", "none");
                SqlDataReader sdr = SysClass.SysUser.GetSingleUserByReader(_UserID);
                if (sdr.Read())
                {
                    if (ddlOrganID.Items.FindByValue(sdr["OrganID"].ToString()) != null)
                    {
                        ddlOrganID.SelectedValue = sdr["OrganID"].ToString();
                    }
                    txtOpCode.Text = sdr["OpCode"].ToString();
                    txtOpName.Text = sdr["OpName"].ToString();
                    txtPosition.Text = sdr["Position"].ToString();
                    txtPlace.Text = sdr["Place"].ToString();
                    rblSex.SelectedValue = sdr["Sex"].ToString();
                    txtPhone.Text = sdr["Phone"].ToString();
                    txtTelNo.Text = sdr["TelNo"].ToString();
                    txtAddress.Text = sdr["Address"].ToString();
                    txtZipCode.Text = sdr["ZipCode"].ToString();
                    txtUserDesc.Text = sdr["UserDesc"].ToString();
                    txtIDNumber.Text = sdr["IDNumber"].ToString();
                    if (rbIsCanLogin.Items.FindByValue(sdr["IsCanLogin"].ToString()) != null)
                    {
                        rbIsCanLogin.SelectedValue = sdr["IsCanLogin"].ToString();
                    }
                }

                for (int i = 0; i < dlList.Items.Count; i++)
                {
                    SalaryControl.CustomExtEdit da = (SalaryControl.CustomExtEdit)dlList.Items[i].FindControl("WorklogExtEdit1");
                    da.UserFieldValue = sdr[da.UserFieldName].ToString();
                }

                sdr.Close();

                LoadUserRoles();
            }
            else
            {
                if (ddlOrganID.Items.FindByValue(_OrganID.ToString()) != null)
                {
                    ddlOrganID.SelectedValue = _OrganID.ToString();
                }
                txtOpCode.Text = "";
                txtOpName.Text = "";
                txtPassword.Text = "";
                txtConfirmPassword.Text = "";
            }
        }
        private void LoadUserRoles()
        {
            SqlDataReader sdr = SysClass.SysRole.GetUserRolePurviewByReader(_UserID);
            while (sdr.Read())
            {
                ListItem liItem = ddcRole.Items.FindByValue(sdr["ID"].ToString());
                if (liItem != null)
                {
                    liItem.Selected = true;
                }
            }
            sdr.Close();
        }


        private void LoadChildNodes(int POrganID, int Level)
        {
            SqlDataReader sdr = SysClass.SysOrgan.GetOrganLstByReader(POrganID);
            while (sdr.Read())
            {
                ListItem newItem = new ListItem();
                newItem.Value = sdr["ID"].ToString();
                newItem.Text = HttpUtility.HtmlDecode(SysClass.SysGlobal.RepeatString("　", Level) + "" + sdr["OrganName"].ToString());
                ddlOrganID.Items.Add(newItem);

                LoadChildNodes(int.Parse(sdr["ID"].ToString()), Level+1);
            }
            sdr.Close();
        }   

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (txtOpCode.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtOpCode, "工号不能为空！");
            }
            else if (SysClass.SysUser.CheckOpCodeExists(_UserID, txtOpCode.Text))
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtOpCode, "工号不能重复！");
            }
            else if (txtOpName.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtOpName, "姓名不能为空！");
            }
            return bFlag;
        }

        public string UpdateSaveUserRoles()
        {           
            if (_UserID <= 0)
            {
                _UserID = SysClass.SysUser.GetUserIDByOpCode(txtOpCode.Text);
            }

            string _ResultSQL = " ";
            _ResultSQL = _ResultSQL + " Delete From SysUserRoles_Info Where SystemID=" + SysClass.SysParams.GetPurviewSystemID().ToString() + " And UserID=" + _UserID.ToString() + ";";
            for (int i = 0; i < ddcRole.Items.Count; i++)
            {
                if (ddcRole.Items[i].Selected)
                {
                    _ResultSQL = _ResultSQL + " Insert Into SysUserRoles_Info (SystemID, UserID, RoleID)"
                        + " Values(" + SysClass.SysParams.GetPurviewSystemID().ToString() + "," + _UserID.ToString() + ", " + ddcRole.Items[i].Value.ToString() + ");";
                }
            }
            _ResultSQL = _ResultSQL + " ";

            return _ResultSQL;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                if (_RecGuid.Length == 0)
                {
                    _RecGuid = SysClass.SysGlobal.GetCreateGUID();
                }
                string[] FieldValues ={                                          
                                     txtOpCode.Text,
                                     txtOpName.Text,                                                                          
                                     ddlOrganID.SelectedValue.ToString(),
                                     txtPosition.Text,
                                     txtPlace.Text,
                                     rblSex.SelectedValue.ToString(),
                                     txtPhone.Text,
                                     txtTelNo.Text,
                                     txtAddress.Text,
                                     txtUserDesc.Text,
                                     txtIDNumber.Text,
                                     txtPassword.Text,
                                     txtZipCode.Text,
                                        _RecGuid,
                                        rbIsCanLogin.SelectedValue
                                     };

                if (SysClass.SysUser.UpdateSingleUser(_UserID, FieldValues) > 0)
                {
                    string sSqlText = UpdateSaveUserRoles() + GetSaveCustomFieldSQL(_RecGuid);
                    DataCommon.QueryData(sSqlText);
                    Dialog.OpenDialogInAjax(upForm, "恭喜您，保存信息成功……", "User_Lst.aspx?page=" + UserCommonOperation.GetSessionIntValue(SysClass.SysUser.UserLst_PageNo.ToString(), 1) + "&OrganID=" + _OrganID.ToString());
                }
            }
        }

        //更新自定义字段代码
        public string GetSaveCustomFieldSQL(string BillTableRecGuid)
        {
            string sUpdateSQL = " if not Exists(Select 1 From SysUserExt_Info Where UserID='" + _UserID.ToString() + "' and UserID > 0) ";
            sUpdateSQL += " begin";
            sUpdateSQL += " Insert Into SysUserExt_Info (UserID, " + GetSaveNewFields() + ") (Select ID, " + GetSaveNewFieldValues() + " From SysUser_Info Where Guid='" + BillTableRecGuid + "');";
            sUpdateSQL += " end else begin";
            sUpdateSQL += " Update SysUserExt_Info Set ";
            for (int i = 0; i < dlList.Items.Count; i++)
            {
                SalaryControl.CustomExtEdit da = (SalaryControl.CustomExtEdit)dlList.Items[i].FindControl("WorklogExtEdit1");
                if (i > 0)
                {
                    sUpdateSQL += ",";
                }
                sUpdateSQL += da.GetUpdateSQL;
            }
            sUpdateSQL += " Where UserID=" + _UserID.ToString() + ";";

            sUpdateSQL += " end;";
            return sUpdateSQL;
        }

        //新增自定义字段列表
        private string GetSaveNewFields()
        {
            string sUpdateSQL = "";
            for (int i = 0; i < dlList.Items.Count; i++)
            {
                SalaryControl.CustomExtEdit da = (SalaryControl.CustomExtEdit)dlList.Items[i].FindControl("WorklogExtEdit1");
                if (i > 0)
                {
                    sUpdateSQL += ",";
                }
                sUpdateSQL += da.UserFieldName;
            }
            return sUpdateSQL;
        }

        //新增自定义字段值列表
        private string GetSaveNewFieldValues()
        {
            string sUpdateSQL = "";
            for (int i = 0; i < dlList.Items.Count; i++)
            {
                SalaryControl.CustomExtEdit da = (SalaryControl.CustomExtEdit)dlList.Items[i].FindControl("WorklogExtEdit1");
                if (i > 0)
                {
                    sUpdateSQL += ",";
                }
                sUpdateSQL += da.GetNewFieldValue;
            }
            return sUpdateSQL;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("User_Lst.aspx?page=" + UserCommonOperation.GetSessionIntValue(SysClass.SysUser.UserLst_PageNo.ToString(), 1) + "&OrganID=" + _OrganID.ToString());
        }   
    }
}
