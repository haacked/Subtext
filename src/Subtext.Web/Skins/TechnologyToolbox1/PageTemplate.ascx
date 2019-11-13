<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RecentPosts" Src="Controls/RecentPosts.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TagCloud" Src="Controls/TagCloud.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NextPost" Src="Controls/PreviousNext.ascx" %>
<%@ Register TagPrefix="caelum" Namespace="TechnologyToolbox.Caelum.Website.Controls"
    Assembly="TechnologyToolbox.Caelum.Website, Version=2.0.0.0, Culture=neutral, PublicKeyToken=f55b5d7768fcda39" %>
<caelum:JavaScriptReference runat="server"
  SourceFile="/Scripts/Caelum-1.0.1.min.js"
  DebugSourceFile="/Scripts/Caelum-1.0.1.js" />

<div id="wrapper">
    <div id="branding">
        <h1>
            <a href="/">
                <img alt="Technology Toolbox" src="/Images/TechnologyToolbox-Logo.png" /></a></h1>
        <blockquote>
            <p>
                Your technology Sherpa for the Microsoft platform</p>
            <p>
                <cite>Jeremy Jameson - Founder and Principal</cite></p>
        </blockquote>
    </div>
    <div id="siteSearch">
        <h2>
            Search</h2>
        <input type="text" id="searchKeywords" />
        <a href="javascript:submitSearch($('#searchKeywords'))">
            <img alt="Search" src="/Images/icon-search-22x22.png" /></a>
        <script type="text/javascript">
            $(document).ready(function () { configureSearchBox($('#searchKeywords')); });
        </script>
    </div>
    <div id="navMain">
        <h2>
            Site features</h2>
        <ul id="navFeatures">
            <li><a href="/Services">Services</a></li>
            <li><a href="/Company">Company</a></li>
            <li><a href="/Contact">Contact</a></li>
            <li><a href="/blog/jjameson">Blog</a></li>
        </ul>
    </div>
    <div id="pageContentPlaceholder" class="clear-fix">
        <div id="blogHome">
            <div class="container_12">
                <div id="pageHeader">
                    <h1 class="blog-title">
                        Random Musings of Jeremy Jameson</h1>
                    <asp:SiteMapPath runat="server" ID="BreadcrumbPath" CssClass="breadcrumb">
                        <CurrentNodeStyle CssClass="current" />
                        <RootNodeStyle CssClass="root" />
                        <PathSeparatorStyle CssClass="separator" />
                    </asp:SiteMapPath>
                </div>
                <div id="contentMain" class="grid_8">
                    <DT:contentregion id="MPMain" runat="server" />
                </div> <!-- #contentMain -->
                <div id="contentSub" class="grid_4">
                    <p class="rss-feed">
                        <a href="http://feeds.feedburner.com/Random-Musings-of-Jeremy-Jameson">
                            <asp:Image runat="server" AlternateText="RSS icon" Height="30" Width="30" ImageUrl="~/Skins/TechnologyToolbox1/Images/icon-rss-30x30.png" />
                            Subscribe to this blog (RSS)</a>
                    </p>
                    <uc1:News runat="server" />
                    <uc1:RecentPosts runat="server" ID="RecentPosts" />
                    <uc1:TagCloud runat="server" ItemCount="20" />
                    <uc1:SingleColumn runat="server" />
                    <caelum:PostArchiveList runat="server" />
                </div>
            </div>
        </div>
    </div>
    <div id="siteInfo" class="clear-fix">
        <hr />
        <h2 class="link-to-top">
            <a href="#technology-toolbox-com" title="Back to top">Technology Toolbox</a></h2>
        <ul>
            <li>Copyright &copy; 2011 Technology Toolbox - All rights reserved.</li>
            <li><a href="/Terms-of-Use.aspx">Terms of Use</a></li>
            <li><a href="/Privacy-Policy.aspx">Privacy Policy</a></li>
        </ul>
    </div>
</div>
<caelum:AnalyticsScript runat="server" />
