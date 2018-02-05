using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JtgTMS.SalaryControl
{
    public partial class CustomExtView : System.Web.UI.UserControl
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
                //lblFieldValue.Visible = value == "1";

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
                return lblFieldValue.Text;
            }
            set
            {
                lblFieldValue.Text = value;
            }
        }

        public string GetNewFieldValue
        {
            get
            {
                if (lblFieldType.Text == "1")
                {
                    return "'" + lblFieldValue.Text + "'";
                }
                else
                {
                    if (lblFieldValue.Text.Length > 0)
                    {
                        return lblFieldValue.Text;
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
                lblFieldValue.Text = value;
            }
        }

        public string GetUpdateSQL
        {
            get
            {
                if (lblFieldType.Text == "1")
                {
                    return lblFieldName.Text + "='" + lblFieldValue.Text + "'";
                }
                else
                {
                    if (lblFieldValue.Text.Length > 0)
                    {
                        return lblFieldName.Text + "=" + lblFieldValue.Text + "";
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