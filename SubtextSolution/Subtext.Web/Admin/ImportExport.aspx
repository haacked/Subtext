<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Import Export" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="ImportExport.aspx.cs" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" Inherits="Subtext.Web.Admin.Pages.ImportExportPage" %>

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
			<legend>Export to BlogML</legend>
			<p>
				This function will generate BlogML for your blog and output it as an xml file. 
				After the BlogML generation phase you will be presented with a link to the 
				file.
			</p>
			
			<asp:CheckBox id="chkEmbedAttach" runat="server" Text="Embed Attachments?" Checked="True" CssClass="checkbox" />
			
			<div class="buttons">
				<asp:Button id="btnSave" runat="server" Text="Save" CssClass="button" CausesValidation="false" />&nbsp;&nbsp;
				<asp:HyperLink id="hypBlogMLFile" runat="server" Visible="False">Download BlogML File</asp:HyperLink>
			</div>
		</fieldset>
		
		<!-- BlogML Reader -->
		<fieldset>
			<legend>Import from BlogML.</legend>
			<p>
				Import content from another blog that was exported as BlogML into this blog.
			</p>
			<p>
				<asp:FileUpload ID="importBlogMLFile" runat="server" runat="server" />
				<asp:RequiredFieldValidator ID="fileRequired" runat="server" ControlToValidate="importBlogMLFile" ErrorMessage="Please specify a BlogML File" ValidationGroup="importGroup" />
			</p>			
			<div class="buttons">
				<asp:Button id="btnLoad" runat="server" Text="Load!" CssClass="button" ValidationGroup="importGroup" />
			</div>
		</fieldset>
		
		<fieldset>
			<legend>Clear Blog Content.</legend>
			<p>
				This will remove all content (Entries, Comments, Track/Ping-backs, Statistices, etc...) from this blog.<br />
				After doing this, all content will be lost <strong>forever!</strong>
			</p>
			<asp:Panel id="uppnlClearContent" runat="server">
				<st:MessagePanel id="msgpnlClearContent" runat="server"></st:MessagePanel>
				
				<asp:CheckBox id="chkClearContent" runat="server" Text="Clear Content." CssClass="checkbox" Checked="false" />
				
				<div class="buttons">
					<asp:Button id="btnClearContent" runat="server" Text="Clear It!" CssClass="button" CausesValidation="false" OnClick="btnClearContent_Click" />
				</div>
			</asp:Panel>
		</fieldset>
	
	</st:AdvancedPanel>
</asp:Content>
