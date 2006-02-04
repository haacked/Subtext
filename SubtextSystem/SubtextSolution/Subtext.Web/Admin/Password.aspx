<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Page language="c#" Codebehind="Password.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.Password" %>
<ANW:Page runat="server" id="PageContainer" TabSectionID="Options">
	<ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>
	<ANW:AdvancedPanel id="Results" runat="server" HeaderText="Password" LinkStyle="Image"
		DisplayHeader="True" HeaderCssClass="CollapsibleHeader" Collapsible="False">
		<div class="Edit">
	
			<p>
				<label>Current Password</label>
				<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="tbCurrent"
						ErrorMessage="Please enter your current passowrd" ForeColor="#990066"/>
			</p>
			<p>
					<asp:TextBox id="tbCurrent" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
			</p>		
			
			<p>
				<label>New Password</label>
					<asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" Display="Dynamic" ControlToValidate="tbPassword"
						ErrorMessage="Please enter a password" ForeColor="#990066" />
					<asp:CompareValidator id="CompareValidator1" runat="server" Display="Dynamic" ControlToValidate="tbPasswordConfirm"
						ErrorMessage="Your passwords do not match" ControlToCompare="tbPassword" ForeColor="#990066" /></p>
			<p>
					<asp:TextBox id="tbPassword" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
			</p>		
			<p>
				<label for="Edit_tbPasswordConfirm">Confirm Password</label>
				<asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" Display="Dynamic" ControlToValidate="tbPasswordConfirm"
						ErrorMessage="Please confirm your password" ForeColor="#990066" /></p>
			<p>
					<asp:TextBox id="tbPasswordConfirm" runat="server" TextMode="Password" Width="200px"></asp:TextBox>
			</p>
			<div>
					<asp:LinkButton id="btnSave" runat="server" CssClass="Button" Text="Save"></asp:LinkButton><br /><br />
			</div>
		</div>
	</ANW:AdvancedPanel>
</ANW:Page>
