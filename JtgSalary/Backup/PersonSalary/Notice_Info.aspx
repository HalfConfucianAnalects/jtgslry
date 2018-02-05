<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Notice_Info.aspx.cs" Inherits="JtgTMS.Admin.Notice_Info" EnableEventValidation="false" ValidateRequest="false"%>

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
    <style type="text/css">
        .style1
        {
            height: 29px;
        }
    </style>
     
</head>
<body>
    <form id="upForm" class="form-inline" runat="server">
   
   <div>
   
       <ul class="breadcrumb" style="display:none"></span></li><li>通知管理 <span class="divider">/</span></li><li class="active">通知</li></ul>

              <div class="popover popover-static">
                  <h3 class="popover-title">通知</h3>
                      <div class="popover-content">

                        <table class="table noborder table-hover">                                
                          <tr>
                            <td>通知标题：</td>
                            <td>
                                <asp:Label ID="txtNoticeTitle" MaxLength="128" Width="400" runat="server"></asp:Label>
                              </td>
                          </tr>
                          
                              <tr>
                            <td>通知内容：</td>
                            <td>
                                <asp:Literal ID="txtContent" runat="server"></asp:Literal>                                
                            </td>
                          </tr>
                          <tr>
                                        <td colspan="4">
                                            <div class="field">
                                                <li>
		                                            <label class="FieldLabel">上传附件<b>*</b></label>
		                                            
                                                    <table id="tbFiles" runat="server" width="500px" enableviewstate="true" class="styleTable4">
                                                        <tr>
                                                            <td>
                                                                <asp:Literal ID="ltDownloadFiles" runat="server"></asp:Literal>        
                                                            </td>
                                                        </tr>
                                                        
                                                    </table>
                                                </li>
	                                        </div>	  
                                        </td>
                                    </tr>
                          
                        </table>
                      
                        <hr />
                        <table class="table noborder">
                          <tr>
                            <td class="center">
                                <asp:Button ID="btnApply" CssClass="btn btn-primary" Text="返 回" onclick="btnCancel_Click" runat="server"/>
                            </td>
                          </tr>
                        </table>
                  
                    </div>
                   
               </div>
       
              
   </div>
   
   
    </form>
</body>
</html>
