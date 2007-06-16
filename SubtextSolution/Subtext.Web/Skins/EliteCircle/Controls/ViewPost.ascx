<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.ViewPost" %>
<%@ Register TagPrefix="st" TagName="previousNext" Src="PreviousNext.ascx" %>

<st:previousNext id="previousNext" runat="server" />

<h1><asp:HyperLink  Runat="server" ID="editLink" /><asp:hyperlink runat="server" id="TitleUrl" /></h1>

<asp:literal id="Body" runat="server" />

<p class="post-footer align-right">					
	<asp:Label id="commentCount" runat="server" />
	<a href="javascript:window.print();" class="printIcon"><span>Print</span></a>
	<asp:Label id="date" runat="server" Format="HH:mm - MMM dd, yyyy" CssClass="date" />

	
	<asp:literal id="PingBack" runat="server" />
	<asp:literal id="TrackBack" runat="server" />
</p>