using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JtgTMS.SalaryControl
{
    public partial class CustomExtEdit : System.Web.UI.UserControl
    {
        protected string _UserFieldType = "0";
        protected string _UserFieldName = "";
        protected string _UserFieldValue = "";

        public string UserFieldType
        {
            get
            {
                return lblFieldType.Text;
            }
            set
            {
                if (value == "1")
                {
                    txtFieldValue.Attributes.Remove("onkeypress");
                }
                lblFieldType.Text = value;
            }
        }

        public string UserIsReadOnly
        {
            get
            {
                return lblIsReadOnly.Text;
            }
            set
            {
                txtFieldValue.Visible = ((value != "1") && (lblFieldName.Text.Length > 0));
                lblFieldValue.Visible = ((value == "1") && (lblFieldName.Text.Length > 0));

                lblIsReadOnly.Text = value;
            }
        }

        public string UserFieldName
        {
            get
            {
                return lblFieldName.Text;
            }
            set
            {
                lblFieldName.Text = value;
            }
        }

        public string UserFieldValue
        {
            get
            {
                return txtFieldValue.Text;
            }
            set
            {
                UserIsReadOnly = UserIsReadOnly;
                txtFieldValue.Text = value;
                lblFieldValue.Text = value;
            }            
        }

        public string GetNewFieldValue
        {
            get
            {
                if (lblFieldType.Text == "1")
                {
                    return "'" + txtFieldValue.Text + "'";
                }
                else
                {
                    if (txtFieldValue.Text.Length > 0)
                    {
                        return txtFieldValue.Text;
                    }
                    else
                    {
                        return "0";
                    }
                }
            }
            set
            {
                this._UserFieldValue = value;
                txtFieldValue.Text = value;
            }
        }

        public string GetUpdateSQL
        {
            get
            {
                if (lblFieldType.Text == "1")
                {
                    return lblFieldName.Text + "='" + txtFieldValue.Text + "'";
                }
                else
                {
                    if (txtFieldValue.Text.Length > 0)
                    {
                        return lblFieldName.Text + "=" + txtFieldValue.Text + "";
                    }
                    else
                    {
                        return lblFieldName.Text + "=0";
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}