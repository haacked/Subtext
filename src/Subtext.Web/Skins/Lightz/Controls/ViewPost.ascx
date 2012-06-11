<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<%@ Register TagPrefix="uc1" TagName="PreviousNext" Src="PreviousNext.ascx" %>

<uc1:PreviousNext id="PreviousNext" runat="server" />

<div class="block">
	<h1 class="block_title"><asp:HyperLink Runat="server" ID="editLink" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> <asp:HyperLink Runat="server" ID="TitleUrl" /></h1>
	<div class="post">
		<div class="postcontent">
			<asp:Literal id="Body"  runat="server" />
		</div>
		<div class="itemdesc">
			<a href="javascript:window.print();" class="printIcon"><span>Print</span></a> | posted on <asp:Literal id="PostDescription"  runat="server" />
		</div>
	</div>
	<div class="seperator">&nbsp;</div>

	<asp:Literal ID="TrackBack" Runat="server" />