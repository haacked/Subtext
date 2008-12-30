<%@ Page CodeBehind="default.aspx.cs" EnableViewState="false" Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web._default" %>
<%@ OutputCache Duration="600" VaryByParam="GroupID" VaryByHeader="Accept-Language" %>
<%@ Import namespace="Subtext.Framework.Configuration"%>
<%@ Register TagPrefix="uc1" TagName="AggSyndication" Src="~/Skins/Aggregate/Simple/Controls/AggSyndication.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggBlogStats" Src="~/Skins/Aggregate/Simple/Controls/AggBlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggBloggers" Src="~/Skins/Aggregate/Simple/Controls/AggBloggers.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggRecentPosts" Src="~/Skins/Aggregate/Simple/Controls/AggRecentPosts.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggRecentImages" Src="~/Skins/Aggregate/Simple/Controls/AggRecentImages.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggPinnedPost" Src="~/Skins/Aggregate/Simple/Controls/AggPinnedPost.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title><asp:Literal id="title" runat="server" Text="<%$ AppSettings:AggregateTitle %>" /></title>
		<asp:Literal id="Style" runat="Server" />
		<script type="text/javascript" src="<%= VirtualPathUtility.ToAbsolute("~/Scripts/common.js") %>" ></script>
        <script type="text/javascript">
			var subtextBlogInfo = new blogInfo('<%= Blog.VirtualDirectoryRoot %>', '<%= Config.CurrentBlog.VirtualUrl %>');
		</script>
		<script type="text/javascript" src="<%= VirtualPathUtility.ToAbsolute("~/Scripts/prototype.js") %>" ></script>
		<script type="text/javascript" src="<%= VirtualPathUtility.ToAbsolute("~/Scripts/scriptaculous.js") %>" ></script>
		<script type="text/javascript" src="<%= VirtualPathUtility.ToAbsolute("~/Scripts/effects.js") %>" ></script>
        <script type="text/javascript" src="<%= VirtualPathUtility.ToAbsolute("~/Scripts/lightbox.js") %>" ></script>
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
					posts from every blog installed in this server. To modify this page, edit the default.aspx page 
					in your Subtext installation.
				</p>
				<p>
					To learn more about the application, check out <a href="http://subtextproject.com/" title="Subtext Project Website" rel="external">
					the Subtext Project Website</a>.
				</p>
				<p>
					Powered By:<br />
					<asp:HyperLink NavigateUrl="http://subtextproject.com/" ImageUrl="~/images/PoweredBySubtext85x33.png" ToolTip="Powered By Subtext"
						Runat="server" BorderWidth="0" id="HyperLink1" />
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
