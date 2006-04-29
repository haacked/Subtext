<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<%@ Register TagPrefix="uc1" TagName="PreviousNext" Src="PreviousNext.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RelatedLinks" Src="RelatedLinks.ascx" %>
<uc1:PreviousNext id="PreviousNext1" runat="server" />
<div class="post">
	<div class="title">
		<asp:HyperLink Runat="server" ID="editLink" /><asp:HyperLink Runat="server" ID="TitleUrl" />
	</div>
	<div class="body">
		<asp:Literal id="Body"  runat="server" />
	</div>
	<div class="info">
		posted @ <asp:Literal id="PostDescription" runat="server" />
	</div>
	<uc1:RelatedLinks id="RelatedLinks1" runat="server" />
</div>
	<asp:Literal ID = "PingBack" Runat="server" />
	<asp:Literal ID = "TrackBack" Runat="server" />

	