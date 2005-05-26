<%@ Control %>
<%@ Register TagPrefix="DT" Namespace="Subtext.Web.UI.WebControls" Assembly="Subtext.Web" %>	
<%@ Register TagPrefix="uc1" TagName="Header" Src="Controls/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="Controls/Footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="News" Src="Controls/News.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SingleColumn" Src="Controls/SingleColumn.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BlogStats" Src="Controls/BlogStats.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MyLinks" Src="Controls/MyLinks.ascx" %>
	<div id="Header">
		<uc1:header id="Header1" runat="server" />
	</div>
	<div id="Content">
		<uc1:mylinks id="MyLinks1" runat="server" />
		<uc1:blogstats id="BlogStats1" runat="server" />
		<dt:contentregion id="MPMain" runat="server" />
		<uc1:news id="News1" runat="server" />
		<uc1:singlecolumn id="SingleColumn1" runat="server" />
	</div>
	<div id="Footer">
		<uc1:footer id="Footer1" runat="server" />
	</div>
	<script type="text/javascript">
	//<![CDATA[
			new DarkHorseLayoutEngine();
	//]]>
	</script>

