<%@ Control %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<?xml version="1.0"?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
	<head>
		<title><MP:ContentRegion id="MPTitle" runat="server" /></title>
		<link rel="stylesheet" type="text/css" href="~/HostAdmin/style/Style.css" runat="server" ID="Link1" />
		<link rel="stylesheet" type="text/css" href="~/Admin/Resources/scripts/helptip.css" runat="server" ID="Link2" />
		<MP:ScriptTag language="javascript" src="~/Admin/Resources/scripts/helptip.js" runat="server" />
	</head>
	<body>
		<div id="main">
			<div id="logo"><h1><MP:ContentRegion id="MPSectionTitle" runat="server" /></h1></div>
			
			<ul id="menu">
				<MP:MenuItem href="~/HostAdmin/"	title="Manage the blogs installed on this server" runat="server" ID="Menuitem1">Installed Blogs</MP:MenuItem>
			</ul><!-- #menu -->
			
			<div id="sidebar">
				<MP:ContentRegion id="MPSidebar" runat="server" />
			</div><!-- #sidebar -->
			
			<div id="content">
				<MP:ContentRegion id="MPContent" runat="server" />
			</div> <!-- #content -->
			<div id="footer">&nbsp;</div>
			
		</div> <!-- #main -->
		<div id="bottom"></div> <!-- #bottom -->
	</body>
<!-- 
	With thanks to Paul Wilson for the MasterPages system.
	http://authors.aspalliance.com/paulwilson/Articles/?id=14
//-->
</html>