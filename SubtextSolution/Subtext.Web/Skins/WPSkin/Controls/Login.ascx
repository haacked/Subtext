<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Login" %>
<div class = "listtitle">Login</div>
	<ul class = "list">
		u: <asp:TextBox id="tbUserName" runat="server" /><br />
		p: <asp:TextBox id="tbPassword" TextMode="Password" runat="server" Width="155px" /><br />
		Remember Me: <asp:CheckBox ID = "RememberMe" Runat = "server" /><br />
		<asp:Button id="btnLogin" runat="server" Text="Log In" CausesValidation="False" />
		&nbsp;<asp:Literal id="Message" runat="server" />
</ul>