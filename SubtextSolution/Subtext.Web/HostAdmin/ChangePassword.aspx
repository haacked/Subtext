<%@ Page Language="C#" EnableTheming="false"  Title="Subtext - Host Admin - Change Password" MasterPageFile="~/HostAdmin/HostAdminTemplate.Master" Codebehind="ChangePassword.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.HostAdmin.ChangePassword" %>

<asp:Content id="sectionTitle" ContentPlaceHolderID="MPSectionTitle" runat="server">Subtext - Host Admin - Change HostAdmin Password</asp:Content>
<asp:Content id="passwordChangeForm" ContentPlaceHolderID="MPContent" runat="server">
	<fieldset id="change-password">
		<legend>Change Your Password</legend>
	<div class="form">
		<asp:ChangePassword ID="ChangePassword1" runat="server" ContinueDestinationPageUrl="~/HostAdmin/">
			<ChangePasswordTemplate>
				<div>
					<asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Password:</asp:Label>
					<asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password" />
					<asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
							ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
				</div>
				
				<div>
					<asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">New Password:</asp:Label>
					<asp:TextBox ID="NewPassword" runat="server" TextMode="Password" />
					<asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
						ErrorMessage="New Password is required." ToolTip="New Password is required."
						ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
				</div>	
				
				<div>
					<asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>					
					<asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password" />
					<asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
						ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
						ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
					<asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
						ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry."
						ValidationGroup="ChangePassword1" />
				</div>
			
				<div>
					<asp:Literal ID="FailureText" runat="server" EnableViewState="False" />
				</div>

				<div class="button-row">
					<asp:Button ID="ChangePasswordPushButton" runat="server" CommandName="ChangePassword"
						Text="Change Password" ValidationGroup="ChangePassword1" />
				
					<asp:Button ID="CancelPushButton" runat="server" CausesValidation="False" CommandName="Cancel"
						Text="Cancel" />
				</div>
			</ChangePasswordTemplate>
		</asp:ChangePassword>
	</div>
	</fieldset>
</asp:Content>