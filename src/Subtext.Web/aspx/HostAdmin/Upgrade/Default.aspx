<%@ Page Language="C#" EnableTheming="false"  Title="Subtext - Host Admin - Installed Blogs" MasterPageFile="~/aspx/HostAdmin/HostAdminTemplate.Master" Codebehind="Default.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.HostAdmin.Upgrade.Default" %>

<asp:Content id="sectionTitle" ContentPlaceHolderID="MPSectionTitle" runat="server">Subtext Upgrade Wizard: Welcome</asp:Content>
<asp:Content id="sidebar" ContentPlaceHolderID="MPSideBar" runat="server"></asp:Content>
<asp:Content id="mainContent" ContentPlaceHolderID="MPContent" runat="server">
	<p>Welcome to the Subtext Upgrade Wizard.</p>
	<asp:PlaceHolder ID="plcHolderUpgradeMessage" runat="server" Visible="false">
		<p class="error">
			<asp:Literal ID="messageLiteral" runat="server" />
		</p>
	</asp:PlaceHolder>
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
	<p><asp:Button id="btnUpgrade" runat="server" text="Upgrade" onclick="OnUpgradeClick" /></p>
</asp:Content>