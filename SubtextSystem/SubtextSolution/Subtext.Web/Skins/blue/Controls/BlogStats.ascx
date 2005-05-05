<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.BlogStats" %>
<div class = "listtitle">Blog Stats</div>
	<ul class = "list">
		<li class = "listitem">Posts - <asp:Literal ID = "PostCount" Runat = "server" />
		<li class = "listitem">Stories - <asp:Literal ID = "StoryCount" Runat = "server" />
		<li class = "listitem">Comments - <asp:Literal ID = "CommentCount" Runat = "server" />
		<li class = "listitem">Trackbacks - <asp:Literal ID = "PingTrackCount" Runat = "server" />
	</li>
</ul>