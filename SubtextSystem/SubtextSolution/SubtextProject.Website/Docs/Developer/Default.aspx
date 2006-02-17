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
		This is a copy of <a href="http://haacked.com/archive/2005/05/12/3178.aspx" title="Quickstart Guide to CVS and SourceForge" rel="external">the post</a> on <a href="http://haacked.com/" title="Phil Haack's Blog" rel="external">http://haacked.com/</a> 
		for developers who have WRITE access to the CVS repository.
	</p>
	
	<h3><a href="~/Docs/Developer/CodingGuidelines/" title="Coding Guidelines" runat="server">Coding Guidelines</a></h3>
	<p>
		A style guide for contributing code to the Subtext project.
	</p>
	
	<h3><a href="~/Docs/Developer/HowAnUrlIsMappedToABlog/" title="How An Url Is Mapped To A Blog" runat="server" ID="A2">How An Url Is Mapped To A Blog</a></h3>
	<p>
		Describes how urls are mapped to blogs within Subtext.  
		This is a necessary topic for developers since Subtext 
		installations supports multiple blogs.
	</p>
	
	<h3><a href="Documentation.chm" title="Code Documentation">Code Documentation</a></h3>
	<p>
		This is an <a href="http://ndoc.sourceforge.net/" title="NDoc Code Documentation" rel="external">NDoc</a> generated CHM 
		(Compiled Help Manual) file that contains documentation for the source code. 
		It&#8217;s sparsely populated, but we&#8217;re working to steadily change that.
	</p>
	
	
	
	
</MP:MasterPage>