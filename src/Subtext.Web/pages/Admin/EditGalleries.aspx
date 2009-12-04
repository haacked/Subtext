<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Edit Galleries" Codebehind="EditGalleries.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.EditGalleries" MaintainScrollPositionOnPostback="true" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server" />

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server">
    <h2>Galleries</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server">
    <st:CategoryLinks ID="categoryLinks" runat="server" CategoryType="ImageCollection" />
</asp:Content>

<asp:Content ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server" ErrorIconUrl="~/images/icons/ico_critical.gif" ErrorCssClass="ErrorPanel" MessageIconUrl="~/images/icons/ico_info.gif" MessageCssClass="MessagePanel"></st:MessagePanel>
	<h2>Galleries</h2>
	<asp:PlaceHolder id="Results" runat="server">
	    <div class="section">
		<asp:DataGrid id="dgrSelectionList" runat="server" CssClass="listing highlightTable" GridLines="None" AutoGenerateColumns="False">
			<AlternatingItemStyle CssClass="alt"></AlternatingItemStyle>
				<HeaderStyle CssClass="Header"></HeaderStyle>

				<Columns>
					<asp:TemplateColumn HeaderText="Gallery">
						<ItemTemplate>
							<asp:label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Title") %>' ID="label1" NAME="label1" />
							<asp:label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>' ID="label3" NAME="label1" />
						</ItemTemplate>

						<EditItemTemplate>
						    <fieldset>
						        <legend>Edit Gallery</legend>
							    <asp:Label runat="server" AssociatedControlID="txbTitle" Text="Title" />
							    <asp:TextBox CssClass="textbox" id="txbTitle" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Title") %>' />
							    <asp:Label runat="server" AssociatedControlID="txbDescription" Text="Description" />
							    <asp:TextBox CssClass="textarea" rows="5" textmode="MultiLine" id="txbDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>'></asp:TextBox>
							</fieldset>
						</EditItemTemplate>
					</asp:TemplateColumn>

					<asp:TemplateColumn HeaderText="Visible">
						<ItemTemplate>
							<asp:label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IsActive") %>' ID="label2"></asp:label>
						</ItemTemplate>

						<EditItemTemplate>
							<asp:CheckBox id="ckbIsActive" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.IsActive") %>' CssClass="checkbox" />
						</EditItemTemplate>
					</asp:TemplateColumn>
					
					<asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" CancelText="Cancel" EditText="Edit" />
					
					<asp:ButtonColumn Text="Delete" CommandName="Delete" />
				</Columns>
			</asp:DataGrid>
		
			<!-- add new item panel -->
			<st:AdvancedPanel id="Add" runat="server" DisplayHeader="false" HeaderCssClass="CollapsibleTitle" Collapsible="False" Collapsed="false" BodyCssClass="Edit">
				<fieldset>
				    <legend>Add New Gallery</legend>
				    <label>Title</label>
				    <asp:TextBox id="txbNewTitle" runat="server" />&nbsp; 
				    <asp:CheckBox id="ckbNewIsActive" runat="server" Checked="true" CssClass="checkbox" Text="Visible" TextAlign="Left" />
				    <label>Description (1000 characters including HTML)</label>
				    <asp:TextBox id="txbNewDescription" MaxLength="1000"  runat="server" CssClass="textarea" rows="5" textmode="MultiLine" />
				    <div class="button-div">
					    <asp:Button id="lkbPost" runat="server" CssClass="buttonSubmit" Text="Add" onclick="lkbPost_Click"></asp:Button><br />&nbsp; 
				    </div>
				</fieldset>
			</st:AdvancedPanel>
		    </div>
		</asp:PlaceHolder>
		
		<!-- add/upload a new file -->
		<asp:PlaceHolder id="ImagesDiv" runat="server">
		    <st:AdvancedPanel id="AddImages" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="false" HeaderCssClass="CollapsibleTitle" HeaderText="Add New Image (Single file or ZIP archive)" Collapsible="False" Collapsed="false" BodyCssClass="Edit">
                <fieldset class="edit-form">
                    <legend>Add New Image (Single file or ZIP archive)</legend>    
                
			        <label>Local File Location</label> 
			        <input class="FileUpload" id="ImageFile" type="file" size="82" name="ImageFile" runat="server" /> 
			        <label>Image Description (ignored for ZIP archives)</label> 
			        <asp:TextBox id="txbImageTitle" runat="server" MaxLength="82" />&nbsp; 
			        <asp:CheckBox id="ckbIsActiveImage" runat="server" CssClass="checkbox" Checked="true" Text="Visible" />
    				
			        <asp:PlaceHolder ID="PanelDefaultName" runat="server">
			        <div class="button-div">
				        <asp:Button id="lbkAddImage" runat="server" OnClick="OnAddImage" CssClass="buttonSubmit" Text="Add" /><br /> 
			        </div>
			        </asp:PlaceHolder>
    				
			        <asp:Panel ID="PanelSuggestNewName" runat="server" visible="false">
				        <label>Uploaded Image File Name</label> 
				        <asp:TextBox id="TextBoxImageFileName" runat="server" MaxLength="82"/> 
				        <div class="button-div">
					        <asp:Button id="lbkNewFile" runat="server" OnClick="OnAddImageUserProvidedName" CssClass="buttonSubmit" Text="Add"/><br />
				        </div>
			        </asp:Panel>
			    </fieldset>			
		    </st:AdvancedPanel>
            <h2><asp:PlaceHolder id="plhImageHeader" runat="server"/></h2>
		    <asp:Repeater id="rprImages" runat="server"> 
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
		    </asp:Repeater>
		</asp:Placeholder>
</asp:Content>