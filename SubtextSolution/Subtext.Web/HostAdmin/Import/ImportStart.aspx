<%@ Page Title="Subtext - Host Admin - Import Wizard" MasterPageFile="~/HostAdmin/HostAdminTemplate.Master" Language="C#" EnableTheming="false"  Codebehind="ImportStart.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.HostAdmin.ImportStart" %>

<asp:Content id="sectionTitle" ContentPlaceHolderID="MPSectionTitle" runat="server">Subtext - Host Admin - Import Wizard</asp:Content>
<asp:Content id="sidebar" ContentPlaceHolderID="MPSideBar" runat="server">
	<asp:Button id="btnRestartWizard" runat="server" text="Restart Import"></asp:Button>
</asp:Content>
<asp:Content id="mainContent" ContentPlaceHolderID="MPContent" runat="server">
	<p>
		Welcome to the .TEXT Import Wizard!
	</p>
	<p>
		This wizard is strictly for bulk importing .TEXT data.  If you wish to import from 
		other blogging engines, you can do that in the admin section of your specific blog.
	</p>
	<p>
		We’ll walk you through the steps to get you up and running in no time.
	</p>
	<p>
		<asp:Button id="btnNext" runat="server" text="Next - Specify Connection Information" onclick="btnNext_Click"></asp:Button>
	</p>
</asp:Content>
