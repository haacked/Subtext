<%@ Page language="c#" Codebehind="Step01_GatherInstallData.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.Step01_GatherInstallData" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Install/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Installation: Step 1 - Gather Installation Data 
</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Step 1 - Data 
Installation </MP:ContentRegion>
	<OL>
		<LI>
			<STRONG>Gather Installation Information</STRONG>
		<LI>
		Install the database
		<LI>
		Configure the Host Admin
		<LI>
			Create a Blog
		</LI>
	</OL>
	<p class="error"><asp:Literal id="ltlErrorMessage" Runat="server"></asp:Literal></p>
	<P>Welcome to the Subtext Installation Wizard!
	</P>
	<P>In order to install the Subtext database*, we need to gather a bit of 
		information from you.
	</P>
	<P>Please fill in the following fields.</P>
	<P>
		<asp:Panel id="pnlInstallationInformation" runat="server"></asp:Panel>
	</P>
	<P>
		<asp:Button id="btnSave" runat="server" text="Go to Step 2"></asp:Button></P>
	<P class="footnote">* Please note that the current version of Subtext only supports 
		Microsoft SQL Server 2000 and above. Future versions of Subtext may add support 
		for other databases and file system storage.
	</P>
</MP:MasterPage>
