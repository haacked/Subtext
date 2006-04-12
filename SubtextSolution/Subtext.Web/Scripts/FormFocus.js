/*
Method for applying a specific target and style to 
external links.  Adapted from 

http://www.justinfrench.com/index.php?id=16
and
http://www.boagworld.com/archives/2006/01/external_links_and_new_windows.html
*/
function externalLinks() 
{
    if (!document.getElementsByTagName) return;
    var anchors = document.getElementsByTagName("a");
    for (var i = 0; i < anchors.length; i++) 
    {
        var anchor = anchors[i];
        if (anchor.getAttribute("href") && anchor.getAttribute("rel")) 
        {
			if(anchor.getAttribute("rel").indexOf("external") >= 0)
			{
				anchor.target = "_blank";
				if(anchor.title.length > 0)
					anchor.title = anchor.title + ' (new window)';
				else
					anchor.title = '(new window)';
				if(anchor.className.length > 0)
					anchor.className += ' newWindowStyle';
				else
					anchor.className = 'newWindowStyle';
			}
        }
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

addLoadEvent(externalLinks);