<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MySignUserSalary_Lst.aspx.cs" Inherits="JtgTMS.PersonSalary.MySignUserSalary_Lst" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <script language="javascript" src="/SiteFiles/bairong/jquery/jquery-1.8.3.min.js"></script>
    <script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>

    <link rel="stylesheet" type="text/css" href="/SiteFiles/bairong/jquery/bootstrap/css/bootstrap.css">
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
            window.location = "../PersonSalary/MySignUserSalary_Lst.aspx";
            //document.getElementById("btnSearch").click();
        }

        function refSalaryWindow() {
            window.location = "../PersonSalary/MySignUserSalary_Lst.aspx";
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

        function print_Data() {
            bdhtml = window.document.body.innerHTML;
            sprnstr = "<!--startprint-->";
            eprnstr = "<!--endprint-->";
            prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 17);
            prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
            window.document.body.innerHTML = prnhtml;
            window.print();
            window.document.body.innerHTML = bdhtml;
        }
    </script>
</head>
<body>

<form id="upForm" name="ctl00" class="form-inline" runat="server">
  <%--<ul class="breadcrumb" style="display:none"><li>段工资签收 <span class="divider">/</span></li><li>已签收工资</ul>--%>

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
               <asp:PostBackTrigger ControlID="btnExport" />
            </Triggers>
            <ContentTemplate>
                <ul class="nav nav-pills">    
                <li ><a href="MyNotSignUserSalary_Lst.aspx"><lan>未签收工资单</lan></a></li>
                <li class="active"><a href="MySignUserSalary_Lst.aspx"><lan>已签收工资单</lan></a></li>
              </ul>
                <div class="well well-small">
                
                <table class="table table-noborder">
                  <tr>
                    <td width="250">
                        工资月份起始：<asp:TextBox ID="txtSalaryYears" onClick="WdatePicker({dateFmt:'yyyyMM'})" MaxLength="6" Width="120px" runat="server"></asp:TextBox>
                    </td>                    
                    <td width="250">
                      工资月份结束：<asp:TextBox ID="txtSalaryYears2" onClick="WdatePicker({dateFmt:'yyyyMM'})" MaxLength="6" Width="120px" runat="server"></asp:TextBox>
                    </td>
                    <td>                        
                      <asp:Button ID="btnSearch" CssClass="btn" Text="搜 索" runat="server" TabIndex="0" onclick="btnSearch_Click" />
                      <asp:Button ID="btnExport" Text="导出Excel" CssClass="btn" runat="server" OnClick="btnExport_Click" />
                      <asp:Button ID="btnPrint" Text="打 印" CssClass="btn" runat="server" OnClientClick="print_Data()" />
                    </td>
                  </tr>
                  <tr>
                    <td colspan="2">
                        查询包含项目：
                        <asp:CheckBox ID="chkGongzi" runat="server" Text="工资" Width="80px" Checked="True"/>
                        <asp:CheckBox ID="chkJiangjin" runat="server" Text="奖金" Width="80px" Checked="True"/>
                        <asp:CheckBox ID="chkBaoxiao" runat="server" Text="报销" Width="80px" Checked="True"/>
                      </td>
                    <td></td>
                  </tr>
                </table>
              </div>
                <!--startprint-->
                <asp:GridView ID="gvLists" name="contents" runat="server" 
                    CssClass="table table-bordered info table-hover"  GridLines="none"
                    AutoGenerateColumns="False" DataKeyNames="ID" 
                    OnRowDataBound="gvLists_RowDataBound" ShowFooter="True">
                    <HeaderStyle CssClass="info thead" />                 
                    <EmptyDataTemplate>
                           工资单为空。
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
                        <asp:TemplateField HeaderText="姓名">
                            <ItemTemplate>
                                <%# Eval("OpName").ToString()%>
                            </ItemTemplate>
                            <HeaderStyle Width="10%"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="工资年月">
                            <ItemTemplate>
                                <%# Eval("SalaryYears").ToString()%>
                            </ItemTemplate>
                            <HeaderStyle Width="10%"/>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="发放金额">
                            <ItemTemplate>
                                <%# Eval("TotalSalary").ToString()%>
                            </ItemTemplate>
                            <ItemStyle Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="签收状态">
                            <ItemTemplate>
                                <%# Eval("SignStatus").ToString() == "1" ? "已签收" : "未签收"%>
                            </ItemTemplate>
                            <HeaderStyle Width="10%"/>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="签收时间">
                            <ItemTemplate>
                                <%# Eval("SignDate").ToString()%>
                            </ItemTemplate>
                            <ItemStyle Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="签收端">
                            <ItemTemplate>
                                <%# Eval("SignPlatform").ToString() == "1" ? "手机端" : "PC端"%>
                            </ItemTemplate>
                            <ItemStyle Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="备注">
                            <ItemTemplate>
                                <%# Eval("Description").ToString()%>
                            </ItemTemplate>
                            <ItemStyle Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:LinkButton ID="hyView" OnClientClick='<%#"return PopSalaryInfo(this, " + Eval("ID").ToString()+");"%>' text="查看" runat="server"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle CssClass="center" Width="4%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <a id="hyEdit" href='MySignUserSalary_Lst.aspx?CancelID=<%# Eval("ID").ToString()%>' onclick="return confirm('此操作将取消签收所选的工资单，确认吗？')">取消签收</a>
                            </ItemTemplate>
                            <ItemStyle CssClass="center" Width="6%" />
                        </asp:TemplateField>
                    </Columns>
            </asp:GridView>
                <!--endprint-->
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
