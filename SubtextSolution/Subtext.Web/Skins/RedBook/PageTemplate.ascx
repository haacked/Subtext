<%@ Control %>
<%@ Register TagPrefix="uc1" TagName="RecentComments" Src="Controls/RecentComments.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="Controls/BlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Search" Src="Controls/SubtextSearch.ascx" %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>

		<div id="rondo"><p>&nbsp;</p></div>
		<div id="all">
			
			<uc1:Header id="header" runat="server" />
			<uc1:BlogStats id="BlogStats" runat="server" />
			<div id="navigation">
					<uc1:MyLinks id="MyLinks" runat="server" />
			</div>
			<div id="container">
				<div id="content">
						<DT:contentregion id="MPMain" runat="server" />
				</div>
				<div id="sidebar">
					<uc1:Search ID="search" runat="server" />
					<uc1:News id="News" runat="server" />
					<uc1:RecentComments id="RecentComments" runat="server" />
					<st:TagCloud runat="server" ItemCount="20" />
					<uc1:SingleColumn id="SingleColumn" runat="server" />
					<div>
						<h2>Hosted by</h2>
						<p class="subtextlogo">
							<a href="http://www.subtextproject.com/" title="Subtext Project Homepage"><img src="~/Images/PoweredBySubtext85x33.png" alt="Subtext Blog" runat="server" /></a>
						</p>
					</div>
				</div>
				<div class="clear"><span></span>&nbsp;</div>	
			</div>
			<div id="footer"><p>&nbsp;</p></div>
		</div>
		<div id="rondobottom"><p>&nbsp;</p></div>
		<uc1:Footer id="Footer" runat="server"></uc1:Footer>
