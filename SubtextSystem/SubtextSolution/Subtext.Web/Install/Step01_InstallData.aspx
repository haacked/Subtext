<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Step01_InstallData.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.Step01_InstallData" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Install/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Installation: Step 1 - Data 
Installation</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Step 1 - Data Installation</MP:ContentRegion>
	<OL>
		<LI>
			<STRONG>Install the database</STRONG>
		<LI>
		Configure the Host Admin
		<LI>
			Create a Blog
		</LI>
	</OL>
	<asp:Literal id="ltlMessage" Runat="server"></asp:Literal>
	<P>This step allows you to install a fresh Subtext database or upgrade an existing 
		installation.
	</P>
	<P>
		<asp:RadioButton id="radUpgrade" runat="server" GroupName="InstallOption" Text="Upgrade .TEXT 0.95"></asp:RadioButton><BR>
		<asp:RadioButton id="radInstallFresh" runat="server" GroupName="InstallOption" Text="Install Subtext"></asp:RadioButton></P>
	<P>
		<asp:CheckBox id="chkStoredProcs" runat="server" Checked="True" Text="Stored Procedures"></asp:CheckBox></P>
	<P><A href="Step02_ConfigureHost.aspx">Go to step 2</A>.
	</P>
</MP:MasterPage>
