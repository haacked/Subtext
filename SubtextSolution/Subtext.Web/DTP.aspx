<%@ Page language="c#" AutoEventWireup="false" Inherits="Subtext.Web.UI.Pages.SubtextMasterPage" %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Import Namespace="Subtext.Framework.Configuration" %>
<asp:Literal ID="docTypeDeclaration" Runat="server" />
	<head>
		<title><asp:Literal ID="pageTitle" Runat="server" /></title>
		<meta http-equiv="content-type" content="text/html; charset=UTF-8" />
		<asp:Literal id="authorMetaTag" runat="server" />
		
		<link id="RSSLink" title="RSS" type="application/rss+xml" rel="alternate" runat="Server" />
		<asp:Literal ID="styles" Runat="server" />
		<link id="MainStyle" type="text/css" rel="stylesheet" runat="Server" />
		<link id="SecondaryCss" type="text/css" rel="stylesheet" runat="Server" />
		<link id="CustomCss" type="text/css" rel="stylesheet" runat="Server" />
		<st:ScriptTag id="blogInfoScript" runat="server" src="~/scripts/BlogInfo.js" />
		<script type="text/javascript">
			<%= AllowedHtmlJavascriptDeclaration %>
			var subtextBlogInfo = new blogInfo('<%= Config.CurrentBlog.VirtualDirectoryRoot %>', '<%= Config.CurrentBlog.VirtualUrl %>" />');
		</script>
		<asp:Literal ID="scripts" Runat="server" />
		<asp:PlaceHolder ID="coCommentPlaceholder" Runat="server" />
		<asp:Literal ID="pinbackLinkTag" runat="server" />
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<DT:MasterPage id="MPContainer" runat="server">
				<DT:ContentRegion id="MPMain" runat="server">
					<asp:PlaceHolder id="CenterBodyControl" runat="server"></asp:PlaceHolder>
				</DT:ContentRegion>
			</DT:MasterPage>
		</form>
	</body>
</html>