<%@ Control %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="st" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="st" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="st" TagName="BlogStats" Src="Controls/BlogStats.ascx" %>
<%@ Register TagPrefix="st" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="st" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="st" TagName="RecentComments" Src="Controls/RecentComments.ascx" %>
<%@ Register TagPrefix="st" TagName="RecentPosts" Src="Controls/RecentPosts.ascx" %>
<%@ Register TagPrefix="st" TagName="Search" Src="Controls/SubtextSearch.ascx" %>
<%@ Register TagPrefix="st" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="st" TagName="TagCloud" Src="Controls/TagCloud.ascx" %>

<div id="main">
    <st:Header id="Header" runat="server" />
    
	<div id="content">
		<st:contentregion id="MPMain" runat="server" />       
	</div>
	<div id="sidebar">
	    <st:Search ID="search" runat="server" />
		<div id="nav">
			<st:MyLinks id="MyLinks" runat="server" />
		</div>
		<st:TagCloud ID="tagCloud" runat="server" ItemCount="20" />
		<st:SingleColumn ID="singleColumn" runat="server" />
	</div>
</div>
<div id="bottom">
	<div id="bottom-info">
		<div id="bottom-recent-posts">
			<h2>Recent Posts</h2>
			<st:RecentPosts id="recentPosts" runat="server" />
		</div>
		<div id="bottom-navigation">
			<h2>Recent Comments</h2>
			<st:RecentComments id="recentComments" runat="server" />
		</div>
		<div id="bottom-about">
			<st:News ID="news" runat="server" />
		</div>
	</div>
</div>	
<div id="footer">
	<st:Footer id="footer1" runat="server"></st:Footer>
</div>
