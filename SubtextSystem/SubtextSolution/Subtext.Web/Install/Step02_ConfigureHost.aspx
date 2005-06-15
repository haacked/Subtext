<%@ Page language="c#" Codebehind="Step02_ConfigureHost.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Install.Step02_ConfigureHost" %>
<%@ Register TagPrefix="MP" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<MP:MasterPage id="MPContainer" TemplateFile="~/Install/PageTemplate.ascx" runat="server">
	<MP:ContentRegion id="MPTitle" runat="server">Subtext Installation: Step 2 - Host 
Configuration</MP:ContentRegion>
	<MP:ContentRegion id="MPSubTitle" runat="server">Step 2 - Host Configuration</MP:ContentRegion>
	<OL>
		<LI>
		Install the database
		<LI>
			<STRONG>Configure the Host Admin</STRONG>
		<LI>
			Create a Blog
		</LI>
	</OL>
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
					<asp:RequiredFieldValidator id="vldUsernameRequired" runat="server" ErrorMessage="Please specify a User Name"
						ControlToValidate="txtUserName" Display="Dynamic">*</asp:RequiredFieldValidator></TD>
				<TD>
					<asp:TextBox id="txtUserName" Runat="server" EnableViewState="False"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD>Password:</TD>
				<TD>
					<asp:RequiredFieldValidator id="vldPasswordRequired" runat="server" ErrorMessage="Please specify a Password"
						ControlToValidate="txtPassword" Display="Dynamic">*</asp:RequiredFieldValidator>
					<asp:CompareValidator id="vldComparePasswords" runat="server" ErrorMessage="The passwords do not match."
						ControlToValidate="txtPassword" Display="Dynamic" EnableViewState="False" ControlToCompare="txtConfirmPassword"
						ValueToCompare="Text">*</asp:CompareValidator></TD>
				<TD>
					<asp:TextBox id="txtPassword" Runat="server" EnableViewState="False" TextMode="Password"></asp:TextBox></TD>
			</TR>
			<TR>
				<TD>Confirm Password:</TD>
				<TD>
					<asp:RequiredFieldValidator id="vldConfirmPasswordRequired" runat="server" ErrorMessage="Please confirm your password."
						ControlToValidate="txtConfirmPassword" Display="Dynamic">*</asp:RequiredFieldValidator></TD>
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
