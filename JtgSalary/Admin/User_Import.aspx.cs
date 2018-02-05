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

namespace JtgTMS.Admin
{
    public partial class User_Import : System.Web.UI.Page
    {
        private int _OrganID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["OrganID"] != null)
            {
                _OrganID = int.Parse(Request.Params["OrganID"]);
            }

            if (!Page.IsPostBack)
            {                
                BindPageData();
            }
        }

        private void BindPageData()
        {
            SysClass.SysCustomField.FullToCustomLst(SysClass.SysCustomField.UserInfo_TableNo, ddlTargetName, false, false);

            txtOrganName.Text = SysClass.SysOrgan.GetOrganNameByID(_OrganID);

            //SysClass.SysUserSalary.FullToSalaryFieldsLst(ddlTargetName, txtUserSalaryYears.Text);

            //SysClass.SysUserSalary.FullToSalaryImportRecLst(ddlImportRec, txtUserSalaryYears.Text);
        }

        private bool SaveCheck()
        {
            bool bFlag = true;
            if (lblUploadFile.Text.Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtOrganName, "请选择Excel工资文件！");
            }
            else if (GetOpCodeFieldName().Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtOrganName, "请选择工号字段！");
            }
            else if (GetOpNameFieldName().Length == 0)
            {
                bFlag = false;
                Dialog.OpenDialogInAjax(txtOrganName, "请选择姓名字段！");
            }
            else
            { 
                string _OpCodeFieldName = GetOpCodeFieldName();
                string _OpNameFieldName = GetOpNameFieldName();
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
                    string _SQL = "Select Top 1 1 From SysUser_Info Where OpCode='" + ds.Tables[0].Rows[i][_OpCodeFieldName].ToString() + "' And OpName<>'" + ds.Tables[0].Rows[i][_OpNameFieldName].ToString() + "'";
                    if (SysClass.SysGlobal.GetExecSqlIsExist(_SQL))
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
                    Dialog.OpenDialogInAjax(txtOrganName, "下列工号和姓名与系统内有异，请检查！");
                }
            }

            return bFlag;
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

        private string GetOrganFieldName()
        {
            string _OpCodeFieldName = "";

            DataSet dsImportSet = LoadImportSetByDataSet();

            for (int n = 0; n < dsImportSet.Tables[0].Rows.Count; n++)
            {
                if (dsImportSet.Tables[0].Rows[n]["TargetName"].ToString().ToLower() == "Organ".ToLower())
                {
                    _OpCodeFieldName = dsImportSet.Tables[0].Rows[n]["SourceName"].ToString();
                }
            }

            return _OpCodeFieldName;
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

        private string GetOpNameFieldName()
        {
            string _OpCodeFieldName = "";

            DataSet dsImportSet = LoadImportSetByDataSet();

            for (int n = 0; n < dsImportSet.Tables[0].Rows.Count; n++)
            {
                if (dsImportSet.Tables[0].Rows[n]["TargetName"].ToString().ToLower() == "OpName".ToLower())
                {
                    _OpCodeFieldName = dsImportSet.Tables[0].Rows[n]["SourceName"].ToString();
                }
            }

            return _OpCodeFieldName;
        }        

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                string sUpdateSQL = "", sEditSQL = "", sCustomInsertSQL = "", sCustomEditSQL = "";                

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

                string _GetOpCodeFieldName = GetOpCodeFieldName();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    sUpdateSQL = " if not Exists(Select top 1 1 From SysUser_Info Where OpCode='" + ds.Tables[0].Rows[i][_GetOpCodeFieldName].ToString() + "') "
                            + " begin";

                    sCustomInsertSQL = " if not Exists(Select top 1 1 From SysUserExt_Info Where UserID in (Select ID From SysUser_Info Where OpCode='" + ds.Tables[0].Rows[i][_GetOpCodeFieldName].ToString() + "')) "
                            + " begin";

                    sUpdateSQL += " Insert Into SysUser_Info (SystemID , Guid, OrganID, Password";
                    sEditSQL = " Update SysUser_Info Set SystemID=SystemID, OrganID=";
                    
                    sCustomInsertSQL += " Insert Into SysUserExt_Info (UserID ";
                    sCustomEditSQL = " Update SysUserExt_Info Set UserID=SysUser_Info.ID" ;

                    for (int n = 0; n < dsImportSet.Tables[0].Rows.Count; n++)
                    {
                        string sTargetFieldName = dsImportSet.Tables[0].Rows[n]["TargetName"].ToString();

                        if (sTargetFieldName != "Organ")
                        {
                            if (sTargetFieldName.Length > 0)
                            {
                                if (dsImportSet.Tables[0].Rows[n]["TargetName"].ToString().IndexOf("|") > 0)
                                {
                                    string _TargerName = dsImportSet.Tables[0].Rows[n]["TargetName"].ToString();

                                    _TargerName = "Column_" + _TargerName.Substring(2, _TargerName.Length - 2);

                                    sCustomInsertSQL += "," + _TargerName;
                                }
                                else
                                {
                                    sUpdateSQL += "," + dsImportSet.Tables[0].Rows[n]["TargetName"].ToString();
                                }
                            }
                        }
                    }

                    sUpdateSQL += ")"
                         + " Values("
                         + SysClass.SysParams.GetPurviewSystemID().ToString()
                         + ", '" + SysClass.SysGlobal.GetCreateGUID() + "'";

                    sCustomInsertSQL += ")"
                         + " (Select ID ";

                    string _OrganFieldName = GetOrganFieldName();
                    if ((_OrganFieldName.Length > 0) && (SysClass.SysOrgan.GetOrganIDByOrganName(ds.Tables[0].Rows[i][_OrganFieldName].ToString()) > 0))
                    {
                        int _TOrganID = SysClass.SysOrgan.GetOrganIDByOrganName(ds.Tables[0].Rows[i][_OrganFieldName].ToString());
                        if (_TOrganID == 0)
                        {
                            _TOrganID = _OrganID;
                        }
                        sUpdateSQL += ", " + _TOrganID.ToString();
                        sEditSQL += _TOrganID.ToString();
                    }
                    else
                    {
                        //sUpdateSQL += ", " + _OrganID.ToString();
                        sUpdateSQL += ", 58";  //不存在的单位导入临时单位下
                        sEditSQL += "OrganID";
                    }

                    sUpdateSQL += " ,'111111'";

                    for (int n = 0; n < dsImportSet.Tables[0].Rows.Count; n++)
                    {
                        string sTargetFieldName = dsImportSet.Tables[0].Rows[n]["TargetName"].ToString();
                        string sSourceFieldName = dsImportSet.Tables[0].Rows[n]["SourceName"].ToString();

                        if (sTargetFieldName != "Organ")
                        {
                            if (sTargetFieldName.Length > 0)
                            {
                                if (dsImportSet.Tables[0].Rows[n]["TargetName"].ToString().IndexOf("|") > 0)
                                {
                                    string sValue = ds.Tables[0].Rows[i][sSourceFieldName].ToString();
                                    sCustomInsertSQL += ",'" + sValue + "'";

                                    string _TargerName = dsImportSet.Tables[0].Rows[n]["TargetName"].ToString();
                                    string _A = _TargerName.Substring(0, 1);
                                    _TargerName = _TargerName.Substring(2, _TargerName.Length - 2);

                                    if (_A == "1")
                                    {
                                        sCustomEditSQL += ",Column_" + _TargerName + "='" + sValue + "'";
                                    }
                                    else
                                    {
                                        if (sValue.Length == 0)
                                        {
                                            sCustomEditSQL += "," + "Column_" + _TargerName + "=0";
                                        }
                                        else
                                        {
                                            sCustomEditSQL += "," + "Column_" + _TargerName + "=" + sValue;
                                        }
                                    }
                                }
                                else
                                {
                                    string sValue = ds.Tables[0].Rows[i][sSourceFieldName].ToString();
                                    string _TargerName = dsImportSet.Tables[0].Rows[n]["TargetName"].ToString();

                                    if (_TargerName == "IsCanLogin")
                                    {
                                        if (sValue == "是")
                                        {
                                            sValue = "1";
                                        }
                                        else
                                        {
                                            sValue = "0";
                                        }

                                        sUpdateSQL += "," + sValue;
                                        sEditSQL += "," + _TargerName + "=" + sValue + "";
                                    }
                                    else if (_TargerName == "Sex")
                                    {
                                        if (sValue == "女")
                                        {
                                            sValue = "1";
                                        }
                                        else
                                        {
                                            sValue = "0";
                                        }

                                        sUpdateSQL += ",'" + sValue + "'";

                                        sEditSQL += "," + _TargerName + "='" + sValue + "'";
                                    }
                                    else
                                    {
                                        sUpdateSQL += ",'" + sValue + "'";

                                        sEditSQL += "," + _TargerName + "='" + sValue + "'";
                                    }
                                }
                            }
                        }
                    }

                    sCustomEditSQL += " From SysUser_Info Where SysUser_Info.OpCode='" + ds.Tables[0].Rows[i][_GetOpCodeFieldName].ToString() + "' And SysUserExt_Info.UserID=SysUser_Info.ID; ";

                    sCustomInsertSQL += " From SysUser_Info Where OpCode='" + ds.Tables[0].Rows[i][_GetOpCodeFieldName].ToString() + "'); end "
                        + " else begin " + sCustomEditSQL + " end;";

                    sEditSQL += " Where OpCode='" + ds.Tables[0].Rows[i][_GetOpCodeFieldName].ToString() + "';";

                    sUpdateSQL += ");";

                    sUpdateSQL += " end else begin " + sEditSQL + " end; " + sCustomInsertSQL;

                    if (CyxPack.OperateSqlServer.DataCommon.QueryData("begin " + sUpdateSQL + " end;") > 0)
                    {
                        iUp += 1;

                        lblProcess.Text = "正在导入第" + iUp.ToString() + "条记录，总" + ds.Tables[0].Rows.Count.ToString() + "条"; ;
                    }
                    
                }

                string _SQL = "begin"
        
                    + " Insert Into SysUserRoles_Info (SystemID, UserID, RoleID) (Select 2, ID, 30 From SysUser_Info Where ID not in (Select UserID From SysUserRoles_Info Where SystemID=2));"

                    + " Delete from SysUserRoles_Info Where UserID not in (Select ID From SysUser_Info);"

                    + " end;";


                CyxPack.OperateSqlServer.DataCommon.QueryData("begin " + _SQL + " end;");

                if (sUpdateSQL.Length > 0)
                {
                    if (iUp > 0)
                    {

                        lblProcess.Text = "已经成功导入 " + iUp.ToString() + " 条记录。";
                        Dialog.OpenDialogInAjax(upForm, "恭喜您，数据导入成功" + iUp.ToString() + " 条记录……", "User_Import.aspx?OrganID=" + _OrganID.ToString());
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

                CyxPack.CommonOperation.DataBinder.BindGridViewData(gvLists, ds);
                conn.Close();

                DataSet dk = SysClass.SysUserSalary.GetUserSalaryFieldsImportLstByDataSet(" and 1<>1");

                bool _HasOpName = false;

                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    DataRow row = dk.Tables[0].NewRow();

                    row["SourceName"] = ds.Tables[0].Columns[i].ColumnName;
                    if (ddlTargetName.Items.FindByText(ds.Tables[0].Columns[i].ColumnName) != null)
                    {
                        row["TargetName"] = ddlTargetName.Items.FindByText(ds.Tables[0].Columns[i].ColumnName).Value;
                    }
                    else if (ddlTargetName.Items.FindByText(ds.Tables[0].Columns[i].ColumnName+"|扩展") != null)
                    {
                        row["TargetName"] = ddlTargetName.Items.FindByText(ds.Tables[0].Columns[i].ColumnName + "|扩展").Value;
                    }

                    dk.Tables[0].Rows.Add(row);

                    if (ds.Tables[0].Columns[i].ColumnName == "姓名")
                    {
                        _HasOpName = true;
                    }
                }

                gvImportSet.DataSource = dk.Tables[0].DefaultView;
                gvImportSet.DataBind();                
            }
            else
            {
                lblProcess.Text = "请选择上传XLS文件。"; 
            }
        }

        private int IndexByExtValue(string FieldValue)
        {
            int _index = -1;
            for (int i = 0; i < ddlTargetName.Items.Count; i++)
            {
                if ((ddlTargetName.Items[i].Value.Substring(1, 1) == "|") && (ddlTargetName.Items[i].Value.Substring(2, ddlTargetName.Items[i].Value.Length - 2) == FieldValue))
                {
                    _index = -1;
                }

            }

            return _index;
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
