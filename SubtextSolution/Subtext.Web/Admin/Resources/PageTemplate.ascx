<%@ Control Language="c#" AutoEventWireup="false" Codebehind="PageTemplate.ascx.cs" Inherits="Subtext.Web.Admin.PageTemplate" targetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="ST" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Import Namespace = "Subtext.Web.Admin" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>
			<ANW:PlaceHolder id="PageTitle" runat="server">Subtext - Manage</ANW:PlaceHolder>
		</title>
		<ST:ScriptTag id="NiceForms" type="text/javascript" src="~/admin/resources/Scripts/niceForms.js" runat="server" />
		<ST:ScriptTag id="HelptipJs" type="text/javascript" src="~/admin/resources/Scripts/helptip.js" runat="server" />
		<ST:ScriptTag id="AdminJs" type="text/javascript" src="~/admin/resources/Scripts/admin.js" runat="server" />
		<ST:ScriptTag id="tableEffectsJS" type="text/javascript" src="~/admin/resources/Scripts/tableEffects.js" runat="server" />
		<ANW:HeaderLink id="HelptipCss" rel="stylesheet" href="resources/scripts/helptip.css" linkType="text/css"
			runat="server" />
		<ANW:HeaderLink id="Css1" rel="stylesheet" href="resources/admin.css" linkType="text/css" runat="server" />
		<ANW:HeaderLink id="NiceFormsStyle" rel="stylesheet" href="resources/scripts/niceforms-default.css" linkType="text/css" runat="server" />
	</head>
	<body id="AdminSection" runat="server">
		<form id="frmMain" method="post" runat="server">
			<table id="BodyTable" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td>
						<a href="http://www.subtextproject.com/"><img id="HeaderLogo" align="right" src='<%= Utilities.ResourcePath + "resources/SubtextLogo_small.png" %>' border="0" ></a>
					</td>
					<td id="header">
						<div id="SiteNav">
							Logged in as <strong>
								<asp:Literal Runat="server" ID="LoggedInUser" /></strong> (<asp:LinkButton id="LogoutLink" runat="server">logout</asp:LinkButton>) 
							| <a href="http://www.subtextproject.com/" title="Subtext Project Site">Subtext Project Site</a>
						</div>
						<div id="BlogTitle">
							<asp:HyperLink id="BlogTitleLink" runat="server" />
						</div>
					</td>
				</tr>
				<tr>
					<td style="vertical-align:bottom;">
						<div id="LeftNavHeader"><ANW:PlaceHolder id="LabelActions" runat="server" /></div>
					</td>
					<td class="NavHeaderRow">
						<div id="headerSS">
							<ul id="TopNav">
								<li id="PostsTab"><a href="EditPosts.aspx" id="TabPosts" title="Posts">Posts</a></li>
								<li id="ArticlesTab"><a href="EditArticles.aspx" id="TabArticles" title="Articles">Articles</a></li>
								<li id="FeedbackTab"><a href="Feedback.aspx" id="TabFeedback" title="Feedback">Feedback</a></li>
								<li id="LinksTab"><a href="EditLinks.aspx" id="TabLinks" title="Links">Links</a></li>
								<li runat="server" id="GalleryTab">
									<a href="EditGalleries.aspx" id="TabGalleries" title="Galleries">Galleries</a>
								</li>
								<li id="StatsTab">
									<a href="Statistics.aspx" id="TabStats" title="Stats">Stats</a>
								</li>
								<li id="OptionsTab">
									<a href="Options.aspx" id="TabOptions" title="Options">Options</a></li>
								</li>
								<li id="ImportExportTab">
									<a href="ImportExport.aspx" id="TabImportExport" title="Import/Export">Import/Export</a></li>
							</ul>
						</div>
						<div id="SubNav">
							<ANW:BreadCrumbs id="BreadCrumbs" UsePrefixText="true" IsPanel="false" IncludeRoot="false" runat="server" />
						</div>
					</td>
				</tr>
				<tr>
					<td class="NavLeftCell">
						<div id="LeftNav">
							<ANW:LinkList id="LinksActions" runat="server" />
						</div>
						<div class="LeftNavHeader">
							<ANW:PlaceHolder id="LabelCategories" runat="server" />
						</div>
						<div id="LeftNav">
							<ANW:LinkList id="LinksCategories" runat="server" />
						</div>
					</td>
					<td id="Body">
						<div id="Main">
							<ANW:PlaceHolder id="PageContent" runat="server">Default page content goes here.</ANW:PlaceHolder>
						</div>
					</td>
				</tr>
			</table>
			<table id="Footer" border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td id="Footer" colspan="2">
						<div>
							Running <strong><%= Subtext.Framework.VersionInfo.VersionDisplayText %></strong>
						</div>
						<div>
							<a href="http://www.asp.net"><img id="PoweredBy" src='<%= Utilities.ResourcePath + "resources/poweredbydotnet.gif" %>' height="33" width="99"></a>
							Subtext Copyright &copy; <a href="http://haacked.com/">Phil Haack</a>, 2005. All rights 
							reserved.<br />
							.TEXT Copyright &copy; <a href="http://scottwater.com/blog">Scott Watermasysk</a>, 2003. 
							All rights reserved.
						</div>
					</td>
				</tr>
			</table>
			<div id="stylesheetTest"></div>
		</form>
	</body>
</html>
