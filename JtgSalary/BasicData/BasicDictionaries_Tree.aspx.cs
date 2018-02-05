using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;


namespace JtgTMS.BasicData
{
    public partial class BasicDictionaries_Tree : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();
            if (!Page.IsPostBack)
            {
                BindPageData();
            }
        }
        private void BindPageData()
        {
            tvList.Nodes.Clear();

            LoadChildNodes(null);
        }
        private void LoadChildNodes(TreeNode PNode)
        {
            SqlDataReader sdr = SysClass.SysBasicDictionaries.GetSysBaseMainLstByReader();
            while (sdr.Read())
            {
                TreeNode tnNode = new TreeNode();
                tnNode.Value = sdr["ID"].ToString();
                tnNode.Text = sdr["MainName"].ToString();
                tnNode.Target = "worklst";
                tnNode.ImageUrl = "../../sitefiles/bairong/icons/tree/site.gif";
                tnNode.NavigateUrl = "BasicDictionaries_Lst.aspx?MainID=" + tnNode.Value;
                if (PNode == null)
                {
                    tnNode.ShowCheckBox = false;
                    tvList.Nodes.Add(tnNode);
                }
                else
                {
                    PNode.ChildNodes.Add(tnNode);
                }
              //  LoadChildNodes(tnNode);
            }
            sdr.Close();
        }       
    }
}
