<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Comments" Src="Controls/RecentComments.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Articles" Src="Controls/ArticleCategories.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TagCloud" Src="Controls/TagCloud.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Search" Src="Controls/SubtextSearch.ascx" %>

		<div class="pagelayout">
		
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			
			<div id="menu">
				<DT:contentregion id="MPLeftColumn" runat="server">
					<uc1:Search ID="Search" runat="server" />
					<uc1:MyLinks id="MyLinks1" runat="server"></uc1:MyLinks>
					<uc1:News id="News1" runat="server"></uc1:News>	
					<uc1:TagCloud id="TagCloud" runat="server" ItemCount="20" />
					<uc1:Comments id="Comments" runat="server"></uc1:Comments>		
					<uc1:Articles id="Articles" runat="server"></uc1:Articles>										
				</DT:contentregion>
				<div class="spacer">&nbsp;</div>
			</div>
			
			<div id="main">
				<DT:contentregion id="MPMain" runat="server"></DT:contentregion>
				<div class="spacer">&nbsp;</div>
			</div>
			
			<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
		
		</div>