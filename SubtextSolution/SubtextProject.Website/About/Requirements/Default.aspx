<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="AboutLinks" Src="~/About/AboutLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - About - Requirements</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:AboutLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>Requirements</h2>
	<p>
	To run Subtext, you (or your host) will need the following.
	</p>
	<ul>
		<li>ASP.NET 1.1</li>
		<li>SQL Server 2000</li>
	</ul>
	<p>
	And of course it should be running IIS 4.0 or later.
	</p>
</MP:MasterPage>
