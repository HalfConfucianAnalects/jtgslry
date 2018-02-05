<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalarySign_Edit.aspx.cs" Inherits="JtgTMS.DepotTool.SalarySign_Edit" %>

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

                parent.refWindow();
                alert("工资条签收确认成功！");
                dom.parentNode.removeChild(dom);
                bg.parentNode.removeChild(bg);
            }
            
            function closeWindow() {
                var dom = window.parent.document.getElementById('DOMWindow'),
			        bg = window.parent.document.getElementById('DOMWindowOverlay');

                parent.refWindow();
                
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
        <asp:ScriptManager ID="smForm" runat="server" EnableScriptGlobalization="True"/>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnApply" />
            </Triggers>
            <ContentTemplate> 
              <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td colspan="2">
                            <div class="modal-header" style="height:30px;">
                                <button type="button" class="close" id="closeDom" data-dismiss="modal" onclick="closeWindow()" aria-hidden="true"><img src="../../sitefiles/bairong/icons/close.png" /></button>            
                                <h3>工资条签收</h3>  
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="10px" align="center">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center" valign="top">
                             <table class="table-noborder table-hover" width="98%">
                                <tr>
                                  <td width="120" valign="top">工资单号：</td>
                                  <td align="left" valign="top">
                                       <asp:Literal ID="ltOrderNos" runat="server"></asp:Literal>
                                  </td>
                                </tr>
                                <tr>
                                  <td valign="top">设置状态：</td>
                                  <td  align="left">
                                        <asp:RadioButtonList ID="rbApprovalStatus" RepeatDirection="Vertical" runat="server">                                        
                                            <asp:ListItem Value="1" Selected="True" Text="签收"></asp:ListItem>
                                        </asp:RadioButtonList>
                                  </td>
                                </tr>
                                <tr id="trDescription" runat="server">
                                  <td valign="top">审批意见：</td>
                                  <td  align="left">
                                        <asp:TextBox ID="txtDescription" TextMode="MultiLine" MaxLength="256" Width="400" Height="100" runat="server"></asp:TextBox>
                                  </td>
                                </tr>
                              </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" height="10px">
                        </td>
                    </tr>
                    <tr>
                        <td width="20"></td>
                            <td>
                                <div class="moda1l-footer">
                                    <asp:Button ID="btnApply" CssClass="btn btn-primary" runat="server" Text="确 定"
                                            onclick="btnApply_Click" />
                                    <button type="button" class="btn" id="Button2" onclick="closeWindow()" aria-hidden="true">关 闭</button>
                                </div>
                            </td>
                       
                    </tr>   
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
      </div>
    </form>
</body>
</html>
