<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Default.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.Upgrade.Default" %>
<MP:MasterPage id="MPContainer" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Upgrade 
Wizard: Welcome</MP:ContentRegion>
	<MP:ContentRegion id="MPSideBar" runat="server"></MP:ContentRegion>
	<p>Welcome to the Subtext Upgrade Wizard.</p>
	<p>
		The database schema needs to be updated. 
		To do this automatically, press the <em>Upgrade</em> button below.
	</p>
	<div>
		<p>
			<strong class="warning">This will make schema changes.</strong>
			Make sure that the database user specified by the connection string in 
			web.config has <strong>db owner rights to the database</strong>. After the 
			upgrade process, you can remove the db owner rights.
		</p>
		<p>
			<strong>We recommend making a database backup first.</strong>
		</p>
	</div>
	<p>
	<p><asp:Button id="btnUpgrade" runat="server" text="Upgrade"></asp:Button></p>
</MP:MasterPage>
