<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Step02_GatherInfo.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.Step02_GatherInfo" %>
<MP:MASTERPAGE id="MPContainer" runat="server" TemplateFile="~/HostAdmin/PageTemplate.ascx">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Import - Step 2 - Gather Information</MP:ContentRegion>
	<MP:ContentRegion id="MPSectionTitle" runat="server">Gathering 
Information</MP:ContentRegion>
	<P class="error">
		<asp:Literal id="ltlErrorMessage" EnableViewState="False" Runat="server"></asp:Literal></P>
	<P>In this step, the import engine needs to gather some information from you in 
		order to continue. Please fill out the following fields.
	</P>
	<P>
		<asp:PlaceHolder id="plcImportInformation" runat="Server"></asp:PlaceHolder></P>
	<P>
		<asp:Button id="btnNext" runat="server" text="Next - Begin Import"></asp:Button></P>
	<P id="paraBeginImportText" runat="server">Thanks for supplying the required 
		information. Double check your information as you are now ready to begin the 
		import. Allow us to repeat our earlier warning:
		<BR>
		<FONT color="#ff0000">Importing data from another engine will erase your current 
			blog data. Once you hit the button below, there is no turning back.</FONT>
		<BR>
		<asp:Button id="btnBeginImport" runat="server" text="Begin the Import!"></asp:Button></P>
</MP:MASTERPAGE>
