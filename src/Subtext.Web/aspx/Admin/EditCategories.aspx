<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Edit Categories" Codebehind="EditCategories.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.EditCategories" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server" />

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server" />

    <h2>Edit Categories</h2>
	<asp:PlaceHolder id="Edit" runat="server">
		<asp:DataGrid id="dgrItems" AutoGenerateColumns="False" GridLines="None" CssClass="listing" runat="server">
			<AlternatingItemStyle CssClass="alt"></AlternatingItemStyle>
			<HeaderStyle CssClass="Header"></HeaderStyle>

			<Columns>
				<asp:TemplateColumn HeaderText="Category">
					<ItemTemplate>
						<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Title") %>'></asp:Label>
						<br />
						<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>' ID="Label2" NAME="Label1"></asp:Label>
					</ItemTemplate>

					<EditItemTemplate>
						<asp:Label runat="server" AssociatedControlID="txbTitle" Text="Title" />
						<asp:TextBox CssClass="textbox" id="txbTitle" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Title") %>' />
						<asp:Label runat="server" AssociatedControlID="txbTitle" Text="Description" />
						<asp:TextBox CssClass="textarea" rows="5" textmode="MultiLine" id="txbDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Description") %>' />
					</EditItemTemplate>
				</asp:TemplateColumn>
				
				<asp:TemplateColumn HeaderText="Visible">
					<ItemTemplate>
						<asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IsActive") %>' />
					</ItemTemplate>

					<EditItemTemplate>
						<asp:CheckBox id="ckbIsActive" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.IsActive") %>'/>
					</EditItemTemplate>
				</asp:TemplateColumn>
				
				<asp:EditCommandColumn ButtonType="LinkButton" UpdateText="Update" CancelText="Cancel" EditText="Edit"></asp:EditCommandColumn>
				
				<asp:ButtonColumn Text="Delete" CommandName="Delete"></asp:ButtonColumn>
			</Columns>
		</asp:DataGrid>
	</asp:PlaceHolder>
	
	<asp:PlaceHolder id="Add" runat="server">
		<fieldset class="section categories-edit">
			<legend>Add New Category</legend>
		
			<label>
				Title
				&nbsp;<asp:RequiredFieldValidator id="valtxbNewTitleRequired" runat="server" ControlToValidate="txbNewTitle" ForeColor="#990066" ErrorMessage="Your category must have a description" />
			</label>
			<asp:TextBox id="txbNewTitle" runat="server" />
			&nbsp; 
			<p><asp:CheckBox id="ckbNewIsActive" runat="server" Checked="true" CssClass="checkbox" Text="Visible" /></p>
			
			<label>Description (1000 characters including HTML)</label>
			<asp:TextBox id="txbNewDescription" max="1000" runat="server" textmode="MultiLine" />
			<div class="buttons">
				<asp:Button id="lkbPost" runat="server" CssClass="button" Text="Add" onclick="lkbPost_Click" />
			</div>
		</fieldset>
	</asp:PlaceHolder>
</asp:Content>