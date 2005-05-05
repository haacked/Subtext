<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
	<div class="post">
		<h2>
			<asp:HyperLink Runat="server" ID="TitleUrl" />
		</h2>
		<asp:Literal id="Body"  runat="server" />
		<p class="postfoot">
			posted on <asp:Literal id="PostDescription"  runat="server" />
		</p>
	</div>
	<asp:Literal ID = "PingBack" Runat = "server" />
	<asp:Literal ID = "TrackBack" Runat = "server" />
	