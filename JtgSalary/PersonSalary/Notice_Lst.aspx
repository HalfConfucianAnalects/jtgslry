<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Notice_Lst.aspx.cs" Inherits="JtgTMS.Admin.Notice_Lst" %>

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
    

  <ul class="breadcrumb" style="display:none"><li>系统管理 <span class="divider">/</span></li><li>通知管理 <span class="divider">/</span></li><li class="active"></li></ul>
  

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
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>
        <ContentTemplate>
            <div class="well well-small">
                <table class="table table-noborder">
      
                    <tr>
                        <td>
                            标题关键字：
                            <asp:TextBox ID="txtSearchTitle" MaxLength="500" runat="server"></asp:TextBox>
                            发布时间：
                            <asp:TextBox ID="txtSearchTime" onClick="WdatePicker({dateFmt:'yyyy/MM/dd',onpicked:function(){SalarySelected()}})" MaxLength="6" Width="120px" runat="server"></asp:TextBox>
                            <%--<asp:TextBox ID="txtSearchTime" MaxLength="500" runat="server"></asp:TextBox>--%>
                            发布人：
                            <asp:TextBox ID="txtSearchOpCode" Width="120px" runat="server" AutoPostBack="true"
                                         ontextchanged="txtSearchOpCode_TextChanged"></asp:TextBox>
                            <ajaxToolkit:AutoCompleteExtender ID="txtCostName_AutoCompleteExtender" 
                                                              runat="server" Enabled="True" 
                                                              ServiceMethod="GetCompleteList" ServicePath="../WebService/GetSelfUserLst.asmx" 
                                                              TargetControlID="txtSearchOpCode" CompletionSetCount="100" MinimumPrefixLength="0"
                                                              CompletionInterval="100">
                            </ajaxToolkit:AutoCompleteExtender>
                            <asp:TextBox ID="txtSearchOpName" Width="130px" Enabled="false" runat="server"></asp:TextBox>
                            <asp:TextBox ID="txtSearchUserID" Text="0" Visible="false" runat="server"></asp:TextBox>
          
                            <asp:Button ID="btnSearch" CssClass="btn" Text="搜 索" runat="server" TabIndex="0" onclick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </div>

            <asp:GridView ID="gvLists" name="contents" runat="server" CssClass="table table-bordered info table-hover"  GridLines="none"
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
                        <ItemStyle Width="3%"/>
                    </asp:TemplateField>            
                    <asp:TemplateField HeaderText="通知标题">
                        <ItemTemplate>
                            <a id="A1" href='../PersonSalary/Notice_Info.aspx?ViewType=1&NoticeID=<%# Eval("ID").ToString()%>'>
                                <font style='color:<%# Eval("SelfClickNum").ToString()=="0"?"red":"green"%>'>
                                    <%# Eval("NoticeTitle").ToString()%>
                                </font>
                            </a>
                        </ItemTemplate>
                        <ItemStyle Width="50%" />
                    </asp:TemplateField>               
                    <asp:TemplateField HeaderText="发布时间">
                        <ItemTemplate>
                            <a id="hyEdit" href='../PersonSalary/Notice_Info.aspx?ViewType=1&NoticeID=<%# Eval("ID").ToString()%>'>
                                <font style='color:<%# Eval("SelfClickNum").ToString()=="0"?"red":"green"%>'>
                                    <%# Eval("CreatedTime").ToString()%>
                                </font>
                            </a>
                        </ItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>      
                    <asp:TemplateField HeaderText="发布人">
                        <ItemTemplate>
                            <a id="hyEdit" href='../PersonSalary/Notice_Info.aspx?ViewType=1&NoticeID=<%# Eval("ID").ToString()%>'>
                                <font style='color:<%# Eval("SelfClickNum").ToString()=="0"?"red":"green"%>'>
                                    <%# Eval("OpName").ToString()%>
                                </font>
                            </a>
                        </ItemTemplate>
                        <ItemStyle Width="10%" />
                    </asp:TemplateField>      
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <a id="hyEdit" href='Notice_Edit.aspx?NoticeID=<%# Eval("ID").ToString()%>'>编辑</a>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" Width="6%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <a id="hyEdit" href='Notice_Lst.aspx?DeleteNoticeID=<%# Eval("ID").ToString()%>' onclick="return confirm('此操作将删除所选的通知，确认吗？')">删除</a>
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
                <asp:Button ID="btnAdd" Text="添加通知" CssClass="btn" runat="server" 
                            onclick="btnAdd_Click" />
                <asp:Button ID="btnDelete" CssClass="btn" Text="删 除" OnClientClick="return confirm('此操作将删除所选的通知，确认吗？');" onclick="btnDelete_Click" runat="server" />
            </ul>

        </ContentTemplate>
    </asp:UpdatePanel>
  


<div>
	
</div>
<script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>
    </form>
</body>
</html>
