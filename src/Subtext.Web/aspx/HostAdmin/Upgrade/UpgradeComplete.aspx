<%@ Page Title="Subtext Upgrade: Upgrade Complete!" MasterPageFile="~/aspx/HostAdmin/HostAdminTemplate.Master" Language="C#" EnableTheming="false"  Codebehind="UpgradeComplete.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.HostAdmin.Upgrade.UpgradeComplete" %>

<asp:Content id="sectionTitle" ContentPlaceHolderID="MPSectionTitle" runat="server">Subtext Upgrade: Upgrade Complete!</asp:Content>
<asp:Content id="sidebar" ContentPlaceHolderID="MPSideBar" runat="server"></asp:Content>
<asp:Content id="mainContent" ContentPlaceHolderID="MPContent" runat="server">
	<p>Congratulations. The Subtext upgrade is complete.</p>
	<p>
		Click <a id="lnkHostAdmin" href="~/HostAdmin/" runat="server">here</a> to visit 
		the Host Admin tool.
	</p>
</asp:Content>