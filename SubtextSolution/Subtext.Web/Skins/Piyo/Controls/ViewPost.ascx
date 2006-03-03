<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<div class="entry">
	<h4><asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink Runat="server" ID="TitleUrl" /></h4>
	<asp:Literal id="Body"  runat="server" />

		<p class="post">
			posted @ <asp:Literal id="PostDescription"  runat="server" />
		</p>
</div>
	<asp:Literal ID = "PingBack" Runat="server" />
	<asp:Literal ID = "TrackBack" Runat="server" />