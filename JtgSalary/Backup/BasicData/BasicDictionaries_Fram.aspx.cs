﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JtgTMS.BasicData
{
    public partial class BasicDictionaries_Fram : System.Web.UI.Page
    {
        public int _CategoryID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            SysClass.SysGlobal.CheckSysIsLogined();

            _CategoryID = SysClass.SysBasicDictionaries.GetTopBaseMainID();
        }
    }
}
