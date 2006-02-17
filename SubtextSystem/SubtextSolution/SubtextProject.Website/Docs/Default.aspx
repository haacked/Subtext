<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>Documentation</h2>
	<p>We&#8217;re just getting started on Subtext, but we&#8217;re committed 
	to having the best and clearest documentation possible.
	</p>
	<p>
	If you would like to help, please contribute.  We could really use the help.
	</p>
</MP:MasterPage>
