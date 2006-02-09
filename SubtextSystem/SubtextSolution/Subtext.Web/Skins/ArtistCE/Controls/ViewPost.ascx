<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>

<div class="entry">
	<h4>
		<asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink Runat="server" ID="TitleUrl" />
	</h4>
	
	<div class="post">
		<asp:Literal id="Body"  runat="server" />
	</div>
	
	<div class="info">
		posted @ <asp:Literal id="PostDescription"  runat="server" />
	</div>
</div>

	<asp:Literal ID = "PingBack" Runat="server" />
	<asp:Literal ID = "TrackBack" Runat="server" />