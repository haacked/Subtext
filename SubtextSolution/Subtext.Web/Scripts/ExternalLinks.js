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
				addClass(anchor, 'newWindowStyle');
				
				if(anchor.title.length > 0)
					anchor.title = anchor.title + ' (new window)';
				else
					anchor.title = '(new window)';
			}
        }
    }
}

// addLoadEvent is defined in Subtext.Web/Scripts/common.js
addLoadEvent(externalLinks);