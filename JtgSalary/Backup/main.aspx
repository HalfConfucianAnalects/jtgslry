<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="JtgTMS.main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta charset="utf-8">
    <title>工资电子签收系统</title>
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <link rel="icon" type="image/png" href="pic/favicon.png">
    <!--[if IE]><link rel="shortcut icon" href="pic/favicon.ico"/><![endif]-->
    <script>
    function changeDisplayMode(){
	    if(document.getElementById("bottomframes").cols=="180,7,*"){
		    document.getElementById("bottomframes").cols="0,7,*"; 
		    document.getElementById("separator").contentWindow.document.getElementById('ImgArrow').src="pic/separator2.gif";
	    }else{
		    document.getElementById("bottomframes").cols="180,7,*"
		    document.getElementById("separator").contentWindow.document.getElementById('ImgArrow').src="pic/separator1.gif";
	    }
    }
    </script>
    <!--[if lt IE 9]><script src="/SiteFiles/bairong/jquery/html5shiv/html5shiv.js"></script><![endif]-->
</head>

<frameset id="mainframes" framespacing="0" border="false" rows="58,*" frameborder="0" scrolling="yes">
    <frame name="top" scrolling="no" src="top.aspx?Module=<% =_ModuleNo %>">
    <frameset id="bottomframes" framespacing="0" border="false" cols="<% =_LeftWidth %>"" frameborder="0" scrolling="yes">
	    <frame name="left" scrolling="auto" marginwidth="0" marginheight="0" src="platform/framework_left.aspx?Module=<% =_ModuleNo %>" noresize />
	    <frame id="separator" name="separator" src="separator.aspx" noresize scrolling="no" />
	    <frame name="right" scrolling="auto" src="<% =_DesktopHrf %>">
    </frameset>
</frameset>
</script>
</html>
