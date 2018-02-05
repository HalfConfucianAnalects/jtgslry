<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IPDesktop.aspx.cs" Inherits="JtgTMS.Platform.IPDesktop" %>

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
 <form name="ctl00" method="post" action="background_face.aspx" id="ctl00" class="form-inline" runat="server">

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          欢迎使用 上海动车段 TMS 无线电台管理系统。
        </td>
      </tr>
    </table>
  </div>
  <table class="table noborder table-hover">
        <tr>
            <td width="50%">
                <div class="popover popover-static">
                <h3 class="popover-title">我领用的无线电台</h3>
                    <div class="popover-content">                       
                        <asp:GridView ID="gvLists" name="contents" runat="server" CssClass="table noborder table-hover"
                            GridLines="none" AutoGenerateColumns="False" DataKeyNames="ID" >
                            <HeaderStyle CssClass="info thead" />
                            <EmptyDataTemplate>
                                未归还无线电台为空。
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="序列号">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTableRecGuid" Text='<%# Eval("TableRecGuid").ToString()%>' Visible="false"
                                            runat="server"></asp:Label>
                                        <asp:Label ID="lblID" Text='<%# Eval("InterID").ToString()%>' Visible="false" runat="server"></asp:Label>
                                        <asp:Label ID="lblToolID" Text='<%# Eval("InterID").ToString()%>' Visible="false"
                                            runat="server"></asp:Label>
                                        <asp:Label ID="lblToolNo" Text='<%# Eval("SerialNum").ToString()%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="品牌">
                                    <ItemTemplate>
                                        <asp:Label ID="lblToolName" Text='<%# Eval("BrandNames").ToString()%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="规格">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSpecification" Text='<%# Eval("Specification").ToString()%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>                               
                                <asp:TemplateField HeaderText="借/领用时间">
                                    <ItemTemplate>
                                        <%# Eval("ConsumeDate").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>                                            
                            </Columns>
                        </asp:GridView>
                    </div>
              </div>
            </td>
            <td width="50%">
                <div class="popover popover-static">
                    <h3 class="popover-title">我借用的无线电台</h3>
                    <div class="popover-content">  
                        <asp:GridView ID="gvBorrowLists" name="contents" runat="server" CssClass="table noborder table-hover"
                            GridLines="none" AutoGenerateColumns="False" DataKeyNames="ID">
                            <HeaderStyle CssClass="info thead" />
                            <EmptyDataTemplate>
                                未归还无线电台为空。
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="序列号">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTableRecGuid" Text='<%# Eval("TableRecGuid").ToString()%>' Visible="false"
                                            runat="server"></asp:Label>
                                        <asp:Label ID="lblID" Text='<%# Eval("InterID").ToString()%>' Visible="false" runat="server"></asp:Label>
                                        <asp:Label ID="lblToolID" Text='<%# Eval("InterID").ToString()%>' Visible="false"
                                            runat="server"></asp:Label>
                                        <asp:Label ID="lblToolNo" Text='<%# Eval("SerialNum").ToString()%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="品牌">
                                    <ItemTemplate>
                                        <asp:Label ID="lblToolName" Text='<%# Eval("BrandNames").ToString()%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="规格">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSpecification" Text='<%# Eval("Specification").ToString()%>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="借用时间">
                                    <ItemTemplate>
                                        <%# Eval("ConsumeDate").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>                                                  
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                
            </td>
        </tr>
        <tr id="trApprovaling" runat="server">
            <td width="100%" colspan="2">
                <div class="popover popover-static">
                <h3 class="popover-title">待审批的申请单</h3>
                    <div class="popover-content">                       
                        <asp:GridView ID="gvApprovaling" name="contents" runat="server" CssClass="table noborder table-hover"
                            GridLines="none" AutoGenerateColumns="False" DataKeyNames="ID" >
                            <HeaderStyle CssClass="info thead" />
                            <EmptyDataTemplate>
                                待审批的申请单为空。
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="申请类型">
                                    <ItemTemplate>
                                        <%# Eval("OrderTypeName").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="申请单号">
                                    <ItemTemplate>
                                        <%# Eval("OrderNo").ToString()%>
                                    </ItemTemplate>
                                    <HeaderStyle Width="10%"/>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="申请日期">
                                    <ItemTemplate>
                                        <%# Eval("OrderDate").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>    
                                <asp:TemplateField HeaderText="申请金额">
                                    <ItemTemplate>
                                        <%# Eval("TotalAmount").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="申请车间">
                                    <ItemTemplate>
                                        <%# Eval("OrganName").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="申请人">
                                    <ItemTemplate>
                                        <%# Eval("OrderOpName").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="申请状态">
                                    <ItemTemplate>
                                        <asp:Label ID="lblApprovalStatus" Text='<%# Eval("ApprovalStatus").ToString()%>'  Visible="false" runat="server"></asp:Label>
                                        <a href="#" onclick='PopToolApprovalLog(this, <%# Eval("ID").ToString()%>)'><asp:Label ID="lblApprovalStatusName" Text='<%# Eval("ApprovalStatusName").ToString()%>' runat="server"></asp:Label></a>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="审批人">
                                    <ItemTemplate>
                                        <%# Eval("ApprovalUserName").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="审批时间">
                                    <ItemTemplate>
                                        <%# Eval("ApprovalTime").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
              </div>
            </td>
        </tr>
        <tr id="tr1" visible="false" runat="server">
            <td width="100%" colspan="2">
                <div class="popover popover-static">
                <h3 class="popover-title">待入库的申请单</h3>
                    <div class="popover-content">                       
                        <asp:GridView ID="gvDeliveryNoStorage" name="contents" runat="server" CssClass="table noborder table-hover"
                            GridLines="none" AutoGenerateColumns="False" DataKeyNames="ID" >
                            <HeaderStyle CssClass="info thead" />
                            <EmptyDataTemplate>
                                待入库的申请单为空。
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="配送单号">
                                    <ItemTemplate>
                                        <%# Eval("DeliveryNo").ToString()%>
                                    </ItemTemplate>
                                    <HeaderStyle Width="10%"/>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="配送日期">
                                    <ItemTemplate>
                                        <%# Eval("DeliveryDate").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>    
                                <asp:TemplateField HeaderText="配送金额">
                                    <ItemTemplate>
                                        <%# Eval("TotalAmount").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="配送单位">
                                    <ItemTemplate>
                                        <%# Eval("DeliveryOrganName").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="配送人">
                                    <ItemTemplate>
                                        <%# Eval("DeliveryOpName").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="订单状态">
                                    <ItemTemplate>
                                        <asp:Label ID="lblShipingStatus" Text='<%# Eval("ShipingStatus").ToString()%>'  Visible="false" runat="server"></asp:Label>
                                        <a href="#" onclick='PopToolApprovalLog(this, <%# Eval("ID").ToString()%>)'><asp:Label ID="lblShipingStatusName" Text='<%# Eval("ShipingStatusName").ToString()%>' runat="server"></asp:Label></a>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hyEdit" NavigateUrl='<%# "../Delivery/Delivery_Edit.aspx?ReturnPageType=3&DeliveryID="+Eval("ID").ToString()%>' Text="查看" runat="server"></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="center" Width="4%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
              </div>
            </td>
        </tr>
        <tr id="trNotEvaluateList" runat="server">
            <td width="100%" colspan="2">
                <div class="popover popover-static">
                <h3 class="popover-title">待评价的送修无线电台</h3>
                    <div class="popover-content">                       
                        <asp:GridView ID="gvNotEvaluateList" name="contents" runat="server" CssClass="table noborder table-hover"
                            GridLines="none" AutoGenerateColumns="False" DataKeyNames="ID" >
                            <HeaderStyle CssClass="info thead" />
                            <EmptyDataTemplate>
                                可评价无线电台档案为空。
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle Width="2%" />
                                </asp:TemplateField>                                
                                <asp:TemplateField HeaderText="">                                          
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle Width="3%"/>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="无线电台序列号">
                                    <ItemTemplate>
                                        <%# Eval("SerialNum").ToString()%>
                                    </ItemTemplate>
                                    <HeaderStyle Width="10%"/>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="品牌">
                                    <ItemTemplate>
                                        <%# Eval("b").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>  
                                <asp:TemplateField HeaderText="维修单位">
                                    <ItemTemplate>
                                        <%# Eval("OrganName").ToString()%>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>                                  
                            </Columns>
                        </asp:GridView>
                    </div>
              </div>
            </td>
        </tr>
        
    </table>
            
    <div class="popover popover-static">
            <h3 class="popover-title">登录信息</h3>
            <div class="popover-content">                       
             <table class="table noborder table-hover">
                  <tr>
                    <td width="150"><lan>当前登记用户</lan>：</td>
                    <td>
    	                <asp:Literal ID="ltlUserName" runat="server" />
                    </td>
                  </tr>
                  <tr>
                    <td><lan>最近登录机构</lan>：</td>
                    <td>
    	                <asp:Literal ID="ltlOrganName" runat="server"></asp:Literal>
                    </td>
                  </tr>
                  <tr>
                    <td><lan>最近登录IP</lan>：</td>
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
