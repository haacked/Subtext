<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="Controls/BlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TagCloud" Src="Controls/TagCloud.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Search" Src="Controls/SubtextSearch.ascx" %>
<table width="100%" class="Framework" cellspacing="0" cellpadding="0">
	<tr>
		<td colspan="2">
			<uc1:Header id="Header1" runat="server" />
		</td>
	</tr>
	<tr>
		<td rowspan="2" class="LeftCell">
			<div id="leftmenu">
				<DT:ContentRegion id="MPLeftColumn" runat="server">
					<uc1:MyLinks id="MyLinks1" runat="server" />
					<uc1:Search ID="search" runat="server" />
					<uc1:News id="News1" runat="server" />
					<uc1:TagCloud id="tagCloud" runat="server" ItemCount="20" />
					<uc1:SingleColumn id="SingleColumn1" runat="server" />
				</DT:ContentRegion>
				
			</div>
		</td>
		<td class="MainCell">
			<div id="main">
				<DT:ContentRegion id="MPMain" runat="server" />
			</div>
		</td>
	</tr>
	<tr>
		<td class="FooterCell">
			<uc1:Footer id="Footer1" runat="server" />
		</td>
	</tr>
</table>
