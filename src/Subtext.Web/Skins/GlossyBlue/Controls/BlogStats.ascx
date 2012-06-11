<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.BlogStats" %>
<div id="blogstats">
	<div>
	<span>Blog Stats</span>
	<ul>
		<li>Posts - <asp:Literal ID="PostCount" Runat="server" /></li>
		<li>Articles - <asp:Literal ID="StoryCount" Runat="server" /></li>
		<li>Comments - <asp:Literal ID="CommentCount" Runat="server" /></li>
		<li>Trackbacks - <asp:Literal ID="PingTrackCount" Runat="server" /></li>
	</ul>
	</div>
</div>
<div id="blogstatsback">
<p>&nbsp;</p>
</div>