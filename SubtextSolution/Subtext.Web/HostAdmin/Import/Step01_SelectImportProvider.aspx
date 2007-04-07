<%@ Page Title="Subtext Import - Step 1 - Select an Import Provider" MasterPageFile="~/HostAdmin/HostAdminTemplate.Master" Language="C#" EnableTheming="false"  Codebehind="Step01_SelectImportProvider.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.HostAdmin.Step01_SelectImportProvider" %>

<asp:Content id="sectionTitle" ContentPlaceHolderID="MPSectionTitle" runat="server">Subtext Import - Step 1 - Select an Import Provider</asp:Content>
<asp:Content id="sidebar" ContentPlaceHolderID="MPSideBar" runat="server"></asp:Content>
<asp:Content id="mainContent" ContentPlaceHolderID="MPContent" runat="server">
	<p>In this step, you’ll select an import provider used to import data from another 
		blog engine. If you don’t see your blog engine listed here, then either you 
		don’t have an import provider for that engine installed, or nobody has written 
		one for it yet. Do I smell opportunity?
	</p>
	<h2>Import Providers</h2>
	<p>
		<asp:RadioButtonList id="rdlImportProviders" runat="server"></asp:RadioButtonList>
		<asp:RequiredFieldValidator id="vldImportProviders" runat="server" 
			ControlToValidate="rdlImportProviders" 
			Display="Dynamic"
			ErrorMessage="Please Select an Import Provider"></asp:RequiredFieldValidator>
	</p>
	<p>
		<asp:Button id="btnNext" runat="server" text="Next - Gather Import Information" onclick="btnNext_Click"></asp:Button>
	</p>
</asp:Content>