<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Edit Galleries" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="EditGalleries.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.EditGalleries" %>
<%@ Register TagPrefix="st" TagName="CategoryLinks" Src="~/Admin/UserControls/CategoryLinkList.ascx" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
    Galleries
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
    <st:CategoryLinks ID="categoryLinks" runat="server" CategoryType="ImageCollection" />
</asp:Content>

<asp:Content ID="galleriesContainer" ContentPlaceHolderID="pageContent" runat="server">
	<st:ScrollPositionSaver id="scrollsaver" runat="server" />
	<st:MessagePanel id="Messages" runat="server" ErrorIconUrl="~/images/icons/ico_critical.gif" ErrorCssClass="ErrorPanel" MessageIconUrl="~/images/icons/ico_info.gif" MessageCssClass="MessagePanel"></st:MessagePanel>
	<st:AdvancedPanel id="Results" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True" HeaderCssClass="CollapsibleHeader" HeaderText="Galleries" Collapsible="False" Collapsed="false">
	
		<asp:DataGrid id="dgrSelectionList" runat="server" CssClass="Listing highlightTable" GridLines="None" AutoGenerateColumns="False">
			<AlternatingItemStyle CssClass="Alt"></AlternatingItemStyle>
				<HeaderStyle CssClass="Header"></HeaderStyle>

				<Columns>
					<asp:TemplateColumn HeaderText="Gallery">
						<ItemTemplate>
							<asp:label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Title") %>' ID="label1" NAME="label1"></asp:label>
							<br />
							<asp:label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>' ID="label3" NAME="label1"></asp:label>
						</ItemTemplate>

						<EditItemTemplate>
							<fieldset>
								<legend>Edit Gallery</legend>
								<label for="txbTitle">Title</label>
								<asp:TextBox CssClass="textbox" id="txbTitle" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Title") %>' />
								<label for="txbDescription">Description</label>
								<asp:TextBox rows="5" textmode="MultiLine" id="txbDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>' />
							</fieldset>
						</EditItemTemplate>
					</asp:TemplateColumn>

					<asp:TemplateColumn HeaderText="Visible">
						<ItemTemplate>
							<asp:label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IsActive") %>' ID="label2"></asp:label>
						</ItemTemplate>

						<EditItemTemplate>
							<asp:CheckBox id="ckbIsActive" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.IsActive") %>'/>
						</EditItemTemplate>
					</asp:TemplateColumn>
					
					<asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" CancelText="Cancel" EditText="Edit" />
					
					<asp:ButtonColumn Text="Delete" CommandName="Delete" />
				</Columns>
			</asp:DataGrid>
		
			<!-- add new item panel -->
			<asp:PlaceHolder id="Add" runat="server">
				<fieldset>
					<legend>Add New Gallery</legend>
					<label for="txbNewTitle">Title</label> 
					<asp:TextBox id="txbNewTitle" runat="server" CssClass="textbox" />&nbsp;
					<asp:CheckBox id="ckbNewIsActive" runat="server" Checked="true" Text="Visible" CssClass="checkbox" />
					
					<label for="txbNewDescription">Description (1000 characters including HTML)</label>
					<asp:TextBox id="txbNewDescription" MaxLength="1000" runat="server" textmode="MultiLine" />
					
					<div class="buttons">
						<asp:Button id="lkbPost" runat="server" CssClass="button" Text="Add" onclick="lkbPost_Click" />
					</div>
				</fieldset>
			</asp:PlaceHolder>
		
		</st:AdvancedPanel>
		
		<!-- add/upload a new file -->
		<ASP:Panel id="ImagesDiv" runat="server">
			<st:AdvancedPanel id="AddImages" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="true" HeaderCssClass="CollapsibleTitle" HeaderText="Add New Image (Single file or ZIP archive)" Collapsible="False" Collapsed="false" BodyCssClass="Edit">		
				<label class="Block">Local File Location</label> 
				<input class="FileUpload" id="ImageFile" type="file" size="82" name="ImageFile" runat="server" /> 
				<br class="clear" />
				<label class="Block">Image Description (ignored for ZIP archives)</label> 
				<asp:TextBox id="txbImageTitle" runat="server" MaxLength="82" />&nbsp; 
				Visible <asp:CheckBox id="ckbIsActiveImage" runat="server" Checked="true"/>
				
				<asp:Panel ID="PanelDefaultName" runat="server">
				<div style="margin-top: 8px">
					<asp:Button id="lbkAddImage" runat="server" OnClick="OnAddImage" CssClass="buttonSubmit" Text="Add" /><br /> 
				</div>
				</asp:Panel>
				
				<asp:Panel ID="PanelSuggestNewName" runat="server" visible="false">
					<label class="Block">Uploaded Image File Name</label> 
					<asp:TextBox id="TextBoxImageFileName" runat="server" MaxLength="82"/> 
					<div style="MARGIN-TOP: 8px">
						<asp:Button id="lbkNewFile" runat="server" OnClick="OnAddImageUserProvidedName" CssClass="buttonSubmit" Text="Add"/><br />
					</div>
				</asp:Panel>
								
			</st:AdvancedPanel>
		
		
			<h1><ASP:PlaceHolder id="plhImageHeader" runat="server"/></h1>
			<ASP:Repeater id="rprImages" runat="server"> 
				<HeaderTemplate> 			
					<div class="ImageList">
				</HeaderTemplate>
				<ItemTemplate>
						<div class="ImageThumbnail">
							<div class="ImageThumbnailImage">
								<asp:HyperLink id="lnkThumbnail" runat="server" ImageUrl='<%# EvalImageUrl(Container.DataItem) %>' NavigateUrl='<%# EvalImageNavigateUrl(Container.DataItem) %>'/>
							</div>
							<div class="ImageThumbnailTitle">
								<%# EvalImageTitle(Container.DataItem) %>
								<br />
								<a href='EditImage.aspx?imgid=<%# DataBinder.Eval(Container.DataItem, "ImageID") %>'>Edit</a>
								&nbsp;&bull;&nbsp;
								<asp:Button id="lnkDeleteImage" CssClass="buttonSubmit" CommandName="DeleteImage" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ImageID") %>' Text="Delete" runat="server" />
							</div>
						</div>				
				</ItemTemplate>
				<FooterTemplate>
					</div>
				</FooterTemplate>
			</ASP:Repeater>
			<br class="clear" />
		</ASP:Panel>
</asp:Content>
