<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - Password" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Password.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Password" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="passwordContent" ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server"></st:MessagePanel>
	<st:AdvancedPanel id="Results" runat="server" HeaderText="Password" LinkStyle="Image"
		DisplayHeader="True" HeaderCssClass="CollapsibleHeader" Collapsible="False">
		<div class="Edit">
			<p>
				<label>Current Password</label>
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="tbCurrent"
						ErrorMessage="Please enter your current passowrd" ForeColor="#990066"/>
			</p>
			<p>
					<asp:TextBox id="tbCurrent" runat="server" CssClass="textinput" TextMode="Password"></asp:TextBox>
			</p>		
			
			<p>
				<label>New Password</label>
					<asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" Display="Dynamic" ControlToValidate="tbPassword"
						ErrorMessage="Please enter a password" ForeColor="#990066" />
					<asp:CompareValidator id="CompareValidator1" runat="server" Display="Dynamic" ControlToValidate="tbPasswordConfirm"
						ErrorMessage="Your passwords do not match" ControlToCompare="tbPassword" ForeColor="#990066" /></p>
			<p>
					<asp:TextBox id="tbPassword" runat="server" CssClass="textinput" TextMode="Password"></asp:TextBox>
			</p>		
			<p>
				<label for="Edit_tbPasswordConfirm">Confirm Password</label>
				<asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" Display="Dynamic" ControlToValidate="tbPasswordConfirm"
						ErrorMessage="Please confirm your password" ForeColor="#990066" /></p>
			<p>
					<asp:TextBox id="tbPasswordConfirm" runat="server" CssClass="textinput" TextMode="Password"></asp:TextBox>
			</p>
			<div>
					<asp:Button id="btnSave" runat="server" CssClass="buttonSubmit" Text="Save" onclick="btnSave_Click"></asp:Button><br /><br />
			</div>
		</div>
	</st:AdvancedPanel>
</asp:Content>