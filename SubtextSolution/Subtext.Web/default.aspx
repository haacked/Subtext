<%@ Page CodeBehind="Default.aspx.cs" EnableViewState="false" Language="C#" EnableTheming="false"  AutoEventWireup="false" Inherits="Subtext.Web._default" %>
<%@ OutputCache Duration="120" VaryByParam="GroupID" VaryByHeader="Accept-Language" %>

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
				<p>
					Please contact me (Phil Haack) at <a href="http://haacked.com/contact.aspx" title="Contact Page" rel="external">
						here</a> with any errors, problems, and/or questions.
				</p>
				<p>
					To learn more about the application, check out <a href="http://subtextproject.com/" title="Haacked Blog" rel="external">
					the Subtext Project Website</a>.
				</p>
				<p>
					Powered By:<br />
					<asp:HyperLink NavigateUrl="http://subtextproject.com/" ImageUrl="~/images/PoweredBySubtext85x33.png" ToolTip="Powered By Subtext"
						Runat="server" BorderWidth="0" id="HyperLink1" />
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
							<asp:HyperLink Runat = "server" NavigateUrl = '<%# GetFullUrl(DataBinder.Eval(Container.DataItem,"host").ToString(),DataBinder.Eval(Container.DataItem, "Application").ToString()) %>' Text = '<%# DataBinder.Eval(Container.DataItem,"Author") %>' title = '<%# DataBinder.Eval(Container.DataItem,"Title") %>' ID="Hyperlink1" NAME="Hyperlink1"/>
							<br />
							<small>(
								<asp:Literal runat = "server" Text = '<%# DataBinder.Eval(Container.DataItem,"PostCount") %>' ID="Label2"/>,
								<asp:Literal runat = "server" Text = '<%# (DateTime.Parse(DataBinder.Eval(Container.DataItem,"LastUpdated").ToString())).ToShortDateString() + " " + (DateTime.Parse(DataBinder.Eval(Container.DataItem,"LastUpdated").ToString())).ToShortTimeString() %>' ID="Label1"/>)</small>
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
								<asp:HyperLink Runat = "server" NavigateUrl = '<%# GetEntryUrl(DataBinder.Eval(Container.DataItem,"host").ToString(),DataBinder.Eval(Container.DataItem,"Application").ToString(), DataBinder.Eval(Container.DataItem,"EntryName").ToString(), (DateTime)DataBinder.Eval(Container.DataItem,"DateAdded")) %>' Text = '<%# DataBinder.Eval(Container.DataItem,"Title") %>' ID="Hyperlink2"/></h3>
							<asp:Literal runat = "server" Text = '<%# DataBinder.Eval(Container.DataItem,"Description") %>' ID="Label4"/>
							<p class="postfoot">
								posted @
								<asp:Literal runat = "server" Text = '<%# (DateTime.Parse(DataBinder.Eval(Container.DataItem,"DateAdded").ToString())).ToShortDateString() + " " + (DateTime.Parse(DataBinder.Eval(Container.DataItem,"DateAdded").ToString())).ToShortTimeString() %>' ID="Label5" />
								by
								<asp:HyperLink Runat = "server" CssClass = "clsSubtext" NavigateUrl = '<%# GetFullUrl(DataBinder.Eval(Container.DataItem,"host").ToString(),DataBinder.Eval(Container.DataItem,"Application").ToString())  %>' Text = '<%# DataBinder.Eval(Container.DataItem,"Author") %>' ID="Hyperlink3"/>
							</p>
						</div>
					</ItemTemplate>
				</asp:repeater>
			</div>
		</form>
	</body>
</html>
