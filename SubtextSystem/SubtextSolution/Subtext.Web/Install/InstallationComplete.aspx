<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="InstallationComplete.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.InstallationComplete" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Install/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Installation: Installation Complete</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Installation Is Complete</MP:ContentRegion>
	<p>Congratulations. This Subtext Installation is complete.</p>
	<p>
		Click <a id="lnkBlog" href="~/">here</a> to visit your blog.
	</p>
	<p>
		Click <a id="lnkHostAdmin" href="~/HostAdmin/" runat="server">here</a> to visit 
		the Host Admin tool.
	</p>
	<p>
		If you need to import data from another blogging engine, try 
		the <a href="~/HostAdmin/Import/ImportStart.aspx" runat="server" id="importWizardAnchor">Import Wizard</a>.
	</p>
</MP:MasterPage>
