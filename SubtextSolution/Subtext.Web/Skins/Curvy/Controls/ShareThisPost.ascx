<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.CurrentEntryControl" %>
<div class="share">
	<a class="mailto" href="mailto:?body=Thought+you+would+find+this+interesting.+<%#UrlEncode(Entry.FullyQualifiedUrl.ToString())%>&amp;subject=<%# UrlEncode(Entry.Title) %>" title="Email it">&nbsp;&nbsp;&nbsp;&nbsp;</a><font color="#808080"><b>&nbsp;| </b></font>
	<a class="deli" href="http://del.icio.us/login?url=<%# UrlEncode(Entry.FullyQualifiedUrl.ToString()) %>;title=<%# UrlEncode(Entry.Title) %>" title="Bookmark it at del.icio.us">&nbsp;&nbsp;&nbsp;&nbsp;</a><font color="#808080"><b>&nbsp;| </b></font>
	<a class="digg" href="http://digg.com/submit?url=<%# UrlEncode(Entry.FullyQualifiedUrl.ToString()) %>&amp;phase=2" title="digg it">&nbsp;&nbsp;&nbsp;&nbsp;</a><font color="#808080"><b>&nbsp;| </b></font>
	<a class="redd" href="http://reddit.com/submit?url=<%# UrlEncode(Entry.FullyQualifiedUrl.ToString()) %>&amp;title=<%# UrlEncode(Entry.Title) %>" title="redd it">&nbsp;&nbsp;&nbsp;&nbsp;</a>
</div>