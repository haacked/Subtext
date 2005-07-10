<%@ Control %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en">
	<head>
		<title><MP:ContentRegion id="MPTitle" runat="server" /></title>
		<link rel="stylesheet" id="installStyle" type="text/css" href="~/Install/Style/InstallationStyle.css" runat="server" />
		<link rel="shortcut icon" id="installFavicon"  href="~/favicon.ico" type="image/x-icon" runat="server" />
	</head>
	<body>
		<form id="frmMain" method="post" runat="server">
			<div id="main">
				<img src="~/admin/resources/SubtextLogo.png" runat="server" id="headerLogoImg" vspace="4" />
				<h1>Import Wizard</h1>
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