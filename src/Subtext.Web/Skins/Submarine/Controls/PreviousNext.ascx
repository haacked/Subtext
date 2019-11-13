<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PreviousNext" %>
<div class="previousNext">
		<asp:HyperLink id="PrevLink" runat="server">HyperLink</asp:HyperLink>
		<asp:Label id="LeftPipe" runat="server"> | </asp:Label>
		<asp:HyperLink id="MainLink" runat="server">Home</asp:HyperLink>
		<asp:Label id="RightPipe" runat="server"> | </asp:Label>
		<asp:HyperLink id="NextLink" runat="server">HyperLink</asp:HyperLink>
</div>
