<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.BlogStats" %>
<dl title="Blog Statistics">
	<dt>posts</dt>
	<dd><asp:Literal ID = "PostCount" Runat = "server" /></dd>
	<dt>comments</dt>
	<dd><asp:Literal ID = "CommentCount" Runat = "server" /></dd>
	<dt>trackbacks</dt>
	<dd><asp:Literal ID = "PingTrackCount" Runat = "server" /></dd>
</dl>

<asp:Literal ID="StoryCount" Runat = "server" Visible="False" />