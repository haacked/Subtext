<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Controls.Login" %>
<h1>Login</h1>
<div id="LoginForm">
	Username:<br />
	<asp:TextBox id="tbUserName" runat="server" CssClass="text"/><br />
	Password:<br />
	<asp:TextBox id="tbPassword" TextMode="Password" runat="server" CssClass="text"/><br /><br />
	<asp:Button id="btnLogin" runat="server" Text="Log In" CausesValidation="False" />&nbsp;<asp:Literal id="Message" runat="server" />
	&nbsp;<asp:CheckBox ID = "RememberMe" Runat = "server" /> Remember Me<br />
</div>