<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<%@ Register TagPrefix="uc1" TagName="previousNext" Src="PreviousNext.ascx" %>
<uc1:previousNext id="previousNext" runat="server" />
<div class="post">
	<div class="title">
		<asp:HyperLink  Runat="server" ID="editLink" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> <asp:hyperlink runat="server" id="TitleUrl" />
	</div>
	<div class="body">
		<asp:literal id="Body" runat="server" />
	</div>
	<div class="info">
		posted on <asp:literal id="PostDescription" runat="server" /> | <a href="javascript:window.print();" class="printIcon"><span>Print</span></a>
	</div>
	<div class="trackback">
		<asp:literal id="PingBack" runat="server" />
		<asp:literal id="TrackBack" runat="server" />
	</div>
</div>
