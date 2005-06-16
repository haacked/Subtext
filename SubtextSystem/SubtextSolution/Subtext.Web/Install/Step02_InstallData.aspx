<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Step02_InstallData.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.Step02_InstallData" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Install/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Installation: Step 2 - Data 
Installation</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Step 2 - Data Installation</MP:ContentRegion>
	<OL>
		<li>Gather Installation Information</li>
		<LI>
			<STRONG>Install the database</STRONG>
		<LI>
		Configure the Host Admin
		<LI>
			Create a Blog
		</LI>
	</OL>
	<P>
		Thanks for the information provided in the last step.  
		You are now ready to install the Subtext database.
	</P>
	<p><asp:Literal id="installationStateMessage" Runat="server"></asp:Literal></p>
	<p>
		<asp:CheckBox id="chkFullInstallation" runat="Server" Text="Full Install" />
	</p>
	<P>
		<asp:Button id="btnInstall" runat="server" Text="Install Now!"></asp:Button>
	</P>
</MP:MasterPage>
