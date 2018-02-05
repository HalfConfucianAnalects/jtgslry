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
    public partial class Supplier_Edit : System.Web.UI.Page
    {
        public string _SupplierName = "";
        public int _PSupplierID, _SupplierID = 0;
        int _OrganType = 4;
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();
            if (Request.Params["SupplierID"] != null)
            {
                _SupplierID = int.Parse(Request.Params["SupplierID"]);
            }
            if (Request.Params["PSupplierID"] != null)
            {
                _PSupplierID = int.Parse(Request.Params["PSupplierID"]);
            }
            _SupplierName = SysClass.SysOrgan.GetOrganNameByID(_SupplierID);
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            if (_SupplierID > 0)
            {
                SqlDataReader sdr = SysClass.SysOrgan.GetSingleOrganByReader(_SupplierID);
                if (sdr.Read())
                {
                    txtOrganNo.Text = sdr["OrganNo"].ToString();
                    txtOrganName.Text = sdr["OrganName"].ToString();
                    txtContent.Text = sdr["Description"].ToString();
                }
                sdr.Close();
            }
        }

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (txtOrganNo.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtOrganNo, "供应商编号不能为空！");
            }
            else if (SysClass.SysOrgan.CheckOrganNoExists(_SupplierID, txtOrganNo.Text))
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtOrganName, "供应商编号不能重复！");
            }
            else if (txtOrganName.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtOrganName, "供应商名称不能为空！");
            }
            return bFlag;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                string sContent = Request["txtContent"].ToString().Replace("'", "''");

                string[] FieldValues ={
                                        "0",
                                        txtOrganNo.Text,
                                        txtOrganName.Text,                                                                          
                                        sContent, "0"
                                     };

                if (SysClass.SysOrgan.UpdateSingleOrgan(_SupplierID, _OrganType,  FieldValues) > 0)
                {
                    Dialog.OpenDialogInAjax(upForm, "恭喜您，保存信息成功……", "Supplier_Lst.aspx?PSupplierID=" + _PSupplierID.ToString());
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Supplier_Lst.aspx?PSupplierID=" + _PSupplierID.ToString());
        }   
    }
}
