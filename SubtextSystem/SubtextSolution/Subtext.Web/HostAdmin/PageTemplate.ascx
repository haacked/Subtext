<%@ Control CodeBehind="PageTemplate.ascx.cs" Inherits="Subtext.Web.HostAdmin.PageTemplate" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
	<head>
		<title><MP:ContentRegion id="MPTitle" runat="server" /></title>
		<link rel="stylesheet" type="text/css" href="~/HostAdmin/style/Style.css" runat="server" ID="lnkStyleSheet" />
		<link rel="stylesheet" type="text/css" href="~/Admin/Resources/scripts/helptip.css" runat="server" ID="lnkHelpTipCss" />
		<MP:ScriptTag language="javascript" src="~/Admin/Resources/scripts/helptip.js" runat="server" ID="scrHelpTipJavascript" />
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
					<MP:MenuItem ID="Menuitem2" href="~/HostAdmin/Import/ImportStart.aspx" title="Import blog content into this subText installation."
						runat="server">Import Blog(s)</MP:MenuItem>
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
