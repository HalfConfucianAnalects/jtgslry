using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JtgTMS.SalaryControl
{
    public partial class SalaryEdit : System.Web.UI.UserControl
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
                return txtUserSalaryYears.Text;
            }
            set
            {
                txtUserSalaryYears.Text = value;
            }
        }

        public string GetNewFieldValue
        {
            get
            {
                if (lblFieldType.Text == "1")
                {
                    return "'" + txtUserSalaryYears.Text + "'";
                }
                else
                {
                    if (txtUserSalaryYears.Text.Length > 0)
                    {
                        return txtUserSalaryYears.Text;
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
                txtUserSalaryYears.Text = value;
            }
        }

        public string GetUpdateSQL
        {
            get
            {
                if (lblFieldType.Text == "1")
                {
                    return "," + lblFieldName.Text + "='" + txtUserSalaryYears.Text + "'";
                }
                else
                {
                    if (txtUserSalaryYears.Text.Length > 0)
                    {
                        return "," + lblFieldName.Text + "=" + txtUserSalaryYears.Text + "";
                    }
                    else
                    {
                        return "," + lblFieldName.Text + "=0";
                    }
                }
            }
        }

    
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}