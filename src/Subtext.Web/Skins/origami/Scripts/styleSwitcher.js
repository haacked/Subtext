var layout = new Array('fluidLayout', 'fixedLayout', 'jelloLayout');
var sideBar = new Array('leftNav', 'rightNav');
var fontSize = new Array('mediumText', 'largeText', 'xLargeText');

function attachStyleSwitcherHandlers() {
    $('#personalization-help h2 a').click(function() {
        $('#personalization-instructions').slideDown();
        $(this).blur();
        return false;
    });
    
    $('#personalization-instructions-close').click(function() {
        $('#personalization-instructions').fadeOut();
        $(this).blur();
        return false;
    });

    $('#mediumText').click(function() {
        switchStyles('fontSize', 'mediumText');
        this.blur();
        return false;
    });
    
    $('#largeText').click(function() {
        switchStyles('fontSize', 'largeText');
        this.blur();
        return false;
    });

    $('#xLargeText').click(function() {
        switchStyles('fontSize', 'xLargeText');
        this.blur();
        return false;
    });

    $('#fluidLayout').click(function() {
        switchStyles('layout', 'fluidLayout');
        this.blur();
        return false;
    });

    $('#fixedLayout').click(function() {
        switchStyles('layout', 'fixedLayout');
        this.blur();
        return false;
    });

    $('#jelloLayout').click(function() {
        switchStyles('layout', 'jelloLayout');
        this.blur();
        return false;
    });

    $('#leftNav').click(function() {
        switchStyles('sideBar', 'leftNav');
        this.blur();
        return false;
    });

    $('#rightNav').click(function() {
        switchStyles('sideBar', 'rightNav');
        this.blur();
        return false;
    });
}

var bodyEl;
function switchStyles(styleType, styleClass){
	/* switch classes */
	switch (styleType) {
	    case 'layout':
	        for (i = 0; i < layout.length; i++) {
	            $('body').removeClass(layout[i]);
	        }
	        break;
		case 'sideBar':
	   	  for (i=0;i<sideBar.length;i++){
	   	      $('body').removeClass(sideBar[i]);
		  }
		  break;
		case 'fontSize':
 			for (i=0;i<fontSize.length;i++){
 			    $('body').removeClass(fontSize[i]);
			}
		break;
	}
	
	$('body').addClass(styleClass);
	createCookie('styles', bodyEl.className, 365);	
}

function setUserStyles(){
    var cssClasses = 'jelloLayout rightNav mediumText';
    if (getCookie('styles')){
        cssClasses = getCookie('styles');
	}
	$('body').addClass(cssClasses);
}

/* -- BASIC COOKIE MANIPULATION METHODS -- */
function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

/**
* Gets the value of the specified cookie.
*
* name  Name of the desired cookie.
*
* Returns a string containing value of specified cookie,
*   or null if cookie does not exist.
*/
function getCookie(name) {
    var dc = document.cookie;
    var prefix = name + "=";
    var begin = dc.indexOf("; " + prefix);
    if (begin == -1) {
        begin = dc.indexOf(prefix);
        if (begin != 0) return null;
    } else {
        begin += 2;
    }
    var end = document.cookie.indexOf(";", begin);
    if (end == -1) {
        end = dc.length;
    }
    return unescape(decodeURI(dc.substring(begin + prefix.length, end))).split("+").join(" ");
}

$(function() {
    bodyEl = $('body')[0];
    setUserStyles();
    attachStyleSwitcherHandlers();

    // Toggle comment form
    $('#comment-form-toggle').click(function() {
        var optional = $('#optional-fields');
        if(!optional.is(':visible')) {
            $('#optional-fields').slideDown();
        }
        else {
            $('#optional-fields').slideUp();
        }
        return false;
    });
});