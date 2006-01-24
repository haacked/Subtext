<%@ Page language="c#" Codebehind="Step01_GatherInstallData.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.Step01_GatherInstallData"%>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Install/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Installation: Step 1 - Gather Installation Data 
</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Step 1 - Data 
Installation </MP:ContentRegion>
	<ol>
		<p>
			<STRONG>Gather Installation Information</STRONG>
		<p>
		Install the database
		<p>
		Configure the Host Admin
		<p>
			Create a Blog
		</p>
	</ol>
	<p class="error"><asp:Literal id="ltlErrorMessage" Runat="server"></asp:Literal></p>
	<p>Welcome to the Subtext Installation Wizard!</p>
	<p>In order to install the Subtext database*, we need to gather a bit of 
		information from you.
	</p>
	<p>Please fill in the following fields.</p>
	<p>
		<asp:Panel id="pnlInstallationInformation" runat="server"></asp:Panel>
	</p>
	<p>
		<asp:Button id="btnSave" runat="server" text="Go to Step 2"></asp:Button></p>
	<P class="footnote">* Please note that the current version of Subtext only supports 
		Microsoft SQL Server 2000 and above. Future versions of Subtext may add support 
		for other databases and file system storage.
	</p>
</MP:MasterPage>
