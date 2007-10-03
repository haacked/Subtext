<%@ Page Language="C#" EnableTheming="false" AutoEventWireup="false" Inherits="Subtext.Web.UI.Pages.SubtextMasterPage" %>
<%@ Import namespace="Subtext.Framework.Components"%>
<%@ Import namespace="Subtext.Framework.Configuration"%>
<%@ Import namespace="Subtext.Framework.Configuration"%>
<%@ Import namespace="Subtext.Framework"%>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head runat="server">
		<title><asp:Literal ID="pageTitle" Runat="server" /></title>
		<meta http-equiv="content-type" content="text/html; charset=UTF-8" />
		<asp:Literal id="authorMetaTag" runat="server" />
		<asp:Literal id="versionMetaTag" runat="server" />
		<asp:Literal id="additionalMetaTags" runat="server" />
		<link id="RSSLink" title="RSS Feed" type="application/rss+xml" rel="alternate" runat="Server" />
		<link id="AtomLink" title="Atom Feed" type="application/rss+xml" rel="alternate" runat="Server" />
        <asp:literal id="styles" runat="server"></asp:literal>
        <link id="MainStyle" runat="server" type="text/css" rel="stylesheet" />
        <link id="CustomCss" runat="server" type="text/css" rel="stylesheet" />
		<link id="Rsd" rel="EditURI" type="application/rsd+xml" title="RSD" runat="server" />
		<st:ScriptTag id="commonJs" src="~/Scripts/common.js" runat="server" />
		<script type="text/javascript">
			<%= AllowedHtmlJavascriptDeclaration %>
			var subtextBlogInfo = new blogInfo('<%= BlogInfo.VirtualDirectoryRoot %>', '<%= Config.CurrentBlog.VirtualUrl %>');
		</script>
		<asp:Literal ID="scripts" Runat="server" />
		<asp:PlaceHolder ID="coCommentPlaceholder" Runat="server" />
		<asp:Literal ID="pinbackLinkTag" runat="server" />
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
            <asp:ScriptManager ID="SubtextScriptManager" runat="server" EnablePartialRendering="true">
            </asp:ScriptManager>
			<DT:MasterPage id="MPContainer" runat="server">
				<DT:ContentRegion id="MPMain" runat="server">
					<asp:PlaceHolder id="CenterBodyControl" runat="server"></asp:PlaceHolder>
				</DT:ContentRegion>
			</DT:MasterPage>
		</form>
	<asp:Literal ID="customTrackingCode" Runat="server" />
	</body>
</html>
