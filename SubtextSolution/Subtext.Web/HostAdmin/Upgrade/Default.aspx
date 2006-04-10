<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Default.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.Upgrade.Default" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Upgrade Wizard: Welcome</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server"></MP:ContentRegion>
	<p>Welcome to the Subtext Upgrade Wizard.</p>
	<p>
		You have recently updated your Subtext installation assemblies. 
		The database needs to be updated as well. I can try and do this 
		for you automatically, or you can obtain the SQL scripts and 
		run them yourself manually.
	</p>
	<div>
		<p>
		Make sure that the database user specified by the connection string in 
		web.config has <strong>db owner rights to the database</strong>.  After the installation 
		process, you can remove the db owner rights.
		</p>
	</div>
	<p><asp:Button id="btnUpgrade" runat="server" text="Upgrade"></asp:Button></p>
	<p class="footnote">* Please note that the current version of Subtext only supports 
		Microsoft SQL Server 2000 and above. Future versions of Subtext may add support 
		for other databases and file system storage.
	</p>
</MP:MasterPage>

