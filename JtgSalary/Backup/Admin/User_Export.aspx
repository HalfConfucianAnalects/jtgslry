<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Export.aspx.cs" Inherits="JtgTMS.Admin.User_Export" EnableEventValidation="false" %>

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

        function PopSalaryInfo(_this, _id) {
            $(_this).openDOMWindow({
                width: 900,
                height: 600,
                borderSize: 1,
                draggable: 1,
                borderColor: '#6D0413',
                overlayColor: '#ccc',
                overlayOpacity: '60',
                modal: 1,
                windowSource: 'iframe',
                windowSourceURL: '../PersonSalary/UserSalary_Info.aspx?UserSalaryID=' + _id.toString(),
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
  <ul class="breadcrumb" style="display:none"><li>段工资签收 <span class="divider">/</span></li><li>未签收工资</ul>

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
               <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
            <ContentTemplate>
               
              </ContentTemplate>
        </asp:UpdatePanel>
        <ul class="breadcrumb breadcrumb-button">    
                <asp:Button ID="btnExport" Text="导出Excel" CssClass="btn" 
                      runat="server" onclick="btnExport_Click"/>
              </ul>
                <asp:GridView ID="gvLists" name="contents" runat="server" 
      CssClass="table table-bordered info table-hover"  GridLines="none"
                    AutoGenerateColumns="False" DataKeyNames="ID" 
      onrowdatabound="gvLists_RowDataBound">
                    <HeaderStyle CssClass="info thead" />                 
                    <EmptyDataTemplate>
                           工资单为空。
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="">                                          
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle Width="3%"/>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="工号">
                            <ItemTemplate>
                                <%# Eval("OpCode").ToString()%>
                            </ItemTemplate>
                            <HeaderStyle Width="10%"/>
                        </asp:TemplateField>
                                          
                        <asp:TemplateField HeaderText="姓名">
                            <ItemTemplate>
                                <%# Eval("OpName").ToString()%>
                            </ItemTemplate>
                            <ItemStyle Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="单位部门">
                            <ItemTemplate>
                                <%# Eval("OrganName").ToString()%>
                            </ItemTemplate>
                            <ItemStyle Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="性别">
                            <ItemTemplate>
                                <%# Eval("Sex").ToString()=="0"?"男":"女"%>
                            </ItemTemplate>
                            <HeaderStyle Width="5%"/>
                        </asp:TemplateField>      
                        <asp:TemplateField HeaderText="在职状态">
                            <ItemTemplate>
                                <%# Eval("IsCanLogin").ToString()=="1"?"是":"否"%>
                            </ItemTemplate>
                            <ItemStyle Width="10%" />
                        </asp:TemplateField>
                    </Columns>
            </asp:GridView>
              
              
  
          
<div>	
</div>
<script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>
    </form>
</body>
</html>
