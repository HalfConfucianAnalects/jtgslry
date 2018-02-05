<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="framework_userPassword.aspx.cs" Inherits="JtgTMS.Platform.framework_userPassword" %>

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
        <ul class="nav nav-pills"  style="display:none">
        <li><a href="framework_userProfile.aspx"><lan>修改资料</lan></a></li>
        <li class="active"><a href="framework_userPassword.aspx"><lan>更改密码</lan></a></li>
      </ul>
      
      
  <div class="popover popover-static">
  <h3 class="popover-title"><lan>更改密码</lan></h3>
  <div class="popover-content">
    
    <asp:ScriptManager ID="smForm" runat="server" EnableScriptGlobalization="false" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
            </Triggers>
            <ContentTemplate>
            
        <table class="table noborder table-hover">
          <tr>
            <td width="150" height="28">工号：</td>
            <td><asp:Literal ID="ltUserName" runat="server"></asp:Literal></td>
          </tr>
          <tr>
            <td>当前密码：</td>
            <td>
              <asp:TextBox ID="CurrentPassword" runat="server" MaxLength="50" Size="20" TextMode="Password"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="CurrentPassword" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
            </td>
          </tr>
          <tr>
            <td>新密码：</td>
            <td>
              <asp:TextBox ID="NewPassword" runat="server" MaxLength="50" Size="20" TextMode="Password"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="NewPassword" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
              <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                runat="server"
                ControlToValidate="NewPassword"
                ValidationExpression="[^']+"
                ErrorMessage="不能输入单引号"
                Display="Dynamic" />
              </td>
          </tr>
          <tr>
            <td>重复输入新密码：</td>
            <td>
              <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password" MaxLength="50" Size="20"></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="ConfirmNewPassword" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
              <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" Display="Dynamic" foreColor="red" ErrorMessage=" 两次输入的新密码不一致！请再输入一遍您上面填写的新密码。"></asp:CompareValidator>
            </td>
          </tr>
        </table>
      
        <hr />
        <table class="table noborder">
          <tr>
            <td class="center">
              <asp:Button ID="Submit" class="btn btn-primary" OnClick="Submit_Click" runat="server" Text="修 改"  />
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
