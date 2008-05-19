<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - Security" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Security.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Security" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="passwordContent" ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server"></st:MessagePanel>
	<st:AdvancedPanel id="Results" runat="server" HeaderText="Password" LinkStyle="Image"
		DisplayHeader="false" HeaderCssClass="CollapsibleHeader" Collapsible="False">
		<fieldset>
		    <legend>Password</legend>
				<label>Current Password
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" ValidationGroup="ChangePassword" Display="Dynamic" ControlToValidate="tbCurrent"
						ErrorMessage="Please enter your current passowrd" ForeColor="#990066"/>
				</label>
				<asp:TextBox id="tbCurrent" runat="server" CssClass="textbox" TextMode="Password" />
				<label>New Password
				<asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" ValidationGroup="ChangePassword" Display="Dynamic" ControlToValidate="tbPassword"
						ErrorMessage="Please enter a password" ForeColor="#990066" />
				<asp:CompareValidator id="CompareValidator1" runat="server" ValidationGroup="ChangePassword" Display="Dynamic" ControlToValidate="tbPasswordConfirm"
						ErrorMessage="Your passwords do not match" ControlToCompare="tbPassword" ForeColor="#990066" /></label>
				<asp:TextBox id="tbPassword" runat="server" CssClass="textbox" TextMode="Password" />
				<label for="Edit_tbPasswordConfirm">Confirm Password
				<asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" ValidationGroup="ChangePassword" Display="Dynamic" ControlToValidate="tbPasswordConfirm"
						ErrorMessage="Please confirm your password" ForeColor="#990066" /></label>
				<asp:TextBox id="tbPasswordConfirm" runat="server" CssClass="textbox" TextMode="Password" />
			<div>
				<asp:Button id="btnChangePassword" runat="server" CssClass="buttonSubmit" Text="Change Password" onclick="btnChangePassword_Click" />
			</div>
		</fieldset>
		<fieldset>
		    <legend>Security Options</legend>
				<label>OpenID URL
				</label>
				<asp:TextBox id="tbOpenIDURL" runat="server" CssClass="textbox" />
			<div>
				<asp:Button id="btnSaveOptions" runat="server" CssClass="buttonSubmit" Text="Save" onclick="btnSaveOptions_Click" />
			</div>
		</fieldset>
	</st:AdvancedPanel>
</asp:Content>