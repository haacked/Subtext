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

Original Source for this JS taken from the Subtext Project:
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
//
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
*/

Subtext.CommentLivePreview =
{
	livePreviewClass: "livepreview",
	allowedTags: subtextAllowedHtmlTags,
	allowedTagsRegExp: null,
	previewElement: null,
	paraRegExp: new RegExp("(.*)\n\n([^#*\n\n].*)", "g"),
	lineBreakRegExp: new RegExp("(.*)\n([^#*\n].*)", "g"),
	updatingPreview: false,

	init: function()
	{
		this.previewElement = $("div." + this.livePreviewClass);
		if(this.previewElement.length == 0) { 
		    return; 
		}

		var tagNamesRegex = "";

		for(var i = 0; i < this.allowedTags.length; i++)
		{
			tagNamesRegex += this.allowedTags[i] + "|";
		}

		if(tagNamesRegex.length > 0)
		{
			tagNamesRegex = tagNamesRegex.substring(0, tagNamesRegex.length - 2);
		}

		this.allowedTagsRegExp = new RegExp("&lt;(/?(" + tagNamesRegex + ")(\\s+.*?)?)&gt;", "g");

		var textarea = $('textarea.' + this.livePreviewClass);
		textarea.bind('keyup', this.handleKeyUp);

		this.reloadPreview(textarea.attr('id'));
	},

    handleKeyUp: function() {
            // Subject to race condition. But it's not a big deal. The next keypress
			// will solve it. Worst case is the preview is off by the last char in rare
			// situations.
			var preview = Subtext.CommentLivePreview;
			var textarea = $(this);
			if(!preview.updatingPreview) {
				preview.updatingPreview = true;
				textarea.unbind('keyup', preview.handleKeyUp);
				window.setTimeout("Subtext.CommentLivePreview.reloadPreview('" + textarea.attr('id') + "')", 20);
			}
			return false;
    },

	reloadPreview: function(textareaId)
	{
	    var textarea = $('#' + textareaId);
	    
	    if(textarea.length == 0) {
	        return;
	    }
	    
		var previewString = textarea.val();

		if (previewString.length > 0)
		{
			previewString = this.htmlUnencode(previewString);
			previewString = previewString.replace(this.paraRegExp, "<p>$1</p><p>$2</p>");
			previewString = previewString.replace(this.lineBreakRegExp, "$1<br />$2");
			previewString = previewString.replace(this.allowedTagsRegExp, "<$1>");
		}
		
		try
		{
			this.previewElement.html(previewString);
		}
		catch(e)
		{
			alert("Sorry, but inserting a block element within is not allowed here.");
		}
		
		this.updatingPreview = false;
		textarea.bind('keyup', this.handleKeyUp);
	},

	htmlUnencode: function(s)
	{
		return s.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
	},
};

$(function(){
    Subtext.CommentLivePreview.init();
});