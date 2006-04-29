<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PreviousNext" %>
<div class="previousNext">
	<div class="title">Browse to previous or next entry</div>
<br />
		<asp:HyperLink id="PrevLink" runat="server" ToolTip="previous post" class="button">HyperLink</asp:HyperLink>
		<asp:Label id="LeftPipe" runat="server"> -|- </asp:Label>
		
		<asp:HyperLink id="MainLink" runat="server" class="button">Home</asp:HyperLink>
		
		<asp:Label id="RightPipe" runat="server"> -|- </asp:Label>
		<asp:HyperLink id="NextLink" runat="server" ToolTip="next post" class="button">HyperLink</asp:HyperLink>
<p></p>
</div>
