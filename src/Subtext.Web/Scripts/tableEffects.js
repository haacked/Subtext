function initRowHighlighting()
{
	if (!document.getElementsByTagName)
		{ return; }

	var tables = getElementsByClass('highlightTable', document, 'table');
	
	for(var i = 0; i < tables.length; i++)
	{
		var table = tables[i];
		//Make sure to use th tags for header row.
		attachRowMouseEvents(table.getElementsByTagName('tr'));
	}
}

function attachRowMouseEvents(rows)
{
	for(var i = 0; i < rows.length; i++)
	{
		var row = rows[i];
		if(i%2 == 1)
		{
			row.onmouseover = function() { addClass(this, 'highlight'); }
			row.onmouseout = function() { removeClass(this, 'highlight'); }
		}
		else
		{
			row.onmouseover = function() { addClass(this, 'highlightAlt'); }
			row.onmouseout = function() { removeClass(this, 'highlightAlt'); }
		}
	}
}

addLoadEvent(initRowHighlighting);