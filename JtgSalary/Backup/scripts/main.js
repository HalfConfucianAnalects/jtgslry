function mycarousel_initCallback(carousel){
	 carousel.clip.hover(function() {
	 carousel.stopAuto();
	 }, function() {
	 carousel.startAuto();
	 });
}
function pop(obj){
	$('#'+$(obj).attr('rel')).show().siblings('div').hide();
}

//ÉÌÆ·Í¼Æ¬ä¯ÀÀ
function btnShow(){
	var l = $('#picList >li').length-1;
	var i = $('#picList >li.current').index();
	if(l<1)return false;
	if(i==0){
		$('#picMain >a.prevPic').addClass('prev_disabled');
	}
	else if(i==l){
		$('#picMain >a.nextPic').addClass('next_disabled');
	}
	$('#picMain').children('a').show();
}
function btnHide(){
	$('#picMain').children('a').hide().removeClass('prev_disabled next_disabled');
}
function smallImg(obj){
	var src = obj.find('img').attr('imgsrc');
	obj.addClass('current').siblings().removeClass('current');
	$('#picMain >img').attr('src',src);
}
function leftClick(obj){
	var _this = $(obj);
	if(_this.hasClass('prev_disabled'))return false;
	var i = $('#picList >li.current').index();
	$('#picList >li.current').prev().addClass('current').siblings().removeClass('current');
	if(i==1){
		_this.addClass('prev_disabled');
	}
	_this.siblings('a').removeClass('next_disabled');
	smallImg($('#picList >li.current'));
}
function rightClick(obj){
	var _this = $(obj);
	if(_this.hasClass('next_disabled'))return false;
	var i = $('#picList >li.current').index();
	var l = $('#picList >li').length-1;
	$('#picList >li.current').next().addClass('current').siblings().removeClass('current');
	if(i==(l-1)){
		_this.addClass('next_disabled');		
	}
	_this.siblings('a').removeClass('prev_disabled');
	smallImg($('#picList >li.current'));
}
function process(obj){
	$(obj).find('li').mouseover(function(){
		if(!$(this).hasClass('current')){
			var i = $(this).index();
			$(this).addClass('current').siblings().removeClass('current');
			$(obj).find('>.processBox_switch').children().hide().eq(i).show();
		}
	});
	$(obj).prev().click(function(){
		$(this).next().toggle();
	});
	$(obj).find('>img').click(function(){
		$(this).parent().hide();
	});
}

$(document).ready(function(){
	$('#category').hover(function(){
		$(this).children('div').show();
	},function(){
		$(this).children('div').hide();
	});
	$('#category_list li').hover(function(){
		$(this).addClass('current');
	},function(){
		$(this).removeClass('current');
	});
	$('#category_all').hover(function(){
		$('#category_list').show();
	},function(){
		if($(this).hasClass('homePage'))return false;
		$('#category_list').hide();
	});
	$('#shop_cart').hover(function(){
		if($(this).find('#amount').html()!=='0'){
			$(this).children('div').show();
		}	
	},function(){
		$(this).children('div').hide();
	});
	if($('#recommendProScroll').length>0){
		$('#recommendProScroll').jcarousel({
			auto: 2,
			scroll: 2,
			width:95,
			wrap: 'last',
			animation: 600,
			buttonPrevHTML:'<span class="scrollBtn scrollBtnL"></span>',
			buttonNextHTML:'<span class="scrollBtn scrollBtnR"></span>',
			initCallback: mycarousel_initCallback
		});
	}
	$('#imgSmall li').mouseover(function(){
		if(!$(this).hasClass('current')){
			var src = $(this).children('img').attr('imgsrc');
			var jqimg = $(this).children('img').attr('jqimg');
			$(this).addClass('current').siblings().removeClass('current');
			$('#jqzoomBox').find('img').attr({'src':src,'jqimg':jqimg});
		}
	});
	$('#proTab >span').click(function(){
		if(!$(this).hasClass('current')){
			var i = $(this).index();
			$(this).addClass('current').siblings().removeClass('current').parent().next().children().eq(i).show().siblings().hide();
		}
	});
	if($('#jqzoom').length>0){
		$("#jqzoom").jqueryzoom({
			xzoom: 400,
			yzoom: 300,
			offset: 5,
			position: "right",
			preload:1,
			lens:1
		});
	}
	$('span.popClose').click(function(){
		$(this).parent().hide();
	});
	$('#expand >div.brand').click(function(){
		if($(this).hasClass('unfold')){
			$(this).removeClass('unfold').next().hide();
		}
		else{
			$(this).addClass('unfold').next().show();
		}
	});
	$('#newPro_area_order >a').click(function(){
		if(!$(this).hasClass('current')){
			var i = $(this).text();
			$(this).addClass('current').siblings().removeClass('current').parent().parent().next().children().eq(i-1).show().siblings().hide();
		}
	});
	$('#picList >li').click(function(){
		if(!$(this).hasClass('current')){
			smallImg($(this));
		}
	});
	$('#picMain').hover(function(){
		btnShow();
	},function(){
		btnHide();
	}).find('a.prevPic').click(function(){
		leftClick(this);
	}).next().click(function(){
		rightClick(this);
	});
	process('#processBox');
	process('#serviceProcess');
	$('div.jqZoomPup').live('click',function(){
		window.open($('#jqzoom').attr('src'), "newwindow");
	});
	if($('#contact').length>0){
		var $sidebar = $('#contact'),
		$window = $(window),
		offset = $sidebar.offset(),
		topPadding = 200;
		$window.scroll(function() {
			if ($window.scrollTop() > offset.top) {
				$sidebar.stop().animate({
					marginTop: $window.scrollTop()-offset.top+topPadding
				});
			}
			else {
				$sidebar.stop().animate({
					marginTop: 0
				});
			}
		});
		$sidebar.hover(function(){
			$(this).css('width','178px');
		},function(){
			$(this).css('width','32px');
		});
	}
});