﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dept_Frame.aspx.cs" Inherits="JtgTMS.Admin.Dept_Frame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<frameset id="frame" framespacing="0" border="false" cols="180,*" frameborder="0" scrolling="yes">
	<frame name="tree" scrolling="auto" marginwidth="0" marginheight="0" src="Dept_Tree.aspx?POrganID=<% =_POrganID %>" >
	<frame name="worklst" scrolling="auto" marginwidth="0" marginheight="0" src="Dept_lst.aspx?POrganID=<% =_POrganID %>">
</frameset>
<body>
    
</body>
</html>
