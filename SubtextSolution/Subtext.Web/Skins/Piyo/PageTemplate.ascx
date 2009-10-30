<%@ Control %>
<!--
Skin inspired from Blojsom 2.0 Theme
Name:     Asual
Author:   Rostislav Hristov
URL:      www.asual.com
Date:     1 October 2004

and later adapted for the SubText blogging platform by Simone Chiaretta www.piyodesign.it
-->

<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Calendar" Src="Controls/SubTextBlogCalendar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TagCloud" Src="Controls/TagCloud.ascx" %>

<div id="main">
	<div id="header">
		<uc1:header id="Header1" runat="server" />		
	</div>
	<div id="contentHeadLeft"><div id="contentHeadRight"><div id="contentHeadCenter"></div></div></div>
		<div id="contentBodyLeft">
		<div id="contentBodyRight">
			<div id="contentBodyCenter">
				<div id="content">
				
					<div id="entries">
					<dt:contentregion id="MPMain" runat="server" />
					</div>
					<div id="column">
						
						<uc1:Calendar id="cal" runat="server" />
						<uc1:News id="news" runat="server" />
						<uc1:MyLinks id="links" runat="server" />
						<div class="links">
							<uc1:TagCloud id="tagCloud" runat="server" ItemCount="20" />
						</div>
						<uc1:SingleColumn id="column" runat="server" />
						<div id="subtext" class="links">
							<asp:PlaceHolder runat="server" ID="poweredBy">
							    <p><a href="http://subtextproject.com/" title="Powered By Subtext"><img src="<%= Url.ImageUrl("PoweredBySubtext85x33.png") %>" width="85" height="33" alt="Powered By Subtext" /></a></p>
							</asp:PlaceHolder>
						</div>
					</div>
				</div>
				<div class="clear">&nbsp;</div>
			</div>
		</div>
	</div>
	<div id="contentFootLeft"><div id="contentFootRight"><div id="contentFootCenter"></div></div></div>

	<div id="Footer">
		<uc1:footer id="Footer1" runat="server" />
	</div>
	
</div>
