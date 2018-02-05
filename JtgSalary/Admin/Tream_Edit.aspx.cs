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
    public partial class Tream_Edit : System.Web.UI.Page
    {
        public string _OrganName = "";
        public int _POrganID, _OrganID = 0;
        int _OrganType = 3;
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();
            if (Request.Params["OrganID"] != null)
            {
                _OrganID = int.Parse(Request.Params["OrganID"]);
            }
            if (Request.Params["POrganID"] != null)
            {
                _POrganID = int.Parse(Request.Params["POrganID"]);
            }
            _OrganName = SysClass.SysOrgan.GetOrganNameByID(_OrganID);
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            LoadChildNodes(0, 0);

            if (_OrganID > 0)
            {
                SqlDataReader sdr = SysClass.SysOrgan.GetSingleOrganByReader(_OrganID);
                if (sdr.Read())
                {
                    if (ddlPOrganID.Items.FindByValue(sdr["POrganID"].ToString()) != null)
                    {
                        ddlPOrganID.SelectedValue = sdr["POrganID"].ToString();
                    }
                    txtOrganNo.Text = sdr["OrganNo"].ToString();
                    txtOrganName.Text = sdr["OrganName"].ToString();
                    txtContent.Text = sdr["Description"].ToString();
                }
                sdr.Close();
            }
            else
            {
                if (ddlPOrganID.Items.FindByValue(_POrganID.ToString()) != null)
                {
                    ddlPOrganID.SelectedValue = _POrganID.ToString();
                }
            }
        }

        private void LoadChildNodes(int POrganID, int Level)
        {
            SqlDataReader sdr = SysClass.SysOrgan.GetOrganLstByReader(POrganID);
            while (sdr.Read())
            {
                ListItem newItem = new ListItem();
                newItem.Value = sdr["ID"].ToString();
                newItem.Text = HttpUtility.HtmlDecode(SysClass.SysGlobal.RepeatString("　", Level) + "" + sdr["OrganName"].ToString());
                ddlPOrganID.Items.Add(newItem);

                LoadChildNodes(int.Parse(sdr["ID"].ToString()), Level+1);
            }
            sdr.Close();
        }   

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (txtOrganNo.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtOrganNo, "班组编号不能为空！");
            }
            else if (SysClass.SysOrgan.CheckOrganNoExists(_OrganID, txtOrganNo.Text))
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtOrganName, "班组编号不能重复！");
            }
            else if (txtOrganName.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtOrganName, "班组名称不能为空！");
            }
            return bFlag;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                string sContent = Request["txtContent"].ToString().Replace("'", "''");

                string[] FieldValues ={ddlPOrganID.SelectedValue.ToString(),
                                     txtOrganNo.Text,
                                     txtOrganName.Text,                                                                          
                                     sContent, "0"
                                     };

                if (SysClass.SysOrgan.UpdateSingleOrgan(_OrganID, _OrganType, FieldValues) > 0)
                {
                    Dialog.OpenDialogInAjax(upForm, "恭喜您，保存信息成功……", "Tream_Lst.aspx?POrganID=" + _POrganID.ToString());
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Tream_Lst.aspx?POrganID=" + _POrganID.ToString());
        }   
    }
}
