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
    public partial class Role_Edit : System.Web.UI.Page
    {
        public int _RoleID = 0;
        public string _Purview = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();
            if (Request.Params["RoleID"] != null)
            {
                _RoleID = int.Parse(Request.Params["RoleID"]);
            }
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            if (_RoleID > 0)
            {
                SqlDataReader sdr = SysClass.SysRole.GetSingleRoleByReader(_RoleID);
                if (sdr.Read())
                {
                    txtRoleName.Text = sdr["RoleName"].ToString();
                    txtDescription.Text = sdr["Description"].ToString();
                    _Purview = sdr["Purview"].ToString();
                }
                sdr.Close();
            }
            txtRoleName.ReadOnly = _RoleID == 30;
            for (int i = _Purview.Length; i < 5090; i++)
            {
                _Purview = _Purview + "0";
            }

            CyxPack.CommonOperation.DataBinder.BindDataListData(dlList, SysClass.SysPurview.GetRoleChildPurvieLstByReader(0, _Purview));
        }

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (txtRoleName.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtRoleName, "角色名称不能为空！");
            }
            return bFlag;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                string sPurview = "";
                for (int i = 0; i < 5000; i++)
                {
                    sPurview = sPurview + "0";
                }

                for (int i = 0; i < dlList.Items.Count; i++)
                {
                    AdminControl.PurviewLst da = (AdminControl.PurviewLst)dlList.Items[i].FindControl("PurviewLst1");
                    sPurview = da.CalcPurviewValue(sPurview);

                }

                string[] FieldValues ={
                                     txtRoleName.Text,                                                                          
                                     txtDescription.Text,
                                     sPurview
                                     };

                if (SysClass.SysRole.UpdateSingleRole(_RoleID, FieldValues) > 0)
                {
                    Dialog.OpenDialogInAjax(upForm, "恭喜您，保存信息成功……", "Role_Lst.aspx");
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Role_Lst.aspx");
        }   
    }
}
