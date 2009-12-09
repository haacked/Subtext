/* ===============================================
 * ================ COMMON =======================
 * Some common functions for the basic JavaScript objects
 *
 * ===============================================
 */

String.prototype.trim = function()
{
    return this.replace(/^\s+|\s+$/, "");
};

// Define methods for the Array data structure.
Array.prototype.indexOf = function(item, start) 
{ 
  var i;
	for (i = (start || 0); i < this.length; i++) 
	{ 
		if (this[i] == item) 
		{ 
			return i; 
		} 
	} 
	return -1; 
};

Array.prototype.remove = function(obj)
{ 
	var x = [];
	var i; 
	for (i=0; i<this.length; i++)
	{ 
		if (this[i] != obj)
		{ 
			x.push(this[i]); 
		} 
	} 
	return x; 
};

Array.prototype.replace = function(obj1, obj2)
{ 
	var x = [];
	var len = this.length;
	var i;
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
};

/*-------------------------------------------------*/
/* Disables submit button during update panel post
/*-------------------------------------------------*/
function pageLoad(sender, args) 
{
    var requestManager = Sys.WebForms.PageRequestManager.getInstance();
    requestManager.add_initializeRequest(initializeRequest);
    requestManager.add_endRequest(endRequest);
}

function initializeRequest(sender, args) {
    //Disable button to prevent double submit
    var button = $get(args._postBackElement.id);
    if (button) 
    {
        button.disabled = true;
        button.oldValue = button.value;
        if (button.oldValue && button.oldValue !== '') {
            button.value = 'Posting...';
        }
        if (button.className == 'buttonSubmit') 
        {
            button.className = 'button-disabled';
        }
    }
}

function endRequest(sender, args) 
{
    //Re-enable button
    var button = $get(sender._postBackSettings.sourceElement.id);
    if (button) 
    {
        button.disabled = false;
        button.value = button.oldValue;
        if (button.className == 'button-disabled') 
        {
            button.className = 'buttonSubmit';
        }
    }
}
