//
// Script inspired by Asual theme for Blojsom
//
//

function setActiveStyleSheet(title) {
	var i, a, main;
	for(i=0; (a = document.getElementsByTagName("link")[i]); i++) {
		if(a.getAttribute("rel").indexOf("style") != -1 && a.getAttribute("title")) {
			a.disabled = true;
			if(a.getAttribute("title") == title) a.disabled = false;
		}
	}
}

function getActiveStyleSheet() {
	var i, a;
	for(i=0; (a = document.getElementsByTagName("link")[i]); i++) {
		if(a.getAttribute("rel").indexOf("style") != -1 && a.getAttribute("title") && !a.disabled) return a.getAttribute("title");
	}
	return null;
}

function getPreferredStyleSheet() {
	var i, a;
	for(i=0; (a = document.getElementsByTagName("link")[i]); i++) {
		if(a.getAttribute("rel").indexOf("style") != -1
		&& a.getAttribute("rel").indexOf("alt") == -1
		&& a.getAttribute("title")
		) return a.getAttribute("title");
	}
	return null;
}

function createCookie(name,value,days) {
	if (days) {
		var date = new Date();
		date.setTime(date.getTime()+(days*24*60*60*1000));
		var expires = "; expires="+date.toGMTString();
	}
	else expires = "";
	document.cookie = name+"="+value+expires+"; path=/";
}

function readCookie(name) {
	var nameEQ = name + "=";
	var ca = document.cookie.split(';');
	for(var i=0;i < ca.length;i++) {
		var c = ca[i];
		while (c.charAt(0)==' ') c = c.substring(1,c.length);
		if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
	}
	return null;
}

function writeSwitcher() {
	var switcher = (getActiveStyleSheet() == "elastic") ? "Switch to Fixed Layout" : "Switch to Elastic Layout";
	document.write('<a id="switchlink" href="#" onclick="switchLayout(); return false;" title="Click here to change the content width"><span>' + switcher + '</span></a>');
}

var cookie = readCookie("style");
var title = cookie ? cookie : getPreferredStyleSheet();
setActiveStyleSheet(title);

function switchLayout() {
	var title = (getActiveStyleSheet() == "elastic") ? "fixed" : "elastic";
	setActiveStyleSheet(title);
	document.getElementById("switchlink").firstChild.firstChild.nodeValue = (getActiveStyleSheet() == "elastic") ? "Switch to Fixed Layout" : "Switch to Elastic Layout";
	createCookie("style", title, 365);
}

function reloadPreviewDiv() {
	var previewString = document.getElementById("PostComment_ascx_tbComment").value;
	if (previewString.length > 0)
	{
		previewString = htmlUnencode(previewString);
		previewString = previewString.replace(new RegExp("(.*)\n\n([^#\*\n\n].*)","g"), "<p>$1</p><p>$2</p>");
		previewString = previewString.replace(new RegExp("(.*)\n([^#\*\n].*)","g"), "$1<br />$2");
	}
	document.getElementById("commentPreview").innerHTML = previewString;
}

function htmlUnencode(s)
{
	return s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
}
