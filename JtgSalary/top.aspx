<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="top.aspx.cs" Inherits="JtgTMS.top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="~/inc/style.css" type="text/css" />
    <link rel="stylesheet" type="text/css" href="~/inc/top_left.css">
</head>
<body  style="margin-top:0px;">
    <form id="form1" runat="server">
        <table cellSpacing=0 cellPadding=0 width="100%" border=0>
          <tbody>
	        <tr>
	          <td rowspan="2" class="center" style="HEIGHT: 57px; width:180px;"><a href="/Main.aspx" target="_top"><img src="/pic/logo.png" /></a></td>
	          <td colspan="2" style="PADDING-RIGHT: 10px; MARGIN-TOP: 0px; line-height:28px; height:28px; text-align:right;">
	            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                            <nobr>&nbsp;您好 &nbsp;&nbsp;
                        <asp:Timer ID="Timer1" runat="server" Interval="1000">
                        </asp:Timer>
                           <asp:Label ID="lblUserName" runat="server"></asp:Label>
                           [<asp:Label ID="lblOrganName" runat="server"></asp:Label>]
                           <asp:Label ID="lblTime" runat="server"></asp:Label> 
                           &nbsp;<a href="main.aspx?module=TMS" target="_top"><asp:Label ID="lblTitle" runat="server"></asp:Label></a>
                            </nobr>
                    </ContentTemplate>
                </asp:UpdatePanel>
                    </td>
	        </tr>
	        <tr>
	          <td>
	                <div id="navigation" class="toptitle" runat="server">
	                    |<a href="platform/framework_userProfile.aspx" target="right" onclick="clickLink(this)" style='font-weight:normal'>个人信息</a>
                        |<a href="platform/framework_userPassword.aspx" target="right" onclick="clickLink(this)" style='font-weight:normal'>更改密码</a>
	                </div>
	          </td>
	          <td align="right">
                <div class="toptitle_r">
                    <A href="Portal/logout.aspx" target="_top">退出</a>
                </div>
              </td>
	        </tr>
	        <tr>
	          <td colspan="3" class="topline"></td>
	        </tr>
          </tbody>
        </table>
    </form>
    <script>
	    var leftLink = document.getElementById("navigation").getElementsByTagName("a")[1];
	    leftLink.style.fontWeight="bold";
	    //window.top.frames["left"].location.href = leftLink.getAttribute("href");
    	
	    function clickLink(hyLink){
		    var links = document.getElementById("navigation").getElementsByTagName("a");
		    for (i = 0; i < links.length; i++){
		        links[i].style.fontWeight = "normal";
		        links[i].style.color = "white";
            }
		    hyLink.style.fontWeight = "bold";
		    hyLink.style.color = "yellow";
		    hyLink.blur();
	    }
    </script>


</body>
</html>
