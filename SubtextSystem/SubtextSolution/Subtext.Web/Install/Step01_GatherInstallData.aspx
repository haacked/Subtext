<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Step01_GatherInstallData.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.Step01_GatherInstallData" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Install/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Installation: Step 1 - Gather Installation Data</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Step 1 - Data Installation</MP:ContentRegion>
	<OL>
		<li><strong>Gather Installation Information</strong></li>
		<LI>
			Install the database
		<LI>
		Configure the Host Admin
		<LI>
			Create a Blog
		</LI>
	</OL>
	<asp:Literal id="ltlMessage" Runat="server"></asp:Literal>
	<P>
		In order to install Subtext, we need to gather a bit of information 
		from you.
	</P>
	<P>
		<asp:RadioButton id="radUpgrade" runat="server" GroupName="InstallOption" Text="Upgrade .TEXT 0.95"></asp:RadioButton><BR>
		<asp:RadioButton id="radInstallFresh" runat="server" GroupName="InstallOption" Text="Install Subtext"></asp:RadioButton></P>
	<P>
		<asp:CheckBox id="chkStoredProcs" runat="server" Checked="True" Text="Stored Procedures"></asp:CheckBox></P>
	<P><A href="Step02_InstallData.aspx">Go to step 2</A>.
	</P>
</MP:MasterPage>
