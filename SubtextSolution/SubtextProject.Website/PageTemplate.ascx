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
		<sp:ScriptTag id="HelptipJs" type="text/javascript" src="~/scripts/helptip.js" runat="server" />
		<link rel="stylesheet" type="text/css" href="~/scripts/helptip.css" runat="server" ID="Link1"/>
	</head>
	<body>
		<div id="main">
			<div id="logo"><h1>Subtext</h1></div>
				
			<ul id="menu">
				<SP:MenuItem href="~/"			title="Home is where you hang your hat" runat="server">Home</SP:MenuItem>
				<SP:MenuItem href="~/About/"	title="Everything you didn't want to know about Subtext." runat="server">About</SP:MenuItem>
				<SP:MenuItem href="~/Docs/"	title="Everything you DID want to know about Subtext." runat="server">Docs</SP:MenuItem>
				<SP:MenuItem href="~/Roadmap/"	title="Everything you will want to know about Subtext." runat="server" ID="Menuitem1">Roadmap</SP:MenuItem>
				<SP:MenuItem href="~/License/"	title="Subtext is licensed under the BSD license." runat="server" ID="mnuLicense">License</SP:MenuItem>
			</ul><!-- #menu -->
			
			<div id="sidebar">
				<MP:ContentRegion id="MPSidebar" runat="server" />
			</div><!-- #sidebar -->
			
			<div id="content">
				<MP:ContentRegion id="MPContent" runat="server" />
				<div id="adwrap">
					<p id="ad">
						<script type="text/javascript">
						<!--
						google_ad_client = "pub-7694059317326582";
						google_ad_width = 468;
						google_ad_height = 60;
						google_ad_format = "234x60_as";
						google_ad_type = "text_image";
						google_ad_channel = "";
						google_color_border = "FFFFFF";
						google_color_bg = "FFFFFF";
						google_color_link = "444444";
						google_color_url = "BF5010";
						google_color_text = "444444";
						//-->
						</script>

						<script type="text/javascript" src="http://pagead2.googlesyndication.com/pagead/show_ads.js"></script>
					</p>
				</div> <!-- end google ad -->
			</div> <!-- #content -->
			<div id="footer">
				<span>Subtext Logo design by <a href="http://turbomilk.com/" title="Powerful and healthy GUI design for the human" rel="external">TurboMilk</a>.</span>
			</div>
			
		</div> <!-- #main -->
		<div id="bottom"></div> <!-- #bottom -->
	</body>
<!-- 
	With thanks to Paul Wilson for the MasterPages system.
	http://authors.aspalliance.com/paulwilson/Articles/?id=14
//-->
</html>