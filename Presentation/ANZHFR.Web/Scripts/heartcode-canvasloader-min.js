(function(w){var k=function(b,c){typeof c=="undefined"&&(c={});this.init(b,c)},a=k.prototype,o,p=["canvas","vml"],f=["oval","spiral","square","rect","roundRect"],x=/^\#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$/,v=navigator.appVersion.indexOf("MSIE")!==-1&&parseFloat(navigator.appVersion.split("MSIE")[1])===8?true:false,y=!!document.createElement("canvas").getContext,q=true,n=function(b,c,a){var b=document.createElement(b),d;for(d in a)b[d]=a[d];typeof c!=="undefined"&&c.appendChild(b);return b},m=function(b,
c){for(var a in c)b.style[a]=c[a];return b},t=function(b,c){for(var a in c)b.setAttribute(a,c[a]);return b},u=function(b,c,a,d){b.save();b.translate(c,a);b.rotate(d);b.translate(-c,-a);b.beginPath()};a.init=function(b,c){if(typeof c.safeVML==="boolean")q=c.safeVML;try{this.mum=document.getElementById(b)!==void 0?document.getElementById(b):document.body}catch(a){this.mum=document.body}c.id=typeof c.id!=="undefined"?c.id:"canvasLoader";this.cont=n("div",this.mum,{id:c.id});if(y)o=p[0],this.can=n("canvas",
this.cont),this.con=this.can.getContext("2d"),this.cCan=m(n("canvas",this.cont),{display:"none"}),this.cCon=this.cCan.getContext("2d");else{o=p[1];if(typeof k.vmlSheet==="undefined"){document.getElementsByTagName("head")[0].appendChild(n("style"));k.vmlSheet=document.styleSheets[document.styleSheets.length-1];var d=["group","oval","roundrect","fill"],e;for(e in d)k.vmlSheet.addRule(d[e],"behavior:url(#default#VML); position:absolute;")}this.vml=n("group",this.cont)}this.setColor(this.color);this.draw();
m(this.cont,{display:"none"})};a.cont={};a.can={};a.con={};a.cCan={};a.cCon={};a.timer={};a.activeId=0;a.diameter=40;a.setDiameter=function(b){this.diameter=Math.round(Math.abs(b));this.redraw()};a.getDiameter=function(){return this.diameter};a.cRGB={};a.color="#000000";a.setColor=function(b){this.color=x.test(b)?b:"#000000";this.cRGB=this.getRGB(this.color);this.redraw()};a.getColor=function(){return this.color};a.shape=f[0];a.setShape=function(b){for(var c in f)if(b===f[c]){this.shape=b;this.redraw();
break}};a.getShape=function(){return this.shape};a.density=40;a.setDensity=function(b){this.density=q&&o===p[1]?Math.round(Math.abs(b))<=40?Math.round(Math.abs(b)):40:Math.round(Math.abs(b));if(this.density>360)this.density=360;this.activeId=0;this.redraw()};a.getDensity=function(){return this.density};a.range=1.3;a.setRange=function(b){this.range=Math.abs(b);this.redraw()};a.getRange=function(){return this.range};a.speed=2;a.setSpeed=function(b){this.speed=Math.round(Math.abs(b))};a.getSpeed=function(){return this.speed};
a.fps=24;a.setFPS=function(b){this.fps=Math.round(Math.abs(b));this.reset()};a.getFPS=function(){return this.fps};a.getRGB=function(b){b=b.charAt(0)==="#"?b.substring(1,7):b;return{r:parseInt(b.substring(0,2),16),g:parseInt(b.substring(2,4),16),b:parseInt(b.substring(4,6),16)}};a.draw=function(){var b=0,c,a,d,e,h,k,j,r=this.density,s=Math.round(r*this.range),l,i,q=0;i=this.cCon;var g=this.diameter;if(o===p[0]){i.clearRect(0,0,1E3,1E3);t(this.can,{width:g,height:g});for(t(this.cCan,{width:g,height:g});b<
r;){l=b<=s?1-1/s*b:l=0;k=270-360/r*b;j=k/180*Math.PI;i.fillStyle="rgba("+this.cRGB.r+","+this.cRGB.g+","+this.cRGB.b+","+l.toString()+")";switch(this.shape){case f[0]:case f[1]:c=g*0.07;e=g*0.47+Math.cos(j)*(g*0.47-c)-g*0.47;h=g*0.47+Math.sin(j)*(g*0.47-c)-g*0.47;i.beginPath();this.shape===f[1]?i.arc(g*0.5+e,g*0.5+h,c*l,0,Math.PI*2,false):i.arc(g*0.5+e,g*0.5+h,c,0,Math.PI*2,false);break;case f[2]:c=g*0.12;e=Math.cos(j)*(g*0.47-c)+g*0.5;h=Math.sin(j)*(g*0.47-c)+g*0.5;u(i,e,h,j);i.fillRect(e,h-c*0.5,
c,c);break;case f[3]:case f[4]:a=g*0.3,d=a*0.27,e=Math.cos(j)*(d+(g-d)*0.13)+g*0.5,h=Math.sin(j)*(d+(g-d)*0.13)+g*0.5,u(i,e,h,j),this.shape===f[3]?i.fillRect(e,h-d*0.5,a,d):(c=d*0.55,i.moveTo(e+c,h-d*0.5),i.lineTo(e+a-c,h-d*0.5),i.quadraticCurveTo(e+a,h-d*0.5,e+a,h-d*0.5+c),i.lineTo(e+a,h-d*0.5+d-c),i.quadraticCurveTo(e+a,h-d*0.5+d,e+a-c,h-d*0.5+d),i.lineTo(e+c,h-d*0.5+d),i.quadraticCurveTo(e,h-d*0.5+d,e,h-d*0.5+d-c),i.lineTo(e,h-d*0.5+c),i.quadraticCurveTo(e,h-d*0.5,e+c,h-d*0.5))}i.closePath();i.fill();
i.restore();++b}}else{m(this.cont,{width:g,height:g});m(this.vml,{width:g,height:g});switch(this.shape){case f[0]:case f[1]:j="oval";c=140;break;case f[2]:j="roundrect";c=120;break;case f[3]:case f[4]:j="roundrect",c=300}a=d=c;e=500-d;for(h=-d*0.5;b<r;){l=b<=s?1-1/s*b:l=0;k=270-360/r*b;switch(this.shape){case f[1]:a=d=c*l;e=500-c*0.5-c*l*0.5;h=(c-c*l)*0.5;break;case f[0]:case f[2]:v&&(h=0,this.shape===f[2]&&(e=500-d*0.5));break;case f[3]:case f[4]:a=c*0.95,d=a*0.28,v?(e=0,h=500-d*0.5):(e=500-a,h=
-d*0.5),q=this.shape===f[4]?0.6:0}i=t(m(n("group",this.vml),{width:1E3,height:1E3,rotation:k}),{coordsize:"1000,1000",coordorigin:"-500,-500"});i=m(n(j,i,{stroked:false,arcSize:q}),{width:a,height:d,top:h,left:e});n("fill",i,{color:this.color,opacity:l});++b}}this.tick(true)};a.clean=function(){if(o===p[0])this.con.clearRect(0,0,1E3,1E3);else{var b=this.vml;if(b.hasChildNodes())for(;b.childNodes.length>=1;)b.removeChild(b.firstChild)}};a.redraw=function(){this.clean();this.draw()};a.reset=function(){typeof this.timer===
"number"&&(this.hide(),this.show())};a.tick=function(b){var a=this.con,f=this.diameter;b||(this.activeId+=360/this.density*this.speed);o===p[0]?(a.clearRect(0,0,f,f),u(a,f*0.5,f*0.5,this.activeId/180*Math.PI),a.drawImage(this.cCan,0,0,f,f),a.restore()):(this.activeId>=360&&(this.activeId-=360),m(this.vml,{rotation:this.activeId}))};a.show=function(){if(typeof this.timer!=="number"){var a=this;this.timer=self.setInterval(function(){a.tick()},Math.round(1E3/this.fps));m(this.cont,{display:"block"})}};
a.hide=function(){typeof this.timer==="number"&&(clearInterval(this.timer),delete this.timer,m(this.cont,{display:"none"}))};a.kill=function(){var a=this.cont;typeof this.timer==="number"&&this.hide();o===p[0]?(a.removeChild(this.can),a.removeChild(this.cCan)):a.removeChild(this.vml);for(var c in this)delete this[c]};w.CanvasLoader=k})(window);




var CLoader = (function(j) {
	var options = {},
		el, elType, position, id,
		cl_container, cl_container_width, cl,
		ajaxCall = false;

	j(function() {
		_init();

		j(document).ajaxSuccess(function () {
		    ajaxCall = true;
			_init();
		});
	});

	function _init() {
		j('.loader').each(function(index, elem) {
			el = j(elem);
			position = el.offset();
			id = 'canvasLoader_' + index;

			elType = el[0].type;
			elTYpe = (elType ? elType.toLowerCase() : undefined);
			if (elType == 'button' || elType == 'submit' || elType == 'reset' || elType == 'file') {
				elType = 'button';
			}

			if (!el.data('loader')) {
			    if (ajaxCall) {
			        j('#' + id + '_container').remove();
			    }

				if (elType == 'button') {
					j('body').append('<div id="' + id + '_container" class="canvasLoader_container"><div class="canvasLoader_bg"></div><div id="' + id + '"></div></div>');
				} else {
					el.html('<div id="' + id + '_container" class="canvasLoader_container"><div class="canvasLoader_bg"></div><div id="' + id + '"></div></div>');
				}
				el.attr({
					'data-type': elType,
					'data-loader': id
				});

				cl_container_width = (el.outerWidth() < el.outerHeight() ? el.outerWidth() : el.outerHeight()) - 4;
				cl_container_width = (cl_container_width > 35 ? 35 : cl_container_width);

				cl_container = j('#' + id + '_container');

				if (elType != 'button') {
					cl_container.parent().css({
						'position': 'relative'
					});
				}

				cl_container.css({
					'position': 'absolute',
					'overflow': 'hidden',
					'top': (elType == 'button' ? position.top : '50%'),
					'left': (elType == 'button' ? position.left : '50%'),
					'width': (elType == 'button' ? el.outerWidth() : cl_container_width),
					'height': (elType == 'button' ? el.outerHeight() : cl_container_width),
					'margin-top': (elType == 'button' ? '' : (cl_container_width * -0.5)),
					'margin-left': (elType == 'button' ? '' : (cl_container_width * -0.5)),
					'z-index': 999999
				}).children('.canvasLoader_bg').css({
					'background': (el.data('background') ? el.data('background') : '#fff'),
					'opacity': (el.data('opacity') ? el.data('opacity') : 0.8),
					'position': 'absolute',
					'top': 0,
					'left': 0,
					'width': '100%',
					'height': '100%'
				});

				_activate(el);
			}
		});

		ajaxCall = false;
	}

	function _activate(el) {
		cl = new CanvasLoader(el.data('loader'));

		options.diameter = (el.outerWidth() < el.outerHeight() ? el.outerWidth() : el.outerHeight()) - 4;
		options.diameter = (options.diameter > 35 ? 35 : options.diameter);

		options.shape = el.data('shape');
		options.color = el.data('color');
		options.density = el.data('density');
		options.range = el.data('range');
		options.speed = el.data('speed');
		options.fps = el.data('fps');

		cl.setShape(options.shape ? options.shape : 'spiral'); // default is 'oval'
		cl.setColor(options.color ? options.color : '#000000'); // default is '#000000'
		cl.setDiameter(options.diameter); // default is 35
		cl.setDensity(options.density ? options.density : 50); // default is 40
		cl.setRange(options.range ? options.range : 1); // default is 1.3
		cl.setSpeed(options.speed ? options.speed : 2); // default is 2
		cl.setFPS(options.fps ? options.fps : 25); // default is 24
		cl.show(); // Hidden by default

		j("#canvasLoader")
			.attr('id', el.data('loader') + '_loader')
			.css({
				'position': 'absolute',
  				'top': '50%',
  				'left': '50%',
  				'margin-top': (options.diameter * -0.5),
  				'margin-left': (options.diameter * -0.5)
			});

		if (el.data('visible') != true) {
			j('#' + el.data('loader') + '_container').hide();
		}
	}

	return {
		reset: function() {
			_init();
		},

		show: function (selector) {
			j(selector).each(function (index, _el) {
				var el = j(_el);

			    if (_el.type == 'button' || _el.type == 'submit' || _el.type == 'reset') {
			    	//el.css('opacity', '0.5');
			    	el.attr('disabled', 'disabled').addClass('disabled');
			        j('#' + el.data('loader') + '_container .canvasLoader_bg').css({
			            'background': 'transparent'
			        });
			    }

			    var el_loader = j('#' + el.data('loader') + '_container');

			    if (el.data('type') == 'button') {
			    	el_loader.css({
			    		'top': el.offset().top,
			    		'left': el.offset().left,
			    		'width': el.outerWidth(),
			    		'height': el.outerHeight()
			    	}).fadeIn();

			    	j(window).resize(function () {
			    		//el_loader.hide();

			    		//setTimeout(function () {
			    			el_loader.css({
			    				'top': el.offset().top,
			    				'left': el.offset().left,
			    				'width': el.outerWidth(),
			    				'height': el.outerHeight()
			    			});
			    		//}, 500);
			    	}).resize();
				} else {
			    	el_loader.fadeIn();
				}
			});
		},

		hide: function (selector) {
			j(selector).each(function (index, _el) {
				//j('#' + j(el).css('opacity', 1).data('loader') + '_container').fadeOut();

				var el = j(_el);

				if (_el.type == 'button' || _el.type == 'submit' || _el.type == 'reset') {
					el.removeAttr('disabled', 'disabled').removeClass('disabled');
					j('#' + el.data('loader') + '_container').fadeOut();
				}
			});
		}
	}
})(jQuery);
