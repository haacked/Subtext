<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="AggSyndication" Src="Controls/AggSyndication.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggBlogStats" Src="Controls/AggBlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggBloggers" Src="Controls/AggBloggers.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggRecentPosts" Src="Controls/AggRecentPosts.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggRecentImages" Src="Controls/AggRecentImages.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggPinnedPost" Src="Controls/AggPinnedPost.ascx" %>

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
		<asp:HyperLink NavigateUrl="http://subtextproject.com/" ImageUrl="~/images/PoweredBySubtext85x33.png" ToolTip="Powered By Subtext" Runat="server" BorderWidth="0" id="HyperLink1" />                    
	</p>
    <uc1:AggSyndication ID="AggSyndication1" runat="server" />				
    <uc1:AggBlogStats ID="AggBlogStats1" runat="server" />				
	<uc1:AggBloggers ID="AggBloggers1" runat="server" ShowGroups="true" />
</div>
<div id="main">
    <!-- Update EntryID in the next line to the ID of a post you want pinned here-->
    <uc1:AggPinnedPost ID="AggPinnedPost1" runat="server" ContentID="0" EntryTitle="Welcome All" />
	<uc1:AggRecentPosts ID="AggRecentPosts1" runat="server" Count="20" />
</div>
<div id="extra">
    <uc1:AggRecentImages ID="AggRecentImages1" runat="server" Count="20" />
</div>
<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="AggSyndication" Src="Controls/AggSyndication.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggBlogStats" Src="Controls/AggBlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggBloggers" Src="Controls/AggBloggers.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggRecentPosts" Src="Controls/AggRecentPosts.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggRecentImages" Src="Controls/AggRecentImages.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggPinnedPost" Src="Controls/AggPinnedPost.ascx" %>

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
		<asp:HyperLink NavigateUrl="http://subtextproject.com/" ImageUrl="~/images/PoweredBySubtext85x33.png" ToolTip="Powered By Subtext" Runat="server" BorderWidth="0" id="HyperLink1" />                    
	</p>
    <uc1:AggSyndication ID="AggSyndication1" runat="server" />				
    <uc1:AggBlogStats ID="AggBlogStats1" runat="server" />				
	<uc1:AggBloggers ID="AggBloggers1" runat="server" ShowGroups="true" />
</div>
<div id="main">
    <!-- Update EntryID in the next line to the ID of a post you want pinned here-->
    <uc1:AggPinnedPost ID="AggPinnedPost1" runat="server" ContentID="0" EntryTitle="Welcome All" />
	<uc1:AggRecentPosts ID="AggRecentPosts1" runat="server" Count="20" />
</div>
<div id="extra">
    <uc1:AggRecentImages ID="AggRecentImages1" runat="server" Count="20" />
</div>
