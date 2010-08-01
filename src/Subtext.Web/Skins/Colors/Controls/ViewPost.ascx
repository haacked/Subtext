<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<%@ Register TagPrefix="uc1" TagName="PreviousNext" Src="PreviousNext.ascx" %>

<uc1:PreviousNext id="PreviousNext" runat="server" />

<div class="dropshadow">
	<div class="contentbox">
		<h2><asp:HyperLink Runat="server" ID="editLink" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> <asp:HyperLink  CssClass="singleposttitle" Runat="server" ID="TitleUrl" /></h2>
		<div class="content">
			<asp:Literal id="Body"  runat="server" />
			<div class = "itemdesc">
				<a href="javascript:window.print();" class="printIcon"><span>Print</span></a> | posted on <asp:Literal id="PostDescription"  runat="server" />
			</div>
		</div>	
	</div>
</div>
<asp:Literal ID="TrackBack" Runat="server" />
