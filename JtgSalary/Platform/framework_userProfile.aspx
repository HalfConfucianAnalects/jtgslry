<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="framework_userProfile.aspx.cs" Inherits="JtgTMS.Platform.framework_userProfile" %>
<%@ Register Src="../SalaryControl/CustomExtView.ascx" TagName="CustomExtView" TagPrefix="ucCustomExtView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta charset="utf-8">
    <script language="javascript" src="/SiteFiles/bairong/jquery/jquery-1.8.3.min.js"></script>
    <script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>

    <link rel="stylesheet" type="text/css" href="/SiteFiles/bairong/jquery/bootstrap/css/bootstrap.min.css">
    <script language="javascript" src="/SiteFiles/bairong/jquery/bootstrap/js/bootstrap.min.js"></script>
    <!--[if lte IE 6]>
    <link rel="stylesheet" type="text/css" href="/SiteFiles/bairong/jquery/bootstrap/ie/bootstrap-ie6.min.css">
    <script type="text/javascript" src="/SiteFiles/bairong/jquery/bootstrap/ie/bootstrap-ie.js"></script>
    <![endif]-->
    <!--[if lte IE 7]>
    <link rel="stylesheet" type="text/css" href="/SiteFiles/bairong/jquery/bootstrap/ie/ie.css">
    <![endif]-->
    <script type="text/javascript">
    (function ($) {
        $(document).ready(function() {
            if ($.isFunction($.bootstrapIE6)) $.bootstrapIE6($(document));
        });
    })(jQuery);
    </script>

    <!--[if lt IE 9]><script src="/SiteFiles/bairong/jquery/html5shiv/html5shiv.js"></script><![endif]-->

    <link rel="stylesheet" href="../inc/style.css" type="text/css" />
    <script language="javascript" src="../inc/script.js"></script>
</head>
<body>
    <form id="form1" class="form-inline" runat="server">
    <ul class="nav nav-pills" runat="server" id="ulNav">    
        <li class="active"><a href="framework_userProfile.aspx"><lan>个人资料</lan></a></li>
        <li><a href="framework_userProfile1.aspx"><lan>录入资料</lan></a></li>
      </ul>

  <div class="popover popover-static">
  <h3 class="popover-title"><lan>个人信息</lan></h3>
  <div class="popover-content">
    <asp:ScriptManager ID="smForm" runat="server" EnableScriptGlobalization="true" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <table class="table noborder table-hover" cellpadding="0" cellspacing="0">
              <tr>
                <td width="125" height="28">
                    &nbsp;&nbsp;&nbsp;&nbsp;工号：
                </td>
                <td width="250">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Literal ID="ltUserName" runat="server"></asp:Literal></td>
                <td width="120">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;姓名：</td>
                <td width="250">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="txtOpName" CssClass="dfinput2" runat="server"></asp:Label>
                  
                </td>
                <td></td>
              </tr>
              </table>              
             <asp:DataList ID="dlList" runat="server" CssClass="table noborder table-hover" RepeatColumns="2" Width="830" RepeatDirection="Horizontal"
                GridLines="None">
                <ItemTemplate>
                    <td align="left" width="150px" height="28">
                        <%# Eval("FieldTitle").ToString().Length > 0 ? Eval("FieldTitle").ToString() + "：" : ""%>
                    </td>
                    <td width="250">
                        <ucCustomExtView:CustomExtView ID="WorklogExtEdit1" UserFieldType='<%# Eval("FieldType").ToString() %>' UserIsReadOnly='<%# Eval("IsReadOnly").ToString() %>' UserFieldName='<%# Eval("UserFieldname").ToString() %>' runat="server" />
                    </td>
                </ItemTemplate>
            </asp:DataList>
            <hr />
            <table class="table noborder">
              <tr>
                <td class="center">
                  <asp:Button ID="Submit" class="btn btn-primary" OnClick="Submit_Click" Visible="false" runat="server" Text="修 改"  />
                </td>
              </tr>
            </table>
         </ContentTemplate>
    </asp:UpdatePanel>
    </div>
  </div>
    </form>
</body>
</html>
