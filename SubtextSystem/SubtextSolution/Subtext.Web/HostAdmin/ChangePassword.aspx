<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="ChangePassword.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.HostAdmin.ChangePassword" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/HostAdmin/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext - Change Host Admin Password</MP:ContentRegion>
	<MP:ContentRegion id="MPSectionTitle" runat="server">Change Host Admin Password</MP:ContentRegion>
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
			<asp:Button id="btnSave" runat="server" Text="Save"></asp:Button>
		</p>
	</div>

</MP:MasterPage>
