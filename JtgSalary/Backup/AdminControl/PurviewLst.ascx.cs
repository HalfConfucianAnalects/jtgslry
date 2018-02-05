using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace JtgTMS.AdminControl
{
    public partial class PurviewLst : System.Web.UI.UserControl
    {
        protected int _PurviewID = 0;
        protected string _Purview = "";
        public int PurivewID
        {
            get
            {
                return this._PurviewID;
            }
            set
            {
                this._PurviewID = value;
            }
        }
        public string Purview
        {
            get
            {
                return this._Purview;
            }
            set
            {
                this._Purview = value;
            }
        }

        public string CalcPurviewValue(string Purview)
        {
            string sPurview = Purview;
            for (int i = 0; i < tvCategory.CheckedNodes.Count; i++)
            {
                int k = int.Parse(tvCategory.CheckedNodes[i].Value);
                sPurview = sPurview.Substring(0, k - 1) + "1" + sPurview.Substring(k, Purview.Length - k);
            }
            return sPurview;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {                
                BindPageData();
            }
        }

        //加载子节点
        private void LoadChildPurview(int _PPurviewID, TreeNode _PNode)
        {
            SqlDataReader recn = SysClass.SysPurview.GetRoleChildPurviewLst(_PPurviewID);
            while (recn.Read())
            {
                TreeNode tnNode = new TreeNode();
                tnNode.Text = recn["PurviewName"].ToString();
                tnNode.Value = " " + recn["PurviewID"].ToString();
                if (_Purview[int.Parse(tnNode.Value) - 1] == char.Parse("1"))
                {
                    tnNode.Checked = true;
                }
                else
                {
                    tnNode.Checked = false;
                }
                tnNode.Expanded = false;
                if (_PNode == null)
                {
                    tvCategory.Nodes.Add(tnNode);
                }
                else
                {
                    _PNode.ChildNodes.Add(tnNode);
                }

                LoadChildPurview(int.Parse(tnNode.Value), tnNode);
            }
            recn.Close();
        }

        //加载当前节点
        private void LoadChildPurview(int _PurviewID)
        {
            SqlDataReader recn = SysClass.SysPurview.GetSingleRolePurview(_PurviewID);
            if (recn.Read())
            {
                TreeNode tnNode = new TreeNode();
                tnNode.Text = recn["PurviewName"].ToString();
                tnNode.Value = " " + recn["PurviewID"].ToString();
                if (_Purview[int.Parse(tnNode.Value) - 1] == char.Parse("1"))
                {
                    tnNode.Checked = true;
                }
                else
                {
                    tnNode.Checked = false;
                }
                tnNode.Expanded = true;

                tvCategory.Nodes.Add(tnNode);

                LoadChildPurview(int.Parse(tnNode.Value), tnNode);
            }
            recn.Close();
        }

        private void BindPageData()
        {
            LoadChildPurview(_PurviewID);
            //SqlDataReader recn = SysClass.SysPurview.GetSingleRolePurview(_PurviewID);
            //if (recn.Read())
            //{
            //    TreeNode tnNode = new TreeNode();
            //    tnNode.Text = recn["PurviewName"].ToString();
            //    tnNode.Value = " " + recn["PurviewID"].ToString();
            //    if (_Purview[int.Parse(tnNode.Value) - 1] == char.Parse("1"))
            //    {
            //        tnNode.Checked = true;
            //    }
            //    else
            //    {
            //        tnNode.Checked = false;
            //    }
            //    tnNode.Expanded = true;

            //    tvCategory.Nodes.Add(tnNode);

            //    LoadChildPurview(int.Parse(tnNode.Value), tnNode);
            //}
            //recn.Close();

            tvCategory.ExpandAll();       
        }

        protected void tvCategory_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            this.CheckTreeNode(e.Node, e.Node.Checked);
        }

        private void CheckTreeNode(TreeNode node, bool bChecked)
        {
            node.Checked = bChecked;
            foreach (TreeNode childNode in node.ChildNodes)
            {
                childNode.Checked = bChecked;
                CheckTreeNode(childNode, bChecked);
            }
        }
    }
}