<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="BlogStats.ascx" %>
<%@ Control Language="C#" EnableTheming="false"  Inherits="Subtext.Web.UI.Controls.Header" %>
<div id="top">
<table width="100%" cellpadding="8" cellspacing="0">
	<tr>
		<td>
			<h1><asp:HyperLink id="HeaderTitle" CssClass="headermaintitle" runat="server" /></h1>
			<span id="subtitle"><asp:Literal id="HeaderSubTitle" runat="server" /></span>
		</td>
	</tr>
</table>
</div>
<div id="sub"><uc1:BlogStats id="BlogStats1" runat="server"></uc1:BlogStats></div>


