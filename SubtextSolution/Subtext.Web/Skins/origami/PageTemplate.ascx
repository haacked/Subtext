<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagName="Header" TagPrefix="origami" Src="Controls/Header.ascx" %>
<%@ Register TagName="Footer" TagPrefix="origami" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="origami" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<script type="text/javascript">setUserStyles();</script>
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
                            <h3 class="static-heading">Me</h3>
                            All postings on this blog are provided "AS IS" with no warranties, and confer no rights. All entries in this blog are my opinion and don't necessarily reflect the opinion of my employer.
                            <ul>
                                <li><a href="~/contact.aspx">Contact</a> </li>
                            </ul>
                        </div>
                        <div class="sidebar-node">
                            <h3 class="static-heading">Some Stuff</h3>
                            <ul>
                                <li><strong>Microsoft SharePoint</strong>
                                    <ul>
                                        <li><a href="http://smilinggoat.net/stuff.aspx">feedreader</a></li>
                                    </ul>
                                </li>
                            </ul>
                        </div>
                        <origami:SingleColumn id="column" runat="server" />
                        
                        <div class="sidebar-node">
                            <h3 id="syndicate-heading">
                                Syndicate</h3>
                            <ul id="syndicate-list">
                                <li class="RSS"><a title="RSS 2.0 feed" href="~/rss.aspx" runat="server">RSS Feed</a></li>
                                <li class="RSS"><a title="Atom feed" href="~/atom.aspx" runat="server">Atom Feed</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<origami:Footer id="Footer" runat="server" />
<script type="text/javascript">
	init();
</script>