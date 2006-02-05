function initRowHighlighting()
{
	if (!document.getElementsByTagName){ return; }
	
	var tables = document.getElementsByTagName('table');
	
	for(var i = 0; i < tables.length; i++)
	{
		var table = tables[i];
		if(table.className && table.className.indexOf('highlightTable') >= 0)
		{
			//Make sure to use th tags for header row.
			attachRowMouseEvents(table.getElementsByTagName('tr'));
		}
	}
}

function attachRowMouseEvents(rows)
{
	for(var i = 0; i < rows.length; i++)
	{
		var row = rows[i];
		if(i%2 == 0)
		{
			row.onmouseover =	function() 
								{ 
									this.setAttribute('oldClass', this.className);
									this.className = 'highlight';
								};
			
			row.onmouseout =	function() 
								{ 
									this.className = this.getAttribute('oldClass');
								};
		}
		else
		{
			row.onmouseover =	function() 
								{ 
									this.setAttribute('oldClass', this.className);
									this.className = 'highlightAlt';
								};
			
			row.onmouseout =	function() 
								{ 
									this.className = this.getAttribute('oldClass');
								};
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

addLoadEvent(initRowHighlighting);