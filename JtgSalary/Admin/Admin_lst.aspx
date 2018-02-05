<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin_lst.aspx.cs" Inherits="JtgTMS.Admin.Admin_lst" %>

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
    

  <ul class="breadcrumb" style="display:none"><li>站点管理 <span class="divider">/</span></li><li>信息管理 <span class="divider">/</span></li><li class="active">内容搜索</li></ul>
  

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
          时间：从
          <input name="DateFrom" type="text" size="12" id="DateFrom" class="input-small" onfocus="WdatePicker({isShowClear:false,readOnly:true,dateFmt:'yyyy-MM-dd'});" />
          &nbsp;到&nbsp;
          <input name="DateTo" type="text" size="12" id="DateTo" class="input-small" onfocus="WdatePicker({isShowClear:false,readOnly:true,dateFmt:'yyyy-MM-dd'});" />
          目标：
          <select name="SearchType" id="SearchType" class="input-small">
	        <option selected="selected" value="Title">标题</option>
	        <option value="SubTitle">副标题</option>
	        <option value="ImageUrl">图片</option>
	        <option value="VideoUrl">视频</option>
	        <option value="FileUrl">附件</option>
	        <option value="LinkUrl">外部链接</option>
	        <option value="Content">内容</option>
	        <option value="Summary">内容摘要</option>
	        <option value="Author">作者</option>
	        <option value="Source">来源</option>
	        <option value="IsRecommend">推荐</option>
	        <option value="IsHot">热点</option>
	        <option value="IsColor">醒目</option>
	        <option value="IsTop">置顶</option>
	        <option value="AddDate">添加日期</option>
	        <option value="ID">内容ID</option>
	        <option value="AddUserName">添加者</option>
	        <option value="LastEditUserName">最后修改者</option>
        </select>
          关键字：
          <input name="Keyword" type="text" maxlength="500" id="Keyword" Size="37" />
          <input type="submit" name="Search" value="搜 索" id="Search" class="btn" />
        </td>
      </tr>
    </table>
  </div>

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td>内容标题(点击查看) </td>
      <td>栏目</td>
      
      <td width="50"> 状态 </td>
      <td width="30">&nbsp;</td>
      <td class="center" width="50">&nbsp;</td>
      <td width="20">
        <input type="checkbox" onClick="selectRows(document.getElementById('contents'), this.checked);">
      </td>
    </tr>
    
        <tr>
          <td>
            <a href='/contents/1/3.html' target='blank'>4</a>
          </td>
          <td>
            首页
          </td>
          
          <td class="center" nowrap>
            <a href="javascript:;" title="设置内容状态" onclick="openWindow('审核状态','modal_checkState.aspx?PublishmentSystemID=1&NodeID=1&ContentID=3&ReturnUrl=background_contentSearch.aspx_question_PublishmentSystemID_equals_1_and_NodeID_equals_1_and_State_equals_All_and_IsDuplicate_equals_False_and_SearchType_equals_Title_and_Keyword_equals__and_DateFrom_equals__and_DateTo_equals_',560,500,'false');return false;">已审核</a>
          </td>
          <td class="center">
            <a href="background_contentAdd.aspx?PublishmentSystemID=1&NodeID=1&ID=3&ReturnUrl=background_contentSearch.aspx_question_PublishmentSystemID_equals_1_and_NodeID_equals_1_and_State_equals_All_and_IsDuplicate_equals_False_and_SearchType_equals_Title_and_Keyword_equals__and_DateFrom_equals__and_DateTo_equals_">编辑</a>
          </td>
          <td class="center" width="50"><a href="background_comment.aspx?PublishmentSystemID=1&NodeID=1&ContentID=3&ReturnUrl=background_contentSearch.aspx_question_PublishmentSystemID_equals_1_and_NodeID_equals_1_and_State_equals_All_and_IsDuplicate_equals_False_and_SearchType_equals_Title_and_Keyword_equals__and_DateFrom_equals__and_DateTo_equals_">评论</a><span style="color:gray">(0)</span></td>
          <td class="center">
            <input type="checkbox" name="IDsCollection" value="1_3" />
          </td>
        </tr>
      
        <tr>
          <td>
            <a href='/contents/1/2.html' target='blank'>TT</a>
          </td>
          <td>
            首页
          </td>
          
          <td class="center" nowrap>
            <a href="javascript:;" title="设置内容状态" onclick="openWindow('审核状态','modal_checkState.aspx?PublishmentSystemID=1&NodeID=1&ContentID=2&ReturnUrl=background_contentSearch.aspx_question_PublishmentSystemID_equals_1_and_NodeID_equals_1_and_State_equals_All_and_IsDuplicate_equals_False_and_SearchType_equals_Title_and_Keyword_equals__and_DateFrom_equals__and_DateTo_equals_',560,500,'false');return false;">已审核</a>
          </td>
          <td class="center">
            <a href="background_contentAdd.aspx?PublishmentSystemID=1&NodeID=1&ID=2&ReturnUrl=background_contentSearch.aspx_question_PublishmentSystemID_equals_1_and_NodeID_equals_1_and_State_equals_All_and_IsDuplicate_equals_False_and_SearchType_equals_Title_and_Keyword_equals__and_DateFrom_equals__and_DateTo_equals_">编辑</a>
          </td>
          <td class="center" width="50"><a href="background_comment.aspx?PublishmentSystemID=1&NodeID=1&ContentID=2&ReturnUrl=background_contentSearch.aspx_question_PublishmentSystemID_equals_1_and_NodeID_equals_1_and_State_equals_All_and_IsDuplicate_equals_False_and_SearchType_equals_Title_and_Keyword_equals__and_DateFrom_equals__and_DateTo_equals_">评论</a><span style="color:gray">(0)</span></td>
          <td class="center">
            <input type="checkbox" name="IDsCollection" value="1_2" />
          </td>
        </tr>
      
  </table>

  <table id="spContents" class="table table-pager" border="0">

</table>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button ID="AddContent" Text="添加发票" CssClass="btn" runat="server" />
    <input type="submit" name="AddContent" value="添加发票" id="AddContent" class="btn" />
    <input type="submit" name="SelectButton" value="选择显示项" onclick="openWindow('选择需要显示的项','modal_selectColumns.aspx?PublishmentSystemID=1&amp;RelatedIdentity=1&amp;IsContent=True&amp;IsList=True',520,550,'false');return false;" id="SelectButton" class="btn" />
    <input type="submit" name="AddToGroup" value="添加到内容组" onclick="if (!_alertCheckBoxCollection(document.getElementsByName('IDsCollection'), '请选择需要设置组别的内容！')){openWindow('添加到内容组', 'modal_addToGroup.aspx?PublishmentSystemID=1&amp;IsContent=True' + '&amp;IDsCollection=' + _getCheckBoxCollectionValue(document.getElementsByName('IDsCollection')),450, 420, 'false');}return false;" id="AddToGroup" class="btn" />
    <input type="submit" name="Translate" value="转 移" onclick="if (!_alertCheckBoxCollection(document.getElementsByName('IDsCollection'), '请选择需要转移的内容！')){_goto('background_contentTranslate.aspx?PublishmentSystemID=1&amp;ReturnUrl=background_contentSearch.aspx_question_PublishmentSystemID_equals_1_and_NodeID_equals_1_and_State_equals_All_and_IsDuplicate_equals_False_and_SearchType_equals_Title_and_Keyword_equals__and_DateFrom_equals__and_DateTo_equals_' + '&amp;IDsCollection=' + _getCheckBoxCollectionValue(document.getElementsByName('IDsCollection')));};return false;" id="Translate" class="btn" />
    <input type="submit" name="Check" value="审 核" onclick="if (!_alertCheckBoxCollection(document.getElementsByName('IDsCollection'), '请选择需要审核的内容！')){openWindow('审核内容', 'modal_contentCheck.aspx?PublishmentSystemID=1&amp;ReturnUrl=background_contentSearch.aspx_question_PublishmentSystemID_equals_1_and_NodeID_equals_1_and_State_equals_All_and_IsDuplicate_equals_False_and_SearchType_equals_Title_and_Keyword_equals__and_DateFrom_equals__and_DateTo_equals_' + '&amp;IDsCollection=' + _getCheckBoxCollectionValue(document.getElementsByName('IDsCollection')),560, 550, 'false');}return false;" id="Check" class="btn" />
    <input type="submit" name="Delete" value="删 除" onclick="if (!_alertCheckBoxCollection(document.getElementsByName('IDsCollection'), '请选择需要删除的内容！')){_goto('background_contentDelete.aspx?PublishmentSystemID=1&amp;IsDeleteFromTrash=False&amp;ReturnUrl=background_contentSearch.aspx_question_PublishmentSystemID_equals_1_and_NodeID_equals_1_and_State_equals_All_and_IsDuplicate_equals_False_and_SearchType_equals_Title_and_Keyword_equals__and_DateFrom_equals__and_DateTo_equals_' + '&amp;IDsCollection=' + _getCheckBoxCollectionValue(document.getElementsByName('IDsCollection')));};return false;" id="Delete" class="btn" />
  </ul>


<div>
	
</div>
<script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>
    </form>
</body>
</html>
