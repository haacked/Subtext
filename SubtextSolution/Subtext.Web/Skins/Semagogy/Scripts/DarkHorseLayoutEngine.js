function DarkHorseLayoutEngine()
{
	var form;
	var forms = document.body.getElementsByTagName("form");
	if (forms.length > 0)
	{
		var form = forms[0];
	}
	CreateLayout();

	function GetChildren(node)
	{
		var result = [];
		var child = node.firstChild;
		while(child)
		{
			result[result.length] = child;
			child = child.nextSibling;
		}
		return result;
	}
	
	function LayoutTable(columnCount, rowCount, className)
	{
		var table = document.createElement("TABLE");
		table.className = "Layout " + (className != null ? className : "");
		var tbody = table.appendChild(document.createElement("TBODY"));
		for(var i = 0; i < rowCount; i++)
		{
			var row = tbody.appendChild(document.createElement("TR"));
			row.className = "r" + (i + 1);
			row.getColumn = function(column)
			{
				return this.cells.length > column && column >= 0 ? this.cells[column] : null;
			}
			for(var j = 0; j < columnCount; j++)
			{
				var cell = row.appendChild(document.createElement("TD"));
				cell.className = "c" + (j + 1);
			}
		}
		this.node = table;
		
		this.getCell = function(column, row)
		{
			return table.rows.length >= row && row > 0 ? table.rows[row - 1].getColumn(column - 1) : null;
		}
		
		this.appendChild = function(column, row, node)
		{
			if (node == null) return null;
			var cell = this.getCell(column, row);
			return cell != null ? cell.appendChild(node) : null;
		}
	}

	function ClassNameBag(node)
	{
		var child = node.firstChild;
		this.__nameless = [];
		while(child)
		{
			if (child.nodeType == 1)
			{
				var className = child.className;
				if (className && className.length > 0)
				{
					var item = this[className];
					if (item)
					{
						if (item instanceof Array)
						{
							item.push(child);
						}
						else
						{
							this[className] = [item, child];
						}
					}
					else
					{
						this[className] = child;
					}
				}
				else
				{
					this.__nameless.push(child);
				}
			}
			child = child.nextSibling;
		}
	}
	
	function GetFirstElementByTagName(node, tagName)
	{
		var elements = node.getElementsByTagName(tagName)
		return elements.length > 0 ? elements[0] : null;
	}
	
	function GetDtContent(dl)
	{
		var dt = GetFirstElementByTagName(dl, "dt");
		return dt ? dt.innerHTML.toLowerCase().replace(/(\W|(<[^>]+>))/g, "") : null;
	}
	
	function CreateLayout()
	{
		var header = document.getElementById("Header");
		var content = document.getElementById("Content");
		var footer = document.getElementById("Footer");
		if (header && content && footer)
		{
			var contentItems = new ClassNameBag(content);
			var archive = null;
			var categories = null;
			var blogRoll = null;
			var articles = null;
			var namelessItems = contentItems.Links;
			for(var i = 0; i < namelessItems.length; i++)
			{
				var namelessItem = namelessItems[i];
				if (namelessItem.tagName == "DL")
				{
					var title = GetDtContent(namelessItem);
					if (title == "archives")
					{
						namelessItem.className = "Links Archive";
						archive = namelessItem;
					} 
					else if (title == "postcategories")
					{
						namelessItem.className = "Links Categories";
						categories = namelessItem;
					}
					else if (title == "blogroll")
					{
						namelessItem.className = "Links BlogRoll";
						blogRoll = namelessItem;
					}
					else if (title == "articles")
					{
						namelessItem.className = "Links Articles";
						articles = namelessItem;
					}
					else
					{
						namelessItem.parentNode.removeChild(namelessItem);
					}
				}				
			}
			var shim = document.createElement("div");
			shim.className = "Shim";
			var layoutTable = new LayoutTable(3, 1);
			layoutTable.appendChild(1, 1, contentItems.Navigation);
			layoutTable.appendChild(3, 1, contentItems.News);
			layoutTable.appendChild(3, 1, archive);
			layoutTable.appendChild(3, 1, categories);
			layoutTable.appendChild(3, 1, articles);
			layoutTable.appendChild(3, 1, blogRoll);
			layoutTable.appendChild(2, 1, header);
			layoutTable.appendChild(2, 1, content);
			layoutTable.appendChild(2, 1, footer);
			layoutTable.appendChild(2, 1, shim);
			form.appendChild(layoutTable.node);
		}
	}	
}
