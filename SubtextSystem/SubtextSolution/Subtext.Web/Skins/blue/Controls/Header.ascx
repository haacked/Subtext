<%@ Control Language="c#" Inherits="Subtext.Web.UI.Controls.Header" %>
<div class="header">
	<div>
		<asp:HyperLink id="HeaderTitle" CssClass="headermaintitle" runat="server" />
	</div>
	<div>
		<asp:Literal id="HeaderSubTitle" runat="server" />
	</div>
</div>