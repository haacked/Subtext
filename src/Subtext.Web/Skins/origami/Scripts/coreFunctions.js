/* -- BASIC CLASS MANIPULATION AND TESTING METHODS -- */

// add class to the element
function addClass(o,c){
	if(!checkClass(o,c)){o.className+=o.className==''?c:' '+c;}
}

// add class to the element
function removeClass(o,c){
	if(checkClass(o,c)){o.className+=o.className==''?c:-c;}
}

// swap classes
function swapClass(o,c1,c2){
	var cn=o.className
	o.className=!checkClass(o,c1)?cn.replace(c2,c1):cn.replace(c1,c2);
}

// check if an element has the defined class
function checkClass(o,c){
	return new RegExp('\\b'+c+'\\b').test(o.className);
}

/* -- BASIC COOKIE MANIPULATION METHODS -- */
function createCookie(name,value,days)
{
	if (days)
	{
		var date = new Date();
		date.setTime(date.getTime()+(days*24*60*60*1000));
		var expires = "; expires="+date.toGMTString();
	}
	else var expires = "";
	document.cookie = name+"="+value+expires+"; path=/";
}

function readCookie(name)
{
	var nameEQ = name + "=";
	var ca = document.cookie.split(';');
	for(var i=0;i < ca.length;i++)
	{
		var c = ca[i];
		while (c.charAt(0)==' ') c = c.substring(1,c.length);
		if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
	}
	return null;
}

function eraseCookie(name)
{
	createCookie(name,"",-1);
}