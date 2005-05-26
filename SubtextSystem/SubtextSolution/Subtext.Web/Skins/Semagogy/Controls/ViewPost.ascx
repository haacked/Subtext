<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<dl class="Post">
	<dt>
		<asp:hyperlink runat="server" id="TitleUrl" />
	</dt>
	<dd class="Text">
		<asp:literal id="Body"  runat="server" />
	</dd>
	<dd class="Info">
		posted on <asp:literal id="PostDescription"  runat="server" />
	</dd>
	<dd class="Ping">
		<asp:literal id = "PingBack" runat="server" />
		<asp:literal id = "TrackBack" runat="server" />
	</dd>
</dl>
