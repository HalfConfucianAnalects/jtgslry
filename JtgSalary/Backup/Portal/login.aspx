<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="JtgTMS.Portal.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    
    <title>工资签收管理系统</title>
    <link href="~/css/admin/admin.css" rel="stylesheet" type="text/css" />
    <script type='text/javascript' src='~/cssjs/jquery.min.js'></script>
    <script type='text/javascript' src='~/cssjs/admin/admin.js'></script>
    <link href="~/cssjs/showLoading/css/showLoading.css" rel="stylesheet" media="screen" /> 
    <script type="text/javascript" src="~/cssjs/showLoading/js/jquery.showLoading.js"></script>
    <script language="javascript" type="text/javascript">
    function refreshCc() {
	    var ccImg = document.getElementById("checkCodeImg");
	    if (ccImg) {
		    ccImg.src= ccImg.src + '&' +Math.random();
	    }
    }

    function load_script(url, callback){
	    var head = document.getElementsByTagName('head')[0];
	    var script = document.createElement('script');
	    script.type = 'text/javascript';
	    script.src = url;
	    script.onload = script.onreadystatechange = function(){
		    if((!this.readyState || this.readyState === "loaded" || this.readyState === "complete")){
			    callback && callback();
			    // Handle memory leak in IE
			    script.onload = script.onreadystatechange = null;
			    if ( head && script.parentNode ) {
				    head.removeChild( script );
			    }
		    }
	    };
	    // Use insertBefore instead of appendChild  to circumvent an IE6 bug.
	    head.insertBefore( script, head.firstChild );
    }

    function load_iframe(url, callback){
	    var iframe = document.createElement("iframe");
	    iframe.src = url;
	    iframe.width = 0;
	    iframe.height = 0;
	    if (iframe.attachEvent){
		    iframe.attachEvent("onload", function(){
			    callback && callback();
		    });
	    } else {
		    iframe.onload = function(){
			    callback && callback();
		    };
	    }
	    document.body.appendChild(iframe);
    }

    $(document).ready(function(){
	    $('#username').focus();
	    if (window.top.location.href.indexOf('index.aspx') != -1){
		    window.top.location = self.location;
	    }
    });

    function ajax_submit() {
	    $('#login-main').showLoading();
	    var url = 'ajax/form.aspx?action=login';
	    var data = {
			    'username': $('#username').val(),
			    'password': $('#password').val(),
			    'verifyCode': $('#verifyCode').val()
		    };
	    $.post(url, data, function(json){
		    if ('string' == typeof json) {
			    json = eval('(' + json + ')');
		    }
		    if (json.state == '200') {
			    if (json.isSSO == 'true'){
				    var urlList = json.ssoUrls.split(',');
				    if (urlList.length > 0){
					    window.urlCount = urlList.length;
					    for( var i = 0; i < urlList.length; i ++ ){
						    load_script(urlList[i],function(){
							    if (!window.urlLoad) window.urlLoad = 0;
							    if (++window.urlLoad == window.urlCount){
								    window.location.href = 'index.aspx';
							    }
						    });
					    }
				    }else{
					    window.location.href = 'index.aspx';
				    }
			    }
			    else{
				    window.location.href = 'index.aspx';
			    }
		    } else {
			    $('#login-main').hideLoading()
			    if (json.state == '402') {
				    $('#verify_code_msg').html(json.errorMessage).addClass("tips-error").show();
				    $('#username_msg').hide();
			    } else {
				    $('#username_msg').html(json.errorMessage).addClass("tips-error").show();
				    $('#verify_code_msg').hide();
			    }
		    }
	    })
    }
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
            <div class="tit" style="text-align:right"></div>
        </div>
        <div id="login-cont" class="login-cont">
            <form id="loginFrm" action="" runat="server" method="post" >
            <asp:ScriptManager ID="smForm" runat="server" EnableScriptGlobalization="True"/>
                <asp:UpdatePanel ID="upForm" runat="server">
                    <Triggers>
                    </Triggers>
                    <ContentTemplate> 
                        <div class="account1">
                            <label>帐号：</label>
                            <asp:TextBox ID="txtLoginCode" CssClass="input-txt w180" runat="server" autocomplete="off" AutoPostBack="true"
                                ontextchanged="txtLoginCode_TextChanged"></asp:TextBox>					
                            <span id="username_msg"></span>
                        </div>
                        <div class="account1">
                            <label>姓名：</label>
                            <asp:TextBox ID="txtOpName" CssClass="input-txt w180" BackColor="ActiveBorder" ReadOnly="true" runat="server"></asp:TextBox>					
                            <span id="Span1"></span>
                        </div>
                        <div class="account1">
                            <label for="">密码：</label>
                            <asp:TextBox Name="txtPassword" TextMode="Password" ID="txtPassword" CssClass="input-txt w180" autocomplete="off" TabIndex="1" runat="server"></asp:TextBox>
                            <span id="password_msg"></span>
                        </div>
        				
                        <div class="account2" style="display:none">
                            <label for="">验证码：</label>
                            <input class="input-txt w180" id="verifyCode" name="verifyCode" type="text" autocomplete="off" />
                            <span id="verify_code_msg"></span>
                        </div>
                        <div class="account3" style="display:none">
                          <img id="checkCodeImg" src="/sitefiles/services/platform/validateCode2.aspx?cookieName=BAIRONG.VC.LOGIN&isCrossDomain=False" />
                            <a href="javascript:refreshCc();">看不清楚，换一张</a>
                        </div>
                        <asp:Button ID="btnLogin" CssClass="admin-btn" Text="登 录" onfocus="this.blur()" 
                            runat="server" onclick="btnLogin_Click" />
                            <br />
                            <a style="font-size:large; color:Red" href="../upload/chrome_installer.exe">如使用IE6，请下载谷歌浏览器安装使用</a>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </form>
        </div>                    
    </div>
           
</div>
    
</body>
</html>
