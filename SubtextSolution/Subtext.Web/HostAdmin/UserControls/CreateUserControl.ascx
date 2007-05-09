<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateUserControl.ascx.cs" Inherits="Subtext.Web.HostAdmin.UserControls.CreateUserControl" %>
<asp:CreateUserWizard ID="createUserWizard" runat="server" OnCreatedUser="OnCreatedUser" OnCancelButtonClick="OnCancelClick">
	<WizardSteps>
		<asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
			<ContentTemplate>
				<h3>Create New User</h3>
				<div class="error">
					<asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False" />
				</div>
				<div>
					<asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
					<asp:TextBox ID="UserName" runat="server" />
					<asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
						ControlToValidate="UserName"
						ErrorMessage="User Name is required." 
						ToolTip="User Name is required." 
						Display="Dynamic" 
						ValidationGroup="CreateUserControl">* User Name is required</asp:RequiredFieldValidator>
				</div>
				<div>
					<asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
					<asp:TextBox ID="Password" runat="server" TextMode="Password" />
					<asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
						ControlToValidate="Password"
						ErrorMessage="Password is required." 
						ToolTip="Password is required." 
						Display="Dynamic" 
						ValidationGroup="CreateUserControl">* Password is required</asp:RequiredFieldValidator>
				</div>
				<div>
					<asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label>
					<asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" />
					<asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" 
						ControlToValidate="ConfirmPassword"
						ErrorMessage="Confirm Password is required." 
						ToolTip="Confirm Password is required."
						Display="Dynamic" 
						ValidationGroup="CreateUserControl">* Confirm Password is required</asp:RequiredFieldValidator>
					<asp:CompareValidator ID="PasswordCompare" runat="server" 
						ControlToCompare="Password"
						ControlToValidate="ConfirmPassword" 
						Display="Dynamic" 
						ErrorMessage="The Password and Confirmation Password must match."
						ValidationGroup="CreateUserControl"></asp:CompareValidator>
				</div>
				<div>
					<asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
					<asp:TextBox ID="Email" runat="server" />
					<asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
						ControlToValidate="Email"
						ErrorMessage="E-mail is required." 
						ToolTip="E-mail is required." 
						Display="Dynamic" 
						ValidationGroup="CreateUserControl">* E-mail is required</asp:RequiredFieldValidator>
				</div>
				<div>
					<asp:Label ID="QuestionLabel" runat="server" AssociatedControlID="Question">Security Question:</asp:Label>
					<asp:TextBox ID="Question" runat="server" />
					<asp:RequiredFieldValidator ID="QuestionRequired" runat="server" 
						ControlToValidate="Question"
						ErrorMessage="Security question is required." 
						ToolTip="Security question is required."
						Display="Dynamic" 
						ValidationGroup="CreateUserControl">* Security question is required</asp:RequiredFieldValidator>
				</div>
				<div>
					<asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="Answer">Security Answer:</asp:Label>
					<asp:TextBox ID="Answer" runat="server"></asp:TextBox>
					<asp:RequiredFieldValidator ID="AnswerRequired" runat="server" 
						ControlToValidate="Answer"
						ErrorMessage="Security answer is required." 
						ToolTip="Security answer is required."
						Display="Dynamic" 
						ValidationGroup="CreateUserControl">* Security answer is required</asp:RequiredFieldValidator>
				</div>
			</ContentTemplate>
			<CustomNavigationTemplate>
				<div class="button-row" style="width:100%; text-align:left;">
					<asp:Button ID="createUserButton" runat="server" CommandName="MoveNext" Text="Save User" ValidationGroup="CreateUserControl" CausesValidation="true" /> or 
					<asp:LinkButton ID="CancelButton" runat="server" CommandName="Cancel" Text="cancel and don&#8217;t save changes" CausesValidation="false" />
				</div>
			</CustomNavigationTemplate>
		</asp:CreateUserWizardStep>
	</WizardSteps>
</asp:CreateUserWizard>