<%@ Page Title="Subtext - Host Admin - Change Password" Codebehind="ChangePassword.aspx.cs" Inherits="Subtext.Web.HostAdmin.ChangePassword" %>

<asp:Content ContentPlaceHolderID="MPSectionTitle" runat="server">Subtext - Host Admin - Change HostAdmin Info</asp:Content>

<asp:Content ContentPlaceHolderID="MPSideBar" runat="server" />

<asp:Content ContentPlaceHolderID="MPContent" runat="server">
	<p>Use this page to change the HostAdmin info..</p>
	
	<fieldset>
	    <legend>Change Email</legend>
        <div class="form">
            <p>
			    <asp:Label id="emailChangedLabel" runat="server" CssClass="success" Visible="False" Text="Email Changed!" />
			</p>
            <p>
		        <asp:Label AssociatedControlID="txtEmail" runat="server">
			        Email:
		        </asp:Label> 
		        <asp:TextBox id="txtEmail" runat="server" ValidationGroup="email" CssClass="text" /> 
	        </p>
	        <p class="clear">
			    <asp:Button runat="server" Text="Change Email" onclick="OnChangeEmailButtonClick" CausesValidation="false" />
		    </p>
        </div>
	</fieldset>
	
	<fieldset>
	    <legend>Change Password</legend>
	    <div class="form">
		    <p>
			    <asp:Label id="lblSuccess" runat="server" CssClass="success" Visible="False" Text="Password Changed!" />
			    <asp:ValidationSummary id="validationSummary" runat="server"></asp:ValidationSummary>
		    </p>
    		
		    <p>
			    <asp:Label AssociatedControlID="txtCurrentPassword" runat="server">
				    <asp:RequiredFieldValidator id="vldCurrentPassword" runat="server" 
					    Text="*" 
					    ControlToValidate="txtCurrentPassword" /> 
				    <asp:CustomValidator id="vldCurrent" runat="server" ControlToValidate="txtCurrentPassword" Display="None"
					    ErrorMessage="The Current Password is not correct.">*</asp:CustomValidator>
				    Current Password:
			    </asp:Label> 
			    <asp:TextBox id="txtCurrentPassword" runat="server" TextMode="Password" CssClass="text" />
		    </p>
		    <p>
			    <asp:Label AssociatedControlID="txtNewPassword" runat="server">
				    <asp:RequiredFieldValidator id="vldNewPassword" runat="server" 
					    Text="*" 
					    ControlToValidate="txtNewPassword"></asp:RequiredFieldValidator> 
				    New Password:
			    </asp:Label>
			    <asp:TextBox id="txtNewPassword" TextMode="Password" runat="server" CssClass="text" />
		    </p>
		    <p>
			    <asp:Label AssociatedControlID="txtConfirmPassword" runat="server">
				    <asp:RequiredFieldValidator id="vldConfirmPassword" runat="server" 
					    Text="*" 
					    ControlToValidate="txtConfirmPassword"></asp:RequiredFieldValidator> 
				    <asp:CompareValidator id="vldComparePasswords" runat="server" 
					    ErrorMessage="The Passwords do not match."
					    ControlToValidate="txtConfirmPassword" 
					    ControlToCompare="txtNewPassword" 
					    Display="None">*</asp:CompareValidator>
				    Confirm Password:
			    </asp:Label>
			    <asp:TextBox id="txtConfirmPassword" TextMode="Password" runat="server" CssClass="text" />
		    </p>
		    <p class="clear">
			    <asp:Button id="btnSave" runat="server" Text="Save" onclick="btnSave_Click"></asp:Button>
		    </p>
	    </div>
	</fieldset>
</asp:Content>