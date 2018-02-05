<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomField_Edit.aspx.cs" Inherits="JtgTMS.Admin.CustomField_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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
   
       <ul class="breadcrumb" style="display:none"><li>成员权限 <span class="divider">/</span></li><li>用户管理 <span class="divider">/</span></li><li class="active">编辑<% =_TableTitle%>字段</li></ul>
        <asp:ScriptManager ID="smForm" runat="server" EnableScriptGlobalization="True"/>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnApply" /> 
            </Triggers>
            <ContentTemplate> 

              <div class="popover popover-static">
                  <h3 class="popover-title">字段信息</h3>
                      <div class="popover-content">

                        <table class="table noborder table-hover">      
                          <tr>
                            <td>字段名：</td>
                            <td>
                                <asp:TextBox ID="txtFieldName" MaxLength="64" runat="server"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="rfvFieldName" ControlToValidate="txtFieldName" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator>
                              </td>
                          </tr>
                          <tr>
                            <td>字段中文名：</td>
                            <td>
                                <asp:TextBox ID="txtFieldTitle" MaxLength="64" runat="server"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="rfvFieldTitle" ControlToValidate="txtFieldTitle" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator>
                              </td>
                          </tr>
                          <tr>
                            <td>字段类型：</td>
                            <td>
                                <asp:DropDownList ID="ddlFieldType" runat="server">
                                    <asp:ListItem Value="0" Text="金额"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="文本"></asp:ListItem>
                                </asp:DropDownList>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlFieldType" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator>
                              </td>
                          </tr>
                          <tr>
                            <td></td>
                            <td>
                                <asp:RadioButtonList ID="rblIsReadonly" RepeatColumns="2" runat="server">
                                    <asp:ListItem Value="0" Text="可编辑"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="只读"></asp:ListItem>
                                </asp:RadioButtonList>
                              </td>
                          </tr>
                              <tr>
                            <td>备注：</td>
                            <td>
                                <asp:TextBox ID="txtDescription" TextMode="MultiLine" MaxLength="256" runat="server"></asp:TextBox>         
                            </td>
                          </tr>
                          
                        </table>
                    </div>
               </div>              
               
                <hr />
                <table class="table noborder">
                  <tr>
                    <td class="center">
                        <asp:Button ID="btnApply" CssClass="btn btn-primary" Text="确 定" onclick="btnApply_Click" runat="server"/>
                        <input type="submit" name="btnReturn" value="返 回" onclick="location.href='CustomField_Lst.aspx?TableNo=<%=_TableNo %>';return false;" id="btnReturn" class="btn" />
                    </td>
                  </tr>
                </table>
       
              </ContentTemplate>
        </asp:UpdatePanel>
   </div>
   
   
    </form>
</body>
</html>
