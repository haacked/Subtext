<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
	<div class="blogpost">
		<h2 class="postTitle"><asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink Runat="server" ID="TitleUrl" /> <asp:Label ID="postDate" CssClass="postTitleDate" runat="server" Format="MMM dd" /> </h2>
		<asp:Literal id="Body"  runat="server" />
		<p class="postfooter">
			<a href="javascript:window.print();" class="printIcon"><span>Print</span></a> | posted on <asp:Literal id="PostDescription" runat="server" />
		</p>
	</div>
	<asp:Literal ID = "PingBack" Runat="server" />
	<asp:Literal ID = "TrackBack" Runat="server" />