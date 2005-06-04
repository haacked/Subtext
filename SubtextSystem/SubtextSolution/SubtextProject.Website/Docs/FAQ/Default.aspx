<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs - FAQ</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>Frequently Asked Questions</h2>
	
	<p>
	Well, we are just starting out so there haven&#8217;t been too many questions. 
	But we&#8217;ll get there.
	</p>
	
	<h3>General</h3>
	<dl>
	<dt>What is Subtext?</dt>
	<dd>Subtext is a personal blog publishing engine that has evolved from .TEXT.</dd>
	</dl>
</MP:MasterPage>
