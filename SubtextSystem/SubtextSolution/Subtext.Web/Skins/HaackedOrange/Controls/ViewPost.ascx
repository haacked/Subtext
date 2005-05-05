<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
	<div class="blogpost">
		<h1><span class="title"><asp:HyperLink Runat="server" ID="TitleUrl" /></span></h1>
		<asp:Literal id="Body"  runat="server" />
		<p class="postfooter">
			posted on <asp:Literal id="PostDescription"  runat="server" />
		</p>
	</div>
	<asp:Literal ID = "PingBack" Runat="server" />
	<asp:Literal ID = "TrackBack" Runat="server" />