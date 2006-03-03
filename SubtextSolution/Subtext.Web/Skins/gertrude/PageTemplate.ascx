<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="Controls/BlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
			<uc1:Header id="Header1" runat="server"></uc1:Header>
			
			<div id="rightmenu">
				<DT:contentregion id="MPRightColumn" runat="server">
					<uc1:MyLinks id="MyLinks1" runat="server"></uc1:MyLinks>
					<uc1:BlogStats id="BlogStats1" runat="server"></uc1:BlogStats>
					<uc1:News id="News1" runat="server"></uc1:News>
					<uc1:SingleColumn id="SingleColumn1" runat="server"></uc1:SingleColumn>
				</DT:contentregion>
				
			</div>
			
			<div id="main">
				<DT:contentregion id="MPMain" runat="server"></DT:contentregion>
			</div>
			
			<uc1:Footer id="Footer1" runat="server"></uc1:Footer>
	
