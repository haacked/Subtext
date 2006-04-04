<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<div class="block">
	<h1 class="block_title"><asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink Runat="server" ID="TitleUrl" /></h1>
	<div class="post">
		<div class="postcontent">
			<asp:Literal id="Body"  runat="server" />
		</div>
		<div class="itemdesc">
			posted on <asp:Literal id="PostDescription"  runat="server" />
		</div>
	</div>
	<div class="seperator">&nbsp;</div>
	<asp:Literal ID = "PingBack" Runat = "server" />
	<asp:Literal ID = "TrackBack" Runat = "server" />
