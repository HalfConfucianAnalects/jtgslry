using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;
using System.Data;

namespace JtgTMS.Platform
{
    public partial class framework_userProfile1 : System.Web.UI.Page
    {
        public string _RecGuid = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            SysClass.SysGlobal.CheckSysIsLogined();

            _RecGuid = SysClass.SysUser.GetRecGuidByUserID(SysClass.SysGlobal.GetCurrentUserID());

            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            //自定义字段

            DataSet ds = SysClass.SysCustomField.GetCustomFieldLstByDataset(" And TableNo='UserInfo' And IsReadOnly=0");
            if (ds.Tables[0].Rows.Count % 2 == 1)
            {
                DataRow OldRow = ds.Tables[0].NewRow();

                ds.Tables[0].Rows.Add(OldRow);
            }

            CyxPack.CommonOperation.DataBinder.BindDataListData(dlList, ds);

            SqlDataReader sdr = SysClass.SysUser.GetUserInfoByReader(SysClass.SysGlobal.GetCurrentUserID());
            if (sdr.Read())
            {
                ltUserName.Text = sdr["OpCode"].ToString();
                txtOpName.Text = sdr["OpName"].ToString();
               
                for (int i = 0; i < dlList.Items.Count; i++)
                {
                    SalaryControl.CustomExtEdit da = (SalaryControl.CustomExtEdit)dlList.Items[i].FindControl("WorklogExtEdit1");
                    if (da.UserFieldName.Length > 0)
                    {
                        da.UserFieldValue = sdr[da.UserFieldName].ToString();
                    }
                }

            }
            sdr.Close();
        }

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (txtOpName.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtOpName, "姓名不能为空！");
            }
            return bFlag;
        }

        //更新自定义字段代码
        private string GetSaveCustomFieldSQL(string BillTableRecGuid)
        {
            string sUpdateSQL = " if not Exists(Select 1 From SysUserExt_Info Where UserID=" + SysClass.SysGlobal.GetCurrentUserID().ToString() + " and UserID > 0) ";
            sUpdateSQL += " begin";
            sUpdateSQL += " Insert Into SysUserExt_Info (UserID, " + GetSaveNewFields() + ") (Select ID, " + GetSaveNewFieldValues() + " From SysUser_Info Where Guid='" + BillTableRecGuid + "');";
            sUpdateSQL += " end else begin";
            sUpdateSQL += " Update SysUserExt_Info Set ";
            for (int i = 0; i < dlList.Items.Count; i++)
            {
                SalaryControl.CustomExtEdit da = (SalaryControl.CustomExtEdit)dlList.Items[i].FindControl("WorklogExtEdit1");
                if (da.UserFieldName.Length > 0)
                {
                    if (i > 0)
                    {
                        sUpdateSQL += ",";
                    }
                    sUpdateSQL += da.GetUpdateSQL;
                }
            }
            sUpdateSQL += " Where UserID=" + SysClass.SysGlobal.GetCurrentUserID().ToString() + ";";

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
                if (da.UserFieldName.Length > 0)
                {
                    if (i > 0)
                    {
                        sUpdateSQL += ",";
                    }
                    sUpdateSQL += da.UserFieldName;
                }
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
                if (da.UserFieldName.Length > 0)
                {
                    if (i > 0)
                    {
                        sUpdateSQL += ",";
                    }
                    sUpdateSQL += da.GetNewFieldValue;
                }
            }
            return sUpdateSQL;
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                string[] FieldValues ={                                     
                                     txtOpName.Text,                                                                                                               
                                     };

                if (SysClass.SysUser.UpdateSingleUserInfoEx(SysClass.SysGlobal.GetCurrentUserID(), FieldValues, GetSaveCustomFieldSQL(_RecGuid)) > 0)
                {
                    Dialog.OpenDialogInAjax(txtOpName, "恭喜您，保存信息成功……");
                }
            }
        }
    }
}
