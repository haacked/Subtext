<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.BlogStats" %>
<ul style="display: none;">
	<li>Posts - <asp:Literal ID="PostCount" Runat = "server" /></li>
	<li>Stories - <asp:Literal ID="StoryCount" Runat = "server" /></li>
	<li>Comments - <asp:Literal ID="CommentCount" Runat = "server" /></li>
	<li>Trackbacks - <asp:Literal ID="PingTrackCount" Runat = "server" /></li>
</ul>
