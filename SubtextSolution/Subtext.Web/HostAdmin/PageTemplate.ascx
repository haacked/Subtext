<%@ Control CodeBehind="PageTemplate.ascx.cs" Inherits="Subtext.Web.HostAdmin.PageTemplate" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title><MP:ContentRegion id="MPTitle" runat="server" /></title>
		<link rel="stylesheet" type="text/css" href="~/HostAdmin/style/Style.css" runat="server" ID="lnkStyleSheet" />
		<link rel="stylesheet" type="text/css" href="~/Skins/_System/dropshadow.css" runat="server" ID="lnkDropShadow" />
		<link rel="stylesheet" type="text/css" href="~/Skins/_System/csharp.css" runat="server" ID="lnkCsharp" />
		<link rel="stylesheet" type="text/css" href="~/Skins/_System/commonstyles.css" runat="server" ID="lknCommon" />
		<link rel="stylesheet" type="text/css" href="~/HostAdmin/scripts/helptip.css" runat="server" ID="lnkHelpTipCss" />
		<MP:ScriptTag language="javascript" src="~/HostAdmin/scripts/helptip.js" runat="server" ID="scrHelpTipJavascript" />
	</head>
	<body>
		<form id="frmMain" method="post" runat="server">
			<div id="main">
				<div id="logo">
					<div id="loginStatus">Logged in As
						<span id="hostAdminNameText">
							<asp:Literal ID="hostAdminName" Runat="server" Text="" />
						</span> 
						<span id="logoutLinkText">[<a href="~/Logout.aspx" id="logoutLink" runat="server">Logout</a>]</span></div>
					<h1><MP:ContentRegion id="MPSectionTitle" runat="server">Subtext Host Admin</MP:ContentRegion></h1>
				</div>
				<ul id="menu">
					<MP:MenuItem ID="Menuitem1" href="~/HostAdmin/Default.aspx" title="Manage the blogs installed on this server"
						runat="server">Installed Blogs</MP:MenuItem>
					<MP:MenuItem ID="Menuitem2" href="~/HostAdmin/Import/ImportStart.aspx" title="Import .TEXT Data"
						runat="server">Import .TEXT</MP:MenuItem>
					<MP:MenuItem ID="Menuitem3" href="~/HostAdmin/ChangePassword.aspx" title="Change HostAdmin Password."
						runat="server">Change Password</MP:MenuItem>
				</ul> <!-- /#menu -->
				<div id="sidebar">
					<MP:ContentRegion id="MPSidebar" runat="server" />
				</div> <!-- /#sidebar -->
				<div id="content">
					<MP:ContentRegion id="MPContent" runat="server" />
				</div> <!-- /#content -->
				<div id="footer">&nbsp;</div>
			</div> <!-- /#main -->
			<div id="bottom"></div> <!-- /#bottom -->
		</form>
		<!-- 
			With thanks to Paul Wilson for the MasterPages system.
			http://authors.aspalliance.com/paulwilson/Articles/?id=14
		//-->
	</body>
</html>
