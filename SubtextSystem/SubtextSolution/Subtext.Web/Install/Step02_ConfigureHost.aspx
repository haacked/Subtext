<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Page language="c#" Codebehind="Step02_ConfigureHost.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.Step02_ConfigureHost" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Install/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Installation: Step 2 - Host 
Configuration</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Step 2 - Host Configuration</MP:ContentRegion>
		<ol>
			<li>Install the database</li>
			<li><strong>Configure the Host Admin</strong></li>
			<li>Create a Blog</li>
		</ol>
		<asp:Literal id="ltlMessage" Runat="server"></asp:Literal>
		<P>
			<TABLE id="tblConfigForm" border="0" runat="server">
				<TR>
					<TD colSpan="3">
						<asp:ValidationSummary id="vldSummary" runat="server"></asp:ValidationSummary></TD>
				</TR>
				<TR>
					<TD>Host Admin User Name:
					</TD>
					<TD>
						<asp:RequiredFieldValidator id="vldUsernameRequired" runat="server" Display="Dynamic" ControlToValidate="txtUserName"
							ErrorMessage="Please specify a User Name">*</asp:RequiredFieldValidator></TD>
					<TD>
						<asp:TextBox id="txtUserName" Runat="server" EnableViewState="False"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD>Password:</TD>
					<TD>
						<asp:RequiredFieldValidator id="vldPasswordRequired" runat="server" Display="Dynamic" ControlToValidate="txtPassword"
							ErrorMessage="Please specify a Password">*</asp:RequiredFieldValidator>
						<asp:CompareValidator id="vldComparePasswords" runat="server" Display="Dynamic" ControlToValidate="txtPassword"
							ErrorMessage="The passwords do not match." EnableViewState="False" ValueToCompare="Text" ControlToCompare="txtConfirmPassword">*</asp:CompareValidator></TD>
					<TD>
						<asp:TextBox id="txtPassword" Runat="server" EnableViewState="False" TextMode="Password"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD>Confirm Password:</TD>
					<TD>
						<asp:RequiredFieldValidator id="vldConfirmPasswordRequired" runat="server" Display="Dynamic" ControlToValidate="txtConfirmPassword"
							ErrorMessage="Please confirm your password.">*</asp:RequiredFieldValidator></TD>
					<TD>
						<asp:TextBox id="txtConfirmPassword" Runat="server" EnableViewState="False" TextMode="Password"></asp:TextBox></TD>
				</TR>
				<TR>
					<TD align="right" colSpan="3">
						<asp:Button id="btnSave" Runat="server" Text="Save"></asp:Button></TD>
				</TR>
			</TABLE>
		</P>
</MP:MasterPage>
