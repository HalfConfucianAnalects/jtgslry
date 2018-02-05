<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Notice_Edit.aspx.cs" Inherits="JtgTMS.Admin.Notice_Edit" EnableEventValidation="false" ValidateRequest="false"%>

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
    <style type="text/css">
        .style1
        {
            height: 29px;
        }
    </style>
    
    <script type="text/javascript" charset="utf-8" src="../kindeditor/kindeditor.js"></script>
    <script type="text/javascript">
        KE.show({
            id: 'txtContent',
            imageUploadJson: '../../../handler/upload_json.ashx',
            fileManagerJson: '../../../handler/file_manager_json.ashx',
            allowFileManager: true,
            afterCreate: function(id) {
                KE.event.ctrl(document, 13, function() {
                    KE.util.setData(id);
                    document.forms['fmForm'].submit();
                });
                KE.event.ctrl(KE.g[id].iframeDoc, 13, function() {
                    KE.util.setData(id);                    
                    document.forms['fmForm'].submit();
                });
            }
        });
    </script> 
    
    <script type="text/javascript">
          function AddAttachments() {
              document.getElementById('attach').innerText = "继续添加附件";

              tb = document.getElementById('attAchments');
              newRow = tb.insertRow();
              newRow.insertCell().innerHTML = "<input name='File' Class='input_k' size='50' type='file'>&nbsp;&nbsp; <img name='a' src='../images/delete.png' onclick='delFile(this.parentElement.parentElement.rowIndex)' />";
          }
          function delFile(index) {
              document.getElementById('attAchments').deleteRow(index);
              tb.rows.length > 0 ? document.getElementById('attach').innerText = "继续添加附件" : document.getElementById('attach').innerText = "添加附件";
          }        
    </script>
</head>
<body>
    <form id="upForm" class="form-inline" enctype="multipart/form-data"  runat="server">
   
   <div>
   
       <ul class="breadcrumb" style="display:none"><li>基础数据 <span class="divider">/</span></li><li>通知管理 <span class="divider">/</span></li><li class="active">编辑通知</li></ul>
        

              <div class="popover popover-static">
                  <h3 class="popover-title">编辑通知</h3>
                      <div class="popover-content">

                        <table class="table noborder table-hover">     
                            <tr>
                            <td>接收部门：</td>
                            <td>
                                <asp:DropDownList ID="ddlOrganID" runat="server"></asp:DropDownList>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtNoticeTitle" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator>
                              </td>
                          </tr>                           
                          <tr>
                            <td>通知标题：</td>
                            <td>
                                <asp:TextBox ID="txtNoticeTitle" MaxLength="128" Width="400" runat="server"></asp:TextBox>
                              <asp:RequiredFieldValidator ID="rfvNoticeTitle" ControlToValidate="txtNoticeTitle" ErrorMessage="*" runat="server"></asp:RequiredFieldValidator>
                              </td>
                          </tr>
                          
                              <tr>
                            <td>通知内容：</td>
                            <td>
                                <div id="main-content1" style="">
                                                    <textarea id="txtContent" name="txtContent" style="width:900px;height:400px;visibility:visible;" runat="server">                                  
                                                    </textarea>     
                                                </div>
                            </td>
                          </tr>
                          <tr>
                                        <td colspan="4">
                                            <div class="field">
                                                <li>
		                                            <label class="FieldLabel">上传附件<b>*</b></label>
		                                            
                                                    <table id="tbFiles" runat="server" width="500px" enableviewstate="true" class="styleTable4">
                                                        <tr>
                                                            <td>
                                                                <asp:Literal ID="ltDownloadFiles" runat="server"></asp:Literal>        
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div><table id="attAchments"></table></div>
                                                                <span>
                                                                    <IMG src="../Images/attach.png">
                                                                    <A id="attach" title="如果您要发送多个附件，您只需多次点击“继续添加附件”即可, 要注意附件总量不能超过发送限制的大小。" onclick="AddAttachments();" href="javascript:;" name="attach">添加附件</A>
                                                                </span>                             
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </li>
	                                        </div>	  
                                        </td>
                                    </tr>
                          
                        </table>
                      
                        <hr />
                        <table class="table noborder">
                          <tr>
                            <td class="center">
                                <asp:Button ID="btnApply" CssClass="btn btn-primary" Text="确 定" onclick="btnApply_Click" runat="server"/>
                                <input type="submit" name="btnReturn" value="返 回" onclick="javascript :history.back(-1);" id="btnReturn" class="btn" />
                            </td>
                          </tr>
                        </table>
                  
                    </div>
                   
               </div>
       
   </div>
   
   
    </form>
</body>
</html>
