<%@ Page Title="Subtext Import - Step 2 - Gather Information" MasterPageFile="~/HostAdmin/HostAdminTemplate.Master" Language="C#" EnableTheming="false"  Codebehind="Step02_GatherInfo.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.HostAdmin.Step02_GatherInfo" %>

<asp:Content id="sectionTitle" ContentPlaceHolderID="MPSectionTitle" runat="server">Gathering Information</asp:Content>
<asp:Content id="sidebar" ContentPlaceHolderID="MPSideBar" runat="server"></asp:Content>
<asp:Content id="mainContent" ContentPlaceHolderID="MPContent" runat="server">
	<p class="error">
		<asp:Literal id="ltlErrorMessage" EnableViewState="False" Runat="server"></asp:Literal>
	</p>
	<h2>Connection Information</h2>
	<p>
		In this step, the import engine needs to gather database connection information from you in 
		order to continue. 
	</p>
	<p>
		This import wizard assumes that your .TEXT database is on the same server as your Subtext 
		database.  If they are not, you may wish to copy the database over before running this wizard 
		or look into our BlogML import process.
	</p>
	<p>
		Please fill out the following fields.
	</p>
	<p>
		<asp:PlaceHolder id="plcImportInformation" runat="Server"></asp:PlaceHolder>
	</p>
	<p>
		<asp:Button id="btnNext" runat="server" text="Next - Begin Import" onclick="btnNext_Click"></asp:Button>
	</p>
	<p id="paraBeginImportText" runat="server">
		Thanks for supplying the required information. 
		Double check your information as you are now ready to begin the 
		import. Allow us to repeat our earlier warning:
		<br />
		<span class="warning">Importing data from another engine could conflict with your current 
			blog data. Once you hit the button below, there is no turning back.
		</span>
		<br />
		<asp:Button id="btnBeginImport" runat="server" text="Begin the Import!" onclick="btnBeginImport_Click"></asp:Button>
	</p>
</asp:Content>