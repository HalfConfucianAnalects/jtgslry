<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BasicDictionaries_Edit.aspx.cs" Inherits="JtgTMS.BasicData.BasicDictionaries_Edit" %>

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
    <form id="upForm" class="form-inline" runat="server">
   <div>
   
       <ul class="breadcrumb" style="display:none"><li>基础数据 <span class="divider">/</span></li><li>基础字典管理 <span class="divider">/</span></li><li class="active">
               编辑基础字典</li></ul>
        <asp:ScriptManager ID="smForm" runat="server" EnableScriptGlobalization="True"/>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnApply" /> 
            </Triggers>
            <ContentTemplate> 

              <div class="popover popover-static">
                  <h3 class="popover-title">编辑基础字典</h3>
                      <div class="popover-content">
                         <table  class="table noborder table-hover">
                              <tr>
                                  <td width="150px">
                                     字典编号：</td>
                                  <td>
                                     <asp:TextBox ID="txtDictionariesNo" MaxLength="32" runat="server"></asp:TextBox>
                                      <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtDictionariesNo" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator></td>
                              </tr>
                              <tr>
                                  <td width="150px">
                                      字典名称：</td>
                                  <td>
                                      <asp:TextBox ID="txtDictionariesName" MaxLength="256" runat="server"></asp:TextBox>
                                       <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtDictionariesName" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator></td>
                              </tr>
                             <tr>
                              <td width="150px">
                                  备注：</td>
                              <td>
                               <asp:TextBox ID="txtNote" MaxLength="256" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                             </tr>
                          </table>
                        &nbsp;<hr />
                        <table class="table noborder">
                          <tr>
                            <td class="center">
                                <asp:Button ID="btnApply" CssClass="btn btn-primary" Text="确 定" onclick="btnApply_Click" runat="server"/>
                                <input type="submit" name="btnReturn" value="返 回" onclick="location.href='BasicDictionaries_Lst.aspx?MainID=<% =_MainID %>';return false;" id="btnReturn" class="btn" />
                            </td>
                          </tr>
                        </table>
                    </div>
               </div>
              </ContentTemplate>
        </asp:UpdatePanel>
   </div>
    </form>
</body>
</html>

