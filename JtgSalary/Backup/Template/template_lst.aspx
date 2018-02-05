<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="template_lst.aspx.cs" Inherits="JtgTMS.Template.template_lst" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
   <!--#include file="../inc/openWindow.html"-->
   <div id="openWindowModal" class="modal hide fade form-horizontal">   
        <div class="modal-header" style="height:30px;">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true"><img src="../../sitefiles/bairong/icons/close.png" /></button>
            <h3></h3>  
        </div>
        <div class="modal-body" style="width:100%; height:100%; padding:5px 0; margin:0;">
            <iframe id="openWindowIFrame" style="width:100%;height:100%;background-color:#ffffff;" scrolling="auto" frameborder="0" width="100%" height="100%"></iframe>
        </div>
        <div class="modal-footer">
            <button id="openWindowBtn" class="btn btn-primary" data-loading-text="提交中...">确 定</button>
            <button class="btn" data-dismiss="modal" aria-hidden="true">取 消</button>
        </div>
    </div>

<form id="ctl00" name="ctl00" class="form-inline" runat="server">
<div>
<input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="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" />
</div>

  <ul class="breadcrumb" style="display:none"><li>论坛管理 <span class="divider">/</span></li><li>界面管理 <span class="divider">/</span></li><li class="active">表情管理</li></ul>
  
  <table class="table table-bordered table-hover" cellspacing="0" rules="all" border="1" id="dgContents" style="border-collapse:collapse;">
	<tr class="info thead">
		<td>文件夹名称</td><td>标题</td><td>上升</td><td>下降</td><td>&nbsp;</td><td>&nbsp;</td>
	</tr><tr>
		<td align="left">
				&nbsp;yoci
			</td><td align="left">
				&nbsp;悠嘻猴
			</td><td class="center" style="width:40px;">
				<a id="dgContents_ctl02_hlUpLinkButton" href="template_lst.aspx?FaceName=yoci&amp;Up=True"><img src="../../SiteFiles/bairong/icons/up.gif" border="0" alt="上升" /></a>
			</td><td class="center" style="width:40px;">
				<a id="dgContents_ctl02_hlDownLinkButton" href="template_lst.aspx?FaceName=yoci&amp;Down=True"><img src="../../SiteFiles/bairong/icons/down.gif" border="0" alt="下降" /></a>
			</td><td class="center" style="width:50px;">
				<a href="javascript:void(0);" onclick="openWindow('编辑表情','template_edit.aspx?FaceName=yoci',450,320,'false');return false;">编辑</a>
			</td><td class="center" style="width:50px;">
				<a href="template_lst.aspx?FaceName=yoci&Delete=True" onClick="javascript:return confirm('此操作将删除表情“悠嘻猴”，确认吗？');">删除</a>
			</td>
	</tr><tr>
		<td align="left">
				&nbsp;tuzki
			</td><td align="left">
				&nbsp;兔斯基
			</td><td class="center" style="width:40px;">
				<a id="dgContents_ctl03_hlUpLinkButton" href="template_lst.aspx?FaceName=tuzki&amp;Up=True"><img src="../../SiteFiles/bairong/icons/up.gif" border="0" alt="上升" /></a>
			</td><td class="center" style="width:40px;">
				<a id="dgContents_ctl03_hlDownLinkButton" href="template_lst.aspx?FaceName=tuzki&amp;Down=True"><img src="../../SiteFiles/bairong/icons/down.gif" border="0" alt="下降" /></a>
			</td><td class="center" style="width:50px;">
				<a href="javascript:void(0);" onclick="openWindow('编辑表情','template_edit.aspx?FaceName=tuzki',450,320,'false');return false;">编辑</a>
			</td><td class="center" style="width:50px;">
				<a href="template_lst.aspx?FaceName=tuzki&Delete=True" onClick="javascript:return confirm('此操作将删除表情“兔斯基”，确认吗？');">删除</a>
			</td>
	</tr><tr>
		<td align="left">
				&nbsp;oniontou
			</td><td align="left">
				&nbsp;洋葱头
			</td><td class="center" style="width:40px;">
				<a id="dgContents_ctl04_hlUpLinkButton" href="template_lst.aspx?FaceName=oniontou&amp;Up=True"><img src="../../SiteFiles/bairong/icons/up.gif" border="0" alt="上升" /></a>
			</td><td class="center" style="width:40px;">
				<a id="dgContents_ctl04_hlDownLinkButton" href="template_lst.aspx?FaceName=oniontou&amp;Down=True"><img src="../../SiteFiles/bairong/icons/down.gif" border="0" alt="下降" /></a>
			</td><td class="center" style="width:50px;">
				<a href="javascript:void(0);" onclick="openWindow('编辑表情','template_edit.aspx?FaceName=oniontou',450,320,'false');return false;">编辑</a>
			</td><td class="center" style="width:50px;">
				<a href="template_lst.aspx?FaceName=oniontou&Delete=True" onClick="javascript:return confirm('此操作将删除表情“洋葱头”，确认吗？');">删除</a>
			</td>
	</tr>
</table>

  <ul class="breadcrumb breadcrumb-button">
    <input type="submit" name="AddButton" value="添加表情" onclick="openWindow('添加表情','template_edit.aspx',450,320,'false');return false;" id="AddButton" class="btn btn-success1" />
  </ul>


<div>

	<input type="hidden" name="__EVENTVALIDATION" id="__EVENTVALIDATION" value="/wEWAgKNqIWVAQL5oNLsBCPSQa0J4YRy5xriNDd523uVugzK" />
</div></form>


</form>
<script type="text/javascript">
if (window.top.location.href.toLowerCase().indexOf("main.aspx") == -1){
	var initializationUrl = window.top.location.href.toLowerCase().substring(0, window.top.location.href.toLowerCase().indexOf("/siteserver/")) + "/siteserver/initialization.aspx";
	window.top.location.href = initializationUrl;
}
</script>
</body>
</html>
