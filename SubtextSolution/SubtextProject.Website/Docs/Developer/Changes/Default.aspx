<%@ Page %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="MP" TagName="DocLinks" Src="~/Docs/DocLinks.ascx" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Docs - Developer</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server">
		<MP:DocLinks id="AboutLinks" runat="server" />
	</MP:ContentRegion>
	
	<h2>Changes From .TEXT 0.95</h2>
	[<a href="../../Developer/" title="Dev Docs">Back To Developer Docs</a>]
	<ul>
		<li>
		Providers implemented following the Provider Model Design Pattern and Specification as 
		written up in the following two articles: <a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnaspnet/html/asp02182004.asp" title="Provider Design Pattern Part 1" rel="external">Part 1</a> 
		and <a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnaspnet/html/asp04212004.asp" title="Provider Design Pattern Part 2" rel="external">Part 2</a>.
		</li>
		<li>
		DTOProvider is now the ObjectProvider.  Currently there&#8217;s still only one concrete 
		implementation, the SQLDataProvider.
		</li>
		<li>
		<strong>Subtext.Extensibility</strong> assembly added. This contains the ProviderBase class and 
		will contain the code for the plugin architecture.  Please note, this assembly 
		is in flux and the name and its purpose may change at any time.
		</li>
		<li>
		Many classes have been moved out of <strong>Subtext.Common</strong> and 
		put in <strong>Subtext.Framework</strong>.  Subtext.Common will be removed 
		at a later date in favor of more specific assemblies.
		</li>
		<li>
		<strong>Subtext.Web</strong> is a class library project, rather than a Web Project according to the 
		outline written up by Fritz Onion in his article 
		<a href="http://pluralsight.com/wiki/default.aspx/Fritz.AspNetWithoutWebProjects" title="How to implement ASP.NET 1.1 web sites as a class library" rel="external">ASP.NET Applications 
		Without Web Projects</a>.
		</li>
		<li>
		<strong>Subtext.Web.Controls</strong> contains custom ASP.NET controls that have no 
		dependencies on Subtext.  These are essentially controls that can be reused anywhere.
		</li>
		<li>
		<strong>Subtext.Installation</strong> houses the SQL installation scripts an 
		embedded resources.
		</li>
		<li>
		<strong>Subtext.Scripting</strong> contains the SqlScriptRunner class. This is useful 
		for running embedded sql scripts as if the user were in Query Analyzer. It includes 
		support for template variables.
		</li>
		<li>
		<strong>UnitTests.Subtext</strong> is where Subtext  unit tests are housed.  
		These unit tests are run via <a href="http://www.mertner.com/confluence/display/MbUnit/Home" title="MBUnit Home" rel="external">MBUnit</a> 
		and make use of the RollBack attribute so that writes to the database are 
		not persisted.
		</li>
	</ul>	
</MP:MasterPage>