<%@ Page language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Pages.SubtextMasterPage" %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<!--StartFragment -->
<!DOCTYPE html 
     PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
     "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title><asp:Literal ID="pageTitle" Runat="server" /></title>
		<link id="MainStyle" type="text/css" rel="stylesheet" runat="Server" />
		<link id="SecondaryCss" type="text/css" rel="stylesheet" runat="Server" />
		<link id="RSSLink" title="RSS" type="application/rss+xml" rel="alternate" runat="Server" />
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<DT:MASTERPAGE id="MPContainer" runat="server">
				<DT:contentregion id="MPMain" runat="server">
					<asp:PlaceHolder id="CenterBodyControl" runat="server"></asp:PlaceHolder>
				</DT:contentregion>
			</DT:MASTERPAGE>
		</form>
	</body>
</html>