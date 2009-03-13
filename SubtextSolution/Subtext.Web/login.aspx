<%@ Page Language="C#" EnableTheming="false"  EnableViewState="False" Codebehind="login.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Pages.login" %>
<%@ Register Assembly="DotNetOpenId" Namespace="DotNetOpenId.RelyingParty" TagPrefix="cc1" %>  

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title>Subtext - Login</title>
		<link rel="stylesheet" type="text/css" href="<%= VirtualPathUtility.ToAbsolute("~/Skins/_System/SystemStyle.css") %>" />
		<link rel="stylesheet" type="text/css" href="<%= VirtualPathUtility.ToAbsolute("~/Skins/_System/forms.css") %>" />
		<link rel="stylesheet" type="text/css" href="<%= VirtualPathUtility.ToAbsolute("~/Skins/_System/login.css") %>" />
	</head>
	<body>
		<form method="post" runat="server" id="form1" defaultfocus="tbUserName">
			<div id="loginPage" class="main">
				<div id="logo"></div>
				<fieldset>
				    <legend>Please Sign In Below</legend>
				    
				    <span class="error"><asp:Literal EnableViewState="false" id="Message" runat="server" /></span>
				    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="tbUserName">Username</asp:Label>
					<asp:TextBox id="tbUserName" runat="server" CssClass="textbox" TabIndex="1" />
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="tbUserName"
							ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login">*</asp:RequiredFieldValidator>

				    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="tbPassword">Password</asp:Label>
				    <asp:TextBox id="tbPassword" TextMode="Password" runat="server" CssClass="textbox" TabIndex="2" />
				    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="tbPassword"
							ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login">*</asp:RequiredFieldValidator>
				    
				    <asp:CheckBox ID="chkRememberMe" runat="server" Text="Remember me" TabIndex="3" CssClass="rememberMe" />
				    <asp:Button id="btnLogin" runat="server" Text="Login" CssClass="button" onclick="btnLogin_Click" TabIndex="4" />
				    <% if (Config.CurrentBlog != null) { %>
				    <p class="forgotPassword">
					    <a href="ForgotPassword.aspx" title="Click to reset your password" tabindex="5">Forgot Your Password?</a>
				    </p>
				    <% } %>
				</fieldset>
				
				<fieldset>
				    <legend>Sign In with OpenID</legend>
				    <span class="error"><asp:Literal ID="openIdMessage" runat="server" /></span>
                    <cc1:openidlogin id="btnOpenIdLogin" cssclass="openidLogin" runat="server" 
                        LabelText="" RequestEmail="Require" RequestNickname="Request" RegisterVisible="false"   
                         RememberMeVisible="True" PolicyUrl="~/PrivacyPolicy.aspx" TabIndex="6" 
                         OnLoggedIn="btnOpenIdLogin_LoggedIn" ButtonText="Login" Text="http://" OnLoggingIn="btnOpenIdLogin_LoggingIn" />
				</fieldset>
			</div>
		</form>
	</body>
</html>
