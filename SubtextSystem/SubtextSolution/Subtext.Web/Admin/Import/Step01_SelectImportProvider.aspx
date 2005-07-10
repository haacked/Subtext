<%@ Page language="c#" Codebehind="Step01_SelectImportProvider.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Import.Step01_SelectImportProvider" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Admin/Import/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Import - Step 1 - Select an Import Provider</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Select an Import Provider</MP:ContentRegion>
	<p>
		In this step, you&#8217;ll select an import provider used to 
		import data from another blog engine.  If you don&#8217;t see 
		your blog engine listed here, then either you don&#8217;t have 
		an import provider for that engine installed, or nobody has 
		written one for it yet.  Do I smell opportunity?
	</p>
	<h2>Import Providers</h2>
	<p>
		<asp:RadioButtonList id="rdlImportProviders" runat="server"></asp:RadioButtonList>		
	</p>
	<p>
		<asp:Button id="btnNext" runat="server" text="Next - Gather Import Information"></asp:Button>
	</p>
	
</MP:MasterPage>
