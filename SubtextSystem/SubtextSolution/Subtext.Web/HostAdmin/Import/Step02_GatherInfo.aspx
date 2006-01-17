<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Step02_GatherInfo.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.Step02_GatherInfo" %>
<MP:MasterPage id="MPContainer" runat="server" TemplateFile="~/HostAdmin/PageTemplate.ascx">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Import - Step 2 - Gather Information</MP:ContentRegion>
	<MP:ContentRegion id="MPSectionTitle" runat="server">Gathering Information</MP:ContentRegion>
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
		<asp:Button id="btnNext" runat="server" text="Next - Begin Import"></asp:Button>
	</p>
	<p id="paraBeginImportText" runat="server">
		Thanks for supplying the required information. 
		Double check your information as you are now ready to begin the 
		import. Allow us to repeat our earlier warning:
		<br />
		<font color="#ff0000">Importing data from another engine will erase your current 
			blog data. Once you hit the button below, there is no turning back.</font>
		<br />
		<asp:Button id="btnBeginImport" runat="server" text="Begin the Import!"></asp:Button></P>
</MP:MasterPage>
