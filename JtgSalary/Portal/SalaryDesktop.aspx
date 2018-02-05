<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalaryDesktop.aspx.cs" Inherits="JtgTMS.Platform.SalaryDesktop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <script language="javascript" src="/SiteFiles/bairong/jquery/jquery-1.8.3.min.js"></script>
    <script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>

    <link rel="stylesheet" type="text/css" href="/SiteFiles/bairong/jquery/bootstrap/css/bootstrap.min.css">
    <script language="javascript" src="/SiteFiles/bairong/jquery/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        (function ($) {
            $(document).ready(function () {
                if ($.isFunction($.bootstrapIE6)) $.bootstrapIE6($(document));
            });
        })(jQuery);
    </script>

    <link rel="stylesheet" href="../inc/style.css" type="text/css" />
    <script language="javascript" src="../inc/script.js"></script>

    <script type="text/javascript" src="../scripts/jquery.js"></script>
    <script type="text/javascript" src="../scripts/jquery-ui-1.8.11.custom.js"></script>
    <script type="text/javascript" src="../scripts/jquery.DOMWindow.js"></script>

    <script type="text/javascript">
        function refWindow() {
            window.location = "../Portal/SalaryDesktop.aspx";
        }

        function refSalaryWindow() {
        }

        function PopSignEdit(_this, _id) {
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
                windowSourceURL: '../PersonSalary/SalarySign_Edit.aspx?IDs=' + _id.toString(),
                windowPadding: 0
            });
            return false;
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

        function SalarySelected() {
            document.getElementById("btnSalarys").click();
        }
    </script>
</head>
<body>
    <form class="form-inline" runat="server">

        <div class="well well-small">
            <table class="table table-noborder">
                <tr>
                    <td>
                        <input type="text" style="display: none" onchange="alert(this.value);">
                        欢迎使用 上海动车段 工资电子签收系统。 <a style="font-size: large; color: Red" href="../upload/chrome_installer.exe">如使用IE6，请下载谷歌浏览器安装使用</a>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Button ID="btnSalarys" Style="display: none" runat="server" Text="insert" CausesValidation="false"
            OnClick="btnSalarys_Click" />
        <table class="table noborder table-hover">
            <tr style="display: none">
                <td width="50%">
                    <div class="popover popover-static">
                        <h3 class="popover-title" style="width: auto">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td height="30">未签收的工资单</td>
                                    <td align="right" width="60"><a href="../PersonSalary/MyNotSignUserSalary_Lst.aspx">更多</a></td>
                                </tr>
                            </table>
                        </h3>
                        <div class="popover-content">
                            <asp:GridView ID="gvLists" name="contents" runat="server" CssClass="table noborder table-hover"
                                GridLines="none" AutoGenerateColumns="False" DataKeyNames="ID" OnRowDataBound="gvLists_RowDataBound">
                                <HeaderStyle CssClass="info thead" />
                                <EmptyDataTemplate>
                                    未签收的工资单为空。
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <%# Eval("ID").ToString().Length > 0? (Container.DataItemIndex+1).ToString() :"" %>&nbsp;
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="工资年月">
                                        <ItemTemplate>
                                            <%# Eval("SalaryYears").ToString()%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="发放日期">
                                        <ItemTemplate>
                                            <%# Eval("SalaryDate").ToString().Length > 0 ? DateTime.Parse(Eval("SalaryDate").ToString()).ToString("yyyy-MM-dd") : ""%>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="发放金额">
                                        <ItemTemplate>
                                            <%# Eval("TotalSalary").ToString()%>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="hyView" OnClientClick='<%#"return PopSalaryInfo(this, " + Eval("ID").ToString()+");"%>' Text="查看" Visible='<%# Eval("ID").ToString().Length > 0? true :false %>' runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="hySign" OnClientClick='<%#"return PopSignEdit(this, " + Eval("ID").ToString()+");"%>' Text="签收" Visible='<%# Eval("ID").ToString().Length > 0? true :false %>' runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" Width="8%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
                <td width="50%">
                    <div class="popover popover-static">
                        <h3 class="popover-title">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>已签收的工资单</td>
                                    <td></td>
                                    <td align="right" width="160">
                                        <asp:TextBox ID="txtMonth" MaxLength="64" Width="100" AutoPostBack="true" onkeyup="refSalaryWindow()" onchange="refSalaryWindow()"
                                            onClick="WdatePicker({dateFmt:'yyyyMM',onpicked:function(){SalarySelected()}})" runat="server"
                                            OnTextChanged="txtMonth_TextChanged">

                                        </asp:TextBox>
                                        &nbsp;<a href="../PersonSalary/MySignUserSalary_Lst.aspx">更多</a>

                                    </td>
                                </tr>
                            </table>
                        </h3>
                        <div class="popover-content">
                            <asp:GridView ID="gvBorrowLists" name="contents" runat="server" CssClass="table noborder table-hover"
                                GridLines="none" AutoGenerateColumns="False" DataKeyNames="ID" OnRowDataBound="gvLists_RowDataBound">
                                <HeaderStyle CssClass="info thead" />
                                <EmptyDataTemplate>
                                    已签收的工资单为空。
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <%# Eval("ID").ToString().Length > 0? (Container.DataItemIndex+1).ToString() :"" %>&nbsp;
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="工资年月">
                                        <ItemTemplate>
                                            <%# Eval("SalaryYears").ToString()%>
                                        </ItemTemplate>
                                        <HeaderStyle Width="15%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="发放日期">
                                        <ItemTemplate>
                                            <%# Eval("SalaryDate").ToString().Length > 0 ? DateTime.Parse(Eval("SalaryDate").ToString()).ToString("yyyy-MM-dd") : ""%>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="发放金额">
                                        <ItemTemplate>
                                            <%# Eval("TotalSalary").ToString()%>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="签收时间">
                                        <ItemTemplate>
                                            <%# Eval("SignDate").ToString()%>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="hyView" OnClientClick='<%#"return PopSalaryInfo(this, " + Eval("ID").ToString()+");"%>' Visible='<%# Eval("ID").ToString().Length > 0? true :false %>' Text="查看" runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" Width="6%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                </td>
            </tr>
            <tr id="trApprovaling" runat="server">
                <td width="100%" colspan="1" valign="top">
                    <div class="popover popover-static">
                        <h3 class="popover-title">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>通知公告</td>
                                    <td align="right" width="60">&nbsp;<a href="../PersonSalary/MyNotice_Lst.aspx">更多</a></td>
                                </tr>
                            </table>
                        </h3>
                        <div class="popover-content">
                            <asp:GridView ID="gvNotice" name="contents" runat="server" CssClass="table noborder table-hover" ShowHeader="false"
                                GridLines="none" AutoGenerateColumns="False" DataKeyNames="ID">
                                <HeaderStyle CssClass="info thead" />
                                <EmptyDataTemplate>
                                    通知公告为空。
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <%# Eval("ID").ToString().Length > 0? (Container.DataItemIndex+1).ToString() :"" %>&nbsp;
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="通知标题">
                                        <ItemTemplate>
                                            <a id="hyEdit" href='../PersonSalary/Notice_Info.aspx?ViewType=2&NoticeID=<%# Eval("ID").ToString()%>'>
                                                <font style='color: <%# Eval("SelfClickNum").ToString()=="0"?"red":"green"%>'>
                                                    <%# Eval("NoticeTitle").ToString()%>
                                                </font>
                                            </a>
                                        </ItemTemplate>
                                        <ItemStyle Width="50%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="发布时间">
                                        <ItemTemplate>
                                            <a id="hyEdit" href='../PersonSalary/Notice_Info.aspx?ViewType=2&NoticeID=<%# Eval("ID").ToString()%>'>
                                                <font style='color: <%# Eval("SelfClickNum").ToString()=="0"?"red":"green"%>'>
                                                    <%# Eval("CreatedTime").ToString()%>
                                                </font>
                                            </a>
                                        </ItemTemplate>
                                        <ItemStyle Width="30%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="发布人">
                                        <ItemTemplate>
                                            <a id="hyEdit" href='../PersonSalary/Notice_Info.aspx?ViewType=2&NoticeID=<%# Eval("ID").ToString()%>'>
                                                <font style='color: <%# Eval("SelfClickNum").ToString()=="0"?"red":"green"%>'>
                                                    <%# Eval("OpName").ToString()%>
                                                </font>
                                            </a>
                                        </ItemTemplate>
                                        <ItemStyle Width="15%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td width="100%" colspan="1">
                    <div class="popover popover-static">
                        <h3 class="popover-title">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>登录信息</td>
                                </tr>
                            </table>
                        </h3>
                        <div class="popover-content">
                            <table class="table noborder table-hover" style="color: Red">
                                <tr style="display: none">
                                    <td width="150">
                                        <lan>当前登记用户</lan>
                                        ：</td>
                                    <td>
                                        <asp:Literal ID="ltlUserName" runat="server" />
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td>
                                        <lan>最近登录机构</lan>
                                        ：</td>
                                    <td>
                                        <asp:Literal ID="ltlOrganName" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="150">
                                        <lan>最近登录IP</lan>
                                        ：</td>
                                    <td>

                                        <asp:Literal ID="ltlLastLoginIP" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <lan>最近登录计算机</lan>
                                        ：</td>
                                    <td>
                                        <asp:Literal ID="LtlComputerName" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <lan>上次登录时间</lan>
                                        ：</td>
                                    <td>
                                        <asp:Literal ID="ltlLastLoginDate" runat="server"></asp:Literal>
                                    </td>
                                </tr>
                            </table>

                        </div>
                    </div>
                </td>
            </tr>
        </table>

    </form>
    <script type="text/javascript">
        if (window.top.location.href.toLowerCase().indexOf("main.aspx") == -1) {
            var initializationUrl = window.top.location.href.toLowerCase().substring(0, window.top.location.href.toLowerCase().indexOf("/siteserver/")) + "/siteserver/initialization.aspx";
            window.top.location.href = initializationUrl;
            console.log("Test Test ======= " + window.top.location.href);
        }
    </script>
</body>
</html>
