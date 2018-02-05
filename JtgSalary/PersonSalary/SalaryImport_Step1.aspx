<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalaryImport_Step1.aspx.cs" Inherits="JtgTMS.PersonSalary.SalaryImport_Step1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <script language="javascript" src="/SiteFiles/bairong/jquery/jquery-1.8.3.min.js"></script>
    <script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>

    <link rel="stylesheet" type="text/css" href="/SiteFiles/bairong/jquery/bootstrap/css/bootstrap.min.css">
    <script language="javascript" src="/SiteFiles/bairong/jquery/bootstrap/js/bootstrap.min.js"></script>
    
    <script type="text/javascript" src="../scripts/jquery.js"></script>
    <script type="text/javascript" src="../scripts/jquery-ui-1.8.11.custom.js"></script>
    <script type="text/javascript" src="../scripts/jquery.DOMWindow.js"></script>
    
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
    
    <script type="text/javascript">
        function refWindow() {
            document.getElementById("btnInsert").click();
        }

        function PopToolApprovalLog(_this, _id) {
            $(_this).openDOMWindow({
                width: 600,
                height: 470,
                borderSize: 1,
                draggable: 1,
                borderColor: '#6D0413',
                overlayColor: '#ccc',
                overlayOpacity: '60',
                modal: 1,
                windowSource: 'iframe',
                windowSourceURL: '../PersonSalary/ToolApprovalLog_Info.aspx?ID=' + _id.toString(),
                windowPadding: 0
            });
            return false;
        }

        function PopUserInfoChoice(_this) {
            $(_this).openDOMWindow({
                width: 880,
                height: 500,
                borderSize: 1,
                draggable: 1,
                borderColor: '#6D0413',
                overlayColor: '#ccc',
                overlayOpacity: '60',
                modal: 1,
                windowSource: 'iframe',
                windowSourceURL: '../Admin/UserInfo_Choice.aspx',
                windowPadding: 0
            });
            return false;
        }
    </script>
</head>
<body>

<form id="upForm" name="ctl00" class="form-inline" runat="server">
  <ul class="breadcrumb" style="display:none"><li>工资管理 <span class="divider">/</span></li><li>工资导入</ul>

  <script type="text/javascript">
      $(document).ready(function() {
      loopRows(document.getElementById('gvLists'), function(cur) { cur.onclick = chkSelect; });
          $(".popover-hover").popover({ trigger: 'hover', html: true });
      });
  </script>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
               
            </Triggers>
            <ContentTemplate>
                <div class="well well-small">
                
                <table class="table table-noborder">
                  <tr>
                    <td width="70">
                        工资月份：
                    </td>
                    <td width="330">
                        <asp:TextBox ID="txtUserSalaryYears" onClick="WdatePicker({dateFmt:'yyyyMM'})"  Width="120px" runat="server"
                                        ></asp:TextBox>                        
                    </td>
                    <td>
                        
                    </td>
                  </tr>
                  
                </table>
              </div>
           

              <ul class="breadcrumb breadcrumb-button">    
                <asp:Button ID="btnAdd" Text="下一步" CssClass="btn" runat="server" 
                      onclick="btnAdd_Click"  />
                
              </ul>
              
            
  
          </ContentTemplate>
        </asp:UpdatePanel>
<div>	
</div>
<script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>
    </form>
</body>
</html>
