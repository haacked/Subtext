<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.BlogStats" %>
<dl title="Blog Statistics" id="blogStats">
	<dt>Posts</dt>
	<dd><asp:Literal ID="PostCount" Runat="server" /></dd>
	<dt>Stories</dt>
	<dt><asp:Literal ID="StoryCount" Runat="server" /></dt>
	<dt>Comments</dt>
	<dd><asp:Literal ID="CommentCount" Runat="server" /></dd>
	<dt>Trackbacks</dt>
	<dd><asp:Literal ID="PingTrackCount" Runat="server" /></dd>
</dl>

