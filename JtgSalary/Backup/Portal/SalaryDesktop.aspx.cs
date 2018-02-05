using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Data;

namespace JtgTMS.Platform
{
    public partial class SalaryDesktop : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (!Page.IsPostBack)
            {
                txtMonth.Text = DateTime.Now.ToString("yyyyMM");
                BindPageData();
            }
        }

        private void BindPageData()
        {
            string sWhereSQL = " And a.OpCode='" + SysClass.SysGlobal.GetCurrentOpCode() + "' And SignStatus=0"
                + " And a.SalaryRecGuid in (Select SalaryRecGuid From UserUserImportRec_Info where ApprovalStatus=1)";

            DataSet ds = SysClass.SysUserSalary.GetTopUserSalaryLstByDataSet(sWhereSQL);

            for (int i = ds.Tables[0].Rows.Count; i < 6; i++)
            {
                DataRow OldRow = ds.Tables[0].NewRow();

                ds.Tables[0].Rows.Add(OldRow);
            }

            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvLists, ds);

            sWhereSQL = " And a.OpCode='" + SysClass.SysGlobal.GetCurrentOpCode() + "' And SignStatus=1"
                + " And a.SalaryRecGuid in (Select SalaryRecGuid From UserUserImportRec_Info where ApprovalStatus=1)";

            if (txtMonth.Text.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears='" + txtMonth.Text + "'";
            }

            ds = SysClass.SysUserSalary.GetTopUserSalaryLstByDataSet(sWhereSQL);
            for (int i = ds.Tables[0].Rows.Count; i < 6; i++)
            {
                DataRow OldRow = ds.Tables[0].NewRow();

                ds.Tables[0].Rows.Add(OldRow);
            }

            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvBorrowLists, ds);

            ds = SysClass.SysNotice.GetTopToolsNoticeLstByDataSet(3, " And a.OrganID in (select ID from [GetParentOrganByID](" + SysClass.SysGlobal.GetCurrentUserOrganID() + "))");

            for (int i = ds.Tables[0].Rows.Count; i < 3; i++)
            {
                DataRow OldRow = ds.Tables[0].NewRow();

                ds.Tables[0].Rows.Add(OldRow);
            }

            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvNotice, ds);
            
            bool _IsError = false;
            SqlDataReader sdr = SysClass.SysUser.GetSingleUserByReader(int.Parse(SysClass.SysGlobal.GetCurrentUserID().ToString()));
            if (sdr.Read())
            { 
                if (sdr["IsError"].ToString() == "1")
                {
                    _IsError = true;
                }

            }
            sdr.Close();            

            ltlUserName.Text = SysClass.SysGlobal.GetCurrentOpName();
            ltlOrganName.Text = SysClass.SysGlobal.GetCurrentUserOrganName();
            ltlLastLoginDate.Text = SysClass.SysGlobal.GetLastDate();
            ltlLastLoginIP.Text = SysClass.SysGlobal.GetLastIp();
            LtlComputerName.Text = SysClass.SysGlobal.GetLastComputerName();

            if (_IsError)
            {
                ltlUserName.Text = "<font style='color:Red'>" + ltlUserName.Text + "</font>";
                ltlOrganName.Text = "<font style='color:Red'>" + ltlOrganName.Text + "</font>";
                ltlLastLoginDate.Text = "<font style='color:Red'>" + ltlUserName.Text + "</font>";
                ltlLastLoginIP.Text = "<font style='color:Red'>" + ltlLastLoginIP.Text + "</font>";
                LtlComputerName.Text = "<font style='color:Red'>" + LtlComputerName.Text + "</font>";
            }
           
        }

        protected void gvLists_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblApprovalStatus = (Label)e.Row.FindControl("lblApprovalStatus");
            Label lblApprovalStatusName = (Label)e.Row.FindControl("lblApprovalStatusName");

            if ((lblApprovalStatus != null) && (lblApprovalStatusName != null))
            {
                if (lblApprovalStatus.Text == SysClass.SysUserSalary.UserSalary_ApprovalIsOK.ToString())
                {
                    //审批通过
                    lblApprovalStatusName.ForeColor = System.Drawing.Color.Green;
                }

                HyperLink hyDelete = (HyperLink)e.Row.FindControl("hyDelete");
                if (hyDelete != null)
                {
                    hyDelete.Visible = int.Parse(lblApprovalStatus.Text) == SysClass.SysUserSalary.UserSalary_Draft;
                }

                CheckBox CheckRow = (CheckBox)e.Row.FindControl("CheckRow");
                if (CheckRow != null)
                {
                    CheckRow.Visible = int.Parse(lblApprovalStatus.Text) == SysClass.SysUserSalary.UserSalary_Draft;
                }

                HyperLink hyEdit = (HyperLink)e.Row.FindControl("hyEdit");

                if (hyEdit != null)
                {
                    if (int.Parse(lblApprovalStatus.Text) == SysClass.SysUserSalary.UserSalary_ApprovalIsOK)
                    {
                        hyEdit.Text = "查看";
                    }
                    else
                    {
                        hyEdit.Text = "编辑";
                    }
                }
            }
        }

        protected void txtMonth_TextChanged(object sender, EventArgs e)
        {
            string sWhereSQL = " And a.OpCode='" + SysClass.SysGlobal.GetCurrentOpCode() + "' And SignStatus=1"
                + " And a.SalaryRecGuid in (Select SalaryRecGuid From UserUserImportRec_Info where ApprovalStatus=1)";

            if (txtMonth.Text.Length > 0)
            {
                sWhereSQL += " And a.SalaryYears='" + txtMonth.Text + "'";
            }

            DataSet ds = SysClass.SysUserSalary.GetTopUserSalaryLstByDataSet(sWhereSQL);
            for (int i = ds.Tables[0].Rows.Count; i < 6; i++)
            {
                DataRow OldRow = ds.Tables[0].NewRow();

                ds.Tables[0].Rows.Add(OldRow);
            }

            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvBorrowLists, ds);
        }

        protected void btnSalarys_Click(object sender, EventArgs e)
        {
            txtMonth_TextChanged(sender, e);
        }
    }
}
