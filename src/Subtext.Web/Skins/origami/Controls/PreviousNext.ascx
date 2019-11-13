<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PreviousNext" %>
<div class="previousNext">
	<asp:HyperLink id="PrevLink" runat="server" ToolTip="previous post" Format="<< {0}" />
	<asp:Label id="LeftPipe" runat="server" CssClass="prevNextSeparator"> | </asp:Label>
	<asp:HyperLink id="MainLink" runat="server">Home</asp:HyperLink>
	<asp:Label id="RightPipe" runat="server" CssClass="prevNextSeparator"> | </asp:Label>
	<asp:HyperLink id="NextLink" runat="server" ToolTip="next post" Format="{0} >>" />
</div>
