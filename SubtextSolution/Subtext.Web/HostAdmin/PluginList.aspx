<%@ Page Language="C#" MasterPageFile="~/HostAdmin/HostAdminTemplate.Master" AutoEventWireup="true" CodeBehind="PluginList.aspx.cs" Inherits="Subtext.Web.HostAdmin.PluginList" Title="Subtext - Host Admin - Plugin List" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>

<asp:Content ID="sectionTitle" ContentPlaceHolderID="MPSectionTitle" runat="server">
Subtext - Host Admin - Plugin List
</asp:Content>

<asp:Content ID="blogList" ContentPlaceHolderID="MPContent" runat="server">
	
<div id="plugins-page">
	<fieldset>
		<legend>Plugins</legend>
		<st:RepeaterWithEmptyDataTemplate id="pluginList" runat="server">
			<HeaderTemplate>
				<table class="log highlightTable" cellSpacing="0" cellPadding="0" border="0">
				<tr class="header">
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
				<tr class="alt">
					<td colspan="4">
						<%# DataBinder.Eval(Container.DataItem, "Id") %>
					</td>
				</tr>
				<tr class="alt">
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
	</fieldset>
	
	<fieldset>
		<legend>Plugin Loading Errors</legend>
	
		<st:RepeaterWithEmptyDataTemplate id="loadingErrorList" runat="server">
			<HeaderTemplate>
				<table class="log highlightTable" cellSpacing="0" cellPadding="0" border="0">
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td>
						<%# Container.DataItem %>
					</td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="alt">
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
	</fieldset>
</div>
</asp:Content>
