<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<dl class="Post">
	<dt>
		<asp:HyperLink  Runat="server" ID="editLink" /><asp:hyperlink runat="server" id="TitleUrl" />
	</dt>
	<dd class="Text">
		<asp:literal id="Body"  runat="server" />
	</dd>
	<dd class="Info">
		posted on <asp:literal id="PostDescription"  runat="server" /> <a href="javascript:window.print();" class="printIcon"><span>Print</span></a>
	</dd>
	<dd class="Ping">
		<asp:literal id = "PingBack" runat="server" />
		<asp:literal id = "TrackBack" runat="server" />
	</dd>
</dl>
