<%@ Control Inherits="Subtext.Web.UI.Controls.AggregateUserControl" %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="AggSyndication" Src="Controls/AggSyndication.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggBlogStats" Src="Controls/AggBlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggBloggers" Src="Controls/AggBloggers.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggRecentImages" Src="Controls/AggRecentImages.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AggPinnedPost" Src="Controls/AggPinnedPost.ascx" %>

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
        <a href="http://subtextproject.com/"><img src="<%# ImageUrl("poweredbysubtext85x33.png") %>" alt="Powered by Subtext" /></a>
    </p>
    <uc1:AggSyndication ID="AggSyndication" runat="server" />				
    <uc1:AggBlogStats ID="AggBlogStats" runat="server" />				
    <uc1:AggBloggers ID="AggBloggers" runat="server" ShowGroups="true" />
</div>
<div id="main">
    <!-- Update EntryID in the next line to the ID of a post you want pinned here-->
    <uc1:AggPinnedPost ID="AggPinnedPost1" runat="server" ContentID="3" EntryTitle="Welcome All" />
    <st:ContentRegion id="MPMain" runat="server" />
</div>
<div id="extra">
    <uc1:AggRecentImages ID="AggRecentImages1" runat="server" Count="20" />
</div>
