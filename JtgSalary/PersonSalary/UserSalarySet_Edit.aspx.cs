using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;
using System.Data;
using System.Diagnostics;

namespace JtgTMS.PersonSalary
{
    public partial class UserSalarySet_Edit : System.Web.UI.Page
    {
        public int _UserSalarySetID = 0;
        private string _TableRecGuid = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();
            if (Request.Params["UserSalarySetID"] != null)
            {
                _UserSalarySetID = int.Parse(Request.Params["UserSalarySetID"]);
            }
            _TableRecGuid = SysClass.SysUserSalary.GetTableRecGuidBySetID(_UserSalarySetID);
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            if (_UserSalarySetID > 0)
            {
                SqlDataReader sdr = SysClass.SysUserSalary.GetSingleUserSalarySetByReader(_UserSalarySetID);
                if (sdr.Read())
                {
                    txtBeginYears.Text = sdr["BeginYears"].ToString();
                    txtEndYears.Text = sdr["EndYears"].ToString();
                    txtDescription.Text = sdr["Description"].ToString();
                }
                sdr.Close();

                CyxPack.CommonOperation.DataBinder.BindGridViewData(gvLists, SysClass.SysUserSalary.GetUserSalarySetFieldsLstByDataSet(_UserSalarySetID, ""));
            }
            else
            {
                DataSet ds = LoadOldPackToolMember("");
                SqlDataReader sdr = SysClass.SysUserSalary.GetSalaryFieldsLstByReader();
                while (sdr.Read())
                {
                    DataRow row = ds.Tables[0].NewRow();

                    row["TableRecGuid"] = SysClass.SysGlobal.GetCreateGUID();
                    row["ID"] = 0;
                    row["FieldName"] = sdr["FieldName"].ToString();
                    row["FieldType"] = sdr["FieldType"].ToString();
                    row["UserFieldTitle"] = sdr["FieldTitle"].ToString();
                    row["UserIsVisible"] = int.Parse(sdr["IsVisible"].ToString());

                    ds.Tables[0].Rows.Add(row);
                }
                sdr.Close();

                gvLists.DataSource = ds.Tables[0].DefaultView;
                gvLists.DataBind();
            }
        }

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (txtBeginYears.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtBeginYears, "生效月份不能为空！");
            }
            else if (txtEndYears.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtBeginYears, "失效月份不能为空！");
            }
            return bFlag;
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                if (_TableRecGuid.Length == 0)
                {
                    _TableRecGuid = SysClass.SysGlobal.GetCreateGUID();
                }
                string[] FieldValues ={
                                        _TableRecGuid,
                                        txtBeginYears.Text,                                                                          
                                        txtEndYears.Text,
                                        txtDescription.Text,
                                     };
                //SysClass.SysUserSalary.UpdateDefaultSalarySet(_TableRecGuid, GetSaveDefaultPackToolMemberSQL(_TableRecGuid));
                ////DataCommon.QueryData(GetSaveDefaultPackToolMemberSQL());
                //DataCommon.QueryData(GetSavePackToolMemberSQL(_TableRecGuid));
                //return;
                if (SysClass.SysUserSalary.UpdateSingleUserSalarySet(_UserSalarySetID, FieldValues, GetSavePackToolMemberSQL(_TableRecGuid)) > 0)
                {
                    //Add by lk 20151214 start
                    //if (_UserSalarySetID == 0)
                    //{
                        int iresult = SysClass.SysUserSalary.UpdateUserSalaryFieldsInfo(GetUpdUserSalaryFieldsInfoSQL());
                    //}
                    //Add by lk 20151214 end
                    Dialog.OpenDialogInAjax(upForm, "恭喜您，保存信息成功……", "UserSalarySet_Lst.aspx");
                }
            }
        }

        private string GetSaveDefaultPackToolMemberSQL(string BillTableRecGuid)
        {
            Debug.WriteLine("GetSaveDefaultPackToolMemberSQL:Enter");

            string sUpdateDefaultSQL = "", sIDs = "";

            DataSet ds = LoadDefaultPackToolMember();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (int.Parse(ds.Tables[0].Rows[i]["ID"].ToString()) == 0)
                {
                    sUpdateDefaultSQL = sUpdateDefaultSQL + " Insert Into UserSalarySet_Fields_Info (TableRecGuid"
                                 + ", MasterID"
                                 + ", FieldName"
                                 + ", UserFieldTitle"
                                 + ", UserIsVisible"
                                 + ", SortID)"
                                 + " Select "
                                 + "'" + ds.Tables[0].Rows[i]["TableRecGuid"].ToString() + "'"
                                 + ",0"
                                 + ",'" + ds.Tables[0].Rows[i]["FieldName"].ToString() + "'"
                                 + ",'" + ds.Tables[0].Rows[i]["UserFieldTitle"].ToString() + "'"
                                 + "," + ds.Tables[0].Rows[i]["UserIsVisible"].ToString() + ""
                                 + "," + i.ToString()
                                 + " From UserSalarySet_Info Where TableRecGuid = '" + BillTableRecGuid + "';";
                }
                else
                {
                    if (sIDs.Length > 0)
                    {
                        sIDs = sIDs + ",";
                    }
                    sIDs = sIDs + ds.Tables[0].Rows[i]["ID"].ToString();

                    sUpdateDefaultSQL = sUpdateDefaultSQL + " Update UserSalarySet_Fields_Info Set "
                                 + " FieldName='" + ds.Tables[0].Rows[i]["FieldName"].ToString() + "'"
                                 + ", UserFieldTitle='" + ds.Tables[0].Rows[i]["UserFieldTitle"].ToString() + "'"
                                 + ", UserIsVisible=" + ds.Tables[0].Rows[i]["UserIsVisible"].ToString() + ""
                                 + ", SortID=" + i.ToString()
                                 + " Where ID = " + ds.Tables[0].Rows[i]["ID"].ToString() + ";";
                }
            }

            if (sIDs.Length > 0)
            {
                sUpdateDefaultSQL = " Delete From UserSalarySet_Fields_Info Where ID not in (" + sIDs + ") And MasterID=" + _UserSalarySetID.ToString() + "; " + sUpdateDefaultSQL;
            }
            Debug.WriteLine("GetSaveDefaultPackToolMemberSQL" + sUpdateDefaultSQL);
            Debug.WriteLine("GetSaveDefaultPackToolMemberSQL:Leave");

            return sUpdateDefaultSQL;
        }

        private string GetSavePackToolMemberSQL(string BillTableRecGuid)
        {
            string sUpdateSQL = "", sIDs = "";

            DataSet ds = LoadOldPackToolMember("");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (int.Parse(ds.Tables[0].Rows[i]["ID"].ToString()) == 0)
                {
                    sUpdateSQL = sUpdateSQL + " Insert Into UserSalarySet_Fields_Info (TableRecGuid"
                        + ", MasterID"
                        + ", FieldName"
                        + ", UserFieldTitle"    
                        + ", UserIsVisible"
                        + ", SortID)"
                        + " Select "
                        + "'" + ds.Tables[0].Rows[i]["TableRecGuid"].ToString() + "'"
                        + ",ID"
                        + ",'" + ds.Tables[0].Rows[i]["FieldName"].ToString() + "'"
                        + ",'" + ds.Tables[0].Rows[i]["UserFieldTitle"].ToString() + "'"
                        + "," + ds.Tables[0].Rows[i]["UserIsVisible"].ToString() + ""
                        + "," + i.ToString()
                        + " From UserSalarySet_Info Where TableRecGuid = '" + BillTableRecGuid + "';";
                }
                else
                {
                    if (sIDs.Length > 0)
                    {
                        sIDs = sIDs + ",";
                    }
                    sIDs = sIDs + ds.Tables[0].Rows[i]["ID"].ToString();

                    sUpdateSQL = sUpdateSQL + " Update UserSalarySet_Fields_Info Set "
                            + " FieldName='" + ds.Tables[0].Rows[i]["FieldName"].ToString() + "'"
                            + ", UserFieldTitle='" + ds.Tables[0].Rows[i]["UserFieldTitle"].ToString() + "'"
                            + ", UserIsVisible=" + ds.Tables[0].Rows[i]["UserIsVisible"].ToString() + ""
                            + ", SortID=" + i.ToString()
                            + " Where ID = " + ds.Tables[0].Rows[i]["ID"].ToString() + ";";
                }
            }

            if (sIDs.Length > 0)
            {
                sUpdateSQL = " Delete From UserSalarySet_Fields_Info Where ID not in (" + sIDs + ") And MasterID=" + _UserSalarySetID.ToString() + "; " + sUpdateSQL;
            }
            else if(_UserSalarySetID!=0)
            {
                sUpdateSQL = " Delete From UserSalarySet_Fields_Info Where MasterID=" + _UserSalarySetID.ToString() + "; " + sUpdateSQL;
            }
            Debug.WriteLine("GetSavePackToolMemberSQL" + sUpdateSQL);

            return sUpdateSQL;
        }       

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserSalarySet_Lst.aspx");
        }

        protected void gvLists_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                DataSet ds = LoadOldPackToolMember("," + e.CommandArgument.ToString() + ",");
                gvLists.DataSource = ds.Tables[0].DefaultView;
                gvLists.DataBind();
            }
            else if (e.CommandName == "Up")
            {
                DataSet Oldds = LoadOldPackToolMember("");

                int _SortID = int.Parse(e.CommandArgument.ToString());

                DataSet ds = SysClass.SysUserSalary.GetUserSalarySetFieldsLstByDataSet(_UserSalarySetID, " AND 1<>1");

                DataRow OldRow = ds.Tables[0].NewRow();

                for (int i = 0; i < Oldds.Tables[0].Rows.Count; i++)
                {
                    if (i == _SortID - 1)
                    {
                        OldRow["TableRecGuid"] = Oldds.Tables[0].Rows[i]["TableRecGuid"].ToString();
                        OldRow["ID"] = int.Parse(Oldds.Tables[0].Rows[i]["ID"].ToString());
                        OldRow["FieldName"] = Oldds.Tables[0].Rows[i]["FieldName"].ToString();
                        OldRow["FieldType"] = Oldds.Tables[0].Rows[i]["FieldType"].ToString();
                        OldRow["UserFieldTitle"] = Oldds.Tables[0].Rows[i]["UserFieldTitle"].ToString();
                        OldRow["UserIsVisible"] = int.Parse(Oldds.Tables[0].Rows[i]["UserIsVisible"].ToString());
                    }
                    else
                    {
                        DataRow row = ds.Tables[0].NewRow();

                        row["TableRecGuid"] = Oldds.Tables[0].Rows[i]["TableRecGuid"];
                        row["ID"] = Oldds.Tables[0].Rows[i]["ID"];
                        row["FieldName"] = Oldds.Tables[0].Rows[i]["FieldName"];
                        row["FieldType"] = Oldds.Tables[0].Rows[i]["FieldType"].ToString();
                        row["UserFieldTitle"] = Oldds.Tables[0].Rows[i]["UserFieldTitle"];
                        row["UserIsVisible"] = Oldds.Tables[0].Rows[i]["UserIsVisible"];

                        ds.Tables[0].Rows.Add(row);
                    }

                    if ((_SortID == i) && (_SortID > 0))
                    {
                        ds.Tables[0].Rows.Add(OldRow);
                    }
                }

                gvLists.DataSource = ds.Tables[0].DefaultView;
                gvLists.DataBind();
            }
            else if (e.CommandName == "Down")
            {
                DataSet Oldds = LoadOldPackToolMember("");

                int _SortID = int.Parse(e.CommandArgument.ToString());

                DataSet ds = SysClass.SysUserSalary.GetUserSalarySetFieldsLstByDataSet(_UserSalarySetID, " AND 1<>1");

                DataRow OldRow = ds.Tables[0].NewRow();

                for (int i = 0; i < Oldds.Tables[0].Rows.Count; i++)
                {
                    if (i == _SortID)
                    {
                        OldRow["TableRecGuid"] = Oldds.Tables[0].Rows[i]["TableRecGuid"].ToString();
                        OldRow["ID"] = int.Parse(Oldds.Tables[0].Rows[i]["ID"].ToString());
                        OldRow["FieldName"] = Oldds.Tables[0].Rows[i]["FieldName"].ToString();
                        OldRow["FieldType"] = int.Parse(Oldds.Tables[0].Rows[i]["FieldType"].ToString());
                        OldRow["UserFieldTitle"] = Oldds.Tables[0].Rows[i]["UserFieldTitle"].ToString();
                        OldRow["UserIsVisible"] = int.Parse(Oldds.Tables[0].Rows[i]["UserIsVisible"].ToString());
                    }
                    else
                    {
                        DataRow row = ds.Tables[0].NewRow();

                        row["TableRecGuid"] = Oldds.Tables[0].Rows[i]["TableRecGuid"];
                        row["ID"] = Oldds.Tables[0].Rows[i]["ID"];
                        row["FieldName"] = Oldds.Tables[0].Rows[i]["FieldName"];
                        row["FieldType"] = int.Parse(Oldds.Tables[0].Rows[i]["FieldType"].ToString());
                        row["UserFieldTitle"] = Oldds.Tables[0].Rows[i]["UserFieldTitle"];
                        row["UserIsVisible"] = Oldds.Tables[0].Rows[i]["UserIsVisible"];

                        ds.Tables[0].Rows.Add(row);
                    }

                    if (((_SortID == i - 1) && (_SortID < Oldds.Tables[0].Rows.Count - 1)) ||
                                ((_SortID == Oldds.Tables[0].Rows.Count - 1) && (_SortID == i)))
                    {
                        ds.Tables[0].Rows.Add(OldRow);
                    }
                }

                gvLists.DataSource = ds.Tables[0].DefaultView;
                gvLists.DataBind();
            }
        }
        private DataSet LoadDefaultPackToolMember()
        {
            Debug.WriteLine("LoadDefaultPackToolMember:Enter");

            DataSet ds = SysClass.SysUserSalary.GetUserSalarySetFieldsLstByDataSet(0, "");
            for (int i = 0; i < gvLists.Rows.Count; i++)
            {
                DataRow row = ds.Tables[0].NewRow();
                row["TableRecGuid"] = ((Label) gvLists.Rows[i].Cells[1].FindControl("lblTableRecGuid")).Text;
                row["ID"] = ((Label) gvLists.Rows[i].Cells[1].FindControl("lblID")).Text;
                row["FieldName"] = ((Label) gvLists.Rows[i].Cells[1].FindControl("lblFieldName")).Text;
                row["FieldType"] = ((Label) gvLists.Rows[i].Cells[2].FindControl("lblFieldType")).Text;

                row["UserFieldTitle"] = ((TextBox) gvLists.Rows[i].Cells[3].FindControl("txtUserFieldTitle")).Text;

                if (((CheckBox) gvLists.Rows[i].Cells[4].FindControl("chkIsVisible")).Checked)
                {
                    row["UserIsVisible"] = 1;
                }
                else
                {
                    row["UserIsVisible"] = 0;
                }

                row["SortID"] = i;

                ds.Tables[0].Rows.Add(row);

            }

            return ds;
        }
        private DataSet LoadOldPackToolMember(string DeleteRecGuids)
        {
            DataSet ds = SysClass.SysUserSalary.GetUserSalarySetFieldsLstByDataSet(_UserSalarySetID, "");
            for (int i = 0; i < gvLists.Rows.Count; i++)
            {
                DataRow row = ds.Tables[0].NewRow();
                row["TableRecGuid"] = ((Label)gvLists.Rows[i].Cells[1].FindControl("lblTableRecGuid")).Text;
                row["ID"] = ((Label)gvLists.Rows[i].Cells[1].FindControl("lblID")).Text;
                row["FieldName"] = ((Label)gvLists.Rows[i].Cells[1].FindControl("lblFieldName")).Text;
                row["FieldType"] = ((Label)gvLists.Rows[i].Cells[2].FindControl("lblFieldType")).Text;

                row["UserFieldTitle"] = ((TextBox)gvLists.Rows[i].Cells[3].FindControl("txtUserFieldTitle")).Text;

                if (((CheckBox)gvLists.Rows[i].Cells[4].FindControl("chkIsVisible")).Checked)
                {
                    row["UserIsVisible"] = 1;
                }
                else
                {
                    row["UserIsVisible"] = 0;
                }
                
                row["SortID"] = i;
                
                if (DeleteRecGuids.IndexOf("," + row["TableRecGuid"].ToString() + ",") < 0)
                {
                    ds.Tables[0].Rows.Add(row);
                }
            }
            return ds;
        }

        //Add by lk 20151214 start
        private string GetUpdUserSalaryFieldsInfoSQL()
        {
            string sUpdateSQL = "";

            for (int i = 0; i < gvLists.Rows.Count; i++)
            {
                sUpdateSQL += " Update UserSalaryFields_Info Set ";
                sUpdateSQL += " FieldTitle='" + ((TextBox)gvLists.Rows[i].Cells[3].FindControl("txtUserFieldTitle")).Text + "'";
                if (((CheckBox)gvLists.Rows[i].Cells[4].FindControl("chkIsVisible")).Checked)
                {
                    sUpdateSQL += " , IsVisible= 1";
                }
                else
                {
                    sUpdateSQL += " , IsVisible= 0";
                }
                sUpdateSQL += ", SortID=" + (i+1).ToString();
                sUpdateSQL += " Where FieldName = '" + ((Label)gvLists.Rows[i].Cells[1].FindControl("lblFieldName")).Text +"';";
            }

            return sUpdateSQL;
        }    
        //Add by lk 20151214 end
    }
}
