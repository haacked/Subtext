<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>	
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="~/Skins/_System/Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="Controls/BlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RecentComments" Src="Controls/RecentComments.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RecentPosts" Src="Controls/RecentPosts.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TagCloud" Src="Controls/TagCloud.ascx" %>


<div id="frame">
<div id="container">

<div id="header">
<uc1:header id="Header1" runat="server" />
    <div id="header-right">
        <div id="navigation">
            <uc1:mylinks id="MyLinks1" runat="server" />
        </div>
    </div>
    <div class="clearer">&nbsp;</div>
</div>
    
    <div id="content">
    <div id="main-section">
        <dt:contentregion id="MPMain" runat="server" />
        <div class="clearer">&nbsp;</div>
    </div>
    
    
    <div id="sidebarholder">

    <uc1:news id="News1" runat="server" />

    
    <div class="sidebar">
        <div class="side-bar-top"></div>
            <div class="side-bar-middle">
                <uc1:RecentComments id="RecentComments1" runat="server" />
            </div>
        <div class="side-bar-bottom"></div>
    </div>
    
    <uc1:RecentPosts id="RecentPosts1" runat="server" />
    
    <div class="sidebar">
        <div class="side-bar-top"></div>
            <div class="side-bar-middle">
                <uc1:singlecolumn id="SingleColumn1" runat="server" />
            </div>
        <div class="side-bar-bottom"></div>
    </div>
	

    <uc1:TagCloud ID="tagCloud" runat="server" ItemCount="20" />

    
    <div class="clearer">&nbsp;</div>
    </div>
    
    <div class="clearer">&nbsp;</div>
    </div>
	
	
	<div id="footer">
		<uc1:footer id="Footer1" runat="server" />
        <div id="n3o-link">
        Eco Theme by n3o <a href="http://www.n3o.co.uk">Web Designers</a>
        </div>
	</div>
</div>
</div>
