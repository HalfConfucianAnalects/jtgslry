<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="JtgTMS.Portal.Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    
    <title>用户登录</title>
    <link href="~/css/admin/admin.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="/SiteFiles/bairong/jquery/bootstrap/css/bootstrap.min.css">
    <script language="javascript" type="text/javascript">
  
    </script>
    <style type="text/css">
        html{ background:#F2F5F8;}
        body{ background:#F2F5F8;}
    </style>
    
    <script>   
        if(top!=self)   top.location.href=self.location.href   
    </script>
</head>
<body>
    
    
<div id="login-wrap">
    <div id="login-main" class="login-main">
        <div class="login-tit">
            <div class="admin-logo"></div>
            <div class="tit" style="text-align:right"><a href="logout.aspx">返回</a></div>
        </div>
        <div id="login-cont" class="login-cont1">
            <form id="loginFrm" action="" runat="server">
            <asp:ScriptManager ID="smForm" runat="server" EnableScriptGlobalization="True"/>
                <asp:UpdatePanel ID="upForm" runat="server">
                    <Triggers>
                    </Triggers>
                    <ContentTemplate>                         
                        <table width="100%">
                            <tr>
                                <td colspan="3" height="10" align="center">
                                    <font size="6">请选择登录子系统</font>
                                </td>
                            </tr>
                            <tr>
                                <td  height="20"></td>
                            </tr>
                            <tr >
                                <td width="27%" align="right">
                                    <asp:Button ID="btnToolApp" Text="工资签收" CssClass="btn" onfocus="this.blur()" Font-Size="18" Font-Bold="true" Height="75" Width="170"
                                        runat="server" onclick="btnToolApp_Click" />
                                </td>
                                <td width="3%">&nbsp;</td>
                                <td  width="27%" align="left">
                                    <asp:Button ID="btnInterPhone" CssClass="btn" Text="无线电台管理" onfocus="this.blur()"  Font-Size="18" Font-Bold="true" Height="75" Width="170"
                                        runat="server" onclick="btnInterPhone_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" height="20"></td>
                            </tr>
                            <tr style="display:none">
                                <td colspan="3" align="center">

                                        <asp:Button ID="btnSalary" CssClass="btn" Text="在线工资" onfocus="this.blur()"  Font-Size="18" Font-Bold="true" Height="75" Width="170"
                                            runat="server" onclick="btnSalary_Click" />
                                
                                </td>
                            </tr>
                        </table>
                        
                            &nbsp;&nbsp;&nbsp;
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </form>
        </div>                    
    </div>
           
</div>
    
</body>
</html>
