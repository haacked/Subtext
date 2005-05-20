<%@ Control %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="SP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<?xml version="1.0"?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
	<head>
		<title><MP:ContentRegion id="MPTitle" runat="server" /></title>
		<link rel="stylesheet" type="text/css" href="~/style/Style.css" runat="server" />
		<link rel="shortcut icon"  href="~/favicon.ico" type="image/x-icon" runat="server" />
	</head>
	<body>
		<div id="main">
			<div id="logo"><a href="~/" title="Subtext" runat="server" ID="A1"><img src="~/images/header_logo.gif" align="right" alt="Temporary Logo" runat="server" ID="Img1" border="0" /></a></div>
			
			<ul id="menu">
				<SP:MenuItem href="~/"			title="Home is where you hang your hat" runat="server">Home</SP:MenuItem>
				<SP:MenuItem href="~/About/"	title="Everything you didn't want to know about Subtext." runat="server">About</SP:MenuItem>
				<SP:MenuItem href="~/Docs/"	title="Everything you DID want to know about Subtext." runat="server">Docs</SP:MenuItem>
				<SP:MenuItem href="~/Roadmap/"	title="Everything you will want to know about Subtext." runat="server" ID="Menuitem1">Roadmap</SP:MenuItem>
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