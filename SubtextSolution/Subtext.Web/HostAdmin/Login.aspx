<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Subtext.Web.HostAdmin.Login" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title>Subtext - Login</title>
		<st:StyleTag id="SystemStyle" href="~/Skins/_System/SystemStyle.css" runat="server" />
	</head>
	<body>
		<form method="post" runat="server" ID="frmLogin">
			<asp:Login ID="loginControl" runat="server">
				<LayoutTemplate>
					<div id="Main">
						<div id="logo"></div>
						<div id="Heading">Please Log In Below</div>
						
						<asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
						<asp:TextBox ID="UserName" runat="server" CssClass="Textbox" />
						<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
							ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
											
						<asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
											
						<asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="Textbox" />
						<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
							ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
						
						<asp:CheckBox ID="RememberMe" runat="server" CssClass="LoginFloat" Text="Remember me next time." />
											
						<asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
					
						<asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="Login1" />
					</div>
				</LayoutTemplate>
			</asp:Login>
		</form>
	</body>
</html>
