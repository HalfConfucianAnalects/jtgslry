<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSalary_Edit.aspx.cs"
    Inherits="JtgTMS.PersonSalary.UserSalary_Edit" %>
<%@ Register Src="../SalaryControl/SalaryEdit.ascx" TagName="SalaryEdit" TagPrefix="ucSalaryEdit" %>

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

        function UpdateSuccess() {
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
        <ul class="breadcrumb" style="display:none">
            <li>段工资签收 <span class="divider">/</span></li><li>
            </ul>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnApply" />
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
                <asp:Button ID="btnChoiceUser" Style="display: none" runat="server" Text="insert" CausesValidation="false"
                    OnClick="btnChoiceUser_Click" />
                <div class="popover popover-static">
                    <h3 class="popover-title">
                        工资单信息</h3>
                    <div class="popover-content">
                        <table class="table noborder table-hover">                           
                            <tr>
                                <td width="120px">
                                    工资年月：
                                </td>
                                <td width="250px">
                                    <asp:TextBox ID="txtUserSalaryYears" onClick="WdatePicker({dateFmt:'yyyyMM'})" AutoPostBack="true"
                                        runat="server" ontextchanged="txtUserSalaryYears_TextChanged"></asp:TextBox>
                                </td>
                                <td width="100px">
                                    姓名：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUserSalaryOpCode" Width="60px" runat="server" AutoPostBack="true"
                                        ontextchanged="txtUserSalaryOpCode_TextChanged"></asp:TextBox>
                                    <ajaxToolkit:AutoCompleteExtender ID="txtCostName_AutoCompleteExtender" 
                                                runat="server" Enabled="True" 
                                                ServiceMethod="GetCompleteList" ServicePath="../WebService/GetSelfUserLst.asmx" 
                                                TargetControlID="txtUserSalaryOpCode" CompletionSetCount="100" MinimumPrefixLength="0"
                                                CompletionInterval="100">
                                            </ajaxToolkit:AutoCompleteExtender>
                                    <asp:TextBox ID="txtUserSalaryOpName" Width="130px" Enabled="false" runat="server"></asp:TextBox>
                                    <asp:TextBox ID="txtUserSalaryUserID" Text="0" Visible="false" runat="server"></asp:TextBox>
                                    <asp:ImageButton ID="btnSearchOpCode" ImageUrl="../pic/icon/help.gif" OnClientClick="return PopUserInfoChoice(this);"
                                        runat="server" />
                                </td>
                            </tr>                            
                        </table>
                    </div>
                    <div class="popover-content">
                        <h3 class="popover-title">
                            领用清单</h3>
                        <asp:DataList ID="dlList" name="contents" runat="server" RepeatColumns="2"  Width="100%" RepeatDirection="Horizontal"
                            GridLines="Both">
                            <ItemTemplate>
                                <table width="100%" height="32" class="table noborder table-hover">
                                    <tr>
                                        <td width="220px" ><%# Eval("UserFieldTitle").ToString() != "" ? Eval("UserFieldTitle").ToString() : Eval("FieldTitle").ToString()%></td>
                                        <td width="160px" valign="top" align="left"><ucSalaryEdit:SalaryEdit ID="SalaryEdit2" UserFieldType='<%# Eval("FieldType").ToString() %>' UserFieldName='<%# Eval("FieldName").ToString() %>' runat="server" />    </td>
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
                                    <asp:TextBox ID="txtDescription" TextMode="MultiLine" MaxLength="256" Width="558px" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button ID="btnApply" CssClass="btn btn-primary" Text="确 定" OnClick="btnApply_Click"
                                runat="server" />
                            <input type="submit" name="btnReturn" visible="false" style="display:none" onclick="location.href='<% =_ReturnPage %>';return false;"
                                id="btnReturn" class="btn" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
