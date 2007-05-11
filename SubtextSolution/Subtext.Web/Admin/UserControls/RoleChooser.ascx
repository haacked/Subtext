<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoleChooser.ascx.cs" Inherits="Subtext.Web.Admin.UserControls.RoleChooser" %>
<st:RepeaterWithEmptyDataTemplate id="rolesRepeater" runat="server" Visible="true">
	<HeaderTemplate>	
		<div class="role-chooser">
			<ul>
	</HeaderTemplate>
	<ItemTemplate>
			<li>
				<asp:CheckBox ID="roleCheckBox" runat="server" 
					Text="<%# Container.DataItem.ToString() %>" 
					AutoPostBack="false" 
					ValidationGroup="<%# ValidationGroup %>" />
			</li>
	</ItemTemplate>
	<FooterTemplate>
			</ul>
		</div>
	</FooterTemplate>
	<EmptyDataTemplate>
		<li>No Roles Have Been Defined.</li>
	</EmptyDataTemplate>
</st:RepeaterWithEmptyDataTemplate>
