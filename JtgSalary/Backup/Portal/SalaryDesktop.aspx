<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalaryDesktop.aspx.cs" Inherits="JtgTMS.Platform.SalaryDesktop" %>

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
            //alert(document.getElementById("txtSalaryYears").value);
        }
    </script>
</head>
<body>
 <form  class="form-inline" runat="server">

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td><input type="text" style="display:none"  onchange="alert(this.value);" >
          欢迎使用 上海动车段 工资电子签收系统。 <a style="font-size:large; color:Red" href="../upload/chrome_installer.exe">如使用IE6，请下载谷歌浏览器安装使用</a>
        </td>
      </tr>
    </table>
  </div><asp:Button ID="btnSalarys" Style="display: none" runat="server" Text="insert" CausesValidation="false"
                    OnClick="btnSalarys_Click" />
  <table class="table noborder table-hover">
        <tr style="display:none">
            <td width="50%">
                <div class="popover popover-static">
                <h3 class="popover-title" style="width:auto">
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
                                    <HeaderStyle Width="15%"/>
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
                                        <asp:LinkButton ID="hyView" OnClientClick='<%#"return PopSalaryInfo(this, " + Eval("ID").ToString()+");"%>' text="查看" Visible='<%# Eval("ID").ToString().Length > 0? true :false %>' runat="server"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="center" Width="8%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="hySign"  OnClientClick='<%#"return PopSignEdit(this, " + Eval("ID").ToString()+");"%>' Text="签收" Visible='<%# Eval("ID").ToString().Length > 0? true :false %>' runat="server"></asp:LinkButton>
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
                        <td align="right" width="160"><asp:TextBox ID="txtMonth" MaxLength="64" Width="100" AutoPostBack="true" onkeyup='refSalaryWindow();' onchange='refSalaryWindow();';
                                onClick="WdatePicker({dateFmt:'yyyyMM',onpicked:function(){SalarySelected()}})" runat="server" 
                                ontextchanged="txtMonth_TextChanged"></asp:TextBox>
                        &nbsp;<a href="../PersonSalary/MySignUserSalary_Lst.aspx">更多</a></td>
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
                                    <HeaderStyle Width="15%"/>
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
                                        <asp:LinkButton ID="hyView" OnClientClick='<%#"return PopSalaryInfo(this, " + Eval("ID").ToString()+");"%>' Visible='<%# Eval("ID").ToString().Length > 0? true :false %>' text="查看" runat="server"></asp:LinkButton>
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
                        <td align="right" width="60">
                        &nbsp;<a href="../PersonSalary/MyNotice_Lst.aspx">更多</a></td>
                    </tr>
                </table>
                </h3>
                    <div class="popover-content">                       
                        <asp:GridView ID="gvNotice" name="contents" runat="server" CssClass="table noborder table-hover" ShowHeader="false"
                            GridLines="none" AutoGenerateColumns="False" DataKeyNames="ID" >
                            <HeaderStyle CssClass="info thead" />
                            <EmptyDataTemplate>
                                通知公告为空。
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="">                                          
                                <ItemTemplate>
                                    <%# Eval("ID").ToString().Length > 0? (Container.DataItemIndex+1).ToString() :"" %>&nbsp;
                                </ItemTemplate>
                                <ItemStyle Width="3%"/>
                            </asp:TemplateField>            
                            <asp:TemplateField HeaderText="通知标题">
                                <ItemTemplate>
                                    <a id="hyEdit" href='../PersonSalary/Notice_Info.aspx?ViewType=2&NoticeID=<%# Eval("ID").ToString()%>'>
                                    <font style='color:<%# Eval("SelfClickNum").ToString()=="0"?"red":"green"%>'>
                                        <%# Eval("NoticeTitle").ToString()%>
                                    </font>
                                    </a>
                                </ItemTemplate>
                                <ItemStyle Width="50%" />
                            </asp:TemplateField>      
                            <asp:TemplateField HeaderText="发布时间">
                                <ItemTemplate>
                                    <a id="hyEdit" href='../PersonSalary/Notice_Info.aspx?ViewType=2&NoticeID=<%# Eval("ID").ToString()%>'>
                                    <font style='color:<%# Eval("SelfClickNum").ToString()=="0"?"red":"green"%>'>
                                        <%# Eval("CreatedTime").ToString()%>
                                    </font>
                                    </a>
                                </ItemTemplate>
                                <ItemStyle Width="30%" />
                            </asp:TemplateField>      
                            <asp:TemplateField HeaderText="发布人">
                                <ItemTemplate>
                                    <a id="hyEdit" href='../PersonSalary/Notice_Info.aspx?ViewType=2&NoticeID=<%# Eval("ID").ToString()%>'>
                                    <font style='color:<%# Eval("SelfClickNum").ToString()=="0"?"red":"green"%>'>
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
            <h3 class="popover-title"><table width="100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>登录信息</td>                        
                    </tr>
                </table></h3>
            <div class="popover-content">                       
             <table class="table noborder table-hover" style="color:Red">
                  <tr style="display:none">
                    <td width="150"><lan>当前登记用户</lan>：</td>
                    <td>
    	                <asp:Literal ID="ltlUserName" runat="server" />
                    </td>
                  </tr>
                  <tr style="display:none">
                    <td><lan>最近登录机构</lan>：</td>
                    <td>
    	                <asp:Literal ID="ltlOrganName" runat="server"></asp:Literal>
                    </td>
                  </tr>
                  <tr>
                    <td width="150"><lan>最近登录IP</lan>：</td>
                    <td> 

    	                <asp:Literal ID="ltlLastLoginIP" runat="server"></asp:Literal>
                    </td>
                  </tr>
                  <tr>
                    <td><lan>最近登录计算机</lan>：</td>
                    <td>
    	                <asp:Literal ID="LtlComputerName" runat="server"></asp:Literal>
                    </td>
                  </tr>
                  <tr>
                    <td><lan>上次登录时间</lan>：</td>
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
            
    
    
          <%--  <div class="popover popover-static">
                <h3 class="popover-title">车间工资签收</h3>
                <div class="popover-content">
                
                  <ul class="breadcrumb breadcrumb-button">
                    <input type="submit" name="AddChannel1" value="工具采购申请"  onclick="return false;" id="AddChannel1" class="btn btn-success1" />
                    <input type="submit" name="AddToGroup" value="工具配送入库" onclick="return false;" id="AddToGroup" class="btn" />
                    <input type="submit" name="Translate" value="工具领用" onclick="return false;" id="Translate" class="btn" />
                    <input type="submit" name="Import" value="工具借用" onclick="return false;" id="Export" class="btn" />
                    <input type="submit" name="ExportTestReport" value="工具归还" onclick="return false;" id="Delete" class="btn" />
                  </ul>
                  
                  <ul class="breadcrumb breadcrumb-button">
                    <input type="submit" name="Create" value="个人工具注销" onclick="return false;" id="Create" class="btn" />
                    <input type="submit" name="Publish" value="不合格工具注销" onclick="return false;" id="Publish" class="btn" />
                    <input type="submit" name="Publish" value="工具盘点" onclick="return false;" id="Submit12" class="btn" />
                    <input type="submit" name="Publish" value="工具损益" onclick="return false;" id="Submit15" class="btn" />
                    <input type="submit" name="Publish" value="工具库存" onclick="return false;" id="Submit16" class="btn" />
                  </ul>

                </div>
              </div>
     
  
  
  <div class="popover popover-static">
  <h3 class="popover-title">备件管理</h3>
  <div class="popover-content">
    
      <ul class="breadcrumb breadcrumb-button">
        <input type="submit" name="AddChannel1" value="备件入库"  onclick="return false;" id="Submit1" class="btn btn-success1" />
        <input type="submit" name="AddToGroup" value="备件领用" onclick="return false;" id="Submit2" class="btn" />
        <input type="submit" name="Translate" value="段内备件调拨" onclick="return false;" id="Submit3" class="btn" />
        <input type="submit" name="Import" value="车间备件调拨" onclick="return false;" id="Submit4" class="btn" />
        <input type="submit" name="ExportTestReport" value="备件报废" onclick="return false;" id="Submit5" class="btn" />
      </ul>
      
      <ul class="breadcrumb breadcrumb-button">
        <input type="submit" name="Create" value="备件库存" onclick="return false;" id="Submit6" class="btn" />
      </ul>

    </div>
  </div>

<div class="popover popover-static">
  <h3 class="popover-title">统计报表</h3>
  <div class="popover-content">
    
      <ul class="breadcrumb breadcrumb-button">
        <input type="submit" name="AddChannel1" value=" 段Excel表格总账"  onclick="return false;" id="Submit7" class="btn btn-success1" />
        <input type="submit" name="AddToGroup" value="车间Excel表格总账" onclick="return false;" id="Submit8" class="btn" />
        <input type="submit" name="Translate" value="无线电台调拨单" onclick="return false;" id="Submit9" class="btn" />
        <input type="submit" name="Import" value="配件更换申请单" onclick="return false;" id="Submit10" class="btn" />
        <input type="submit" name="ExportTestReport" value="无线电台修理申请单" onclick="return false;" id="Submit11" class="btn" />
        <input type="submit" name="ExportTestReport" value="增减、闲置表格" onclick="return false;" id="Submit13" class="btn" />
        <input type="submit" name="ExportTestReport" value=" 配件、维修质量问题汇总表" onclick="return false;" id="Submit14" class="btn" />
      </ul>
    </div>
  </div>--%>
 
</form>
<script type="text/javascript">
if (window.top.location.href.toLowerCase().indexOf("main.aspx") == -1){
	var initializationUrl = window.top.location.href.toLowerCase().substring(0, window.top.location.href.toLowerCase().indexOf("/siteserver/")) + "/siteserver/initialization.aspx";
	window.top.location.href = initializationUrl;
}
</script>
</body>
</html>
