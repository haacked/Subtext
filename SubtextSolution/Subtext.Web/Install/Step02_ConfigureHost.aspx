
<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Installation: Step 3 - Host Configuration" MasterPageFile="~/Install/InstallTemplate.Master" Codebehind="Step02_ConfigureHost.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Install.Step02_ConfigureHost" %>

<asp:Content ID="mainContent" ContentPlaceHolderID="Content" runat="server">
	<fieldset>
		<legend>Host Administration Account</legend>
	    <ol>
		    <li>Step 1: Install Database</li>
		    <li><strong>Step 2: Configure the Admin</strong></li>
		    <li>Step 3: Create or Import a Blog</li>
	    </ol>

		<p>
			Please enter information for the default Host Administrator.
		</p>
		<p>
			A Host Administrator is a user with the ability to maintain a 
			Subtext <em>installation</em>. This user is not an administrator 
			of any specific blog, but can add, edit, and delete blogs and 
			users.
		</p>
		<p>
			The first Host Administrator you create here will be flagged 
			as the <em>owner</em> of the installation and cannot be deleted. 
		</p>
	
	    <asp:Literal id="ltlMessage" Runat="server" />
	
		<div id="hostForm" runat="server">
			<asp:ValidationSummary ID="vldHostAdminSummary" runat="server" ValidationGroup="HostAdministration" HeaderText="Please correct the following issues" />
			
			<label for="txtUserName">UserName</label> 
			<asp:TextBox ID="txtUserName" runat="server" CssClass="textbox" ValidationGroup="HostAdministration" />
			<asp:RequiredFieldValidator ID="vldHostUsernameRequired" runat="server" ControlToValidate="txtUserName" ValidationGroup="HostAdministration" Text="*" Display="Dynamic" />
			
			<label for="txtEmail">Email</label> 
			<asp:TextBox ID="txtEmail" runat="server" CssClass="textbox" />
			<asp:RequiredFieldValidator ID="vldHostEmailRequired" runat="server" ControlToValidate="txtEmail" ValidationGroup="HostAdministration" Text="*" Display="Dynamic" />
			<asp:RegularExpressionValidator ID="vldHostEmail" runat="server" ControlToValidate="txtEmail" ValidationGroup="HostAdministration" Text="*" ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ErrorMessage="Email address is not valid" Display="Dynamic" />
		    	
			<label for="txtPassword">Password</label> 
			<asp:TextBox ID="txtPassword" runat="server" CssClass="textbox" TextMode="Password" />
			<asp:RequiredFieldValidator ID="vldHostAdminPassword" runat="server" ControlToValidate="txtPassword" ValidationGroup="HostAdministration" Text="*" Display="Dynamic" />
			
			<label for="txtConfirmPassword">Confirm Password</label> 
			<asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="textbox" TextMode="Password" />
			<asp:RequiredFieldValidator ID="vldHostComparePassword" runat="server" ControlToValidate="txtConfirmPassword" ValidationGroup="HostAdministration" Text="*" Display="Dynamic" />
			<asp:CompareValidator ID="vldHostPasswordsMatch" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" ValidationGroup="HostAdministration" Text="*" ErrorMessage="The passwords do not match." Display="Dynamic" />
		</div>
		<asp:Button id="btnSave" Runat="server" Text="Save" onclick="btnSave_Click" />
	</fieldset>
</asp:Content>