<%@ Page language="c#" Codebehind="BlogNotConfiguredError.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.BlogNotConfiguredError" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Blog Not Configured Error</title>
		<style>
			BODY { PADDING-RIGHT: 32px; PADDING-LEFT: 32px; FONT-SIZE: 13px; BACKGROUND: #eee; PADDING-BOTTOM: 32px; MARGIN-LEFT: auto; WIDTH: 80%; COLOR: #000; MARGIN-RIGHT: auto; PADDING-TOP: 32px; FONT-FAMILY: verdana, arial, sans-serif }
			DIV { BORDER-RIGHT: #bbb 1px solid; PADDING-RIGHT: 32px; BORDER-TOP: #bbb 1px solid; PADDING-LEFT: 32px; BACKGROUND: #fff; PADDING-BOTTOM: 32px; BORDER-LEFT: #bbb 1px solid; PADDING-TOP: 32px; BORDER-BOTTOM: #bbb 1px solid }
			H1 { PADDING-RIGHT: 0px; PADDING-LEFT: 0px; FONT-WEIGHT: bold; FONT-SIZE: 150%; PADDING-BOTTOM: 36px; MARGIN: 0px; COLOR: #904; PADDING-TOP: 0px; FONT-FAMILY: "trebuchet ms", ""lucida grande"", verdana, arial, sans-serif }
			H2 { PADDING-RIGHT: 0px; PADDING-LEFT: 0px; FONT-WEIGHT: bold; FONT-SIZE: 105%; PADDING-BOTTOM: 0px; MARGIN: 0px 0px 8px; TEXT-TRANSFORM: uppercase; COLOR: #999; PADDING-TOP: 0px; BORDER-BOTTOM: #ddd 1px solid; FONT-FAMILY: "trebuchet ms", ""lucida grande"", verdana, arial, sans-serif }
			P { PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 6px; MARGIN: 0px; PADDING-TOP: 6px }
			A:link { COLOR: #002c99; TEXT-DECORATION: none }
			A:visited { COLOR: #002c99; TEXT-DECORATION: none }
			A:hover { COLOR: #cc0066; BACKGROUND-COLOR: #f5f5f5; TEXT-DECORATION: underline }
			</style>
	</HEAD>
	<body>
		<!-- 
		//TODO: Make this page templated like the rest of the system.
	-->
		<form id="frmMain" method="post" runat="server">
			<div>
				<h1>Subtext - Your Blog Has Not Been Configured Yet</h1>
				<h2>But I Can Help You</h2>
				<p>
					Welcome! The Subtext Blogging Engine has been installed, but has not been 
					properly configured just yet.
				</p>
				<p>
					If you'd like me to automatically configure Subtext, just specify an 
					administrative username and password below and I'll send you to the admin 
					section where you can finish configuring your blog.
				</p>
				<p>
					<table border="0">
						<tr>
							<td colspan="3">
								<asp:ValidationSummary id="vldSummary" runat="server"></asp:ValidationSummary>
							</td>
						</tr>
						<tr>
							<td>User Name:
							</td>
							<td><asp:RequiredFieldValidator id="vldUsernameRequired" runat="server" ErrorMessage="Please specify a User Name"
									ControlToValidate="txtUserName" Display="Dynamic">*</asp:RequiredFieldValidator></td>
							<td><asp:TextBox ID="txtUserName" Runat="server" EnableViewState="False" /></td>
						</tr>
						<tr>
							<td>Password:</td>
							<td>
								<asp:RequiredFieldValidator id="vldPasswordRequired" runat="server" ErrorMessage="Please specify a Password"
									ControlToValidate="txtPassword" Display="Dynamic">*</asp:RequiredFieldValidator>
								<asp:CompareValidator id="vldComparePasswords" runat="server" ErrorMessage="The passwords do not match."
									ControlToCompare="txtConfirmPassword" ControlToValidate="txtPassword" EnableViewState="False" ValueToCompare="Text"
									Display="Dynamic">*</asp:CompareValidator></td>
							<td><asp:TextBox ID="txtPassword" Runat="server" TextMode="Password" EnableViewState="False" /></td>
						</tr>
						<tr>
							<td>Confirm Password:</td>
							<td>
								<asp:RequiredFieldValidator id="vldConfirmPasswordRequired" runat="server" ErrorMessage="Please confirm your password."
									ControlToValidate="txtConfirmPassword" Display="Dynamic">*</asp:RequiredFieldValidator></td>
							<td><asp:TextBox ID="txtConfirmPassword" Runat="server" TextMode="Password" EnableViewState="False" /></td>
						</tr>
						<tr>
							<td colspan="3" align="right">
								<asp:Button ID="btnSave" Runat="server" Text="Save" />
							</td>
						</tr>
					</table>
				</p>
			</div>
		</form>
	</body>
</HTML>
