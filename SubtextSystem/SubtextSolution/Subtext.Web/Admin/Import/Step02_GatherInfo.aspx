<%@ Page language="c#" Codebehind="Step02_GatherInfo.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Import.Step02_GatherInfo" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Admin/Import/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Import - Step 2 - Gather Information</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Gathering Information</MP:ContentRegion>
	<p class="error"><asp:Literal id="ltlErrorMessage" Runat="server"></asp:Literal></p>
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
</MP:MasterPage>
