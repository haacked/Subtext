<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.CurrentEntryControl" %>
<div class="share">
	<span>Share this control</span>
	<ul>
		<li>
			<a href="http://del.icio.us/login?url=<%# UrlEncode(Entry.Link) %>;title=<%# UrlEncode(Entry.Title) %>">bookmark It</a>
		</li>
		<li>
			<a href="http://digg.com/submit?url=<%# UrlEncode(Entry.Link) %>&phase=2">digg It</a>
		</li>
		<li>
			<a href="http://reddit.com/submit?url=<%# UrlEncode(Entry.Link) %>&title=<%# UrlEncode(Entry.Title) %>">redd It</a>
		</li>
	</ul>
</div>