using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using CyxPack.CommonOperation;
using System.Data;
using System.IO;

namespace JtgTMS.PersonSalary
{
    public partial class SalaryImport_Step2 : System.Web.UI.Page
    {
        private string SalaryYears = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["SalaryYears"] != null)
            {
                SalaryYears = Request.Params["SalaryYears"];
            }

            if (!Page.IsPostBack)
            {                
                BindPageData();
            }
        }

        private void BindPageData()
        {
            if (SalaryYears.Length > 0)
            {
                txtUserSalaryYears.Text = SalaryYears;
            }
            else
            {
                txtUserSalaryYears.Text = DateTime.Now.ToString("yyyyMM");
            }

            SysClass.SysUserSalary.FullToSalaryFieldsLst(ddlTargetName, txtUserSalaryYears.Text);

            SysClass.SysUserSalary.FullToSalaryImportRecLst(ddlImportRec, txtUserSalaryYears.Text);
        }

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (txtUserSalaryYears.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtUserSalaryYears, "工资月份不能为空！");
            }
            else if (lblUploadFile.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtUserSalaryYears, "请选择Excel工资文件！");
            }
            else if (GetOpCodeFieldName().Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtUserSalaryYears, "请选择导入工号字段！");
            }
            else
            {
                string _OpCodeFieldName = GetOpCodeFieldName();
                string strConn, sheetname = "Sheet1$";

                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + lblUploadFile.Text + ";Extended Properties=Excel 8.0;";
                System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(strConn);

                conn.Open();

                DataTable dtExcelSchema = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                if (dtExcelSchema.Rows.Count > 0)
                {
                    sheetname = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                }

                System.Data.OleDb.OleDbDataAdapter oada = new System.Data.OleDb.OleDbDataAdapter("select * from [" + sheetname + "]", strConn);
                DataSet ds = new DataSet();
                oada.Fill(ds);

                oada = new System.Data.OleDb.OleDbDataAdapter("select * from [" + sheetname + "] Where 1<>1", strConn);
                DataSet ds1 = new DataSet();
                oada.Fill(ds1);

                conn.Close();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string _SQL = "Select Top 1 1 From SysUser_Info Where OpCode='" + ds.Tables[0].Rows[i][_OpCodeFieldName].ToString() + "'";
                    if (!SysClass.SysGlobal.GetExecSqlIsExist(_SQL))
                    {
                        DataRow row = ds1.Tables[0].NewRow();

                        for (int k = 0; k < ds1.Tables[0].Columns.Count; k++)
                        {
                            row[ds1.Tables[0].Columns[k].ColumnName] = ds.Tables[0].Rows[i][ds1.Tables[0].Columns[k].ColumnName].ToString();

                        }
                        ds1.Tables[0].Rows.Add(row);
                    }
                }

                gvAbnormal.DataSource = ds1.Tables[0].DefaultView;
                gvAbnormal.DataBind();

                gvAbnormal.Visible = gvAbnormal.Rows.Count > 0;

                if (gvAbnormal.Rows.Count > 0)
                {
                    bFlag = false;
                    Dialog.OpenDialogInAjax(txtUserSalaryYears, "工号不存在，请检查！");
                }
            }

            return bFlag;
        }

        private string GetOpCodeFieldName()
        {
            string _OpCodeFieldName = "";

            DataSet dsImportSet = LoadImportSetByDataSet();

            for (int n = 0; n < dsImportSet.Tables[0].Rows.Count; n++)
            {
                if (dsImportSet.Tables[0].Rows[n]["TargetName"].ToString().ToLower() == "OpCode".ToLower())
                {
                    _OpCodeFieldName = dsImportSet.Tables[0].Rows[n]["SourceName"].ToString();
                }
            }

            return _OpCodeFieldName;
        }

        protected void gvImportSet_RowCreated(object sender, GridViewRowEventArgs e)
        {
            DropDownList _ddlTaskStatus = (DropDownList)e.Row.FindControl("ddlTargetName");

            if (_ddlTaskStatus != null)
            {
                for (int i = 0; i < ddlTargetName.Items.Count; i++)
                {
                    ListItem li = new ListItem();
                    li.Value = ddlTargetName.Items[i].Value;
                    li.Text = ddlTargetName.Items[i].Text;
                    _ddlTaskStatus.Items.Add(li);
                }

            }
        }

        protected void gvImportSet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DropDownList _ddlTaskStatus = (DropDownList)e.Row.FindControl("ddlTargetName");
            TextBox _txtTaskStatus = (TextBox)e.Row.FindControl("txtTargetName");

            if (_ddlTaskStatus != null)
            {
                if ((_txtTaskStatus != null) && (_ddlTaskStatus.Items.FindByValue(_txtTaskStatus.Text) != null))
                {
                    _ddlTaskStatus.SelectedValue = _txtTaskStatus.Text;
                }                
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                string SalaryRecGuid = SysClass.SysGlobal.GetCreateGUID();

                string sUpdateSQL = "", sUpdateFieldsSQL = "";
                if (ddlImportRec.SelectedIndex == 0)
                {
                    sUpdateSQL = " Insert into UserUserImportRec_Info (SalaryRecGuid, SalaryYears, Description) Values('" + SalaryRecGuid + "', '" + txtUserSalaryYears.Text + "', '" + txtDescription.Text + "');";
                    CyxPack.OperateSqlServer.DataCommon.QueryData("begin " + sUpdateSQL + " end;");
                }
                else
                {                    
                    SalaryRecGuid = ddlImportRec.SelectedValue.ToString();
                    sUpdateSQL = " Update UserUserImportRec_Info Set Description='" + txtDescription.Text + "' Where SalaryRecGuid='" + SalaryRecGuid + "';";
                    sUpdateFieldsSQL += " Delete From UserUserImportRecFields_Info Where SalaryRecGuid='" + SalaryRecGuid + "';";
                }

                lblProcess.Text = "正在导入数据，请稍候。";
                DateTime dtbegin = DateTime.Now;
                int iUp = 0;
                string strConn, sheetname = "Sheet1$";

                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + lblUploadFile.Text + ";Extended Properties=Excel 8.0;";
                System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(strConn);

                conn.Open();

                DataTable dtExcelSchema = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                if (dtExcelSchema.Rows.Count > 0)
                {
                    sheetname = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                }

                System.Data.OleDb.OleDbDataAdapter oada = new System.Data.OleDb.OleDbDataAdapter("select * from [" + sheetname + "]", strConn);
                DataSet ds = new DataSet();
                oada.Fill(ds);

                conn.Close();

                DataSet dsImportSet = LoadImportSetByDataSet();

                //--导入字段列表
                for (int k = 0; k < dsImportSet.Tables[0].Rows.Count; k++)
                {
                    if (dsImportSet.Tables[0].Rows[k]["TargetName"].ToString().Length > 0)
                    {
                        sUpdateFieldsSQL += " Insert UserUserImportRecFields_Info (SalaryRecGuid, FieldName, SortID) Values('" + SalaryRecGuid.ToString() + "','" + dsImportSet.Tables[0].Rows[k]["TargetName"].ToString() + "'," + k.ToString() + ");";
                    }
                }

                if (CyxPack.OperateSqlServer.DataCommon.QueryData("begin " + sUpdateFieldsSQL + " end;") > 0)
                { 
                
                }

                //--导入数据
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ddlImportRec.SelectedIndex > 0)
                    {
                        sUpdateSQL = " Update UserSalary_Info Set SalaryDate=SalaryDate";

                        for (int n = 0; n < dsImportSet.Tables[0].Rows.Count; n++)
                        {
                            string sTargetFieldName = dsImportSet.Tables[0].Rows[n]["TargetName"].ToString();
                            string sSourceFieldName = dsImportSet.Tables[0].Rows[n]["SourceName"].ToString();

                            if (dsImportSet.Tables[0].Rows[n]["TargetName"].ToString().Length > 0)
                            {
                                string sValue = ds.Tables[0].Rows[i][sSourceFieldName].ToString();

                                if ((sTargetFieldName.IndexOf("Text") >= 0)
                                    || ((sTargetFieldName.IndexOf("OpCode") >= 0)))
                                {
                                    sUpdateSQL += "," + dsImportSet.Tables[0].Rows[n]["TargetName"].ToString() + "='" + sValue + "'";
                                }
                                else
                                {
                                    if (sValue.Length > 0)
                                    {
                                        sUpdateSQL += "," + dsImportSet.Tables[0].Rows[n]["TargetName"].ToString() + "=" + sValue;
                                    }
                                    else
                                    {
                                        sUpdateSQL += "," + dsImportSet.Tables[0].Rows[n]["TargetName"].ToString() + "=0";
                                    }
                                }
                            }
                        }

                        sUpdateSQL += " Where SalaryYears='" + txtUserSalaryYears.Text + "' And SalaryRecGuid='" + SalaryRecGuid + "' And OpCode='" + ds.Tables[0].Rows[i][GetOpCodeFieldName()].ToString() + "'";
                    }
                    else
                    { 
                        sUpdateSQL = " Insert Into UserSalary_Info (SalaryYears, SalaryRecGuid, SalaryDate";

                        for (int n = 0; n < dsImportSet.Tables[0].Rows.Count; n++)
                        {
                            if (dsImportSet.Tables[0].Rows[n]["TargetName"].ToString().Length > 0)
                            {
                                sUpdateSQL += "," + dsImportSet.Tables[0].Rows[n]["TargetName"].ToString();
                            }
                        }

                        sUpdateSQL += ")"
                             + " Values("
                             + " '" + txtUserSalaryYears.Text + "'"
                             + ",'" + SalaryRecGuid + "'"
                             + " ,Getdate()";

                        for (int n = 0; n < dsImportSet.Tables[0].Rows.Count; n++)
                        {
                            string sTargetFieldName = dsImportSet.Tables[0].Rows[n]["TargetName"].ToString();
                            string sSourceFieldName = dsImportSet.Tables[0].Rows[n]["SourceName"].ToString();
                            if (sTargetFieldName.Length > 0)
                            {
                                string sValue = ds.Tables[0].Rows[i][sSourceFieldName].ToString();
                                if ((sTargetFieldName.IndexOf("Text") >= 0)
                                    || ((sTargetFieldName.IndexOf("OpCode") >= 0)))
                                {
                                    sUpdateSQL += ",'" + sValue + "'";
                                }
                                else
                                {
                                    if (sValue.Length > 0)
                                    {
                                        sUpdateSQL += "," + sValue;
                                    }
                                    else
                                    {
                                        sUpdateSQL += ",0";
                                    }
                                }
                            }
                        }
                        sUpdateSQL += ");";
                    }

                    if (CyxPack.OperateSqlServer.DataCommon.QueryData("begin " + sUpdateSQL + " end;") > 0)
                    {
                        iUp += 1;

                        lblProcess.Text = "正在导入第" + iUp.ToString() + "条记录，总" + ds.Tables[0].Rows.Count.ToString() + "条"; ;
                    }
                }

                if (sUpdateSQL.Length > 0)
                {
                    if (iUp > 0)
                    {
                        lblProcess.Text = "已经成功导入 " + iUp.ToString() + " 条记录。"; 
                        Dialog.OpenDialogInAjax(upForm, "恭喜您，数据导入成功"+iUp.ToString() + " 条记录……", "SalaryImport_Step1.aspx");
                    }
                    else
                    {
                        Dialog.OpenDialogInAjax(upForm, "恭喜您，数据导入失败……");
                    }
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fpUpload.HasFile)
            {
                string sWJBH = SysClass.SysGlobal.GetCreateGUID();
                char[] de = { '\\' };
                string[] Afilename = fpUpload.FileName.Split(de);
                string strFileName = Afilename[Afilename.Length - 1];
                string[] AExt = fpUpload.FileName.Split('.');
                string strExt = "";
                if (AExt.Length > 1)
                {
                    strExt = AExt[AExt.Length - 1];
                }
                string sUploadFile = sWJBH;
                if (strExt.Length > 0)
                {
                    sUploadFile = sUploadFile + '.' + strExt;
                }

                string sPathFile = Server.MapPath("..") + "\\" + SysClass.SysUploadFile.UploadDirectory + "\\ImportExcel\\" + sUploadFile;

                sPathFile = Server.MapPath("../" + SysClass.SysUploadFile.UploadDirectory);

                if (Directory.Exists(sPathFile) == false)
                {
                    Directory.CreateDirectory(sPathFile);
                }

                sPathFile = Server.MapPath("../" + SysClass.SysUploadFile.UploadDirectory + "/ImportExcel");

                if (Directory.Exists(sPathFile) == false)
                {
                    Directory.CreateDirectory(sPathFile);
                }

                sPathFile = Server.MapPath("..") + "\\" + SysClass.SysUploadFile.UploadDirectory + "\\ImportExcel\\" + sUploadFile;

                fpUpload.SaveAs(sPathFile);

                lblUploadFile.Text = sPathFile;
                lblUploadFileName.Text = fpUpload.FileName;

                string strConn, sheetname = "Sheet1$";
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + lblUploadFile.Text + ";Extended Properties=Excel 8.0;";
                System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(strConn);
                conn.Open();

                DataTable dtExcelSchema = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                if (dtExcelSchema.Rows.Count > 0)
                {
                    sheetname = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                }

                System.Data.OleDb.OleDbDataAdapter oada = new System.Data.OleDb.OleDbDataAdapter("select * from [" + sheetname + "]", strConn);
                DataSet ds = new DataSet();
                oada.Fill(ds);

                //CyxPack.CommonOperation.DataBinder.BindGridViewData(gvLists, ds);
                conn.Close();

                DataSet dk = SysClass.SysUserSalary.GetUserSalaryFieldsImportLstByDataSet(" and 1<>1");

                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    DataRow row = dk.Tables[0].NewRow();

                    row["SourceName"] = ds.Tables[0].Columns[i].ColumnName;
                    if (ddlTargetName.Items.FindByText(ds.Tables[0].Columns[i].ColumnName) != null)
                    {
                        row["TargetName"] = ddlTargetName.Items.FindByText(ds.Tables[0].Columns[i].ColumnName).Value;
                    }

                    dk.Tables[0].Rows.Add(row);
                }

                gvImportSet.DataSource = dk.Tables[0].DefaultView;
                gvImportSet.DataBind();
            }
            else
            {
                lblProcess.Text = "请选择上传XLS文件。"; 
            }
        }

        private DataSet LoadImportSetByDataSet()
        {

            DataSet ds = SysClass.SysUserSalary.GetUserSalaryFieldsImportLstByDataSet(" and 1<>1");

            for (int i = 0; i < gvImportSet.Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].NewRow();

                dr["SourceName"] = ((Label)gvImportSet.Rows[i].Cells[1].FindControl("lblSourceName")).Text;

                DropDownList _ddlTaskStatus = ((DropDownList)gvImportSet.Rows[i].Cells[2].FindControl("ddlTargetName"));
                if ((_ddlTaskStatus != null) && (_ddlTaskStatus.Visible))
                {
                    dr["TargetName"] = ((DropDownList)gvImportSet.Rows[i].Cells[2].FindControl("ddlTargetName")).SelectedValue.ToString();
                }
                else
                {
                    dr["TargetName"] = ((TextBox)gvImportSet.Rows[i].Cells[2].FindControl("txtddlTargetName")).Text;
                }               

                ds.Tables[0].Rows.Add(dr);
            }

            return ds;

        }
    }
}
