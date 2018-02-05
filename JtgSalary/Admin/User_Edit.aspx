<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User_Edit.aspx.cs" Inherits="JtgTMS.Admin.User_Edit" %>
<%@ Register TagPrefix="hw" Namespace="UNLV.IAP.WebControls" Assembly="DropDownCheckList" %>
<%@ Register Src="../SalaryControl/CustomExtEdit.ascx" TagName="CustomExtEdit" TagPrefix="ucCustomExtEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
    <form id="upForm" class="form-inline" runat="server">
   
   <div>
   
       <ul class="breadcrumb" style="display:none"><li>成员权限 <span class="divider">/</span></li><li>用户管理 <span class="divider">/</span></li><li class="active"><% =_UserID > 0 ? "编辑" : "添加"%>用户</li></ul>
        <asp:ScriptManager ID="smForm" runat="server" EnableScriptGlobalization="True"/>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="btnApply" /> 
            </Triggers>
            <ContentTemplate> 

              <div class="popover popover-static">
                  <h3 class="popover-title">用户基本信息</h3>
                      <div class="popover-content">

                        <table class="table noborder table-hover">      
                          <tr>
                            <td width="80px">所属机构：</td>
                            <td width="250px">
                                <asp:DropDownList ID="ddlOrganID" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvddlOrganID" ControlToValidate="ddlOrganID" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator>
                           </td>
                           <td width="80px"></td>
                            <td width="250px">
                               
                           </td>
                           <td></td>
                          </tr>
                          <tr>
                            <td>工号：</td>
                            <td>
                                <asp:TextBox ID="txtOpCode" MaxLength="32" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOpCode" ControlToValidate="txtOpCode" Visible="false" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator>
                              </td>                                                       
                            <td>姓名：</td>
                            <td>
                                <asp:TextBox ID="txtOpName" MaxLength="64" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvOpName" ControlToValidate="txtOpName" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator>
                            </td>
                            <td></td>
                          </tr>    
                          <tr id="trPassword" runat="server">
                            <td >密码：</td>
                            <td>
                                <asp:TextBox ID="txtPassword" TextMode="Password" MaxLength="32" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rvfPassword"
                                    ControlToValidate="txtPassword"
                                    ErrorMessage=" *" foreColor="red"
                                    Display="Dynamic"
                                    runat="server" />
                              </td>                          
                            <td>确认密码：</td>
                            <td>
                                <asp:TextBox ID="txtConfirmPassword" TextMode="Password" MaxLength="32" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvConfirmPassword"
                                    ControlToValidate="txtConfirmPassword"
                                    ErrorMessage=" *" foreColor="red"
                                    Display="Dynamic"
                                    runat="server" />
                                <asp:CompareValidator ID="cvNewPasswordCompare" runat="server" ControlToCompare="txtConfirmPassword" ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage=" 两次输入的密码不一致！请再输入一遍您上面填写的密码。" foreColor="red"></asp:CompareValidator>
                              </td>
                              <td></td>
                          </tr>
                          <tr>
                            <td width="80px">性别：</td>
                            <td height="20px" width="250">
                                <asp:RadioButtonList ID="rblSex" RepeatDirection="Horizontal" BorderWidth="0" Height="10px" runat="server">
                                    <asp:ListItem Value="0" Text="男"  Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="女"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>                        
                            <td width="80px">身份证号：</td>
                            <td width="250">
                                <asp:TextBox ID="txtIDNumber" MaxLength="18" runat="server"></asp:TextBox>
                            </td>
                            <td></td>
                          </tr>
                          <tr>
                            <td>拥有角色：</td>
                            <td>
                                <hw:DropDownCheckList id="ddcRole" runat="server" Width="310px" ClientCodeLocation="../js/DropDownCheckList.js"
		                                     DisplayTextList="Labels" TextWhenNoneChecked="请点击此处所属角色..." DisplayTextWidth="410" RepeatColumns="4" BackColor="white" Height="110">
                                        </hw:DropDownCheckList>
                            </td>
                            <td width="80px">在职状态：</td>
                            <td width="250">
                                <asp:RadioButtonList ID="rbIsCanLogin" RepeatDirection="Horizontal" BorderWidth="0" Height="10px" runat="server">
                                    <asp:ListItem Value="1" Text="是"  Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="否"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td></td>
                          </tr>  
                        </table>                  
                        
                        <table class="table noborder table-hover">      
                          
                          </table>
                          <hr />
                                <asp:DataList ID="dlList" runat="server" CssClass="table noborder table-hover"  RepeatColumns="2"  Width="850" RepeatDirection="Horizontal"
                                    GridLines="None">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td align="left" width="180px" height="28">
                                                    <label><%# Eval("FieldTitle").ToString() %><b></b></label>
                                                </td>
                                                <td width="250">
                                                    <ucCustomExtEdit:CustomExtEdit ID="WorklogExtEdit1" UserFieldType='<%# Eval("FieldType").ToString() %>' UserFieldName='<%# Eval("UserFieldname").ToString() %>' runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                           
                          <table>
                          <tr style="display:none">
                            <td>职位：</td>
                            <td>
                                <asp:TextBox ID="txtPosition" MaxLength="32" runat="server"></asp:TextBox>
                            </td>
                          
                            <td>籍贯：</td>
                            <td>
                                <asp:TextBox ID="txtPlace" MaxLength="32" runat="server"></asp:TextBox>
                            </td>
                             <td></td>
                          </tr>
                          <tr  style="display:none">
                            <td>电话：</td>
                            <td>
                                <asp:TextBox ID="txtTelNo" MaxLength="32" runat="server"></asp:TextBox>
                            </td>                          
                            <td>手机：</td>
                            <td>
                                <asp:TextBox ID="txtPhone" MaxLength="32" runat="server"></asp:TextBox>
                            </td>
                             <td></td>
                          </tr>
                          <tr  style="display:none">
                            <td>联系地址：</td>
                            <td>
                                <asp:TextBox ID="txtAddress" MaxLength="256" runat="server"></asp:TextBox>
                            </td>
                            <td>邮编：</td>
                            <td>
                                <asp:TextBox ID="txtZipCode" MaxLength="32" runat="server"></asp:TextBox>
                            </td>
                             <td></td>
                          </tr>
                          <tr style="display:none">
                            <td>备注：</td>
                            <td colspan="4">
                                <asp:TextBox ID="txtUserDesc" TextMode="MultiLine" MaxLength="256" Width="500px" runat="server"></asp:TextBox>         
                            </td>
                          </tr>                          
                        </table>
                    </div>
               </div>
                <table class="table noborder">
                  <tr>
                    <td class="center">
                        <asp:Button ID="btnApply" CssClass="btn btn-primary" Text="确 定" onclick="btnApply_Click" runat="server"/>
                        <asp:Button ID="Button1" CssClass="btn " Text="返 回" onclick="btnCancel_Click" runat="server"/>
                    </td>
                  </tr>
                </table>
       
              </ContentTemplate>
        </asp:UpdatePanel>
   </div>
    </form>
</body>
</html>
