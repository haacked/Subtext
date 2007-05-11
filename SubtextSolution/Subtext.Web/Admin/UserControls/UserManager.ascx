<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserManager.ascx.cs" Inherits="Subtext.Web.Admin.UserControls.UserManager" %>
<div class="clear" style="display: none;">
	<asp:Label ID="searchLabel" runat="server" AssociatedControlID="searchTypeDropDown" Text="Search "/>
	<asp:DropDownList ID="searchTypeDropDown" runat="server">
		<asp:ListItem Text="UserName" Value="UserName" />
		<asp:ListItem Text="Email" Value="Email" />
	</asp:DropDownList>
	<asp:Label ID="forLabel" runat="server" AssociatedControlID="searchText" Text="for: " />
	<asp:Textbox runat="server" id="searchText" />
	<asp:Button ID="searchButton" runat="server" text="Find User" OnClick="OnSearchClick" ValidationGroup="ValGroupSearchByTerm" />
	<asp:RequiredFieldValidator ID="reqSearchText" runat="server" ControlToValidate="searchText"
		ValidationGroup="ValGroupSearchByTerm" ErrorMessage="Please enter a search term." />
	<p>
		Wildcard characters * and ? are permitted.
	</p>
</div>

<div class="section">
	<asp:Repeater runat="server" id="alphabetRepeater" OnItemCommand="OnLetterClick">
		<itemtemplate>
			<asp:linkbutton runat="server" 
				id="letterButton" CommandName="Display" 
				CommandArgument="<%# Container.DataItem %>" 
				Text="<%# Container.DataItem %>"
				Font-Bold="<%# (Container.DataItem.ToString() == CurrentFilter) %>"
				 />
		</itemtemplate>
	</asp:Repeater>

	<asp:GridView runat="server" id="usersGrid" 
		Autogeneratecolumns="False" 
		Allowpaging="True"
		DataKeyNames="ProviderUserKey"
		EmptyDataText="No Results"
		PageSize="10" 
		CssClass="log users-table"
	>
		<AlternatingRowStyle cssclass="alt" />
		<PagerStyle cssClass="gridPagerStyle" />
		<HeaderStyle cssclass="header" />
		<SelectedRowStyle cssclass="selected"/>
		<Columns>
			<asp:BoundField DataField="ProviderUserKey" HeaderText="ID" ReadOnly="true" Visible="false" />
			<asp:BoundField DataField="UserName" HeaderText="UserName" ReadOnly="true" />
			<asp:BoundField DataField="Email" HeaderText="Email" ReadOnly="true" />
			<asp:TemplateField>
				<ItemTemplate>
					<nobr>
					<asp:LinkButton ID="viewUserDetail" runat="server" 
						ToolTip="Click to view details and edit roles for this user" 
						CommandName="ViewUserDetail" 
						OnClick="OnViewUserDetailClick" 
						Text="View" 
						CommandArgument='<%# Eval("UserName") %>'/>
					</nobr>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</div>

<div class="section">
	<asp:PlaceHolder ID="userInfo" runat="server" Visible="<%# !String.IsNullOrEmpty(SelectedUserName) %>">
		<div id="user-info">
			<div id="roles">
				<st:RepeaterWithEmptyDataTemplate id="rolesRepeater" runat="server" Visible="true">
					<HeaderTemplate>	
						<div class="role-chooser">
							<ul>
							<li><h3><%# Title %></h3></li>
					</HeaderTemplate>
					<ItemTemplate>
							<li>
								<asp:CheckBox ID="CheckBox1" runat="server" 
									Text="<%# Container.DataItem.ToString() %>" 
									AutoPostBack="true" 
									OnCheckedChanged="OnRoleMembershipChanged"
									Checked="<%# Roles.IsUserInRole(SelectedUserName, Container.DataItem.ToString()) %>" />
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
			</div>
		</div>
	</asp:PlaceHolder>
</div>