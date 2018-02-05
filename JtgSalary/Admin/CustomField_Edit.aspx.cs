using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;

namespace JtgTMS.Admin
{
    public partial class CustomField_Edit : System.Web.UI.Page
    {
        public int _CustomFieldID = 0;
        public string _TableNo = "", _TableTitle = "", _RecGuid = "", _TableExtName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();
            if (Request.Params["CustomFieldID"] != null)
            {
                _CustomFieldID = int.Parse(Request.Params["CustomFieldID"]);
            }
            if (Request.Params["TableNo"] != null)
            {
                _TableNo = Request.Params["TableNo"];
                _TableExtName = SysClass.SysCustomField.GetTableExtNameByTableNo(_TableNo);
            }
            _TableTitle = SysClass.SysCustomField.GetTableTitleByTableNo(_TableNo);
            _RecGuid = SysClass.SysCustomField.GetRecGuidByCustomID(_CustomFieldID);
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            if (_CustomFieldID > 0)
            {
                SqlDataReader sdr = SysClass.SysCustomField.GetSingleCustomFieldByReader(_CustomFieldID);
                if (sdr.Read())
                {
                    txtFieldName.Text = sdr["FieldName"].ToString();
                    txtFieldTitle.Text = sdr["FieldTitle"].ToString();
                    if (ddlFieldType.Items.FindByValue(sdr["FieldType"].ToString()) != null)
                    {
                        ddlFieldType.SelectedValue = sdr["FieldType"].ToString();
                    }
                    if (rblIsReadonly.Items.FindByValue(sdr["IsReadonly"].ToString()) != null)
                    {
                        rblIsReadonly.SelectedValue = sdr["IsReadonly"].ToString();
                    }
                    txtDescription.Text = sdr["Description"].ToString();
                }
                sdr.Close();

                txtFieldName.ReadOnly = true;
                ddlFieldType.Enabled = false;
            }            
        }

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (txtFieldName.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtFieldName, "字段名称不能为空！");
            }
            return bFlag;
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
                                          _RecGuid,
                                          _TableNo,
                                     txtFieldName.Text,      
                                     txtFieldTitle.Text,  
                                     ddlFieldType.SelectedValue,
                                     rblIsReadonly.SelectedValue,
                                     txtDescription.Text
                                     };

                if (SysClass.SysCustomField.UpdateSingleCustom(_CustomFieldID, FieldValues, GetUpdateTableFieldSQL()) > 0)
                {
                    Dialog.OpenDialogInAjax(upForm, "恭喜您，保存信息成功……", "CustomField_Lst.aspx?TableNo=" + _TableNo);
                }
            }
        }

        private string GetUpdateTableFieldSQL()
        {
            string _UpdateSQL = "";

            if (_CustomFieldID <= 0)
            {
                _UpdateSQL += " IF not EXISTS (select 1 From syscolumns Where"
                    + " id=object_id('" + _TableExtName + "') And name='Column_" + txtFieldName.Text + "')  ";
                _UpdateSQL += " begin ";
                _UpdateSQL += "  ALTER TABLE " + _TableExtName + " ADD Column_" + txtFieldName.Text + " ";
                if (ddlFieldType.SelectedValue == "0")
                {
                    _UpdateSQL += " decimal(18, 2) Default 0";
                }
                else if (ddlFieldType.SelectedValue == "1")
                {
                    _UpdateSQL += " varchar(128) null";
                }
                else
                {
                    _UpdateSQL += " varchar(128) Default 0";
                }
                _UpdateSQL += ";";
                _UpdateSQL += " End; ";

                _UpdateSQL += " exec usp_RefreshAllView;";
            }

            return _UpdateSQL;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("CustomField_Lst.aspx");
        }   
    }
}
