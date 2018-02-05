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
    public partial class User_Export : System.Web.UI.Page
    {
        public int _OrganID = 0;

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
            SqlDataReader sdr = SysClass.SysCustomField.GetCustomLstByReader("UserInfo", "");
            while (sdr.Read())
            {
                BoundField nameColumn = new BoundField();
                nameColumn.DataField = sdr["UserFieldName"].ToString();
                nameColumn.HeaderText = sdr["FieldTitle"].ToString();                
                gvLists.Columns.Add(nameColumn);
            }
            sdr.Close();

            string sSQL = " And a.OrganID in (Select ID From GetOrganChildren(" + _OrganID.ToString() + "))";
            
            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvLists, SysClass.SysUser.GetSysUserByReader(sSQL));
        }

        private bool SaveCheck()
        {
            bool bFlag = true;

            return bFlag;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (SaveCheck())
            {
                BindPageData();
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (gvLists.Rows.Count > 0)
            {
                //调用导出方法  
                ExportGridViewForUTF8(gvLists, DateTime.Now.ToString() + ".xls");
            }
            else
            {

            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Confirms that an HtmlForm control is rendered for 
        }

        private void ExportGridViewForUTF8(GridView GridView, string filename)
        {

            string attachment = "attachment; filename=" + filename;

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", attachment);

            string StyleText = @"<Style> .text{mso-number-format:\@;} </script>";

            //Response.Charset = "UTF-8";
            Response.Charset = "GB2312";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.ContentType = "application/ms-excel";
            EnableViewState = false;
            System.IO.StringWriter sw = new System.IO.StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);
            GridView.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Write(StyleText);
            Response.Flush();
            Response.End();

        }

        protected void gvLists_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[i].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                }
            }
        }
    }
}
