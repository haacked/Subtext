<%@ Control %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
	<head>
		<title>Subtext - <MP:ContentRegion id="MPTitleBar" runat="server" /></title>
		<link rel="stylesheet" id="installStyle" type="text/css" href="~/SystemMessages/Style/SystemMessageStyle.css" runat="server" />
		<link rel="shortcut icon" id="installFavicon"  href="~/favicon.ico" type="image/x-icon" runat="server" />
	</head>
	<body>
		<form id="frmMain" method="post" runat="server">
			<div id="main">
				<img src="~/images/SubtextLogo.png" runat="server" id="headerLogoImg" align="right" vspace="4" />
				<h1><MP:ContentRegion id="MPTitle" runat="server" /></h1>
				<h2><MP:ContentRegion id="MPSubTitle" runat="server" /></h2>
				<MP:ContentRegion id="Content" runat="server" />		
			</div>
		</form>
	</body>
<!-- 
	With thanks to Paul Wilson for the MasterPages system.
	http://authors.aspalliance.com/paulwilson/Articles/?id=14
//-->
</html>