<%@ Page Language="C#" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="Subtext.Web.Admin.Users" Title="Subtext Admin - Users" %>
<%@ Register TagPrefix="st" TagName="UserManager" Src="~/Admin/UserControls/UserManager.ascx" %>
<%@ Register TagPrefix="st" TagName="RoleChooser" Src="~/Admin/UserControls/RoleChooser.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="actionsHeading" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="categoryListHeading" runat="server"></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="categoryListLinks" runat="server"></asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="pageContent" runat="server">
	<fieldset>
		<legend>Users</legend>
		<st:UserManager id="userManager" runat="server" />
	</fieldset>
	
	<fieldset>
		<legend>Add User</legend>
		<p class="instructions">
			Type the e-mail address of another user in the system 
			to add them to this blog.
		</p>
		<div>
			<asp:Label AssociatedControlID="emailTextBox" runat="server" Text="Email" />
			<asp:TextBox ID="emailTextBox" runat="server" ValidationGroup="AddUserGroup" />
			<asp:RequiredFieldValidator ID="emailRequiredValidator" runat="server"
				ValidationGroup="AddUserGroup"
				Display="Dynamic"
				ControlToValidate="emailTextBox"
				ErrorMessage="Email is required" />
			<asp:CustomValidator ID="emailExistsValidator" runat="server"
				ControlToValidate="emailTextBox"
				ErrorMessage="A user with that email does not exist in the system"
				Display="Dynamic" />
		</div>
		<div>
			<asp:Label AssociatedControlID="roleChooser" runat="server" Text="Role(s)" />
			<st:RoleChooser id="roleChooser" runat="server" EnableViewState="true" />
		</div>
		<div class="button-row">
			<asp:Button ID="addUserButton" runat="server" Text="Add User" ValidationGroup="AddUserGroup" CausesValidation="true" OnClick="OnAddUserClick" />
		</div>
		
	</fieldset>
</asp:Content>
