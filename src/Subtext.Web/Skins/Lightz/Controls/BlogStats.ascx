<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.BlogStats" %>
<h1>Blog Stats</h1>
	<ul class="list">
		<li class="listitem">Posts - <asp:Literal ID = "PostCount" Runat = "server" />
		<li class="listitem">Stories - <asp:Literal ID = "StoryCount" Runat = "server" />
		<li class="listitem">Comments - <asp:Literal ID = "CommentCount" Runat = "server" />
		<li class="listitem">Trackbacks - <asp:Literal ID = "PingTrackCount" Runat = "server" />
	</li>
</ul>