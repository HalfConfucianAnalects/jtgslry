using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace JtgTMS.Admin
{
    public partial class User_Tree : System.Web.UI.Page
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
            SqlDataReader sdr = SysClass.SysOrgan.GetOrganLstByReader(PID);
            while (sdr.Read())
            {
                TreeNode tnNode = new TreeNode();
                tnNode.Value = sdr["ID"].ToString();
                tnNode.Text = sdr["OrganName"].ToString();
                tnNode.Target = "worklst";
                tnNode.ImageUrl = "../../sitefiles/bairong/icons/tree/workshop.png";
                tnNode.NavigateUrl = "User_Lst.aspx?OrganID=" + tnNode.Value; ;
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
                
                LoadChildNodes(tnNode, int.Parse(sdr["ID"].ToString()));
            }
            sdr.Close();
        }             
    }
}
