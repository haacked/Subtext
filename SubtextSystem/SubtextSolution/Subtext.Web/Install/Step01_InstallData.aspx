<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Step01_InstallData.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.Step01_InstallData" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Install/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Installation: Step 1 - Data Installation</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Step 1 - Data Installation</MP:ContentRegion>
		<ol>
			<li><strong>Install the database</strong></li>
			<li>Configure the Host Admin</li>
			<li>Create a Blog</li>
		</ol>
		<asp:Literal id="ltlMessage" Runat="server"></asp:Literal>
		<P>
			Not Ready for prime time...  Please run the database installation scripts manually.	
		</P>
		<p>
			This page will give the user an option to install the database tables and 
			stored procedures.
		</p>
		<p>
		<a href="Step02_ConfigureHost.aspx">Go to step 2</a>.
		</p>
</MP:MasterPage>
