//
// Script inspired by Asual theme for Blojsom
//
//

function setActiveStyleSheet(title) {
    $("link[rel*='stylesheet'][title]").attr("disabled", "disabled");
    $("link[rel*='stylesheet'][title='" + title + "']").removeAttr("disabled");
}

function getActiveStyleSheet() {
    return $("link:enabled[rel*='stylesheet'][title][disabled!='disabled']").attr('title');
}

function getPreferredStyleSheet() {
    return $("link[rel*='stylesheet'][rel!='alt'][title]").attr('title');
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

var cookie = readCookie("style");
var title = cookie ? cookie : getPreferredStyleSheet();
setActiveStyleSheet(title);

function switchLayout() {
	var title = (getActiveStyleSheet() == "elastic") ? "fixed" : "elastic";
	setActiveStyleSheet(title);
	switchText();
	createCookie("style", title, 365);
}

function reloadPreviewDiv(element) {
    var previewString = element.val();
	if (previewString.length > 0)
	{
		previewString = htmlUnencode(previewString);
		previewString = previewString.replace(new RegExp("(.*)\n\n([^#\*\n\n].*)","g"), "<p>$1</p><p>$2</p>");
		previewString = previewString.replace(new RegExp("(.*)\n([^#\*\n].*)","g"), "$1<br />$2");
	}
	$("#commentPreview").html(previewString);
}

function htmlUnencode(s)
{
	return s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
}

function switchText() {
    var switcherText = (getActiveStyleSheet() == "elastic") ? "Switch to Fixed Layout" : "Switch to Elastic Layout";
    $('#switchlink span').text(switcherText);
}

$(function() {
    switchText();
    $('#switchlink').show();
    $('#switchlink').click(function() {
        switchLayout();
        return false;
    });

    $('div.comment textarea').keyup(function(event) {
        reloadPreviewDiv($(this));
    });
});