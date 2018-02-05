<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalaryNotSign_Warming.aspx.cs" Inherits="JtgTMS.WarmingSalary.SalaryNotSign_Warming" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
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
            $(document).ready(function () {
                if ($.isFunction($.bootstrapIE6)) $.bootstrapIE6($(document));
            });
        })(jQuery);
    </script>

    <!--[if lt IE 9]><script src="/SiteFiles/bairong/jquery/html5shiv/html5shiv.js"></script><![endif]-->

    <link rel="stylesheet" href="../inc/style.css" type="text/css" />
    <script language="javascript" src="../inc/script.js"></script>

    <script type="text/javascript">

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

        function SalarySelected() {
            document.getElementById("btnSalarys").click();
        }

        function SalarySelected2() {
            document.getElementById("btnSalarys2").click();
        }
        function refUserWindow() {
            document.getElementById("btnChoiceUser").click();
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</head>
<body>

    <form id="upForm" name="ctl00" class="form-inline" runat="server">
        <ul class="breadcrumb" style="display: none">
            <li>段工资签收 <span class="divider">/</span></li>
            <li>
            工具领用领用
        </ul>

        <script type="text/javascript">
            $(document).ready(function () {
                loopRows(document.getElementById('gvLists'), function (cur) { cur.onclick = chkSelect; });
                $(".popover-hover").popover({ trigger: 'hover', html: true });
            });
        </script>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="well well-small">
                    <table class="table table-noborder">
                        <tr>
                            <td width="60">部门：</td>
                            <td>
                                <asp:TextBox ID="txtDepartmentName" Width="120px" runat="server" AutoPostBack="true"></asp:TextBox>
                            </td>
                            <td>导入时间起始：
                                <asp:TextBox ID="txtSalaryDateStart" onClick="WdatePicker({dateFmt:'yyyy-MM-dd',onpicked:function(){SalarySelected()}})" MaxLength="6" Width="120px" runat="server"></asp:TextBox>
                                <asp:DropDownList ID="ddlImportRec" Width="150" runat="server">
                                    <asp:ListItem Value="0" Text="批次"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:Button ID="btnSearch" CssClass="btn" Text="搜 索" runat="server" TabIndex="0" OnClick="btnSearch_Click" />
                            </td>
                        </tr>

                        <tr>
                            <td width="70"></td>
                            <td width="150"></td>
                            <td>导入时间结束：
                                <asp:TextBox ID="txtSalaryDateEnd" onClick="WdatePicker({dateFmt:'yyyy-MM-dd',onpicked:function(){SalarySelected2()}})" MaxLength="6" Width="120px" runat="server"></asp:TextBox>
                                <asp:DropDownList ID="ddlImportRec2" Width="150" runat="server">
                                    <asp:ListItem Value="0" Text="批次"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>

                <asp:GridView ID="gvLists" name="contents" runat="server" CssClass="table table-bordered info table-hover" FooterStyle-ForeColor="Red" GridLines="none"
                    AutoGenerateColumns="False" OnRowDataBound="gvLists_RowDataBound">
                    <HeaderStyle CssClass="info thead" />
                    <EmptyDataTemplate>
                        工资单为空。
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle Width="5%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="车间">
                            <ItemTemplate>
                                <asp:Label ID="lblWorkShopName" runat="server" Text='<%# Eval("WorkShopName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="部门">
                            <ItemTemplate>
                                <asp:Label ID="lblDepartmentName" runat="server" Text='<%# Eval("DepartmentName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="导入时间">
                            <ItemTemplate>
                                <asp:Label ID="lblSalaryDate" runat="server" Text='<%# Eval("SalaryDateOnly") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="人数">
                            <ItemTemplate>
                                <asp:Label ID="lblPersonNum" runat="server" Text='<%# Eval("PersonNum") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <a id="WarmingDetail" href='../WarmingSalary/SalaryNotSign_WarmingDetail.aspx?SalaryYears=<%# Eval("SalaryYears").ToString()%>&OrganID=<%# Eval("OrganID").ToString()%>'>详细</a>
                                <%--<asp:LinkButton ID="hyView" OnClientClick='<%#"return PopSalaryInfo(this, " + Eval("PersonNum").ToString()+");"%>' Text="详细" runat="server"></asp:LinkButton>--%>
                            </ItemTemplate>
                            <ItemStyle CssClass="center" Width="5%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <table runat="server" class="table table-noborder" border="0">
                    <tr>
                        <td align="left">
                            <div id="PageInfo" runat="server" class="table table-pager">
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
        </div>
        <script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>
    </form>

</body>
</html>
