<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report_lst.aspx.cs" Inherits="JtgTMS.Report.Report_lst" %>

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
<body><!--#include file="../inc/openWindow.html"-->
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

<form id="form1" name="ctl00" class="form-inline" runat="server">
    
<ul class="breadcrumb" style="display:none"><li>工资电子签收系统 <span class="divider">/</span></li><li>无线电台管理管理 <span class="divider">/</span></li><li class="active">内容搜索</li></ul>
  

  <script type="text/javascript">
      $(document).ready(function() {
          loopRows(document.getElementById('contents'), function(cur) { cur.onclick = chkSelect; });
          $(".popover-hover").popover({ trigger: 'hover', html: true });
      });
  </script>

<div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          欢迎使用 上海动车段 TMS 工资电子签收系统 — 无线电台管理。
        </td>
      </tr>
    </table>
  </div>

<div class="popover popover-static">
  <h3 class="popover-title">统计报表</h3>
  <div class="popover-content">
    
      <ul class="breadcrumb breadcrumb-button">
        <input type="submit" name="AddChannel1" value=" 段Excel表格总账"  onclick="return false;" id="Submit14" class="btn btn-success1" />
        <input type="submit" name="AddToGroup" value="车间Excel表格总账" onclick="return false;" id="Submit15" class="btn" />
        <input type="submit" name="Translate" value="无线电台调拨单" onclick="return false;" id="Submit16" class="btn" />
        <input type="submit" name="Import" value="配件更换申请单" onclick="return false;" id="Submit17" class="btn" />
        <input type="submit" name="ExportTestReport" value="无线电台修理申请单" onclick="return false;" id="Submit18" class="btn" />
        <input type="submit" name="ExportTestReport" value="增减、闲置表格" onclick="return false;" id="Submit19" class="btn" />
        <input type="submit" name="ExportTestReport" value=" 配件、维修质量问题汇总表" onclick="return false;" id="Submit20" class="btn" />
      </ul>
    </div>
  </div>
<script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>
    </form>
</body>
</html>
