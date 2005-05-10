<%@ Control %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ArchiveLinks" Src="Controls/ArchiveLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryDisplay" Src="Controls/CategoryDisplay.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="Controls/BlogStats.ascx" %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<div id="top">
	<uc1:Header id="Header1" runat="server"></uc1:Header>
</div>
<div id="leftmenu">
	<DT:contentregion id="MPLeftColumn" runat="server">
		<uc1:MyLinks id="MyLinks1" runat="server"></uc1:MyLinks>
		<uc1:News id="News1" runat="server"></uc1:News>
		<uc1:ArchiveLinks id="ArchiveLinks1" runat="server"></uc1:ArchiveLinks>
	</DT:contentregion>
</div>
<div id="rightmenu">
	<DT:contentregion id="MPRightColumn" runat="server">
		<uc1:BlogStats id="BlogStats1" runat="server"></uc1:BlogStats>
		<uc1:CategoryDisplay id="CategoryDisplay1" runat="server"></uc1:CategoryDisplay>
	</DT:contentregion>
</div>
<div id="main">
	<DT:contentregion id="MPMain" runat="server"></DT:contentregion>
</div>
<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
