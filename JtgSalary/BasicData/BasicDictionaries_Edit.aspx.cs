using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;

namespace JtgTMS.BasicData
{
    public partial class BasicDictionaries_Edit : System.Web.UI.Page
    {
        public int _MainID = 0, _MianDID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();
            if (Request.Params["MainID"] != null)
            {
                _MainID = int.Parse(Request.Params["MainID"]);
            }
            if (Request.Params["MianDID"]!=null)
            {
                _MianDID = int.Parse(Request.Params["MianDID"]);
            }
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }
        private void BindPageData()
        {
            if (_MianDID > 0)
            {
                SqlDataReader sdr = SysClass.SysBasicDictionaries.GetSingleMainDsByReader(_MianDID);
                if (sdr.Read())
                {
                    txtDictionariesNo.Text = sdr["DetailNo"].ToString();
                    txtDictionariesName.Text = sdr["DetailName"].ToString();
                    txtNote.Text = sdr["Description"].ToString();
                }
                sdr.Close();
            }
        }
        private bool SaveCheck()
        {
            bool bFlag = true;
            if (txtDictionariesNo.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtDictionariesNo, "字典编号不能为空！");
            }
            else if (SysClass.SysTool.CheckToolNoExists(_MianDID, txtDictionariesNo.Text))
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtDictionariesNo, "字典编号不能重复！");
            }
            else if (txtDictionariesName.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtDictionariesName, "工具名称不能为空！");
            }
            return bFlag;
        }
        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                string sContent = Request["txtNote"].ToString().Replace("'", "''");

                string[] FieldValues ={
                                     _MainID.ToString(),
                                     txtDictionariesNo.Text,
                                     txtDictionariesName.Text,
                                     sContent,
                                     "0"
                                     };

                if (SysClass.SysBasicDictionaries.UpdateSingleBaseDate(_MianDID, FieldValues) > 0)
                {
                    Dialog.OpenDialogInAjax(upForm, "恭喜您，保存信息成功……", "BasicDictionaries_Lst.aspx?MainID=" + _MainID.ToString());
                }
            }
        }
    }
}
