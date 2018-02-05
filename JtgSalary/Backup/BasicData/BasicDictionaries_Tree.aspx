<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="BasicDictionaries_Tree.aspx.cs" Inherits="JtgTMS.BasicData.BasicDictionaries_Tree" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <script language="javascript" src="/SiteFiles/bairong/jquery/jquery-1.9.1.min.js"></script>
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
    <form id="Form1" runat="server">
    <div>
        <table class="table noborder table-condensed table-hover">
            <tr class="info">
              <td style="padding-left:50px;">
                <a href="BasicDictionaries_Tree.aspx" target="tree" target="department">基础字典管理(刷新)</a></td>
            </tr>
            <tr>
              <td></td>
            </tr>
          </table>
          
          <asp:TreeView ID="tvList" runat="server" CssClass="treelist-condensed table-hover" NodeStyle-Height="26" Width="100%" ExpandDepth="10" ShowCheckBoxes="None" ShowLines="True" 
            EnableClientScript="False" PopulateNodesFromClient="False" EnableTheming="False">
            <SelectedNodeStyle Font-Underline="True" ForeColor="#CC3300" 
                                HorizontalPadding="0px" VerticalPadding="0px" Font-Bold="True" />
                <Nodes>
                    <asp:TreeNode Value=".ROOT" Text="六合盛房产" ImageUrl="../../sitefiles/bairong/icons/tree/department.gif"></asp:TreeNode>
                </Nodes>
                <NodeStyle Font-Names="微笑雅黑" NodeSpacing="0px" VerticalPadding="0px" />
        </asp:TreeView>	 
    
    </div>
    </form>
</body>
</html>
