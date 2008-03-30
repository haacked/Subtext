<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Installation: Step 3 - Host Configuration" MasterPageFile="~/Install/InstallTemplate.Master" Codebehind="Step02_ConfigureHost.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Install.Step02_ConfigureHost" %>

<asp:Content id="subTitleContent" ContentPlaceHolderID="MPSubTitle" runat="server">Step 3 - Host Configuration</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="Content" runat="server">
	<ol>
		<li>Gather Installation Information</li>
		<li>Install the database</li>
		<li><strong>Configure the Host Admin</strong>
		<li>Create a Blog</li>
	</ol>
	<asp:Literal id="ltlMessage" Runat="server"></asp:Literal>
	<div class="bordered">
		<p>
			<table id="tblConfigForm" border="0" runat="server">
				<tr>
					<td colSpan="3">
						<asp:ValidationSummary id="vldSummary" runat="server"></asp:ValidationSummary>
					</td>
				</tr>
				<tr>
					<td>
						Host Admin User Name:
					</td>
					<td>
						<asp:RequiredFieldValidator id="vldUsernameRequired" runat="server" ErrorMessage="Please specify a User Name"
							ControlToValidate="txtUserName" Display="Dynamic">*</asp:RequiredFieldValidator>
					</td>
					<td>
						<asp:TextBox id="txtUserName" Runat="server" EnableViewState="False"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td>Password:</td>
					<td>
						<asp:RequiredFieldValidator id="vldPasswordRequired" runat="server" ErrorMessage="Please specify a Password"
							ControlToValidate="txtPassword" Display="Dynamic">*</asp:RequiredFieldValidator>
						<asp:CompareValidator id="vldComparePasswords" runat="server" ErrorMessage="The passwords do not match."
							ControlToValidate="txtPassword" Display="Dynamic" EnableViewState="False" ControlToCompare="txtConfirmPassword"
							ValueToCompare="Text">*</asp:CompareValidator>
					</td>
					<td>
						<asp:TextBox id="txtPassword" Runat="server" EnableViewState="False" TextMode="Password"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td>Confirm Password:</td>
					<td>
						<asp:RequiredFieldValidator id="vldConfirmPasswordRequired" runat="server" ErrorMessage="Please confirm your password."
							ControlToValidate="txtConfirmPassword" Display="Dynamic">*</asp:RequiredFieldValidator>
					</td>
					<td>
						<asp:TextBox id="txtConfirmPassword" Runat="server" EnableViewState="False" TextMode="Password"></asp:TextBox>
					</td>
				</tr>
				<tr>
					<td align="right" colSpan="3">
						<asp:Button id="btnSave" Runat="server" Text="Save" onclick="btnSave_Click"></asp:Button>
					</td>
				</tr>
			</table>
		</p>
	</div>
</asp:Content>