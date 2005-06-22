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
	<dt>How do I Contribute?</dt>
	<dd>
		The easiest way to help out is to <a href="~/About/ViewTheCode/" runat="server" ID="A1">
		download the source code</a> and start 
		<a href="http://haacked.com/archive/2005/06/18/5153.aspx">submitting patches</a> 
		via <a href="http://sourceforge.net/projects/subtext/">SourceForge</a>. 
		<p>
		We especially can use help with documentation.  In fact, this very site you are 
		reading is in the CVS repository.
		</p>
		<p>
		If we like what you&#8217;re contributions, we&#8217;ll give you write access.
		</p>
	</dd>
	</dl>
</MP:MasterPage>
