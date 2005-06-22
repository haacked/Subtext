<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs - Developer</MP:ContentRegion>
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
	
	<h3><a href="~/Docs/Developer/GuideToCVSAndSourceForge/" title="Quickstart Guide to CVS and SourceForge" runat="server" ID="A1">Quickstart Guide To Open Source Development With CVS and SourceForge</a></h3>
	<p>
		This is a copy of <a href="http://haacked.com/archive/2005/05/12/3178.aspx">the post</a> on <a href="http://haacked.com/">http://haacked.com/</a> 
		for developers who have WRITE access to the CVS repository.
	</p>
	
	<h3><a href="Documentation.chm" title="Code Documentation">Code Documentation</a></h3>
	<p>
		This is an <a href="http://ndoc.sourceforge.net/">NDoc</a> generated CHM 
		(Compiled Help Manual) file that contains documentation for the source code. 
		It&#8217;s sparsely populated, but we&#8217;re working to steadily change that.
	</p>
	
	
</MP:MasterPage>