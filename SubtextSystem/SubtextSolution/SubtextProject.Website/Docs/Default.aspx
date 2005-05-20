<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>Documentation</h2>
	<p>We&#8217;re just getting started on Subtext, but we're committed 
	to having the best and clearest documentation possible.  To that 
	end We&#8217;ve created a <a href="http://wiki.subtextproject.com/">wiki</a> that we&#8217;ll try to 
	keep updated.  Please be patient as there isn&#8217;t much there yet.
	</p>
</MP:MasterPage>
