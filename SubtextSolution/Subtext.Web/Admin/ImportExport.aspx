<%@ Page language="c#" Title="Subtext Admin - Import Export" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="ImportExport.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.ImportExportPage" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="importExportContent" ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server"></st:MessagePanel>
	<st:AdvancedPanel id="Action" runat="server" BodyCssClass="Edit" DisplayHeader="true" HeaderCssClass="CollapsibleHeader"
		HeaderText="Import/Export" Collapsible="False">
		<fieldset>
			<legend>Import &amp; Export BlogML files.</legend>
			<h4>Persist this blog to BlogML Format.</h4>
			<p>
				This function will generate BlogML for your blog and output it as an xml file. 
				After the BlogML generation phase you will be presented with a link to the 
				file.
			</p>
			<p>
				<asp:CheckBox id="chkEmbedAttach" runat="server" Text="Embed Attachments?" Checked="True"></asp:CheckBox><br />
				<br />
				<asp:Button id="btnSave" runat="server" Text="Save" CssClass="buttonSubmit" CausesValidation="false"></asp:Button>&nbsp;&nbsp;
				<asp:HyperLink id="hypBlogMLFile" runat="server" Visible="False">Download BlogML File</asp:HyperLink>
			</p>
			<hr />
			<!-- BlogML Reader -->
			<h4>Import from BlogML.</h4>
			<p>
				Allows you to import an existing blog by loading BlogML content.<br />
				<label class="block">BlogML file:&nbsp; 
					<asp:RequiredFieldValidator id="blogMLFileRequired" runat="server" ForeColor="#990066" Display="Dynamic" ControlToValidate="importBlogMLFile"
						ErrorMessage="You must select a valid BlogML file to import."></asp:RequiredFieldValidator></label>
			</p>
			<p>
				<input id="importBlogMLFile" type="file" name="filImportBlogML" runat="server" ValidationGroup="importGroup" />
				<br />
				<br />
				<asp:Button id="btnLoad" runat="server" Text="Load!" CssClass="buttonSubmit" ValidationGroup="importGroup"></asp:Button>
			</p>
		</fieldset>
	</st:AdvancedPanel>
</asp:Content>