<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="framework_left.aspx.cs" Inherits="JtgTMS.Platform.framework_left" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <script language="javascript" src="/SiteFiles/bairong/jquery/jquery-1.8.3.min.js"></script>
    <script language="javascript" src="/SiteFiles/bairong/scripts/datepicker/wdatepicker.js"></script>

    <link rel="stylesheet" type="text/css" href="/SiteFiles/bairong/jquery/bootstrap/css/bootstrap.min.css">
    <script language="javascript" src="/SiteFiles/bairong/jquery/bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        (function ($) {
            $(document).ready(function () {
                if ($.isFunction($.bootstrapIE6)) $.bootstrapIE6($(document));
            });
        })(jQuery);
    </script>

    <link rel="stylesheet" href="../inc/style.css" type="text/css" />
    <script language="javascript" src="../inc/script.js"></script>
</head>
<body>
    <script type="text/javascript" language="javascript">
        jQuery.fn.center = function () {
            this.css("position", "absolute");
            var t = ($(window).height() - this.height() - 150) / 2;
            if (t <= 0) t = 10;
            var top = t + $(window).scrollTop();
            if (top < 0) top = $(window).height() >= this.height() ? 10 : 0;
            this.css("top", top + "px");
            var left = ($(window).width() - this.width()) / 2 + $(window).scrollLeft();
            if ($(window).width() <= this.width() + 20) left = 0;
            this.css("margin-left", "0");
            this.css("left", left + "px");
            return this;
        }
        function openWindow(title, url, width, height, isCloseOnly) {
            if (width == '0') width = $(window).width() - 40;
            if (height == '0') height = $(window).height() - 20;
            if (!width) width = 450;
            if (!height) height = 350;
            $('#openWindowModal h3').html(title);
            $('#openWindowBtn').show();
            if (isCloseOnly == 'true') $('#openWindowBtn').hide();
            $('#openWindowIFrame').attr('src', url);
            $('#openWindowModal').width(width);
            $('#openWindowModal .modal-body').css('max-height', '9999px');
            $('#openWindowModal').height(height);
            $('#openWindowModal .modal-body').height(height - 115);
            $('#openWindowIFrame').height(height - 125);
            $('#openWindowModal').center();
            //$("body").eq(0).css("overflow","hidden");
            $('#openWindowModal').modal({ keyboard: true });
        }
        function closeWindow() {
            $('#openWindowModal').modal('hide');
        }
        $(document).ready(function () {
            $('#openWindowBtn').click(function (e) {
                //$('#openWindowBtn').button('loading');
                if ($('#openWindowIFrame').contents().find("#btnSubmit").length > 0) {
                    $('#openWindowIFrame').contents().find("#btnSubmit").click();
                } else {
                    $('#openWindowIFrame').contents().find("form").submit();
                }
            });
            $('#openWindowModal').bind('hidden', function () {
                //$("body").eq(0).css("overflow","scroll");
                $('#openWindowIFrame').attr('src', '');
                //$('#openWindowBtn').button('reset');
            });
        });

        function openTips(tips, type) {
            $('#alertType').removeClass();
            if (!type) type = "info";
            if (type == "success") {
                $('#alertType').addClass('alert alert-success');
            } else if (type == "error") {
                $('#alertType').addClass('alert alert-error');
            } else if (type == "info") {
                $('#alertType').addClass('alert alert-info');
            } else if (type == "warn") {
                $('#alertType').addClass('alert alert-block');
            }
            $('#alertType').html(tips);
            $('#openTipsModal').modal();
        }
    </script>
    <div id="openWindowModal" class="modal hide fade form-horizontal">
        <div class="modal-header" style="height: 30px;">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                <img src="../../sitefiles/bairong/icons/close.png" /></button>
            <h3></h3>
        </div>
        <div class="modal-body" style="width: 100%; height: 100%; padding: 5px 0; margin: 0;">
            <iframe id="openWindowIFrame" style="width: 100%; height: 100%; background-color: #ffffff;" scrolling="auto" frameborder="0" width="100%" height="100%"></iframe>
        </div>
        <div class="modal-footer">
            <button id="openWindowBtn" class="btn btn-primary" data-loading-text="提交中...">确 定</button>
            <button class="btn" data-dismiss="modal" aria-hidden="true">取 消</button>
        </div>
    </div>

    <div id="openTipsModal" class="modal hide">
        <div class="modal-header">
            <button class="close" data-dismiss="modal">×</button>
            <h3>提示</h3>
        </div>
        <div class="modal-body">
            <div id="alertType" class="alert alert-info"></div>
        </div>
        <div class="modal-footer">
            <button class="btn" data-dismiss="modal" aria-hidden="true">关 闭</button>
        </div>
    </div>
    <style type="text/css">
        body {
            padding: 0;
            margin: 0;
        }

        .container, .dropdown, .dropdown-menu {
            margin-left: 6px;
            float: none;
        }

        .navbar, .navbar-inner, .nav {
            margin-bottom: 5px;
            padding: 0;
        }

        .dropdown, .dropdown-toggle {
            width: 100%;
        }

        .navbar-inner {
            height: 35px;
            min-height: 35px;
        }

        .table-condensed td {
            padding: 2px 5px;
        }
    </style>
    <div class="container" style="height: 50px; width: 153px;">
        <div class="navbar navbar-fixed-top">
            <div class="navbar-inner">
                <ul class="nav">
                    <li><a href="#" style="font-size: 14px; padding-left: 20px;">
                        <asp:Label ID="lbModuleTitle" runat="server"></asp:Label></a></li>
                </ul>
            </div>
        </div>
    </div>


    <script language="JavaScript">
        //取得Tree的级别，1为第一级
        function getTreeLevel(e) {
            var length = 0;
            if (!isNull(e)) {
                if (e.tagName == 'TR') {
                    length = parseInt(e.getAttribute('treeItemLevel'));
                }
            }
            return length;
        }

        function closeAllFolder() {
            if (isNodeTree) {
                var imgCol = document.getElementsByTagName('IMG');
                if (!isNull(imgCol)) {
                    for (x = 0; x < imgCol.length; x++) {
                        if (!isNull(imgCol.item(x).getAttribute('src'))) {
                            if (imgCol.item(x).getAttribute('src') == '/sitefiles/bairong/icons/tree/openedfolder.gif') {
                                imgCol.item(x).setAttribute('src', '/sitefiles/bairong/icons/tree/folder.gif');
                            }
                        }
                    }
                }
            }

            var aCol = document.getElementsByTagName('A');
            if (!isNull(aCol)) {
                for (x = 0; x < aCol.length; x++) {
                    if (aCol.item(x).getAttribute('isTreeLink') == 'true') {
                        aCol.item(x).style.fontWeight = 'normal';
                    }
                }
            }
        }

        function openFolderByA(element) {
            closeAllFolder();
            if (isNull(element) || element.tagName != 'A') return;

            element.style.fontWeight = 'bold';

            if (isNodeTree) {
                for (element = element.previousSibling; ;) {
                    if (element != null && element.tagName == 'A') {
                        element = element.firstChild;
                    }
                    if (element != null && element.tagName == 'IMG') {
                        if (element.getAttribute('src') == '/sitefiles/bairong/icons/tree/folder.gif') break;
                        break;
                    } else {
                        element = element.previousSibling;
                    }
                }
                if (!isNull(element)) {
                    element.setAttribute('src', '/sitefiles/bairong/icons/tree/openedfolder.gif');
                }
            }
        }

        function getTrElement(element) {
            if (isNull(element)) return;
            for (element = element.parentNode; ;) {
                if (element != null && element.tagName == 'TR') {
                    break;
                } else {
                    element = element.parentNode;
                }
            }
            return element;
        }

        function getImgClickableElementByTr(element) {
            if (isNull(element) || element.tagName != 'TR') return;
            var img = null;
            if (!isNull(element.childNodes)) {
                var imgCol = element.getElementsByTagName('IMG');
                if (!isNull(imgCol)) {
                    for (x = 0; x < imgCol.length; x++) {
                        if (!isNull(imgCol.item(x).getAttribute('isOpen'))) {
                            img = imgCol.item(x);
                            break;
                        }
                    }
                }
            }
            return img;
        }

        //显示、隐藏下级目录
        function displayChildren(element) {
            if (isNull(element)) return;

            var tr = getTrElement(element);

            var img = getImgClickableElementByTr(tr);//需要变换的加减图标

            if (!isNull(img) && img.getAttribute('isOpen') != null) {
                if (img.getAttribute('isOpen') == 'false') {
                    img.setAttribute('isOpen', 'true');
                    img.setAttribute('src', '/sitefiles/bairong/icons/tree/minus.gif');
                } else {
                    img.setAttribute('isOpen', 'false');
                    img.setAttribute('src', '/sitefiles/bairong/icons/tree/plus.gif');
                }
            }

            var level = getTreeLevel(tr);//点击项菜单的级别

            var collection = new Array();
            var index = 0;

            for (var e = tr.nextSibling; !isNull(e) ; e = e.nextSibling) {
                if (!isNull(e) && !isNull(e.tagName) && e.tagName == 'TR') {
                    var currentLevel = getTreeLevel(e);
                    if (currentLevel <= level) break;
                    if (e.style.display == '') {
                        e.style.display = 'none';
                    } else {//展开
                        if (currentLevel != level + 1) continue;
                        e.style.display = '';
                        var imgClickable = getImgClickableElementByTr(e);
                        if (!isNull(imgClickable)) {
                            if (!isNull(imgClickable.getAttribute('isOpen')) && imgClickable.getAttribute('isOpen') == 'true') {
                                imgClickable.setAttribute('isOpen', 'false');
                                imgClickable.setAttribute('src', '/sitefiles/bairong/icons/tree/plus.gif');
                                collection[index] = imgClickable;
                                index++;
                            }
                        }
                    }
                }
            }

            if (index > 0) {
                for (i = 0; i <= index; i++) {
                    displayChildren(collection[i]);
                }
            }
        }
        var isNodeTree = false;
    </script>

    <form name="ctl00" method="post" action="framework_left.aspx?module=BBS&amp;menuID=Forums&amp;isPlatform=False&amp;title=%u8bba%u575b%u7ba1%u7406" id="ctl00">
        <div>
            <input type="hidden" name="__VIEWSTATE" id="__VIEWSTATE" value="/wEPDwULLTEyNjM4MTU3NzdkZJNV8wyjshfI893WMeowbObn81Yr" />
        </div>

        <table class="table table-condensed noborder table-hover">
            <asp:Literal ID="ltFunc" runat="server"></asp:Literal>
        </table>
    </form>

</body>
</html>
<!-- check for 3.6 html permissions -->
<script type="text/javascript">
    if (window.top.location.href.toLowerCase().indexOf("main.aspx") == -1) {
        var initializationUrl = window.top.location.href.toLowerCase().substring(0, window.top.location.href.toLowerCase().indexOf("/siteserver/")) + "/siteserver/initialization.aspx";
        window.top.location.href = initializationUrl;
    }
</script>
