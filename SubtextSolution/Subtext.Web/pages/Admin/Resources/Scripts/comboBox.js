/*
http://particletree.com/features/upgrade-your-select-element-to-a-combo-box/

usage...

<label for="ReferredBy">Referred By</label>
<select name="ReferredBy" class="comboBox">
    <option value=""></option>
    <option value="Friend">Friend</option>
    <option value="Magazine">Magazine</option>

    <option value="TV">TV</option>
</select>


*/
window.onresize=function(){
	setCombobox(false);
}

// ----------------------------------------------------------------- //
// Loop through each select on the page, and check its class.  If it //
// has a class of comboBox, then attach the needed elements       //
// ------------------------------------------------------------------//

var nTop;
var nLeft;
var detect = navigator.userAgent.toLowerCase();

function setCombobox(bMethod) {
	combos=getElementsByClassName('select', "comboBox");
 	for(i=0; i<combos.length; i++) {
		nTop = findPosY(combos[i]);
		nLeft = findPosX(combos[i]);
		if(bMethod == true) {
			inittextfield(combos[i]);
			//Use iframe hack for Internet Explorer
			if(!(detect.indexOf("opera") + 1) && (detect.indexOf("msie") + 1)) {
			initIframe(combos[i]);
			}
		}
		else{
			textfield = document.getElementById("txt" + combos[i].name);
			textfield.style.top = nTop + "px";
			textfield.style.left = nLeft + "px";
			if((detect.indexOf("msie") + 1)) {
			hackFrame = document.getElementById("frame" + combos[i].name);
			hackFrame.style.top = nTop + "px";
			hackFrame.style.left = nLeft + "px";
			}
		}
	}
}

// ------------------------------------------------------------------------ //
// Get all elements with matching class names                        //
// Courtesy of snook - http://www.snook.ca/archives/000370.html //
// ------------------------------------------------------------------------ //
function getElementsByClassName(node, classname)
{
    var a = [];
    var re = new RegExp('(^| )'+classname+'( |$)');
    var els = document.getElementsByTagName(node);
    for(var i=0,j=els.length; i<j; i++)
        if(re.test(els[i].className))a.push(els[i]);
    return a;
}

// ------------------------------------------------------------------ //
// Create the textfield and move it to desired position               //
// ------------------------------------------------------------------ //

function inittextfield(ctrl) {

	selectWidth = ctrl.offsetWidth;  

    //Create textfield
    textfield = document.createElement("input");
	textfield.id = "txt" + ctrl.name;
	textfield.className = "comboText";
	textfield.style.zIndex = "99999";
	
	textfield.value = "Type here.";
	textfield.style.color = "#ccc";
	
	textfield.style.position = "absolute";
    textfield.style.top = nTop + "px";
    textfield.style.left = nLeft + "px";
	textfield.style.border = "none";
	
	//Account for Browser Interface Differences Here
	if((detect.indexOf("safari") + 1)) {
	selectButtonWidth = 18
	textfield.style.marginTop = "0px";
	textfield.style.marginLeft = "0px";
	}
	else if((detect.indexOf("opera") + 1)) {
		selectButtonWidth = 27;
		textfield.style.marginTop = "4px";
		textfield.style.marginLeft = "4px";
	}
	else {
	selectButtonWidth = 27;
	textfield.style.marginTop = "2px";
	textfield.style.marginLeft = "3px";
	}
	
	textfield.style.width = (selectWidth - selectButtonWidth) + "px";
    
	ctrl.parentNode.appendChild(textfield);	
	
	ctrl.onchange=function() {
		val = this.options[this.selectedIndex].value;	
		document.getElementById("txt" + this.name).value = val;
	}
	
	ctrl.onfocus=function() {
		document.getElementById("txt" + this.name).style.color = "#333";
	}

	textfield.onfocus=function() {
			this.style.color = "#333";
	}

}

// ------------------------------------------------------------------ //
// Internet Explorer hack requires an empty iFrame.  We need to add   //
// one right underneath the div -> it will make the zindex work       //
// ------------------------------------------------------------------ //

function initIframe(ctrl) {
	textWidth = textfield.offsetWidth;
	textHeight = textfield.offsetHeight;
    hackFrame = document.createElement("iframe");
    hackFrame.setAttribute("src", "placeHolder.html");
	hackFrame.setAttribute("scrolling", "0");
	hackFrame.setAttribute("tabindex", "-1");
	hackFrame.id = "frame" + ctrl.name;
	hackFrame.style.position = "absolute";
	hackFrame.style.width = textWidth + "px";
	hackFrame.style.height = textHeight + "px";
	hackFrame.style.top = nTop + "px";
	hackFrame.style.left = nLeft + "px";
	hackFrame.style.marginTop = "3px";
	hackFrame.style.marginLeft = "3px";
	ctrl.parentNode.insertBefore(hackFrame, textfield);
}

// ------------------------------------------------------------------------------ //
//  Find the x and y position of the select box and return them                  //
// ----------------------------------------------------------------------------- //

function findPosX(obj)
{
	var curleft = 0;
	if (obj.offsetParent)
	{
		while (obj.offsetParent)
		{
			curleft += obj.offsetLeft
			obj = obj.offsetParent;
			//alert(curleft);
		}
	}
	else if (obj.x)
		curleft += obj.x;
	return curleft;
}

function findPosY(obj)
{
	var curtop = 0;
	if (obj.offsetParent)
	{
		while (obj.offsetParent)
		{
			curtop += obj.offsetTop
			obj = obj.offsetParent;
		}
	}
	else if (obj.y)
		curtop += obj.y;
	return curtop;
}

//
// addLoadEvent()
// Adds event to window.onload without overwriting currently assigned onload functions.
// Function found at Simon Willison's weblog - http://simon.incutio.com/
//
function addLoadEvent(func)
{	
	var oldonload = window.onload;
	if (typeof window.onload != 'function')
	{
    	window.onload = func;
	} 
	else 
	{
		window.onload = function()
		{
			oldonload();
			func();
		}
	}
}

function initComboBox()
{
	setCombobox(true);
}

addLoadEvent(initComboBox);