<%@ Page CodeBehind="AggDefault.aspx.cs" EnableViewState="false" Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web.AggDefault" %>
<%@ OutputCache VaryByParam="GroupID" VaryByHeader="Accept-Language" CacheProfile="ChangedFrequently" %>
<%@ Import namespace="Subtext.Framework.Configuration"%>
<%@ Register TagPrefix="uc1" TagName="AggSyndication" Src="~/Skins/Aggregate/Simple/Controls/AggSyndication.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggBlogStats" Src="~/Skins/Aggregate/Simple/Controls/AggBlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggBloggers" Src="~/Skins/Aggregate/Simple/Controls/AggBloggers.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggRecentPosts" Src="~/Skins/Aggregate/Simple/Controls/AggRecentPosts.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggRecentImages" Src="~/Skins/Aggregate/Simple/Controls/AggRecentImages.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggPinnedPost" Src="~/Skins/Aggregate/Simple/Controls/AggPinnedPost.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head runat="server">
		<title><asp:Literal id="title" runat="server" Text="<%$ AppSettings:AggregateTitle %>" /></title>
		<link href="../skins/_system/csharp.css" rel="stylesheet" type="text/css" />
		<link href="../skins/_system/commonstyle.css" rel="stylesheet" type="text/css" />
		<link href="../skins/_system/commonlayout.css" rel="stylesheet" type="text/css" />
		<asp:Literal id="Style" runat="Server" />
		<script type="text/javascript" src="<%= VirtualPathUtility.ToAbsolute("~/Scripts/common.js") %>"></script>
        <link href="../css/lightbox.css" rel="stylesheet" type="text/css" />
        <script type="text/javascript" src="<%= VirtualPathUtility.ToAbsolute("~/Scripts/lightbox.js") %>"></script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="header">
				<h1><asp:HyperLink ID="TitleLink" Text="<%$ AppSettings:AggregateTitle %>" NavigateUrl="<%# AggregateUrl %>" Runat="server" /></h1>
			</div>
			<div id="authors">
				<h2>Welcome</h2>
				<p>
					This is the generic homepage (aka Aggregate Blog) for a Subtext community website. It aggregates 
					posts from every blog installed in this server. To modify this page, look for the Aggregate skin 
					folder in the Skins directory.
				</p>
				<p>
					To learn more about the application, check out <a href="http://subtextproject.com/" title="Subtext Project Website" rel="external">
					the Subtext Project Website</a>.
				</p>
				<p>
					Powered By:<br />
					<a href="http://subtextproject.com/"><img src="<%= ImageUrl("PoweredBySubtext85x33.png") %>" alt="Powered by Subtext" /></a>
				</p>
                <uc1:AggSyndication ID="AggSyndication1" runat="server" />				
                <uc1:AggBlogStats ID="AggBlogStats1" runat="server" />				
				<uc1:AggBloggers ID="AggBloggers1" runat="server" ShowGroups="true" />
			</div>
			<div id="main">
			    <!-- Update EntryID in the next line to the ID of a post you want pinned here-->
			    <uc1:AggPinnedPost ID="AggPinnedPost1" runat="server" ContentID="3" EntryTitle="Welcome All" />
				<uc1:AggRecentPosts ID="AggRecentPosts1" runat="server" Count="20" />
			</div>
			<div id="extra">
                <uc1:AggRecentImages ID="AggRecentImages1" runat="server" Count="20" />
            </div>
		</form>
	</body>
</html>
