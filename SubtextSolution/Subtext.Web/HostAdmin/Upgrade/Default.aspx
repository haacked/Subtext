<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Default.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.Upgrade.Default" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Upgrade 
Wizard: Welcome</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server"></MP:ContentRegion>
	<P>Welcome to the Subtext Upgrade Wizard.</P>
	<P>You have recently updated your Subtext installation assemblies. The database 
		needs to be updated as well. I can try and do this for you automatically, or 
		you can obtain the SQL scripts and run them yourself manually.
	</P>
	<DIV>
		<P>Make sure that the database user specified by the connection string in 
			web.config has <STRONG>db owner rights to the database</STRONG>. After the 
			upgrade process, you can remove the db owner rights.
		</P>
	</DIV>
	<P>
		<asp:Button id="btnUpgrade" runat="server" text="Upgrade"></asp:Button></P>
	<P class="footnote">* Please note that the current version of Subtext only supports 
		Microsoft SQL Server 2000 and above. Future versions of Subtext may add support 
		for other databases and file system storage.
	</P>
</MP:MasterPage>
