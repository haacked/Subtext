/*
Makes sure that labels give their form input focus for browsers 
in which the <label> tag doesn't work.

Adapted from http://particletree.com/features/10-tips-to-a-better-form/
Item #8
*/

function initLabels() 
{
    labels = document.getElementsByTagName("label");
    for(i = 0; i < labels.length; i++) {
        addEvent(labels[i], "click", labelFocus);
    }
}

function labelFocus() 
{
    new Field.focus(this.getAttribute('for'));
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

addLoadEvent(initLabels);