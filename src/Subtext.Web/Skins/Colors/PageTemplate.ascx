<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="Controls/BlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryDisplay" Src="Controls/CategoryDisplay.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ArchiveLinks" Src="Controls/ArchiveLinks.ascx" %>

<div id="main">
	<uc1:Header id="Header1" runat="server"></uc1:Header>	
	
	<div id="content">
		<div id="sidebar">
			<DT:contentregion id="MPLeftColumn" runat="server">
				<uc1:MyLinks id="MyLinks1" runat="server"></uc1:MyLinks>
				<uc1:News id="News1" runat="server"></uc1:News>
				<st:TagCloud ID="TagCloud" runat="server" ItemCount="20" />
				<uc1:ArchiveLinks id="ArchiveLinks1" runat="server"></uc1:ArchiveLinks>
				<uc1:CategoryDisplay id="CategoryDisplay" runat="server"></uc1:CategoryDisplay>
			</DT:contentregion>
		</div>
		<div id="blogPosts">
			<DT:contentregion id="MPMain" runat="server"></DT:contentregion>
		</div>
	</div>
	<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
</div>