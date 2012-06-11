/*
Method for applying a specific target and style to XFN links.
*/

var xfnRelationships = ['friend', 'acquaintance', 'contact', 'met', 'co-worker', 'colleague', 'co-resident', 'neighbor', 'child', 'parent', 'sibling', 'spouse', 'kin', 'muse', 'crush', 'date','sweetheart', 'me'];

var xfnFriendInfo = 
{
	createFriendInfoBox: function(anchor, html)
	{
		var infoBox = this.createInfoDiv();
		infoBox.innerHTML = html;
		anchor._infoBox = infoBox;
		anchor.friendInfo = this;
		document.body.appendChild(infoBox);

		anchor.style.position = 'relative';		
		anchor.onmouseover = this.showFriendInfoBox;
		anchor.onmouseout = this.hideFriendInfoBox;
	},
	
	createInfoDiv:	function() 
	{
		var div = document.createElement("div");
		div.className = "xfnFriendInfoBox";
		div.style.position = 'absolute';
		div.style.display = 'none';
		return div;
	},
	
	showFriendInfoBox: function(e)
	{
		if (!e) {e = event;}
		var anchor = e.target || e.srcElement;
		if(!anchor)
		{
			return;
		}
		if (anchor._infoBox)
		{
			anchor.friendInfo.positionInfoBox(e, anchor._infoBox);
			anchor._infoBox.style.display = 'block';
		}
	},
	
	hideFriendInfoBox: function(e)
	{
		if (!e) {e = event;}
		var anchor = e.target || e.srcElement;
		if(!anchor)
		{
			return;
		}
		
		if (anchor._infoBox)
		{
			anchor._infoBox.style.display = 'none';
		}
	},
	
	positionInfoBox: function(e, infoBox) 
	{
		var scroll = this.getScroll();
		
		// width
		if (infoBox.offsetWidth >= scroll.width)
		{
			infoBox.style.width = scroll.width - 10 + "px";
		}
		else
		{
			infoBox.style.width = "";
		}
		// left
		if (e.clientX > scroll.width - infoBox.offsetWidth)
		{
			infoBox.style.left = scroll.width - infoBox.offsetWidth + scroll.left + "px";
		}
		else
		{
			infoBox.style.left = e.clientX - 2 + scroll.left + "px";
		}
		// top
		if (e.clientY + infoBox.offsetHeight + 18 < scroll.height)
		{
			infoBox.style.top = e.clientY + 18 + scroll.top + "px";
		}
		else if (e.clientY - infoBox.offsetHeight > 0)
		{
			infoBox.style.top = e.clientY + scroll.top - infoBox.offsetHeight + "px";
		}
		else
		{
			infoBox.style.top = scroll.top + 5 + "px";
    }
			
		infoBox.style.display = "inline";
		//alert(infoBox.style.left + ' ' + infoBox.style.top + ' | ' + infoBox.style.width + ' ' + infoBox.style.height);
	},
	
	// returns the scroll left and top for the browser viewport.
	getScroll:	function () 
	{
		if (document.all && typeof document.body.scrollTop != "undefined") // IE model
		{
			var ieBox = document.compatMode != "CSS1Compat";
			var cont = ieBox ? document.body : document.documentElement;
			return {
				left:	cont.scrollLeft,
				top:	cont.scrollTop,
				width:	cont.clientWidth,
				height:	cont.clientHeight
			};
		}
		else {
			return {
				left:	window.pageXOffset,
				top:	window.pageYOffset,
				width:	window.innerWidth,
				height:	window.innerHeight
			};
		}
	}
};


function highlightXFNLinks() 
{
    $('a[href][rel]').each(function() {
        var rel = $(this).attr('rel');
        //Let's look at all the various potential relationships.
        var relationships = '';

        for (var j = 0; j < xfnRelationships.length; j++) {
            var regex = new RegExp('\\b' + xfnRelationships[j] + '\\b', "i");
            if (rel.match(regex)) {
                if (relationships.length === 0) {
                    relationships = "<h3>XFN Relationships</h3><ul>";
                }
                relationships += "<li>" + xfnRelationships[j] + "</li>";
            }
        }
        if (relationships.length > 0) {
            relationships += "</ul>";
            xfnFriendInfo.createFriendInfoBox($(this)[0], relationships);
            $(this).addClass('xfnRelationship');
        }
    });
}

$(function() {
    highlightXFNLinks();
});