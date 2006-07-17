var layout = new Array('fluidLayout', 'fixedLayout', 'jelloLayout');
var sideBar = new Array('leftNav', 'rightNav');
var fontSize = new Array('mediumText', 'largeText', 'xLargeText');

var styleSwitcherRules = {
	'#personalization-help h2 a' : function(el){
		el.onclick = function(){
			Effect.Appear('personalization-instructions');
			this.blur();
			return false;
		}
	},
	'#personalization-instructions-close' : function(el){
		el.onclick = function(){
			Effect.Puff('personalization-instructions');
			this.blur();
			return false;
		}
	},
	'#mediumText' : function(el){
		el.onclick = function(){
			switchStyles('fontSize', 'mediumText');
			this.blur();
			return false;
		}
	},
	'#largeText' : function(el){
		el.onclick = function(){
			switchStyles('fontSize', 'largeText');
			this.blur();
			return false;
		}
	},
	'#xLargeText' : function(el){
		el.onclick = function(){
			switchStyles('fontSize', 'xLargeText');
			this.blur();
			return false;
		}
	},
	'#fluidLayout' : function(el){
		el.onclick = function(){
			switchStyles('layout', 'fluidLayout');
			this.blur();
			return false;
		}
	},
	'#fixedLayout' : function(el){
		el.onclick = function(){
			switchStyles('layout', 'fixedLayout');
			this.blur();
			return false;
		}
	},
	'#jelloLayout' : function(el){
		el.onclick = function(){
			switchStyles('layout', 'jelloLayout');
			this.blur();
			return false;
		}
	},
	'#leftNav' : function(el){
		el.onclick = function(){
			switchStyles('sideBar', 'leftNav');
			this.blur();
			return false;
		}
	},
	'#rightNav' : function(el){
		el.onclick = function(){
			switchStyles('sideBar', 'rightNav');
			this.blur();
			return false;
		}
	}
};

/* rule registration can be fount in inti.js */

function switchStyles(styleType, styleClass){
	/* switch classes */
	switch (styleType) {
		case 'layout':
	   	  for (i=0;i<layout.length;i++){
			  if(checkClass(bodyEl, layout[i])){swapClass(bodyEl, layout[i], '')};
		  }
		break;
		case 'sideBar':
	   	  for (i=0;i<sideBar.length;i++){
			 if(checkClass(bodyEl, sideBar[i])){swapClass(bodyEl, sideBar[i], '')};
		  }
		  break;
		case 'fontSize':
 			for (i=0;i<fontSize.length;i++){
			  if(checkClass(bodyEl, fontSize[i])){swapClass(bodyEl, fontSize[i], '')};
			}
		break;
	}
	
	addClass(bodyEl, styleClass);
	createCookie('styles',bodyEl.className,365);	
}

function setUserStyles(){
	var setClass = 'jelloLayout rightNav mediumText';

	if (getCookie('styles')){
		setClass = readCookie('styles');
	}
	var body = document.getElementsByTagName('body')[0];
       if(body.className == '')
       {
           body.className = setClass;
       }
       else
       {
           body.setAttribute('class', setClass);
       }
}