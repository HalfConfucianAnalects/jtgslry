using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace JtgTMS.Admin
{
    public partial class Dept_Tree : System.Web.UI.Page
    {
        public int _POrganID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            if (Request.Params["POrganID"] != null)
            {
                _POrganID = int.Parse(Request.Params["POrganID"]);
            }
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }

        private void BindPageData()
        {
            tvList.Nodes.Clear();

            LoadChildNodes(null, 0);
        }

        private void LoadChildNodes(TreeNode PNode, int PID)
        {
            int _OrganType = 0;
            if (PNode == null)
            {
                _OrganType = SysClass.SysOrgan.OrganType_Value;
            }
            else if (PNode.Value == SysClass.SysOrgan.OrganType_Value.ToString())
            {
                _OrganType = SysClass.SysOrgan.WorkshopType_Value;
            }
            SqlDataReader sdr = SysClass.SysOrgan.GetOrganLstByReader(PID, _OrganType);
            while (sdr.Read())
            {
                TreeNode tnNode = new TreeNode();
                //tnNode.Value = sdr["OrganType"].ToString();
                tnNode.Value = sdr["OrganType"].ToString();
                tnNode.Text = sdr["OrganName"].ToString();
                tnNode.Target = "worklst";
                tnNode.ImageUrl = "../../sitefiles/bairong/icons/tree/department.gif";
                tnNode.NavigateUrl = "Dept_Lst.aspx?POrganID=" + sdr["ID"].ToString();
                if (PNode == null)
                {
                    tnNode.ImageUrl = "../../sitefiles/bairong/icons/tree/home.png";
                    tnNode.ShowCheckBox = false;
                    tvList.Nodes.Add(tnNode);
                }
                else if (sdr["OrganType"].ToString() == "1")
                {
                    tnNode.ImageUrl = "../../sitefiles/bairong/icons/tree/workshop.png";
                    PNode.ChildNodes.Add(tnNode);
                }
                else if (sdr["OrganType"].ToString() == "2")
                {
                    tnNode.ImageUrl = "../../sitefiles/bairong/icons/tree/dept.png";
                    PNode.ChildNodes.Add(tnNode);
                }
                else if (sdr["OrganType"].ToString() == "3")
                {
                    tnNode.ImageUrl = "../../sitefiles/bairong/icons/tree/group.png";
                    PNode.ChildNodes.Add(tnNode);
                }
                if (PNode == null)
                {
                    LoadChildNodes(tnNode, int.Parse(sdr["ID"].ToString()));
                }
            }
            sdr.Close();
        }
    }
}
