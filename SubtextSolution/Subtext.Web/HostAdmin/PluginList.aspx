<%@ Page Language="C#" MasterPageFile="~/HostAdmin/HostAdminTemplate.Master" AutoEventWireup="true" CodeBehind="PluginList.aspx.cs" Inherits="Subtext.Web.HostAdmin.PluginList" Title="Subtext - Host Admin - Plugin List" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>

<asp:Content ID="sectionTitle" ContentPlaceHolderID="MPSectionTitle" runat="server">
Subtext - Host Admin - Plugin List
</asp:Content>
<asp:Content ID="sidebar" ContentPlaceHolderID="MPSidebar" runat="server">
</asp:Content>
<asp:Content ID="blogList" ContentPlaceHolderID="MPContent" runat="server">
	<st:AdvancedPanel id="pluginsPnl" runat="server" Collapsible="False" HeaderText="Plugin List" HeaderCssClass="CollapsibleHeader"
		DisplayHeader="true">
		
		<st:RepeaterWithEmptyDataTemplate id="pluginList" runat="server">
			<HeaderTemplate>
				<table class="Listing log highlightTable" cellSpacing="0" cellPadding="0" border="0">
				<tr>
					<th>Name</th>
					<th>Author</th>
					<th>Description</th>
					<th>Version</th>
				</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td colspan="4">
						<%# DataBinder.Eval(Container.DataItem, "Id") %>
					</td>
				</tr>
				<tr>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Info.Name") %>
					</td>				
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Info.Author") %>
					</td>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Info.Description") %>
					</td>				
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Info.Version") %>
					</td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="Alt">
					<td colspan="4">
						<%# DataBinder.Eval(Container.DataItem, "Id") %>
					</td>
				</tr>
				<tr class="Alt">
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Info.Name") %>
					</td>				
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Info.Author") %>
					</td>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Info.Description") %>
					</td>				
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Info.Version") %>
					</td>
				</tr>
			</AlternatingItemTemplate>
			<EmptyDataTemplate>
				<tr>
					<td colspan="4">No Plugin loaded (check the list below to see if any error occurred).</td>
				</tr>
			</EmptyDataTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</st:RepeaterWithEmptyDataTemplate>
	</st:AdvancedPanel>
	<br />
	<hr style="position: relative;left:-200px;width: 770px;" />
	<br />
	<st:AdvancedPanel id="loadingErrorsPnl" runat="server" Collapsible="False" HeaderText="Plugin Loading Problems" HeaderCssClass="CollapsibleHeader"
		DisplayHeader="true">
		<st:RepeaterWithEmptyDataTemplate id="loadingErrorList" runat="server">
			<HeaderTemplate>
				<table class="Listing log highlightTable" cellSpacing="0" cellPadding="0" border="0">
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td>
						<%# Container.DataItem %>
					</td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="Alt">
					<td>
						<%# Container.DataItem %>
					</td>
				</tr>
			</AlternatingItemTemplate>
			<EmptyDataTemplate>
				<tr>
					<td>Fortunately no errors while loading the plugins.</td>
				</tr>
			</EmptyDataTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</st:RepeaterWithEmptyDataTemplate>

	</st:AdvancedPanel>
</asp:Content>
