<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Import Export" Codebehind="ImportExport.aspx.cs" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" Inherits="Subtext.Web.Admin.Pages.ImportExportPage" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Options</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server" />
<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server" />

<asp:Content ID="importExportContent" ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server"></st:MessagePanel>
	<h2>Import/Export</h2>
	<div id="section">
		<fieldset>
			<legend>Import &amp; Export BlogML files.</legend>
			<h4>Export to BlogML</h4>
			<p>
				This function will generate BlogML for your blog and output it as an xml file. 
				After the BlogML generation phase you will be presented with a link to the 
				file.
			</p>
			<asp:CheckBox id="chkEmbedAttach" runat="server" Text="Embed Attachments?" Checked="True" CssClass="checkbox" />
			<p>
				<asp:Button id="btnSave" runat="server" Text="Save" CssClass="buttonSubmit" CausesValidation="false" OnClick="btnSave_Click" />&nbsp;&nbsp;
				<asp:HyperLink id="hypBlogMLFile" runat="server" Visible="False">Download BlogML File</asp:HyperLink>
			</p>
			<hr />
			<!-- BlogML Reader -->
			<h4>Import from BlogML</h4>
			<p>
				Allows you to import an existing blog by loading BlogML content.<br />
				<label>BlogML file:&nbsp; 
					<asp:RequiredFieldValidator id="blogMLFileRequired" runat="server" ForeColor="#990066" Display="Dynamic" ControlToValidate="importBlogMLFile"
						ErrorMessage="You must select a valid BlogML file to import."></asp:RequiredFieldValidator></label>
			</p>
			<p>
				<asp:FileUpload ID="importBlogMLFile" runat="server" runat="server" />
				<asp:RequiredFieldValidator ID="fileRequired" runat="server" ControlToValidate="importBlogMLFile" ErrorMessage="Please specify a BlogML File" ValidationGroup="importGroup" />
			</p>
			<p>
				<asp:Button id="btnLoad" runat="server" Text="Load!" CssClass="buttonSubmit" ValidationGroup="importGroup" OnClick="btnLoad_Click" />
			</p>
			<hr />
			<h4>Clear Blog Content</h4>
			<p>
				This will remove all content (Entries, Comments, Track/Ping-backs, Statistices, etc...) from this blog.<br />
				After doing this, all content will be lost <strong>forever!</strong>
			</p>
			<asp:Panel id="uppnlClearContent" runat="server">
			    <st:MessagePanel id="msgpnlClearContent" runat="server"></st:MessagePanel>
				<asp:CheckBox id="chkClearContent" runat="server" Text="Clear Content." Checked="false" CssClass="checkbox" />
				<p>
					<asp:Button id="btnClearContent" runat="server" Text="Clear It!" CssClass="buttonSubmit" CausesValidation="false" OnClick="btnClearContent_Click"/>
				</p>
			</asp:Panel>
		</fieldset>
	</div>
</asp:Content>
