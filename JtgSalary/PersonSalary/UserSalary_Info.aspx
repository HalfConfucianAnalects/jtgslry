<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSalary_Info.aspx.cs"
    Inherits="JtgTMS.PersonSalary.UserSalary_Info" %>
<%@ Register Src="../SalaryControl/SalaryInfo.ascx" TagName="SalaryInfo" TagPrefix="ucSalaryInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">

    <script language="javascript" src="/SiteFiles/bairong/jquery/jquery-1.9.1.min.js"></script>

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
        (function($) {
            $(document).ready(function() {
                if ($.isFunction($.bootstrapIE6)) $.bootstrapIE6($(document));
            });
        })(jQuery);
    </script>

    <!--[if lt IE 9]><script src="/SiteFiles/bairong/jquery/html5shiv/html5shiv.js"></script><![endif]-->
    <link rel="stylesheet" href="../inc/style.css" type="text/css" />

    <script language="javascript" src="../inc/script.js"></script>

    <script type="text/javascript">

        function refUserWindow() {
            document.getElementById("btnChoiceUser").click();
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
                windowSourceURL: '../Admin/UserInfo_SelfChoice.aspx',
                windowPadding: 0
            });
            return false;
        }

        function closeWindow() {
            var dom = window.parent.document.getElementById('DOMWindow'),
			    bg = window.parent.document.getElementById('DOMWindowOverlay');
            
            parent.refSalaryWindow();
            
            dom.parentNode.removeChild(dom);
            bg.parentNode.removeChild(bg);
        }
    </script>

</head>
<body>
    <form id="upForm" class="form-inline" runat="server">
    <div>
      
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
            
            </Triggers>
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="2">
                            <div class="modal-header" style="height:30px;">
                                <button type="button" class="close" id="closeDom" data-dismiss="modal" onclick="closeWindow()" aria-hidden="true"><img src="../../sitefiles/bairong/icons/close.png" /></button>            
                                <h3>工资条明细</h3>  
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="10px" align="center">
                        </td>
                    </tr>
                </table>
                
                    <div class="popover-content">
                        <table class="table noborder table-hover">                           
                            <tr>
                                <td width="120px">
                                    工资年月：
                                </td>
                                <td width="150px" align="left">
                                    <asp:Label ID="txtUserSalaryYears" runat="server"></asp:Label>
                                </td>
                                <td width="100px">
                                    工号：
                                </td>
                                <td width="150px"  align="left">
                                    <asp:Label ID="txtUserSalaryOpCode" runat="server" ></asp:Label>
                                    
                                </td>
                                <td width="100px">
                                    姓名：
                                </td>
                                <td  align="left">
                                    <asp:Label ID="txtUserSalaryOpName" Enabled="false" runat="server"></asp:Label>
                                </td>
                            </tr>                            
                        </table>
                        
                        <asp:DataList ID="dlList" name="contents" runat="server" RepeatColumns="2"  Width="100%" RepeatDirection="Horizontal"
                            GridLines="None">
                            <ItemTemplate>
                                <table width="100%" height="32" class="table noborder table-hover">
                                    <tr>
                                        <td width="220px" ><%# Eval("UserFieldTitle").ToString() != "" ? Eval("UserFieldTitle").ToString() : Eval("FieldTitle").ToString()%></td>
                                        <td width="160px" valign="top" align="left"><ucSalaryInfo:SalaryInfo ID="SalaryInfo1" UserFieldType='<%# Eval("FieldType").ToString() %>' UserFieldName='<%# Eval("FieldName").ToString() %>' runat="server" />    </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                        
                        <div id="PageInfo" visible="false" runat="server" class="table table-pager">
                        </div>
                    </div>
                    <div class="popover-content">
                        <table class="table noborder table-hover">
                            <tr>
                                <td width="100px">
                                    备注：
                                </td>
                                <td>
                                    <asp:Label ID="txtDescription" TextMode="MultiLine" MaxLength="256" Width="558px" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
