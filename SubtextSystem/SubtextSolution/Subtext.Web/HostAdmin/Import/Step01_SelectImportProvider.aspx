<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Step01_SelectImportProvider.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.Step01_SelectImportProvider" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/HostAdmin/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Import - Step 1 - Select an Import Provider</MP:ContentRegion>
	<MP:ContentRegion id="MPSectionTitle" runat="server">Select an Import Provider</MP:ContentRegion>
	<p>In this step, you’ll select an import provider used to import data from another 
		blog engine. If you don’t see your blog engine listed here, then either you 
		don’t have an import provider for that engine installed, or nobody has written 
		one for it yet. Do I smell opportunity?
	</p>
	<H2>Import Providers</H2>
	<p>
		<asp:RadioButtonList id="rdlImportProviders" runat="server"></asp:RadioButtonList>
		<asp:RequiredFieldValidator id="vldImportProviders" runat="server" 
			ControlToValidate="rdlImportProviders" 
			Display="Dynamic"
			ErrorMessage="Please Select an Import Provider"></asp:RequiredFieldValidator>
	</p>
	<p>
		<asp:Button id="btnNext" runat="server" text="Next - Gather Import Information"></asp:Button>
	</p>
</MP:MasterPage>