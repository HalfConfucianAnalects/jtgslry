<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserImportRec_Lst.aspx.cs" Inherits="JtgTMS.Admin.UserImportRec_Lst" %>

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
    
            <script type="text/javascript" src="../scripts/jquery.js"></script>
    <script type="text/javascript" src="../scripts/jquery-ui-1.8.11.custom.js"></script>
    <script type="text/javascript" src="../scripts/jquery.DOMWindow.js"></script>
    
    <script type="text/javascript">
        function refWindow() {

            window.location = "../PersonSalary/UserImportRec_Lst.aspx";
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
                windowSourceURL: '../PersonSalary/UserImportRecAudit_Edit.aspx?IDs=' + _id.toString(),
                windowPadding: 0
            });
            return false;
        }

        function PopDescriptionEdit(_this, _id) {
            $(_this).openDOMWindow({
                width: 600,
                height: 330,
                borderSize: 1,
                draggable: 1,
                borderColor: '#6D0413',
                overlayColor: '#ccc',
                overlayOpacity: '60',
                modal: 1,
                windowSource: 'iframe',
                windowSourceURL: '../PersonSalary/UserImportRecDescription_Edit.aspx?IDs=' + _id.toString(),
                windowPadding: 0
            });
            return false;
        }        

        function SalarySelected() {
            document.getElementById("btnSalarys").click();
            //alert(document.getElementById("txtSalaryYears").value);
        }
    </script>
</head>
<body>

<form id="upForm" name="ctl00" class="form-inline" runat="server">
    

  <ul class="breadcrumb" style="display:none"><li>系统管理 <span class="divider">/</span></li><li>用户管理 <span class="divider">/</span></li><li class="active">导入记录</li></ul>
  

  <script type="text/javascript">
      $(document).ready(function() {
      loopRows(document.getElementById('gvLists'), function(cur) { cur.onclick = chkSelect; });
          $(".popover-hover").popover({ trigger: 'hover', html: true });
      });
  </script>

  <div class="well well-small">
    <table class="table table-noborder">
      
      <tr>
        <td>
            关键字：
          <asp:TextBox ID="txtUserSalaryYears" onClick="WdatePicker({dateFmt:'yyyyMM'})"  Width="120px" runat="server"
                                        ></asp:TextBox>
          
          <asp:Button ID="btnSearch" CssClass="btn" Text="搜 索" runat="server" TabIndex="0" onclick="btnSearch_Click" />
        </td>
      </tr>
    </table>
  </div>

  <asp:GridView ID="gvLists" name="contents" runat="server" CssClass="table table-bordered info table-hover"  GridLines="none"
        AutoGenerateColumns="False" DataKeyNames="ID" OnRowDataBound="gvLists_RowDataBound">
        <HeaderStyle CssClass="info thead" />                 
        <EmptyDataTemplate>
                导入记录为空。
        </EmptyDataTemplate>
        <Columns>
       
            <asp:TemplateField HeaderText="">                                          
                <ItemTemplate>
                    <%#Container.DataItemIndex+1 %>
                </ItemTemplate>
                <ItemStyle Width="3%"/>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="导入年月">
                <ItemTemplate>
                    <%# Eval("SalaryYears").ToString()%>
                </ItemTemplate>
                <ItemStyle Width="10%" />
            </asp:TemplateField>      
            <asp:TemplateField HeaderText="导入时间">
                <ItemTemplate>
                    <%# Eval("SalaryDate").ToString()%>
                </ItemTemplate>
                <ItemStyle Width="20%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="备注">
                <ItemTemplate>
                    <%# Eval("Description").ToString()%>
                </ItemTemplate>
                <ItemStyle Width="40%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:LinkButton ID="hySi1gn"  OnClientClick='<%#"return PopDescriptionEdit(this, " + Eval("ID").ToString()+");"%>' Text="修改备注" runat="server"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="8%" />
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <%# Eval("ApprovalStatus").ToString()!="1" ? "" :"审核成功" %>
                    <asp:LinkButton ID="hySign"  OnClientClick='<%#"return PopSignEdit(this, " + Eval("ID").ToString()+");"%>' Text="审核" Visible='<%# Eval("ApprovalStatus").ToString()!="1" ? true :false %>' runat="server"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle CssClass="center" Width="8%" />
            </asp:TemplateField> 
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <a id="hyEdit" href='UserImportRec_Lst.aspx?DeleteUserImportRecID=<%# Eval("ID").ToString()%>' onclick="return confirm('此操作将删除所选的导入记录，确认吗？')">删除</a>
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

  

<div>
	
</div>
<script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>
    </form>
</body>
</html>
