/*
Adapted from the following article: http://aspnet.4guysfromrolla.com/articles/111704-1.aspx
*/
var __smartScroller = new smartScroller();

//Constructor.
function smartScroller()
{
	this.GetCoords = GetCoords;
	this.Scroll = Scroll;
	
	function GetCoords()
	{
		var scrollX, scrollY;

		if (document.all)
		{
			if (!document.documentElement.scrollLeft)
				scrollX = document.body.scrollLeft;
			else
				scrollX = document.documentElement.scrollLeft;
		        
			if (!document.documentElement.scrollTop)
				scrollY = document.body.scrollTop;
			else
				scrollY = document.documentElement.scrollTop;
		}   
		else
		{
			scrollX = window.pageXOffset;
			scrollY = window.pageYOffset;
		}

		document.forms[0].__scrollLeft.value = scrollX;
		document.forms[0].__scrollTop.value = scrollY;
	}
	
	function Scroll()
	{
		var x = document.forms[0].__scrollLeft.value;
		var y = document.forms[0].__scrollTop.value;
		window.scrollTo(x, y);
	}
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

addLoadEvent(__smartScroller.Scroll);
window.onscroll = __smartScroller.GetCoords;
window.onkeypress = __smartScroller.GetCoords;
window.onclick = __smartScroller.GetCoords;