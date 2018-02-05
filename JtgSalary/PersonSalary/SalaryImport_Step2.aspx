<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalaryImport_Step2.aspx.cs" Inherits="JtgTMS.PersonSalary.SalaryImport_Step2" %>

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

        function PopToolApprovalLog(_this, _id) {
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
                windowSourceURL: '../PersonSalary/ToolApprovalLog_Info.aspx?ID=' + _id.toString(),
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
  <ul class="breadcrumb" style="display:none"><li>工资管理 <span class="divider">/</span></li><li>工资导入</ul>

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
               <asp:PostBackTrigger ControlID="btnAdd" />
               <asp:PostBackTrigger ControlID="btnUpload" />
            </Triggers>
            <ContentTemplate>
                <div class="well well-small">
                
                <table class="table table-noborder">
                  <tr>
                    <td width="70">
                        工资月份：
                    </td>
                    <td width="330">
                        <asp:TextBox ID="txtUserSalaryYears" Width="120px" runat="server" ReadOnly="true"
                                        ></asp:TextBox>
                    </td>
                    <td>
                        
                    </td>
                  </tr>
                  <tr>
                    <td width="100">
                        Excel文件：
                    </td>
                    <td width="330">                        
                        <asp:FileUpload ID="fpUpload" runat="server" />
                        <asp:Button ID="btnUpload" Text="文件上传" CssClass="btn" runat="server" 
                            onclick="btnUpload_Click" />
                    </td>
                    <td>
                        已上传文件：
                        <asp:Label ID="lblUploadFileName" runat="server"></asp:Label>
                        <asp:Label ID="lblUploadFile" Visible="false" runat="server"></asp:Label>
                    </td>
                    </tr>
                </table>
              </div>

              <ul class="breadcrumb breadcrumb-button">
                <asp:DropDownList ID="ddlImportRec" runat="server">
                    <asp:ListItem Value="0" Text="新导入"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;备注&nbsp;&nbsp;
                  <asp:DropDownList ID="ddlDescription" runat="server">
                      <asp:ListItem Value="0" Text="奖金"></asp:ListItem>
                      <asp:ListItem Value="1" Text="工资"></asp:ListItem>
                      <asp:ListItem Value="2" Text="报销"></asp:ListItem>
                  </asp:DropDownList>
                <asp:Button ID="btnAdd" Text="导入" CssClass="btn" runat="server" OnClientClick="return confirm('此操作将导入工资记录，确认吗？')"
                      onclick="btnAdd_Click"  />
                <asp:Button ID="btnPush" Text="推送通知" CssClass="btn" runat="server" onclick="btnPush_Click"  />
                <asp:Label ID="lblProcess" ForeColor="Red" runat="server"></asp:Label>
              </ul>
              
              <asp:DropDownList ID="ddlTargetName" Visible="false" runat="server">
                                
                                </asp:DropDownList>
              
              <asp:GridView ID="gvAbnormal" Visible="false" name="contents" runat="server" CssClass="table table-bordered info table-hover"  GridLines="none"
                    AutoGenerateColumns="True">
                    <HeaderStyle CssClass="info thead" />                 
                    <EmptyDataTemplate>
                           工资单为空。
                    </EmptyDataTemplate>
                    <Columns>
                        
                    </Columns>
                </asp:GridView>
              <asp:GridView ID="gvImportSet" name="contents" runat="server" CssClass="table table-bordered info table-hover" 
                     GridLines="none" onrowcreated="gvImportSet_RowCreated"  onrowdatabound="gvImportSet_RowDataBound"
                    AutoGenerateColumns="False">
                    <HeaderStyle CssClass="info thead" />                 
                    <EmptyDataTemplate>
                           字段为空!
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>
                            </ItemTemplate>
                            <ItemStyle Width="3%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="源字段">
                            <ItemTemplate>
                                <font style='<%# Eval("TargetName").ToString()==""?"color:red":"color:green"%>'>
                                                            <%# Eval("SourceName").ToString()%>
                                                        </font>
                                                        
                                <asp:Label ID="lblSourceName" style="display:none" Text='<%# Eval("SourceName").ToString()%>' runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="3%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="目标字段">
                            <ItemTemplate>
                                <asp:TextBox ID="txtTargetName" Visible="false" Text='<%# Eval("TargetName").ToString()%>' runat="server"></asp:TextBox>
                                <asp:DropDownList ID="ddlTargetName" runat="server">
                                
                                </asp:DropDownList>
                            </ItemTemplate>
                            <ItemStyle Width="3%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
              
                <asp:GridView ID="gvLists" name="contents" Visible="false" runat="server" CssClass="table table-bordered info table-hover"  GridLines="none"
                    AutoGenerateColumns="True">
                    <HeaderStyle CssClass="info thead" />                 
                    <EmptyDataTemplate>
                           工资单为空。
                    </EmptyDataTemplate>
                    <Columns>
                        
                    </Columns>
                </asp:GridView>
                               
  
          </ContentTemplate>
        </asp:UpdatePanel>
<div>	
</div>
<script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>
    </form>
</body>
</html>
