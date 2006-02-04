<%@ Page language="c#" EnableViewState="False" Codebehind="login.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Pages.login" %>
<%@ Register tagprefix="mbdb" namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.DefaultButtons" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<HTML>
	<HEAD>
		<title>Subtext - Login</title>
		<style>
		BODY { PADDING-RIGHT: 32px; MARGIN-TOP: 100px; PADDING-LEFT: 32px; FONT-SIZE: 13px; BACKGROUND: #eee; PADDING-BOTTOM: 32px; MARGIN-LEFT: auto; WIDTH: 320px; COLOR: #000; MARGIN-RIGHT: auto; PADDING-TOP: 32px; FONT-FAMILY: verdana, arial, sans-serif }
		DIV#Main { BORDER-RIGHT: #bbb 1px solid; PADDING-RIGHT: 32px; BORDER-TOP: #bbb 1px solid; PADDING-LEFT: 32px; BACKGROUND: #fff; PADDING-BOTTOM: 32px; BORDER-LEFT: #bbb 1px solid; PADDING-TOP: 32px; BORDER-BOTTOM: #bbb 1px solid }
		DIV#Heading { PADDING-RIGHT: 0px; PADDING-LEFT: 0px; FONT-WEIGHT: bold; FONT-SIZE: 150%; PADDING-BOTTOM: 18px; MARGIN: 0px; COLOR: #000999; PADDING-TOP: 0px; FONT-FAMILY: "trebuchet ms", ""lucida grande"", verdana, arial, sans-serif }
		H2 { PADDING-RIGHT: 0px; PADDING-LEFT: 0px; FONT-WEIGHT: bold; FONT-SIZE: 105%; PADDING-BOTTOM: 0px; MARGIN: 0px 0px 8px; TEXT-TRANSFORM: uppercase; COLOR: #000999; PADDING-TOP: 0px; BORDER-BOTTOM: #ddd 1px solid; FONT-FAMILY: "trebuchet ms", ""lucida grande"", verdana, arial, sans-serif }
		P { PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 6px; MARGIN: 0px; PADDING-TOP: 6px }
		A:link { COLOR: #002c99; TEXT-DECORATION: none }
		A:visited { COLOR: #002c99; TEXT-DECORATION: none }
		A:hover { COLOR: #cc0066; BACKGROUND-COLOR: #f5f5f5; TEXT-DECORATION: underline }
		LABEL { DISPLAY: block; FONT-WEIGHT: bold; FONT-SIZE: 9px; MARGIN: 6px 0px 2px; TEXT-TRANSFORM: uppercase; COLOR: #999 }
		INPUT.Textbox { PADDING-RIGHT: 2px; PADDING-LEFT: 2px; PADDING-BOTTOM: 2px; WIDTH: 260px; PADDING-TOP: 2px; FONT-FAMILY: verdana, arial, sans-serif }
		A.Button:link { BORDER-RIGHT: #999 1px solid; PADDING-RIGHT: 12px; BORDER-TOP: #999 1px solid; DISPLAY: block; PADDING-LEFT: 12px; FONT-WEIGHT: bold; BACKGROUND: #000999; FLOAT: left; PADDING-BOTTOM: 3px; BORDER-LEFT: #999 1px solid; COLOR: #fff; PADDING-TOP: 3px; BORDER-BOTTOM: #999 1px solid }
		A.Button:visited { BORDER-RIGHT: #999 1px solid; PADDING-RIGHT: 12px; BORDER-TOP: #999 1px solid; DISPLAY: block; PADDING-LEFT: 12px; FONT-WEIGHT: bold; BACKGROUND: #000999; FLOAT: left; PADDING-BOTTOM: 3px; BORDER-LEFT: #999 1px solid; COLOR: #fff; PADDING-TOP: 3px; BORDER-BOTTOM: #999 1px solid }
		.LoginButton { BORDER-RIGHT: #999 1px solid; PADDING-RIGHT: 12px; BORDER-TOP: #999 1px solid; DISPLAY: block; PADDING-LEFT: 12px; FONT-WEIGHT: bold; BACKGROUND: #000999; FLOAT: left; PADDING-BOTTOM: 3px; BORDER-LEFT: #999 1px solid; COLOR: #fff; PADDING-TOP: 3px; BORDER-BOTTOM: #999 1px solid }
		.LoginButton:hover { COLOR: yellow }
		SPAN.ErrorMessage { DISPLAY: block; FONT-WEIGHT: bold; COLOR: #904 }
		P.Small { MARGIN-TOP: 12px; FONT-SIZE: 85% }
		</style>
	</HEAD>
	<body>
		<form method="post" runat="server" ID="frmLogin">
			<mbdb:DefaultButtons runat="server" id="DefaultButtons1">
				<mbdb:DefaultButtonSetting parent="tbPassword" button="btnLogin" />
			</mbdb:DefaultButtons>
			<div id="Main">
				<img src="~/admin/resources/SubtextLogo.png" runat="server" id="headerLogoImg" vspace="4" />
				<div id="Heading">Please Sign In Below</div>
				<label>Username <asp:RequiredFieldValidator ForeColor="#990044" ControlToValidate="tbUserName" Text="Please enter username"
						Runat="server" id="RequiredFieldValidator1" /></label>
				<asp:TextBox id="tbUserName" runat="server" CssClass="Textbox" />
				<label>Password</label>
				<asp:TextBox id="tbPassword" TextMode="Password" runat="server" CssClass="Textbox" />
				<asp:Button id="btnLogin" runat="server" Text="Login" CssClass="Button LoginButton" style="MARGIN-TOP: 8px" />
				<p style="MARGIN: 4px 0px 0px 70px">
					<asp:CheckBox id="chkRemember" runat="server" CssClass="LoginFloat" />Remember 
					me?
				</p>
				<br style="CLEAR: both">
				<asp:Label id="Message" runat="server" ForeColor="#990044" FontBold="true" CssClass="ErrorMessage"></asp:Label>
				<p class="Small">
					Forget your password?
					<br />
					<a visible="false" id="aspnetLink" runat="server" href="http://asp.net/Forums/User/EmailForgottenPassword.aspx?tabindex=1">
						Email me my password</a>
					<asp:LinkButton id="lbSendPassword" runat="server" visible="true">Email me my password</asp:LinkButton>.
				</p>
			</div>
		</form>
	</body>
</HTML>
