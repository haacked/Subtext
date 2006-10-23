<%@ Page Language="C#"  Title="Subtext Admin - Blog Options" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" AutoEventWireup="true" CodeBehind="PluginList.aspx.cs" Inherits="Subtext.Web.Admin.Pages.PluginListPage" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="entryEditor" ContentPlaceHolderID="pageContent" runat="server">
	<ANW:MessagePanel id="Messages" runat="server"></ANW:MessagePanel>
	<ANW:AdvancedPanel ID="Results" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True"
		HeaderCssClass="CollapsibleHeader" HeaderText="Available Plugins" LinkText="[toggle]" Collapsible="True">
		<asp:Repeater runat="server" ID="pluginListRpt" OnItemCommand="pluginListRpt_ItemCommand" OnItemDataBound="pluginListRpt_ItemDataBound">
			<HeaderTemplate>
				<table id="Listing" class="Listing" cellSpacing="0" cellPadding="0" border="0" style="<%= CheckHiddenStyle() %>">
					<tr>
						<th width="50">Name</th>
						<th width="50">Identifier</th>
						<th>Description</th>
						<th width="100">&nbsp;</th>
						<th width="50">&nbsp;</th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td>
						<asp:Label ID="pluginName" runat="server"></asp:Label>
					</td>
					<td>
						<asp:Label ID="pluginId" runat="server"></asp:Label>
					</td>
					<td>
						<asp:Label ID="pluginDescription" runat="server"></asp:Label>
					</td>
					<td>
						<asp:LinkButton id="lnkView" CssClass="buttonSubmit" CommandName="View" Text="View Details" runat="server" />
						<asp:LinkButton id="lnkPluginSettings" CssClass="buttonSubmit" CommandName="Settings" Text="Settings" runat="server" Visible="false" />
				    </td>
					<td>
						<asp:LinkButton id="lnkEnable" CssClass="buttonSubmit" CommandName="Enable" Text="Enable" runat="server" />
						<asp:LinkButton id="lnkDisable" CssClass="buttonSubmit" CommandName="Disable" Text="Disable" runat="server" Visible="false" />
					</td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="Alt">
					<td>
						<asp:Label ID="pluginName" runat="server"></asp:Label>
					</td>
					<td>
						<asp:Label ID="pluginId" runat="server"></asp:Label>
					</td>
					<td>
						<asp:Label ID="pluginDescription" runat="server"></asp:Label>
					</td>
					<td>
						<asp:LinkButton id="lnkView" CssClass="buttonSubmit" CommandName="View" Text="View Details" runat="server" />
						<asp:LinkButton id="lnkPluginSettings" CssClass="buttonSubmit" CommandName="Settings" Text="Settings" runat="server" Visible="false" />
				    </td>
					<td>
						<asp:LinkButton id="lnkEnable" CssClass="buttonSubmit" CommandName="Enable" Text="Enable" runat="server" />
						<asp:LinkButton id="lnkDisable" CssClass="buttonSubmit" CommandName="Disable" Text="Disable" runat="server" Visible="false" />
					</td>
				</tr>
			</AlternatingItemTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</asp:Repeater>
		<st:PagingControl id="resultsPager" runat="server" 
			PrefixText="<div>Goto page</div>" 
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
			UrlFormat="PluginList.aspx?pg={0}" 
			CssClass="Pager" />
		<br class="clear" />
	</ANW:AdvancedPanel>

	<ANW:AdvancedPanel id="View" runat="server" LinkStyle="Image" DisplayHeader="True" HeaderCssClass="CollapsibleTitle"
		HeaderText="Plugin Details" Collapsible="False" Visible="false" BodyCssClass="Edit">
		<p class="ValueLabel Block">Name: <asp:Label ID="pluginViewName" runat="server"></asp:Label></p>
		<p class="ValueLabel Block">GUID: <asp:Label ID="pluginViewGuid" runat="server"></asp:Label></p>
		<p class="ValueLabel Block">Author: <asp:Label ID="pluginViewAuthor" runat="server"></asp:Label></p>
		<p class="ValueLabel Block">Company: <asp:Label ID="pluginViewCompany" runat="server"></asp:Label></p>
		<p class="ValueLabel Block">Copyright: <asp:Label ID="pluginViewCopyright" runat="server"></asp:Label></p>
		<p class="ValueLabel Block">HomePage: <asp:Label ID="pluginViewHomepage" runat="server"></asp:Label></p>
		<p class="ValueLabel Block">Version: <asp:Label ID="pluginViewVersion" runat="server"></asp:Label></p>
		<p class="ValueLabel">Description: <br /><asp:Label ID="pluginViewDescription" runat="server"></asp:Label></p>
	</ANW:AdvancedPanel>
	
	<ANW:AdvancedPanel id="Edit" runat="server" LinkStyle="Image" DisplayHeader="True" HeaderCssClass="CollapsibleTitle"
		HeaderText="Plugin Settings" Collapsible="False" Visible="false">
	</ANW:AdvancedPanel>
</asp:Content>