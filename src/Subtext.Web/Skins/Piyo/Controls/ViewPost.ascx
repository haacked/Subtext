<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<%@ Register TagPrefix="uc1" TagName="PreviousNext" Src="PreviousNext.ascx" %>

<uc1:PreviousNext id="PreviousNext" runat="server" />

<div class="entry">
	<h4><asp:HyperLink Runat="server" ID="editLink" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> <asp:HyperLink Runat="server" ID="TitleUrl" /></h4>
	<asp:Literal id="Body"  runat="server" />

		<p class="post">
			posted @ <asp:Literal id="PostDescription"  runat="server" />
		</p>
</div>
	<a href="javascript:window.print();" class="printIcon"><span>Print</span></a>
	<asp:Literal ID = "PingBack" Runat="server" />
	<asp:Literal ID = "TrackBack" Runat="server" />