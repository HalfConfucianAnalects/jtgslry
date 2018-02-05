<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalaryRemind.aspx.cs" Inherits="JtgTMS.WarmingSalary.SalaryRemind" %>

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
   
       <ul class="breadcrumb" style="display:none"><li>系统预警 <span class="divider">/</span></li><li>系统预警 <span class="divider">/</span></li><li class="active">
               签收预警设置</li></ul>
        <asp:ScriptManager ID="smForm" runat="server" EnableScriptGlobalization="True"/>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnApply" /> 
            </Triggers>
            <ContentTemplate> 

              <div class="popover popover-static">
                  <h3 class="popover-title">签收预警设置</h3>
                      <div class="popover-content">
                         <table  class="table noborder table-hover"> 
                                <tr>
                                  <td>
                                       <asp:Label ID="lblOrganName" runat="server"></asp:Label>
                                  </td>
                              </tr> 
                               <tr>
                                  <td>
                                       <a> 工资发放后：</a><asp:TextBox ID="txtValue" runat="server"></asp:TextBox><a>   天未签收提醒</a>
                                  </td>
                              </tr> 
                          </table>
                        &nbsp;<hr />
                        <table class="table noborder">
                          <tr>
                            <td class="center">
                                <asp:Button ID="btnApply" CssClass="btn btn-primary" Text="确 定" onclick="btnApply_Click" runat="server"/>
                                <input type="submit" name="btnReturn" value="返 回" onclick="location.href='InventoryWaring_List.aspx;return false;" id="btnReturn" class="btn" />
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

