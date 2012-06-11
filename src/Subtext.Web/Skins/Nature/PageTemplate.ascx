<%@ Control %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ArchiveLinks" Src="Controls/ArchiveLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryDisplay" Src="Controls/CategoryDisplay.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="Controls/BlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Search" Src="Controls/SubtextSearch.ascx" %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>

<div id="main">
	<div id="header">
		<uc1:Header id="Header1" runat="server"></uc1:Header>
	</div>
	
	<uc1:MyLinks id="MyLinks1" runat="server"></uc1:MyLinks>
	
	<div id="sidebar">
		<DT:ContentRegion id="MPRightColumn" runat="server">
			<uc1:News id="News" runat="server" />
			<uc1:Search ID="Search" runat="server" />
			<st:TagCloud ID="tagCloud" runat="server" ItemCount="20" />
			<uc1:CategoryDisplay id="CategoryDisplay" runat="server" />
			<uc1:ArchiveLinks id="ArchiveLinks" runat="server" />
			<uc1:BlogStats id="BlogStats" runat="server" />
		</DT:ContentRegion>
	</div>
	
	<div id="content">
		<DT:ContentRegion id="MPMain" runat="server"></DT:ContentRegion>
		<uc1:Footer id="Footer" runat="server"></uc1:Footer>
	</div>
</div>

<!--
Original site design by Bartosz (http://www.bartosz.co.nr/) site no longer functional.
Adapted by Subtext team.
//-->