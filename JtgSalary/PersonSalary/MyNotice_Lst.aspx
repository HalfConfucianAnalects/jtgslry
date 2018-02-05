<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyNotice_Lst.aspx.cs" Inherits="JtgTMS.Admin.MyNotice_Lst" %>

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
</head>
<body>

    <form id="upForm" name="ctl00" class="form-inline" runat="server">
        <script type="text/javascript">
            $(document).ready(function () {
                loopRows(document.getElementById('gvLists'), function (cur) { cur.onclick = chkSelect; });
                $(".popover-hover").popover({ trigger: 'hover', html: true });
            });
        </script>

        <div class="well well-small">
            <table class="table table-noborder">

                <tr>
                    <td>关键字：
                        <asp:TextBox ID="txtSearchKeyword" MaxLength="500" runat="server"></asp:TextBox>
                        <asp:Button ID="btnSearch" CssClass="btn" Text="搜 索" runat="server" TabIndex="0" OnClick="btnSearch_Click" />
                    </td>
                </tr>
            </table>
        </div>

        <asp:GridView ID="gvLists" name="contents" runat="server" CssClass="table table-bordered info table-hover" GridLines="none"
            AutoGenerateColumns="False" DataKeyNames="ID" OnRowDataBound="gvLists_RowDataBound">
            <HeaderStyle CssClass="info thead" />
            <EmptyDataTemplate>
                通知为空。
            </EmptyDataTemplate>
            <Columns>

                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                    <ItemStyle Width="3%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="通知标题">
                    <ItemTemplate>
                        <a id="A1" href='../PersonSalary/Notice_Info.aspx?ViewType=0&NoticeID=<%# Eval("ID").ToString()%>'>
                            <font style='color: <%# Eval("SelfClickNum").ToString()=="0"?"red":"green"%>'>
                                <%# Eval("NoticeTitle").ToString()%>
                            </font>
                        </a>
                    </ItemTemplate>
                    <ItemStyle Width="50%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="发布时间">
                    <ItemTemplate>
                        <a id="hyEdit" href='../PersonSalary/Notice_Info.aspx?ViewType=0&NoticeID=<%# Eval("ID").ToString()%>'>
                            <font style='color: <%# Eval("SelfClickNum").ToString()=="0"?"red":"green"%>'>
                                <%# Eval("CreatedTime").ToString()%>
                            </font>
                        </a>
                    </ItemTemplate>
                    <ItemStyle Width="25%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="发布人">
                    <ItemTemplate>
                        <a id="hyEdit" href='../PersonSalary/Notice_Info.aspx?ViewType=0&NoticeID=<%# Eval("ID").ToString()%>'>
                            <font style='color: <%# Eval("SelfClickNum").ToString()=="0"?"red":"green"%>'>
                                <%# Eval("OpName").ToString()%>
                            </font>
                        </a>
                    </ItemTemplate>
                    <ItemStyle Width="10%" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <table runat="server" class="table table-noborder" border="0">
            <tr runat="server">
                <td align="left">
                    <div id="PageInfo" runat="server" class="table table-pager">
                    </div>
                </td>
            </tr>
        </table>


        <div>
        </div>
        <script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>
    </form>
</body>
</html>
