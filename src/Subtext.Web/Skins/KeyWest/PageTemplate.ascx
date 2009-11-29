<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Calendar" Src="Controls/SubTextBlogCalendar.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Search" Src="Controls/SubtextSearch.ascx" %>
<div id="main">
	<div id="contentBodyLeft">
		<div id="contentBodyRight">
			<div id="contentBodyCenter">
				<div id="header">
					<uc1:header id="Header1" runat="server" />
				</div>
				
				<div id="content">
					<div id="entries">
						<dt:contentregion id="MPMain" runat="server" />
					</div>
					<div id="rightColumn">
						<div id="rightColumnHeader">
						</div>
						<uc1:News id="news" runat="server" />
<uc1:Search ID="search" runat="server" />
						<uc1:MyLinks id="links" runat="server" />
						<uc1:SingleColumn id="column" runat="server" />
						<div id="rightColumnFooter"></div>
					</div>
				</div>
				<div id="Footer">
					<uc1:footer id="Footer1" runat="server" />
				</div>
				<div class="clear">&nbsp;</div>
			</div>
		</div>
	</div>
</div>
