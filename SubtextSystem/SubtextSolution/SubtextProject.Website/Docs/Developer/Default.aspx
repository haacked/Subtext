<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs - FAQ</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>Developer Documentation</h2>
	
	<p>
	This section contains developer specific documentation for 
	those who have CVS write access, or those who want to submit 
	patches.  We welcome any and all contributions.
	</p>
	
	<h3><a href="~/Docs/Developer/Changes/" title="Ch-Ch-Ch-Changes From .TEXT 0.95" runat="server">Ch-Ch-Ch-Changes From .TEXT 0.95</a></h3>
	<p>
	This is a (probably incomplete) list of changes in the codebase 
	as compared to .TEXT 0.95.  This is useful for developers familiar 
	with the .TEXT codebase to get up to speed with how Subtext differs.
	</p>
	
	
</MP:MasterPage>