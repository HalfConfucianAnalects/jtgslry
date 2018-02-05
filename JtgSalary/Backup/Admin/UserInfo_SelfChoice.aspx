<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserInfo_SelfChoice.aspx.cs" Inherits="JtgTMS.BasicData.UserInfo_SelfChoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
        <script language="javascript" src="/SiteFiles/bairong/jquery/jquery-1.9.1.min.js"></script>
        <script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>
         
        <link rel="stylesheet" type="text/css" href="/SiteFiles/bairong/jquery/bootstrap/css/bootstrap.min.css">
        <script language="javascript" src="/SiteFiles/bairong/jquery/bootstrap/js/bootstrap.min.js"></script>

        <script type="text/javascript">

            function UpdateSuccess() {
                var dom = window.parent.document.getElementById('DOMWindow'),
			        bg = window.parent.document.getElementById('DOMWindowOverlay');

                parent.refUserWindow();
                
                dom.parentNode.removeChild(dom);
                bg.parentNode.removeChild(bg);
            }
            
            function closeWindow() {
                var dom = window.parent.document.getElementById('DOMWindow'),
			        bg = window.parent.document.getElementById('DOMWindowOverlay');

                dom.parentNode.removeChild(dom);
                bg.parentNode.removeChild(bg);
            }
            
            $(document).ready(function() {
                var index = 0;
                
                //关闭窗口的方法
                $('a.closeDom').click(function() {
                    var dom = window.parent.document.getElementById('DOMWindow'),
			        bg = window.parent.document.getElementById('DOMWindowOverlay');
                    dom.parentNode.removeChild(dom);
                    bg.parentNode.removeChild(bg);
                });

                $('button.closeDom').click(function() {
                    var dom = window.parent.document.getElementById('DOMWindow'),
			        bg = window.parent.document.getElementById('DOMWindowOverlay');
                    dom.parentNode.removeChild(dom);
                    bg.parentNode.removeChild(bg);
                });
            });
        </script>
        
         <link rel="stylesheet" href="../inc/style.css" type="text/css" />
    <script language="javascript" src="../inc/script.js"></script>
</head>
<body>
    <form id="form1" runat="server">
      <script type="text/javascript">
         $(document).ready(function() {
             loopRows(document.getElementById('gvLists'), function(cur) { cur.onclick = chkSelect; });
             $(".popover-hover").popover({ trigger: 'hover', html: true });
         });
      </script>
      <div>
          <table border="0" cellpadding="0" cellspacing="0" height="100%" width="100%">
                <tr>
                    <td colspan="2">
                        <div class="modal-header" style="height:30px;">
                            <button type="button" class="close" id="closeDom" data-dismiss="modal" onclick="closeWindow()" aria-hidden="true"><img src="../../sitefiles/bairong/icons/close.png" /></button>            
                            <h3>选择人员</h3>  
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="10px">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" height="380" align="center" valign="top">
                        <table width="98%">
                            <tr>
                                <td width="180" align="left" valign="top">
                                
                                
                                    <asp:TreeView ID="tvList" runat="server" 
                                        CssClass="treelist-condensed table-hover" NodeStyle-Height="20" Width="97%" 
                                        ExpandDepth="10" ShowCheckBoxes="None" ShowLines="True" 
                                        onclick="javascript:postBackByObject()" EnableClientScript="False" 
                                        PopulateNodesFromClient="False" EnableTheming="False" 
                                        onselectednodechanged="tvList_SelectedNodeChanged">
                                        <SelectedNodeStyle Font-Underline="True" ForeColor="#CC3300" 
                                                            HorizontalPadding="0px" VerticalPadding="0px" Font-Bold="True" />
                                            <Nodes>
                                                <asp:TreeNode Value=".ROOT" Text="六合盛房产" ImageUrl="../../sitefiles/bairong/icons/tree/department.gif"></asp:TreeNode>
                                            </Nodes>
                                            <NodeStyle Font-Names="微笑雅黑" NodeSpacing="0px" VerticalPadding="0px" />
                                    </asp:TreeView>	 
                                </td>
                                <td valign="top">
                                <div class="we1ll well1-small">
                                        <table class="table table-noborder">
                                          
                                          <tr>
                                            <td>
                                                关键字：
                                            
                                              <asp:TextBox ID="txtSearchKeyword" MaxLength="300" runat="server"></asp:TextBox>
                                            
                                              <asp:Button ID="btnSearch" CssClass="btn" Text="搜 索" runat="server" TabIndex="0" onclick="btnSearch_Click" />
                                            </td>
                                          </tr>
                                        </table>
                                      </div>
                                    <asp:GridView ID="gvLists" name="contents" runat="server" Width="100%" CssClass="table table-bordered info table-hover"  GridLines="none"
                                        AutoGenerateColumns="False" DataKeyNames="ID" OnRowCommand="gvLists_RowCommand" >
                                        <HeaderStyle CssClass="info thead" />                 
                                        <EmptyDataTemplate>
                                                人员档案为空。
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
                                                <HeaderStyle Width="15%" VerticalAlign="Middle"/>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="名称">
                                                <ItemTemplate>
                                                    <%# Eval("OpName").ToString()%>
                                                </ItemTemplate>
                                                <ItemStyle Width="75%" VerticalAlign="Middle"/>
                                            </asp:TemplateField>  
                                             <asp:TemplateField HeaderText="">
                                            <HeaderTemplate>
                                            </HeaderTemplate>
                                            <ItemTemplate>  
                                                <asp:Button ID="btnChoice" CommandName="Choice" CommandArgument='<%# Eval("ID").ToString()%>' ToolTip="选择该用户。" CssClass="btn btn-primary" runat="server" Text="选择"/>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%"/>
                                        </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td width="20"></td>
                        <td>
                            <div class="moda1l-footer">
                                <button type="button" class="btn" id="Button2" onclick="closeWindow()" aria-hidden="true">关 闭</button>
                            </div>
                        </td>
                   
                </tr>   
            </table>
        </div>
    </form>
</body>
</html>
