<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - Password" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Password.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Password" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="passwordContent" ContentPlaceHolderID="pageContent" runat="server">
	<st:AdvancedPanel id="Results" runat="server" HeaderText="Password" LinkStyle="Image"
		DisplayHeader="True" HeaderCssClass="CollapsibleHeader" Collapsible="False">
		<asp:ChangePassword ID="passwordChanger" runat="server">
			<ChangePasswordTemplate>
				<fieldset>
					<legend>Change Your Password</legend>
					
					<span class="error"><asp:Literal ID="FailureText" runat="server" EnableViewState="False" /></span>
					
					<asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Password:</asp:Label>				
					<asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password" CssClass="textbox" />
					<asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
												ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Results$passwordChanger">*</asp:RequiredFieldValidator>
										
					<asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">New Password:</asp:Label>
					<asp:TextBox ID="NewPassword" runat="server" TextMode="Password" CssClass="textbox" />
					<asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
												ErrorMessage="New Password is required." ToolTip="New Password is required."
												ValidationGroup="Results$passwordChanger">*</asp:RequiredFieldValidator>
										
					<asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
					<asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password" CssClass="textbox" />
					<asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
												ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
												ValidationGroup="Results$passwordChanger">*</asp:RequiredFieldValidator>
										
					<asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
												ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry."
												ValidationGroup="Results$passwordChanger" />
					
					<p class="buttons">
						<asp:Button ID="ChangePasswordPushButton" runat="server" CommandName="ChangePassword" CssClass="button"
													Text="Change Password" ValidationGroup="Results$passwordChanger" />											
					</p>						
				</fieldset>
			</ChangePasswordTemplate>
			
			<SuccessTemplate>
				<p class="success">
					Your password was changed successfully!
				</p>
			</SuccessTemplate>
			
		</asp:ChangePassword>
	</st:AdvancedPanel>
</asp:Content>