<%@ Page Title="Subtext - Host Admin - Change Password" Codebehind="ChangePassword.aspx.cs" Inherits="Subtext.Web.HostAdmin.ChangePassword" %>

<asp:Content ContentPlaceHolderID="MPSectionTitle" runat="server">Subtext - Host Admin - Change HostAdmin Password</asp:Content>

<asp:Content ContentPlaceHolderID="MPSideBar" runat="server" />

<asp:Content ContentPlaceHolderID="MPContent" runat="server">
	<p>Use this page to change the HostAdmin password..</p>
	
	<div class="form">
		<p>
			<asp:Label id="lblSuccess" runat="server" CssClass="success" Visible="False" Text="Password Changed!" />
			<asp:ValidationSummary id="validationSummary" runat="server"></asp:ValidationSummary>
		</p>
		<p>
			<label for="txtCurrentPassword">
				<asp:RequiredFieldValidator id="vldCurrentPassword" runat="server" 
					Text="*" 
					ControlToValidate="txtCurrentPassword"></asp:RequiredFieldValidator> 
				<asp:CustomValidator id="vldCurrent" runat="server" ControlToValidate="txtCurrentPassword" Display="None"
					ErrorMessage="The Current Password is not correct.">*</asp:CustomValidator>
				Current Password:
			</label> 
			<asp:TextBox id="txtCurrentPassword" runat="server" TextMode="Password"  />
		</p>
		<p>
			<label for="txtNewPassword">
				<asp:RequiredFieldValidator id="vldNewPassword" runat="server" 
					Text="*" 
					ControlToValidate="txtNewPassword"></asp:RequiredFieldValidator> 
				New Password:
			</label>
			<asp:TextBox id="txtNewPassword" TextMode="Password" runat="server" />
		</p>
		<p>
			<label for="txtConfirmPassword">
				<asp:RequiredFieldValidator id="vldConfirmPassword" runat="server" 
					Text="*" 
					ControlToValidate="txtConfirmPassword"></asp:RequiredFieldValidator> 
				<asp:CompareValidator id="vldComparePasswords" runat="server" 
					ErrorMessage="The Passwords do not match."
					ControlToValidate="txtConfirmPassword" 
					ControlToCompare="txtNewPassword" 
					Display="None">*</asp:CompareValidator>
				Confirm Password:
			</label>
			<asp:TextBox id="txtConfirmPassword" TextMode="Password" runat="server" />
		</p>
		<p class="clear">
			<asp:Button id="btnSave" runat="server" Text="Save" onclick="btnSave_Click"></asp:Button>
		</p>
	</div>
</asp:Content>