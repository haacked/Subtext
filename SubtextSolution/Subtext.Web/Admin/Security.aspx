<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - Security" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Security.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Security" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Options</h2>
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="passwordContent" ContentPlaceHolderID="pageContent" runat="server">
    <st:MessagePanel id="Messages" runat="server"></st:MessagePanel>
	<h2>Security</h2>
	<div class="section">
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
				<asp:TextBox id="tbPassword" runat="server" CssClass="textbox" TextMode="Password" TabIndex="1" />
				<label for="Edit_tbPasswordConfirm">Confirm Password
				<asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" ValidationGroup="ChangePassword" Display="Dynamic" ControlToValidate="tbPasswordConfirm"
						ErrorMessage="Please confirm your password" ForeColor="#990066" /></label>
				<asp:TextBox id="tbPasswordConfirm" runat="server" CssClass="textbox" TextMode="Password" TabIndex="2" />
			<div>
				<asp:Button id="btnChangePassword" runat="server" CssClass="buttonSubmit" Text="Change Password" onclick="btnChangePassword_Click" TabIndex="3" />
			</div>
		</fieldset>
		<fieldset>
		    <legend>Security Options</legend>
				<label>OpenID URL
				</label>
				<asp:TextBox id="tbOpenIDURL" runat="server" CssClass="textbox" />
            <label>
                OpenID Server
                <st:HelpToolTip id="hlpTimeZone" runat="server" HelpText="The OpenID Server and Delegate offload OpenID queries to a third party while still using your blog URL as the OpenID username. For the Server, enter the URL for your provider's server (ex: http://www.myopenid.com/server). For the Delegate, enter your username from your provider (ex: http://username.myopenid.com)."
                    ImageUrl="~/images/icons/help-small.png" ImageWidth="16" ImageHeight="16" />
            </label>
            <asp:TextBox ID="tbOpenIDServer" runat="server" TabIndex="4"></asp:TextBox>
                <label>OpenID Delegate</label>
            <asp:TextBox ID="tbOpenIDDelegate" runat="server" TabIndex="5"></asp:TextBox>
            <div>
				<asp:Button id="btnSaveOptions" runat="server" CssClass="buttonSubmit" Text="Save" onclick="btnSaveOptions_Click" TabIndex="6" />
			</div>
		</fieldset>
    </div>
</asp:Content>