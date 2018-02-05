<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BasicDictionaries_Fram.aspx.cs" Inherits="JtgTMS.BasicData.BasicDictionaries_Fram" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
</head>
<frameset id="frame" framespacing="0" border="false" cols="180,*" frameborder="0" scrolling="yes">
	<frame name="tree" scrolling="auto" marginwidth="0" marginheight="0" src="BasicDictionaries_Tree.aspx" >
	<frame name="worklst" scrolling="auto" marginwidth="0" marginheight="0" src="BasicDictionaries_Lst.aspx?MainID=<% =_CategoryID %>">
</frameset>
<body>
    
</body>
</html>
