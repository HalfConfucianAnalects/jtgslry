<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSalarySet_Edit.aspx.cs" Inherits="JtgTMS.PersonSalary.UserSalarySet_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
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
            $(document).ready(function () {
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

            <ul class="breadcrumb" style="display: none">
                <li>工资管理 <span class="divider">/</span></li>
                <li>字段设置 <span class="divider">/</span></li>
                <li class="active">字段设置</li>
            </ul>
            <asp:ScriptManager ID="smForm" runat="server" EnableScriptGlobalization="True" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnApply" />
                </Triggers>
                <ContentTemplate>

                    <div class="popover popover-static">
                        <h3 class="popover-title">字段设置信息</h3>
                        <div class="popover-content">

                            <table class="table noborder table-hover">
                                <tr>
                                    <td width="100">生效月份：</td>
                                    <td width="250">
                                        <asp:TextBox ID="txtBeginYears" MaxLength="64" onClick="WdatePicker({dateFmt:'yyyyMM'})" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvUserSalarySetName" ControlToValidate="txtBeginYears" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator>
                                    </td>
                                    <td width="100">失效月份：</td>
                                    <td width="250">
                                        <asp:TextBox ID="txtEndYears" MaxLength="64" onClick="WdatePicker({dateFmt:'yyyyMM'})" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtEndYears" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator>
                                    </td>
                                    <td></td>
                                </tr>

                                <tr>
                                    <td>备注：</td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDescription" TextMode="MultiLine" MaxLength="256" Width="500" runat="server"></asp:TextBox>
                                    </td>
                                </tr>

                            </table>
                        </div>
                    </div>
                    <div class="popover popover-static">
                        <h3 class="popover-title">操作权限</h3>
                        <div class="popover-content">
                            <asp:GridView ID="gvLists" name="contents" runat="server" CssClass="table table-bordered info table-hover"
                                GridLines="none" ShowFooter="true" AutoGenerateColumns="False" DataKeyNames="ID,TableRecGuid"
                                OnRowCommand="gvLists_RowCommand">
                                <HeaderStyle CssClass="info thead" />
                                <EmptyDataTemplate>
                                    字段设置为空。
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="字段名">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTableRecGuid" Text='<%# Eval("TableRecGuid").ToString()%>' Visible="false"
                                                runat="server"></asp:Label>
                                            <asp:Label ID="lblID" Text='<%# Eval("ID").ToString()%>' Visible="false" runat="server"></asp:Label>
                                            <asp:Label ID="lblFieldName" Text='<%# Eval("FieldName").ToString()%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="6%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="数据类型">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFieldType" Visible="false" Text='<%# Eval("FieldType").ToString()%>' runat="server"></asp:Label>
                                            <%# Eval("FieldType").ToString() == "1" ? "文本":"金额"%>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="字段标题">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtUserFieldTitle" Text='<%# Eval("UserFieldTitle").ToString()%>' runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="是否显示">
                                        <HeaderTemplate>
                                            <input type="checkbox" id="CheckAll" name="CheckAll" onclick="selectRows(document.getElementById('gvLists'), this.checked);"
                                                title="全选/全不选" />是否显示
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsVisible" name="IDsCollection" Checked='<%# Eval("UserIsVisible").ToString() == "1" ? true : false%>' runat="server" ToolTip="选中/取消" />
                                        </ItemTemplate>
                                        <ItemStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDown" Text="删除" CommandName="Down" ToolTip="下移字段"
                                                CommandArgument='<%#Container.DataItemIndex %>'
                                                ImageUrl="/Images/down.png" runat="server" />
                                            <asp:ImageButton ID="btnUp" Text="删除" CommandName="Up" ToolTip="上传字段"
                                                CommandArgument='<%#Container.DataItemIndex %>'
                                                ImageUrl="/Images/up.png" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="center" Width="3%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    <hr />
                    <table class="table noborder">
                        <tr>
                            <td class="center">
                                <asp:Button ID="btnApply" CssClass="btn btn-primary" Text="确 定" OnClick="btnApply_Click" runat="server" />
                                <input type="submit" name="btnReturn" value="返 回" onclick="location.href = 'UserSalarySet_Lst.aspx'; return false;" id="btnReturn" class="btn" />
                            </td>
                        </tr>
                    </table>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>


    </form>
</body>
</html>
