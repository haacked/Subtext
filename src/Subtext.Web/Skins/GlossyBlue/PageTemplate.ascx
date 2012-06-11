<%@ Control %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="Syn" Src="Controls/Syndications.ascx" %>

<div id="page">
    <uc1:Header ID="header" runat="server"></uc1:Header>
    <div id="content">
        <DT:ContentRegion ID="MPMain" runat="server" />
        <div class="navigation"></div>
    </div>
    <div id="sidebar">
        <ul>
            <li><uc1:News ID="News" runat="server" /></li>
            <uc1:SingleColumn ID="SingleColumn" runat="server" />
        </ul>
    </div>
    <uc1:Footer ID="Footer" runat="server"></uc1:Footer>
</div>
<div id="credits">
    <div class="alignleft"><a href="http://www.ndesign-studio.com/resources/wp-themes/">WP Theme</a> &amp;<a href="http://www.ndesign-studio.com/stock-icons/">Icons</a> by <a href="http://www.ndesign-studio.com">N.Design Studio</a> adapted by <a href="http://timheuer.com">timheuer</a></div>
    <div class="alignright"><uc1:Syn id="links" runat="server" /><span class="loginout"><a href="<%# Url.AdminUrl(string.Empty) %>" title="Login">Login</a></span></div>
</div>
