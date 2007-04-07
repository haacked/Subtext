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

/* ===============================================
 * ================ COMMON =======================
 * Some common functions for the basic JavaScript objects
 *
 * ===============================================
 */

String.prototype.trim = function()
{
    return this.replace(/^\s+|\s+$/, "");
}

// Define methods for the Array data structure.
Array.prototype.indexOf = function(item, start) 
{ 
	for (var i = (start || 0); i < this.length; i++) 
	{ 
		if (this[i] == item) 
		{ 
			return i; 
		} 
	} 
	return -1; 
}

Array.prototype.remove = function(obj)
{ 
	x = []; 
	for (i=0; i<this.length; i++)
	{ 
		if (this[i] != obj)
		{ 
			x.push(this[i]); 
		} 
	} 
	return x; 
}

Array.prototype.replace = function(obj1, obj2)
{ 
	var x = [];
	var len = this.length;
	for (i=0; i<len; i++)
	{ 
		if (this[i] == obj1)
		{ 
			x.push(obj2); 
		} 
		else
		{ 
			x.push(this[i]); 
		} 
	} 
	return x; 
}


/* ===============================================
 * ============= GET ELEMENTS ====================
 * Methods to help retrieve elements from the DOM.
 *
 * ===============================================
 */

/*
 * Gets all the Elements by given Class Name. 
 * 
 * @param string searchClass - Name of the css class to look for.
 * @param object node - (optional) The node you want to start from. Defaults to 'document' if none is specified. 
 * @param string tagName - (optional) Limit  results by adding a tagName. Defaults to '*' if none is specified.
 * 
 * @returns array - Returns an array containing all the nodes given by the specified className.
 */ 
function getElementsByClass(searchClass, node, tagName)
{
	var	classElements =	new	Array();
	if (node == null)
		node = document;
	if (tagName ==	null)
		tagName	= '*';
	
	var	els	= node.getElementsByTagName(tagName);
	var	elsLen = els.length;
	
	for	(i = 0,	j =	0; i < elsLen; i++)
	{
		if (hasClass(els[i], searchClass))
		{
			classElements[j] = els[i];
			j++;
		}
	}
	return classElements;
}

/*
 * Get the element from the DOM Tree. If the given parameter (e) is a string, 
 * it is assumed to be the element's ID, and that element will be retrieved, else 
 * the given parameter is returned. 
 * 
 * @param string/element e - the element ID / element to be retrieved from the DOM tree. 
 * 
 * @returns element
 */
function getElement(e)
{
	if (typeof e ==	'string')
		return document.getElementById(e);
	else
		return e;
}

/* ===============================================
 * ================ STYLING ======================
 * Methods for manipulating an element's styles (CSS).
 *
 * ===============================================
 */

function getClasses(element)
{
	return element.className.trim().split(/\s+/);
}

function hasClass(element, className)
{
	return getClasses(element).indexOf(className) != -1;
}

function addClass(element, className)
{
    var classes = getClasses(element);
    
    if (classes.indexOf(className) == -1)
    {
        classes.push(className);
        element.className = classes.join(' ');
    }
}

function removeClass(element, className)
{
    var classes = getClasses(element);
    var index = classes.indexOf(className);
    
    if (index != -1)
    {
        classes.splice(index, 1);
        element.className = classes.join(' ');
    }
}

function showElement(element)
{ 
	element = getElement(element);
	
	if (element) // if we don't find the element, don't try to access it's properties.
	{
		element.style.display = "";
	}
}

function hideElement(element)
{
	element = getElement(element);
        
	if (element)
	{
		element.style.display = "none";
	}
}

/*
	Prototype for a javascript BlogInfo object 
	that provides information to client scripts 
	much as a server version does.
*/
function blogInfo(virtualRoot, virtualBlogRoot)
{
	this.virtualRoot = virtualRoot;
	this.virtualBlogRoot = virtualBlogRoot;
	
	this.getVirtualRoot = getVirtualRoot;
	this.getVirtualBlogRoot = getVirtualBlogRoot;
	this.getScriptsVirtualRoot = getScriptsVirtualRoot;
	this.getImagesVirtualRoot = getImagesVirtualRoot;
	
	/*
	Returns the virtual root for the entire website.
	*/
	function getVirtualRoot()
	{
		return this.virtualRoot;
	}
	
	/*
	Gets the virtual root for the specific blog.
	*/
	function getVirtualBlogRoot()
	{
		return this.virtualBlogRoot;
	}
	
	/*
	Returns the virtual root to the default "scripts" directory 
	*/
	function getScriptsVirtualRoot()
	{
		return this.virtualRoot + "Scripts/";
	}
	
	/*
	Returns the virtual root to the default "scripts" directory 
	*/
	function getImagesVirtualRoot()
	{
		return this.virtualRoot + "Images/";
	}
}