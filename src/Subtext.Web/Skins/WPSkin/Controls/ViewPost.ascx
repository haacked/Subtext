<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<%@ Register TagPrefix="uc1" TagName="PreviousNext" Src="PreviousNext.ascx" %>

<uc1:PreviousNext id="PreviousNext" runat="server" />

	<div class="post">
		<h2>
			<asp:HyperLink Runat="server" ID="editLink" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> <asp:HyperLink Runat="server" ID="TitleUrl" />
		</h2>
		<asp:Literal id="Body"  runat="server" />
		<p class="postfoot">
			<a href="javascript:window.print();" class="printIcon"><span>Print</span></a> | posted on <asp:Literal id="PostDescription"  runat="server" />
		</p>
	</div>
	<asp:Literal ID="TrackBack" Runat="server" />
	