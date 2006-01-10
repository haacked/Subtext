<%@ OutputCache Duration="120" VaryByParam="GroupID" %>
<%@ Page CodeBehind="default.aspx.cs" EnableViewState="false" Language="c#" AutoEventWireup="false" Inherits="Subtext.Web._default" %>

<html>
	<head>
		<title><asp:Literal id = "TitleTag" runat = "Server" /></title>
		<asp:Literal id="Style" runat="Server" />
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<div id="header">
				<h1><asp:HyperLink ID = "TitleLink" Runat="server" /></h1>
			</div>
			<div id="authors">
				<h2>Welcome</h2>
				<p>Please contact me (Scott Watermasysk) at <a href="http://scottwater.com/blog/contact.aspx">
						here</a> with any errors, problems, and/or questions.</p>
				<p>To learn more about the application, check out <a href="http://scottwater.com/blog">my 
						blog</a>.</p>
				<p>
					Powered By:<br>
					<asp:HyperLink NavigateUrl="http://scottwater.com/blog" ImageUrl="~/images/PoweredBySubtext85x33.png"
						Runat="server" BorderWidth="0" id="HyperLink1" />
				</p>
				<p>
					We are working on some new tools to help handle the increased number of bloggers. Please use the following links in the mean time.
					<br >
					<asp:HyperLink ID="Hyperlink6" Text="Posts by only Microsoft Bloggers" runat="server" NavigateUrl = "~/?GroupID=2" />
					<br >
					<asp:HyperLink ID="Hyperlink7" Text="Posts by Non-Microsoft Bloggers" runat="server" NavigateUrl = "~/?GroupID=4" />
				
				</p>
				<h2>Syndication</h2>
				<ul>
					<li><asp:HyperLink ID="OpmlLink" Text="OPML (list of bloggers)" runat="server" NavigateUrl = "~/Opml.aspx" />
					<li><asp:HyperLink ID="RssLink" Text="RSS (list of recent posts)" runat="server" NavigateUrl = "~/MainFeed.aspx" />
					<li><asp:HyperLink ID="Hyperlink4" Text="RSS (Microsoft Bloggers)" runat="server" NavigateUrl = "~/MainFeed.aspx?GroupID=2" />
					<li><asp:HyperLink ID="Hyperlink5" Text="RSS (Non-Microsoft Bloggers)" runat="server" NavigateUrl = "~/MainFeed.aspx?GroupID=4" /></li>

				</ul>
				<h2>Blog Stats</h2>
				<ul>
					<li>
						Blogs -
						<asp:Literal ID="BlogCount" Runat="server" />
					<li>
						Posts -
						<asp:Literal ID="PostCount" Runat="server" />
					<li>
						Articles -
						<asp:Literal ID="StoryCount" Runat="server" />
					<li>
						Comments -
						<asp:Literal ID="CommentCount" Runat="server" />
					<li>
						Trackbacks -
						<asp:Literal ID="PingtrackCount" Runat="server" /></li>
				</ul>
				<h2>Bloggers (posts, last update)</h2>
				<asp:repeater id="Bloggers" runat="server">
					<HeaderTemplate>
						<ul>
					</HeaderTemplate>
					<ItemTemplate>
						<li>
							<asp:HyperLink Runat = "server" NavigateUrl = '<%# GetFullUrl(DataBinder.Eval(((RepeaterItem)Container).DataItem,"host").ToString(),DataBinder.Eval(((RepeaterItem)Container).DataItem,"Application").ToString()) %>' Text = '<%# DataBinder.Eval(((RepeaterItem)Container).DataItem,"Author") %>' title = '<%# DataBinder.Eval(((RepeaterItem)Container).DataItem,"Title") %>' ID="Hyperlink1" NAME="Hyperlink1"/>
							<br>
							<small>(
								<asp:Literal runat = "server" Text = '<%# DataBinder.Eval(((RepeaterItem)Container).DataItem,"PostCount") %>' ID="Label2"/>,
								<asp:Literal runat = "server" Text = '<%# (DateTime.Parse(DataBinder.Eval(((RepeaterItem)Container).DataItem,"LastUpdated").ToString())).ToShortDateString() + " " + (DateTime.Parse(DataBinder.Eval(((RepeaterItem)Container).DataItem,"LastUpdated").ToString())).ToShortTimeString() %>' ID="Label1"/>)</small>
						</li>
					</ItemTemplate>
					<FooterTemplate>
						</ul>
					</FooterTemplate>
				</asp:repeater>
			</div>
			<div id="main">
				<h2>Latest Posts</h2>
				<asp:repeater id="RecentPosts" runat="server">
					<ItemTemplate>
						<div class="post">
							<h3>
								<asp:HyperLink Runat = "server" NavigateUrl = '<%# GetEntryUrl(DataBinder.Eval(((RepeaterItem)Container).DataItem,"host").ToString(),DataBinder.Eval(((RepeaterItem)Container).DataItem,"Application").ToString(), DataBinder.Eval(((RepeaterItem)Container).DataItem,"EntryName").ToString(), (DateTime)DataBinder.Eval(((RepeaterItem)Container).DataItem,"DateAdded")) %>' Text = '<%# DataBinder.Eval(((RepeaterItem)Container).DataItem,"Title") %>' ID="Hyperlink2"/></h3>
							<asp:Literal runat = "server" Text = '<%# DataBinder.Eval(((RepeaterItem)Container).DataItem,"Description") %>' ID="Label4"/>
							<p class="postfoot">
								posted @
								<asp:Literal runat = "server" Text = '<%# (DateTime.Parse(DataBinder.Eval(((RepeaterItem)Container).DataItem,"DateAdded").ToString())).ToShortDateString() + " " + (DateTime.Parse(DataBinder.Eval(((RepeaterItem)Container).DataItem,"DateAdded").ToString())).ToShortTimeString() %>' ID="Label5"/>
								by
								<asp:HyperLink Runat = "server" CssClass = "clsSubtext" NavigateUrl = '<%# GetFullUrl(DataBinder.Eval(((RepeaterItem)Container).DataItem,"host").ToString(),DataBinder.Eval(((RepeaterItem)Container).DataItem,"Application").ToString())  %>' Text = '<%# DataBinder.Eval(((RepeaterItem)Container).DataItem,"Author") %>' ID="Hyperlink3"/>
							</p>
						</div>
					</ItemTemplate>
				</asp:repeater>
			</div>
		</form>
	</body>
</html>
