/******************************************************************************
* DHTML Tooltips with Amazon Referral Link Image
* by Travis Illig
* based on the DHTML tooltip script from Dynamic Drive (copyright notice below).
*
* Rewrites links of the form
*
* <a href="http://www.amazon.com/exec/obidos/ASIN/[10-digit-ASIN]/[affiliateID]">text</a>
*
* to have a hovering tooltip displaying a small image of the item referred to.
* Works for www.amazon.co.uk or any other www.amazon.* server.  The images will
* all be taken from the server referred to in the constants section of the
* code, below (which should be fine for people of all locales).
*
* To use, just add a reference to this script to the bottom of the page, just
* before the closing body tag:
* <script type="text/javascript" src="dhtmltooltip.js"></script>
*
* The script will automatically rewrite all referral links with tooltips.
*
* You can also provide text-based tooltips using the Dynamic Drive methods
* as discussed here:
* http://www.dynamicdrive.com/dynamicindex5/dhtmltooltip.htm
*
* Note that all Dynamic Drive methods and variables have been prefixed with
* "DDRIVE_" (for example, "DDRIVE_hidetip" instead of just "hidetip"), so
* when referring to the Dynamic Drive documentation, keep that in mind.
******************************************************************************/

/***********************************************
* Cool DHTML tooltip script- © Dynamic Drive DHTML code library (www.dynamicdrive.com)
* This notice MUST stay intact for legal use
* Visit Dynamic Drive at http://www.dynamicdrive.com/ for full source code
***********************************************/

// Constant Initialization
var AMAZON_imageserver = "images.amazon.com";
var DDRIVE_offsetxpoint = -60; //Customize x offset of tooltip
var DDRIVE_offsetypoint = 20; //Customize y offset of tooltip
var DDRIVE_ie = document.all;
var DDRIVE_ns6 = document.getElementById && !document.all;

// Variable Initialization
var DDRIVE_enabletip = false;
var DDRIVE_tipobj = null;

// Add the tooltip pane with requisite styling
function createToolTipDiv() 
{
	var div = document.createElement("div");
	div.id = "DDRIVE_dhtmltooltip";
	div.style.position = 'absolute';
	div.style.width = '150px';
	div.style.border = '3px double black';
	div.style.padding = '2px;';
	div.style.backgroundcolor = '#fff';
	div.style.display = 'none';
	div.style.zindex = '100';
	div.style.filter = 'filter: progid:DXImageTransform.Microsoft.Shadow(color=gray,direction=135)';
	document.body.appendChild(div);
	return div;
}


// Set the tooltip pane object
if (ToolTipCompatibleBrowser()){
	DDRIVE_tipobj = document.all ? document.all["DDRIVE_dhtmltooltip"] : document.getElementById? document.getElementById("DDRIVE_dhtmltooltip") : "";
}
	
// Gets the "body" element
function DDRIVE_ietruebody(){
	return (document.compatMode && document.compatMode!="BackCompat") ? document.documentElement : document.body;
}

// Shows a tooltip
function DDRIVE_tooltip(thetext, thecolor, thewidth){
	if (ToolTipCompatibleBrowser()){
		if (typeof thewidth!="undefined"){
			DDRIVE_tipobj.style.width = thewidth + "px";
		}
		else{
			DDRIVE_tipobj.style.width = "";
		}
		if (typeof thecolor!="undefined" && thecolor!=""){
			DDRIVE_tipobj.style.backgroundColor=thecolor;
		}
		DDRIVE_tipobj.innerHTML = thetext;
		DDRIVE_enabletip = true;
		return false;
	}
}


// Positions the tooltip pane
function DDRIVE_positiontip(e){
	if (ToolTipCompatibleBrowser() && DDRIVE_enabletip){
		var curX = (DDRIVE_ns6) ? e.pageX : event.x + DDRIVE_ietruebody().scrollLeft;
		var curY = (DDRIVE_ns6) ? e.pageY : event.y + DDRIVE_ietruebody().scrollTop;

		//Find out how close the mouse is to the corner of the window
		var rightedge = DDRIVE_ie && !window.opera ? DDRIVE_ietruebody().clientWidth - event.clientX-DDRIVE_offsetxpoint : window.innerWidth - e.clientX - DDRIVE_offsetxpoint - 20;
		var bottomedge = DDRIVE_ie && !window.opera ? DDRIVE_ietruebody().clientHeight - event.clientY-DDRIVE_offsetypoint : window.innerHeight - e.clientY - DDRIVE_offsetypoint - 20;

		var leftedge = (DDRIVE_offsetxpoint < 0) ? DDRIVE_offsetxpoint * (-1) : -1000;

		//if the horizontal distance isn't enough to accomodate the width of the context menu

		if (rightedge < DDRIVE_tipobj.offsetWidth){
			//move the horizontal position of the menu to the left by it's width
			DDRIVE_tipobj.style.left = DDRIVE_ie ? DDRIVE_ietruebody().scrollLeft + event.clientX - DDRIVE_tipobj.offsetWidth + "px" : window.pageXOffset + e.clientX-DDRIVE_tipobj.offsetWidth + "px";
		}
		else if (curX < leftedge){
			DDRIVE_tipobj.style.left = "5px";
		}
		else{
			//position the horizontal position of the menu where the mouse is positioned
			DDRIVE_tipobj.style.left = curX + DDRIVE_offsetxpoint + "px";
		}

		//same concept with the vertical position
		if (bottomedge < DDRIVE_tipobj.offsetHeight){
			DDRIVE_tipobj.style.top = DDRIVE_ie ? DDRIVE_ietruebody().scrollTop + event.clientY - DDRIVE_tipobj.offsetHeight - DDRIVE_offsetypoint + "px" : window.pageYOffset + e.clientY - DDRIVE_tipobj.offsetHeight - DDRIVE_offsetypoint + "px";
		}
		else{
			DDRIVE_tipobj.style.top = curY + DDRIVE_offsetypoint + "px";
		}
		DDRIVE_tipobj.style.display = "inline";		
	}
}


// Hides the tooltip pane
function DDRIVE_hidetip(e){
	if (ToolTipCompatibleBrowser()){
		DDRIVE_enabletip = false;
		DDRIVE_tipobj.style.display = "none";
		DDRIVE_tipobj.style.left = "-1000px";
		DDRIVE_tipobj.style.backgroundColor = '';
		DDRIVE_tipobj.style.width = '';
	}
}


// Shows a tooltip with an Amazon product image in it (
function AmazonImgTooltip(e){
	if (!e) var e = window.event;

	// Get the event target
	var targ;
	if (e.target) targ = e.target;
	else if (e.srcElement) targ = e.srcElement;
	if (targ.nodeType == 3) // defeat Safari bug
		targ = targ.parentNode;
	
	// We have to act on a link, so go up through the DOM tree until we get one
	var docElement = DDRIVE_ietruebody();
	while(targ != docElement && targ.nodeName.toLowerCase() != "a"){
		targ = targ.parentNode;
	}
	
	// We never got a link
	if(targ == docElement){
		return false;
	}
	
	// We got a link; show the tooltip
	var imgurl, thecolor, thewidth;
	if (ToolTipCompatibleBrowser()){
		var amazonImgSrc = GetAmazonImageSource(targ.href);
		if(amazonImgSrc != ""){
			var img = new Image();
			img.src = amazonImgSrc;
			DDRIVE_tipobj.innerHTML = '<img src=\'' + img.src + '\'/>';
			DDRIVE_tipobj.style.width = img.width + "px";
			DDRIVE_tipobj.style.display = 'inline';
			DDRIVE_enabletip = true;
			return false;
		}
	}
}


// Parses an Amazon referral URL and gets the associated image URL (empty string if N/A)
function GetAmazonImageSource(amazonUrl){
	var amazonImgSrc = "";
	var AmazonLinkMatches = amazonUrl.match(/^http:\/\/www\.amazon\.[^\/]+\/exec\/obidos\/ASIN\/([\w\d]{10})\/[-\w\d]+$/i);
	if(AmazonLinkMatches && AmazonLinkMatches.length > 1){
		amazonImgSrc = "http://" + AMAZON_imageserver + "/images/P/" + AmazonLinkMatches[1] + ".01.MZZZZZZZ.jpg"
	}
	return amazonImgSrc;
}


// Rewrites the links in the document to provide image tooltips for Amazon referral links
function RewriteAmazonLinksWithTooltips(){
	for(i = 0; i < document.links.length; i++){
		var amazonImgSrc = GetAmazonImageSource(document.links[i].href);
		if(amazonImgSrc != ""){
			document.links[i].onmouseover = AmazonImgTooltip;
			document.links[i].onmouseout = DDRIVE_hidetip;
			// Preload all images
			var imgLoad = new Image();
			imgLoad.src = amazonImgSrc;
		}
	}
}


// Checks to see if the current browser is "tooltip compatible"
function ToolTipCompatibleBrowser(){
	if(DDRIVE_ns6||DDRIVE_ie){
		return true;
	}
	else{
		return false;
	}
}

document.onmousemove = DDRIVE_positiontip;

function initAmazonLinks()
{
	DDRIVE_tipobj = createToolTipDiv();
	RewriteAmazonLinksWithTooltips();
}

$(function() {
   initAmazonLinks();
});