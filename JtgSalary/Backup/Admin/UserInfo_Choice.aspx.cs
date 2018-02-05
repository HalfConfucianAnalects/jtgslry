using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace JtgTMS.BasicData
{
    public partial class UserInfo_Choice : System.Web.UI.Page
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
            tvList.Nodes.Clear();

            LoadChildNodes(null, _OrganID);

            if (tvList.Nodes.Count > 0)
            { 
                tvList.Nodes[0].Selected = true;
            }

            LoadPageData();
        }

        private void LoadPageData()
        {
            int _selCategoryID = 0;

            if (tvList.SelectedNode != null)
            {
                _selCategoryID = int.Parse(tvList.SelectedNode.Value);
            }

            string sWhereSQL = "";

            if ((ddpSearchName.SelectedIndex > 0) && (txtSearchKeyword.Text.Length > 0))
            {
                sWhereSQL += " And a." + ddpSearchName.SelectedValue.ToString() + " like '%" + txtSearchKeyword.Text + "%'";
            }

            if (chkHasChildren.Checked)
            {
                sWhereSQL += " And a.OrganID in (Select ID From GetOrganChildren(" + _selCategoryID.ToString() + "))";
            }
            else
            {
                sWhereSQL += " And a.OrganID =" + _selCategoryID.ToString();
            }

            CyxPack.CommonOperation.DataBinder.BindGridViewData(gvLists, SysClass.SysUser.GetSysUserByDataSet(sWhereSQL));
        }

        private void LoadChildNodes(TreeNode PNode, int PID)
        {
            SqlDataReader sdr = SysClass.SysOrgan.GetOrganLstByReader(PID);
            while (sdr.Read())
            {
                if (sdr["OrganType"].ToString() != SysClass.SysOrgan.SupplierType_Value.ToString())
                {
                    TreeNode tnNode = new TreeNode();
                    tnNode.Value = sdr["ID"].ToString();
                    tnNode.Text = sdr["OrganName"].ToString();
                    tnNode.Target = "worklst";
                    tnNode.ImageUrl = "../../sitefiles/bairong/icons/tree/department.gif";
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
            }
            sdr.Close();
        }

        protected void gvLists_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Choice")
            {
                SysClass.SysParams.UserInfo_ChoiceValues = e.CommandArgument.ToString();
                ClientScript.RegisterStartupScript(this.GetType(), "info", "<script>UpdateSuccess();</script>");
            }
        }       

        protected void tvList_SelectedNodeChanged(object sender, EventArgs e)
        {
            LoadPageData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindPageData();
        }
    }
}
