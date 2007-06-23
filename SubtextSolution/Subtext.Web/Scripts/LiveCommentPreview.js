/*
Adds Live Comment Preview for comment forms.

USAGE: Simply add the css class "livepreview" to the textarea that 
is the source of the comment and to the <div> element that 
will display the live preview.

Note that livepreview does not have to be the only css class.

ex... Make this edit in PostComment.ascx

<asp:TextBox id="tbComment" runat="server" Rows="10" Columns="40" width="100%" Height="193px"
					TextMode="MultiLine" class="livepreview"></asp:TextBox>
					
<div class="comment livepreview"></div>

*/
var allowedTagsRegExp;
var previewElement;
var paraRegExp = new RegExp("(.*)\n\n([^#\*\n\n].*)", "g");
var lineBreakRegExp = new RegExp("(.*)\n([^#\*\n].*)", "g");
var updatingPreview = false;

function initLiveCommentPreview()
{
	if (!document.getElementsByTagName) { return; }

	var divs = document.getElementsByTagName('div');
	previewElement = getPreviewDisplayElement(divs);

	if(!previewElement) {return;}	
	var textareas = document.getElementsByTagName('textarea');

	var tagNamesRegex = '';
	for(var i = 0; i < subtextAllowedHtmlTags.length; i++)
	{
		tagNamesRegex += subtextAllowedHtmlTags[i] + '|'
	}
	if(tagNamesRegex.length > 0)
		tagNamesRegex = tagNamesRegex.substring(0, tagNamesRegex.length - 2);
		
	allowedTagsRegExp = new RegExp('&lt;(/?(' + tagNamesRegex + ')(\\s+.*?)?)&gt;', "g");

	// loop through all input tags
	for (var i = 0; i < textareas.length; i++)
	{
		var textarea = textareas[i];
		if (getClassName(textarea).indexOf('livepreview') >= 0)
		{
			textarea.onkeyup = function () 
			{
				 //Subject to race condition. But it's not a big deal. The next keypress 
				 //will solve it. Worst case is the preview is off by the last char in rare 
				 //situations.
				if(!updatingPreview)
				{
					updatingPreview = true;
					window.setTimeout("reloadPreview('" + this.id + "')", 20);
				}
				return false;
			}
		}
	}	
}

// Returns the html element responsible for previewing 
// comments.
function getPreviewDisplayElement(elements)
{
	// loop through all elements
	for (var i = 0; i < elements.length; i++)
	{
		var element = elements[i];

		if (getClassName(element).indexOf('livepreview') >= 0)
		{
			return element;
		}
	}
}

function getClassName(element)
{
	if(element.getAttribute && element.getAttribute('class'))
	{
		return element.getAttribute('class');
	}
	else if(element.className)
	{
		return element.className;
	}
	return "";
}

function reloadPreview(textareaId) 
{
	var textarea = document.getElementById(textareaId);
	var previewString = textarea.value;
		
	if (previewString.length > 0)
	{
		previewString = htmlUnencode(previewString);
		previewString = previewString.replace(paraRegExp, "<p>$1</p><p>$2</p>");
		previewString = previewString.replace(lineBreakRegExp, "$1<br />$2");
		previewString = previewString.replace(allowedTagsRegExp, "<$1>");
	}
	try
	{
		previewElement.innerHTML = previewString;
	}
	catch(e)
	{
		alert('Sorry, but inserting a block element within is not allowed here.');
	}
	updatingPreview = false;
}

function htmlUnencode(s)
{
	return s.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
}

// addLoadEvent is defined in Subtext.Web/Scripts/common.js
addLoadEvent(initLiveCommentPreview);	// run initLightbox onLoad