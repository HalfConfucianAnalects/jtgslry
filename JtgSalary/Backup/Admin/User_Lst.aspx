<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Lst.aspx.cs" Inherits="JtgTMS.Admin.User_Lst" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta charset="utf-8">
    <script language="javascript" src="/SiteFiles/bairong/jquery/jquery-1.8.3.min.js"></script>
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

<form id="upForm" name="ctl00" class="form-inline" runat="server">
    

  <ul class="breadcrumb" style="display:none"><li>系统管理 <span class="divider">/</span></li><li>用户管理 <span class="divider">/</span></li><li class="active"><% =_OrganName %></li></ul>
  

  <script type="text/javascript">
      $(document).ready(function() {
      loopRows(document.getElementById('gvLists'), function(cur) { cur.onclick = chkSelect; });
          $(".popover-hover").popover({ trigger: 'hover', html: true });
      });
  </script>

  <div class="well well-small">
    <table class="table table-noborder">
      
      <tr>
        <td width="300px">
            关键字：
          <asp:TextBox ID="txtSearchKeyword" MaxLength="500" runat="server"></asp:TextBox>
          </td>
          <td width="100px">
          在职状态：
          </td>
          <td width="100px">
          <asp:DropDownList ID="ddlIsCanLogin" runat="server" Width="100px">
                <asp:ListItem Value="-1" Text="所有"  Selected="True"></asp:ListItem>
                <asp:ListItem Value="1" Text="是" ></asp:ListItem>
                <asp:ListItem Value="0" Text="否"></asp:ListItem>
          </asp:DropDownList>          
         </td>
         <td width="100px">
            <asp:CheckBox ID="chkHasChildren" runat="server" Text="显示子单位" />
          </td>          
         <td>
          <asp:Button ID="btnSearch" CssClass="btn" Text="搜 索" runat="server" TabIndex="0" onclick="btnSearch_Click" />
        </td>
      </tr>
    </table>
  </div>

  <asp:GridView ID="gvLists" name="contents" runat="server" CssClass="table table-bordered info table-hover"  GridLines="none"
        AutoGenerateColumns="False" DataKeyNames="ID" OnRowCommand="gvLists_RowCommand">
        <HeaderStyle CssClass="info thead" />       
        <EmptyDataTemplate>
                【<% =_OrganName %>】下属用户为空。
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
            <asp:TemplateField HeaderText="性别">
                <ItemTemplate>
                    <%# Eval("Sex").ToString()=="0"?"男":"女"%>
                </ItemTemplate>
                <HeaderStyle Width="5%"/>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="手机">
                <ItemTemplate>
                    <%# Eval("Phone").ToString()%>
                </ItemTemplate>
                <HeaderStyle Width="10%"/>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="名称">
                <ItemTemplate>
                    <%# Eval("OpName").ToString()%>
                </ItemTemplate>
                <ItemStyle Width="30%" />
            </asp:TemplateField>  
            <asp:TemplateField HeaderText="在职状态">
                <ItemTemplate>
                    <%# Eval("IsCanLogin").ToString()=="1"?"是":"否"%>
                </ItemTemplate>
                <ItemStyle Width="10%" />
            </asp:TemplateField>                                                         
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <a id="hyEdit" href='../Admin/User_Edit.aspx?UserID=<%# Eval("ID").ToString()%>&OrganID=<%# Eval("OrganID").ToString()%>'>编辑</a>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="6%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <a id="hyEdit" href='../Admin/User_Lst.aspx?DeleteUserID=<%# Eval("ID").ToString()%>&OrganID=<%# Eval("OrganID").ToString()%>' onclick="return confirm('此操作将删除所选的用户，确认吗？')">删除</a>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="6%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:Button ID="btnRestPassword" CommandName="RestPassword" CommandArgument='<%# Eval("ID").ToString()%>' OnClientClick="return confirm('此操作将重置当前用户密码为初始值，确认吗？')" Text="重置密码" runat="server" />
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="6%" />
            </asp:TemplateField>
             <asp:TemplateField HeaderText="">
            <HeaderTemplate>
                <input type="checkbox" id="CheckAll" name="CheckAll" onclick="selectRows(document.getElementById('gvLists'), this.checked);" title="全选/全不选"/> 
            </HeaderTemplate>
            <ItemTemplate>               
                <asp:CheckBox ID="CheckRow" name="IDsCollection" runat="server" ToolTip="选中/取消" />
            </ItemTemplate>
            <ItemStyle Width="3%"/>
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

  <ul class="breadcrumb breadcrumb-button">    
    <asp:Button ID="btnAdd" Text="添加用户" CssClass="btn" runat="server" 
          onclick="btnAdd_Click" />
    <asp:Button ID="btnImport" Text="导入用户" CssClass="btn" runat="server" 
          onclick="btnImport_Click"/>
  <asp:Button ID="btnExport" Text="导出用户" CssClass="btn" runat="server" 
      onclick="btnExport_Click"/>
    <asp:Button ID="btnDelete" CssClass="btn" Text="删 除" OnClientClick="return confirm('此操作将删除所选的用户吗，确认吗？');" onclick="btnDelete_Click" runat="server" />
  </ul>


<div>
	
</div>
<script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>
    </form>
</body>
</html>
