<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagName="Header" TagPrefix="origami" Src="Controls/Header.ascx" %>
<%@ Register TagName="Footer" TagPrefix="origami" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="origami" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<script type="text/javascript">setUserStyles();</script>
  <!--[if IE]>
  <link href="skins/origami/Styles/core-IE.css?" media="screen" rel="Stylesheet" type="text/css" />
  <![endif]-->
  
  <!--[if IE 5]>
  <link href="skins/origami/Styles/ie5_5.css?" media="screen" rel="Stylesheet" type="text/css" />
  <![endif]-->
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
							<uc1:News ID="news" runat="server" />
							<ul>
                                <li><a href="~/contact.aspx" runat="server">Contact</a> </li>
                            </ul>
                        </div>
                        <origami:SingleColumn id="column" runat="server" />
                        <div class="sidebar-node">
                            <h3 id="syndicate-heading">Syndicate</h3>
                            <ul id="syndicate-list">
                                <li class="RSS"><a title="RSS 2.0 feed" href="#" runat="server">RSS Feed</a></li>
                                <li class="RSS"><a title="Atom feed" href="#" runat="server">Atom Feed</a></li>
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