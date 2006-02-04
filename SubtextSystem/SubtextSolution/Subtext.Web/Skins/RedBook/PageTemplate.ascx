<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="Controls/BlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="RecentComments" Src="Controls/RecentComments.ascx" %>
<div id="rondo"><p>&nbsp;</p></div>
		<div id="all">
			<uc1:Header id="Header" runat="server"></uc1:Header>
			<uc1:BlogStats id="BlogStats" runat="server"></uc1:BlogStats>
			<div id="navigation">
					<uc1:MyLinks id="MyLinks" runat="server"></uc1:MyLinks>
			</div>
			<div id="container">
				<div id="content">
						<DT:contentregion id="MPMain" runat="server" />
				</div>
				<div id="rightbar">
					<uc1:News id="News" runat="server"></uc1:News>
					<uc1:RecentComments id="RecentComments" runat="server"></uc1:RecentComments>
					<uc1:SingleColumn id="SingleColumn" runat="server" />
					<div class="leftbox">
						<h2>Hosted by</h2>
						<p style="text-align:center;padding-top: 8px;"><a href="http://www.uitu.ch/" title="uitu Solutions"><img src="/Skins/rosso/images/uitu.jpg" alt="uitu Solutions" /></a></p>
					</div>
					<div class="leftbox">
						<p style="text-align:center;padding-top: 8px;"><a href="http://www.subtextproject.com/" title="Subtext Project Homepage"><img src="/Images/PoweredBySubtext85x33.png" alt="Subtext Blog" /></a></p>
					</div>
				</div>
				<div class="clear"><span>&nbsp;</span></div>	
			</div>
			<div id="footer"><p>&nbsp;</p></div>
		</div>
		<div id="rondobottom"><p>&nbsp;</p></div>
		<uc1:Footer id="Footer" runat="server"></uc1:Footer>
