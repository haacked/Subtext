<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Installation: Step 2 - Host Configuration" MasterPageFile="~/aspx/Install/InstallTemplate.Master" Codebehind="Step02_ConfigureHost.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Install.Step02_ConfigureHost" %>

<asp:Content ID="mainContent" ContentPlaceHolderID="Content" runat="server">
	<fieldset>
		<legend>Host Administration Account</legend>
	    <ol>
		    <li>Step 1: Install Database</li>
		    <li><strong>Step 2: Configure the Admin Account</strong></li>
		    <li>Step 3: Create A Blog</li>
	    </ol>

		<p>
			Please enter information for the default <strong>Host Administrator</strong>.
		</p>
		<p>
			A <em>Host Administrator</em> is a user with the ability to maintain a 
			Subtext <em>installation</em>. This user is not an administrator 
			of any specific blog, but can add, edit, and delete blogs and 
			users.
		</p>
		<p>
			The Host Administrator you create here will be flagged 
			as the <em>owner</em> of the installation and cannot be deleted. 
		</p>
	
	    <asp:Literal id="ltlMessage" Runat="server" />
	
		<div id="hostForm" runat="server">
			<asp:ValidationSummary ID="vldHostAdminSummary" runat="server" ValidationGroup="HostAdministration" HeaderText="Please correct the following issues" />
			
			<asp:Label AssociatedControlID="txtUserName" AccessKey="u" runat="server"><span class="accesskey">U</span>serName</asp:Label> 
			<asp:TextBox ID="txtUserName" runat="server" CssClass="textbox" ValidationGroup="HostAdministration" />
			<asp:RequiredFieldValidator ID="vldHostUsernameRequired" runat="server" ControlToValidate="txtUserName" ValidationGroup="HostAdministration" Text="*" Display="Dynamic" />
			
			<asp:Label AssociatedControlID="txtEmail" runat="server" AccessKey="e"><span class="accesskey">E</span>mail <span class="note">(Optional, but useful if you forget your Password)</span></asp:Label>
			<asp:TextBox ID="txtEmail" runat="server" CssClass="textbox" />
			<asp:RequiredFieldValidator ID="vldHostEmailRequired" runat="server" ControlToValidate="txtEmail" ValidationGroup="HostAdministration" Text="*" Display="Dynamic" />
			<asp:RegularExpressionValidator ID="vldHostEmail" runat="server" ControlToValidate="txtEmail" ValidationGroup="HostAdministration" Text="*" ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ErrorMessage="Email address is not valid" Display="Dynamic" />
		    	
			<asp:Label AssociatedControlID="txtPassword" runat="server" AccessKey="P"><span class="accesskey">P</span>assword</asp:Label> 
			<asp:TextBox ID="txtPassword" runat="server" CssClass="textbox" TextMode="Password" />
			<asp:RequiredFieldValidator ID="vldHostAdminPassword" runat="server" ControlToValidate="txtPassword" ValidationGroup="HostAdministration" Text="*" Display="Dynamic" />
			
			<asp:Label AssociatedControlID="txtConfirmPassword" runat="server" AccessKey="C"><span class="accesskey">C</span>onfirm Password</asp:Label> 
			<asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="textbox" TextMode="Password" />
			<asp:RequiredFieldValidator ID="vldHostComparePassword" runat="server" ControlToValidate="txtConfirmPassword" ValidationGroup="HostAdministration" Text="*" Display="Dynamic" />
			<asp:CompareValidator ID="vldHostPasswordsMatch" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" ValidationGroup="HostAdministration" Text="*" ErrorMessage="The passwords do not match." Display="Dynamic" />
		</div>
		<asp:Button id="btnSave" Runat="server" Text="Save" CssClass="button" onclick="btnSave_Click" />
	</fieldset>
</asp:Content>