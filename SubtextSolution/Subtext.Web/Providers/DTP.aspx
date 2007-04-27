<%@ Page Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.UI.Pages.SubtextMasterPage" %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Import Namespace="Subtext.Framework.Configuration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head runat="server">
		<title><asp:Literal ID="pageTitle" Runat="server" /></title>
		<meta http-equiv="content-type" content="text/html; charset=UTF-8" />
		<asp:Literal id="authorMetaTag" runat="server" />
		<asp:Literal id="versionMetaTag" runat="server" />
		<link id="RSSLink" title="RSS" type="application/rss+xml" rel="alternate" runat="Server" />
		<asp:Literal ID="styles" Runat="server" />
		<link id="MainStyle" type="text/css" rel="stylesheet" runat="Server" />
		<link id="SecondaryCss" type="text/css" rel="stylesheet" runat="Server" />
		<link id="CustomCss" type="text/css" rel="stylesheet" runat="Server" />
		<link id="Rsd" rel="EditURI" type="application/rsd+xml" title="RSD" runat="server" />
		<link id="AtomLink" title="RSS" type="application/rss+xml" rel="alternate" runat="Server" />
		<st:ScriptTag id="commonJs" src="~/Scripts/common.js" runat="server" />
		<script type="text/javascript">
			<%= AllowedHtmlJavascriptDeclaration %>
			var subtextBlogInfo = new blogInfo('<%= Config.CurrentBlog.VirtualDirectoryRoot %>', '<%= Config.CurrentBlog.VirtualUrl %>');
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
	</body>
</html>
