<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<%@ Register TagPrefix="uc1" TagName="PreviousNext" Src="PreviousNext.ascx" %>
<div class="entry">
<uc1:PreviousNext id="PreviousNext" runat="server" />
	<h4><asp:HyperLink Runat="server" ID="editLink" /> <asp:HyperLink Runat="server" ID="editInWlwLink" /> <asp:HyperLink Runat="server" ID="TitleUrl" /></h4>
	<div class="post"><asp:Literal id="Body" runat="server" /></div>
	<p>
		<a href="javascript:window.print();" class="printIcon"><span>Print</span></a> | posted @ <asp:Literal id="PostDescription" runat="server" />
	</p>
</div>
<asp:Literal ID="PingBack" Runat="server" />
<asp:Literal ID="TrackBack" Runat="server" />
