<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagName="Header" TagPrefix="origami" Src="Controls/Header.ascx" %>
<%@ Register TagName="Footer" TagPrefix="origami" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="origami" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SyndicatonLinks" Src="Controls/SyndicationLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TagCloud" Src="Controls/TagCloud.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Search" Src="Controls/SubtextSearch.ascx" %>

<div class="clearfix" id="container">
    <div id="sizer">
        <div id="expander">
            <origami:Header ID="Header" runat="server" />
            <div class="clearfix" id="content">
                <div id="main-wrapper">
                    <div id="main">
                        <DT:contentregion id="MPMain" runat="server" />
                    </div>
                </div>
                <div id="sidebar-wrapper">
                    <div id="sidebar">
                        <div class="sidebar-node">
							<uc1:Search ID="search" runat="server" />
							<uc1:News ID="news" runat="server" />
							<ul>
                                <uc1:MyLinks ID="myLinks1" runat="Server" />
                            </ul>
                        </div>
                        <div class="sidebar-node">
							<uc1:TagCloud ID="tagCloud" runat="server" ItemCount="20" />
                        </div>
                        <origami:SingleColumn id="column" runat="server" />
                        <div class="sidebar-node">
                            <h3 id="syndicate-heading">Syndicate</h3>
                            <uc1:SyndicatonLinks ID="SyndicationLinks1" runat="Server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<origami:Footer id="Footer" runat="server" />
