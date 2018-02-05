<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="template_edit.aspx.cs" Inherits="JtgTMS.Template.template_edit" %>

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
    <form id="form1" runat="server">
      <table cellpadding="2" cellspacing="2" width="95%" class="center">
        <tr>
          <td width="120">文件夹名称：</td>
          <td>
            <input name="tbFaceName" type="text" id="tbFaceName" style="width:180px;" />
            <span id="ctl02" style="color:Red;display:none;"> *</span></td>
        </tr>
        <tr>
          <td>表情显示名称：</td>
          <td>
            <input name="tbTitle" type="text" id="tbTitle" style="width:180px;" />
            <span id="ctl03" style="color:Red;display:none;"> *</span>
          </td>
        </tr>
        <tr>
          <td>状态：</td>
          <td>
            <table id="rblIsEnabled" border="0">
	            <tr>
		            <td><input id="rblIsEnabled_0" type="radio" name="rblIsEnabled" value="True" checked="checked" /><label for="rblIsEnabled_0">启用</label></td><td><input id="rblIsEnabled_1" type="radio" name="rblIsEnabled" value="False" /><label for="rblIsEnabled_1">禁用</label></td>
	            </tr>
            </table>
          </td>
        </tr>
      </table>
      <asp:Button ID="btnSubmit" NAME="btnSubmit" runat="server" 
            onclick="btnSubmit_Click" /><asp:TextBox ID="A" runat="server"></asp:TextBox>
    </form>
</body>
</html>