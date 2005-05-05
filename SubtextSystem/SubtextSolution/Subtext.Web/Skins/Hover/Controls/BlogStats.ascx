<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.BlogStats" %>
<h3>Blog Stats</h3>
	<ul>
		<li>Posts - <asp:Literal ID = "PostCount" Runat = "server" /></li>
		<li>Stories - <asp:Literal ID = "StoryCount" Runat = "server" /></li>
		<li>Comments - <asp:Literal ID = "CommentCount" Runat = "server" /></li>
		<li>Trackbacks - <asp:Literal ID = "PingTrackCount" Runat = "server" /></li>
	</ul>