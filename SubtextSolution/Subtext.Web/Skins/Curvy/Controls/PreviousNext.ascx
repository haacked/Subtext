<%@ Control Language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.PreviousNext" %>
<div class="previousNext" >
	<table border="0" width="100%">
		<tr>
			<td align="left">
				<asp:HyperLink id="PrevLink" runat="server" CssClass="prev"></asp:HyperLink>
			</td>
			<td align="right">
				<asp:HyperLink id="NextLink" runat="server" CssClass="next"></asp:HyperLink>
			</td>
		</tr>
	</table>
<!--<asp:Label id="LeftPipe" runat="server"> | </asp:Label>
<asp:HyperLink id="MainLink" runat="server" CssClass="home">Home</asp:HyperLink>
<asp:Label id="RightPipe" runat="server"> | </asp:Label>-->
</div>
