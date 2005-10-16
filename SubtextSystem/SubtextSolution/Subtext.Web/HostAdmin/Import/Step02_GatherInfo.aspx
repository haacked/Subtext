<%@ Page language="c#" Codebehind="Step02_GatherInfo.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.Step02_GatherInfo" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/HostAdmin/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Import - Step 2 - Gather Information</MP:ContentRegion>
	<MP:ContentRegion id="MPSectionTitle" runat="server">Gathering Information</MP:ContentRegion>
	<p class="error"><asp:Literal id="ltlErrorMessage" Runat="server" EnableViewState="False"></asp:Literal></p>
	<p>
		In this step, the import engine needs to gather some information 
		from you in order to continue.  Please fill out the following fields.
	</p>
	<p>
		<asp:PlaceHolder id="plcImportInformation" runat="Server"></asp:PlaceHolder>
	</p>
	<p>
		<asp:Button id="btnNext" runat="server" text="Next - Begin Import"></asp:Button>
	</p>
	<p id="paraBeginImportText" runat="server">
		Thanks for supplying the required information.  Double check your information 
		as you are now ready to begin the import.  Allow us to repeat our earlier warning.
		<div>
			Importing data from another engine will erase your current blog data.  
			There is no once you hit the button below, there is no turning back.
		</div>
		<asp:Button id="btnBeginImport" runat="server" text="Begin the Import!"></asp:Button>
	</p>
</MP:MasterPage>
