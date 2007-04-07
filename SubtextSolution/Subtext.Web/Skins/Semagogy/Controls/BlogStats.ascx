<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.BlogStats" %>
<dl class="blogStats">
	<dt>
		Posts
	</dt>
	<dd>
		<asp:literal id = "PostCount" runat = "server" />
	</dd>
	<dt>
		Comments
	</dt>
	<dd>
		<asp:literal id = "CommentCount" runat = "server" />
	</dd>
	<dt>
		Trackbacks
	</dt>
	<dd>
		<asp:literal id = "PingTrackCount" runat = "server" />
	</dd>
</dl>
<asp:Literal ID = "StoryCount" Runat = "server" Visible="False" />