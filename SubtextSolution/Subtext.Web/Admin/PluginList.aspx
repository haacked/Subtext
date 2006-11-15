<%@ Page Language="C#"  Title="Subtext Admin - Plugin List" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" AutoEventWireup="true" CodeBehind="PluginList.aspx.cs" Inherits="Subtext.Web.Admin.Pages.PluginListPage" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="entryEditor" ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server"></st:MessagePanel>
	<st:AdvancedPanel ID="Results" runat="server" LinkStyle="Image" LinkBeforeHeader="True" DisplayHeader="True"
		HeaderCssClass="CollapsibleHeader" HeaderText="Available Plugins" LinkText="[toggle]" Collapsible="True">
		<asp:Repeater runat="server" ID="pluginListRpt" OnItemCommand="pluginListRpt_ItemCommand" OnItemDataBound="pluginListRpt_ItemDataBound">
			<HeaderTemplate>
				<table id="Listing" class="Listing" cellSpacing="0" cellPadding="0" border="0">
					<tr>
						<th width="50">Name</th>
						<th width="50">Version</th>
						<th width="50">Company</th>
						<th>Description</th>
						<th width="100">&nbsp;</th>
						<th width="50">&nbsp;</th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr ID="currentRow" runat="server">
					<td>
						<asp:Label ID="pluginName" runat="server"></asp:Label>
					</td>
					<td>
						<asp:Label ID="pluginVersion" runat="server"></asp:Label>
					</td>
					<td>
						<asp:Label ID="pluginCompany" runat="server"></asp:Label>
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
				<tr class="Alt" ID="currentRow" runat="server">
					<td>
						<asp:Label ID="pluginName" runat="server"></asp:Label>
					</td>
					<td>
						<asp:Label ID="pluginVersion" runat="server"></asp:Label>
					</td>
					<td>
						<asp:Label ID="pluginCompany" runat="server"></asp:Label>
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
	</st:AdvancedPanel>

	<st:AdvancedPanel id="View" runat="server" LinkStyle="Image" DisplayHeader="True" HeaderCssClass="CollapsibleTitle"
		HeaderText="Plugin Details" Collapsible="False" Visible="false" BodyCssClass="Edit">
		<p class="Valuelabel Block"><label>Name:</label> <asp:Label ID="pluginViewName" runat="server"></asp:Label></p>
		<p class="Valuelabel Block"><label>GUID:</label> <asp:Label ID="pluginViewGuid" runat="server"></asp:Label></p>
		<p class="Valuelabel Block"><label>Author:</label> <asp:Label ID="pluginViewAuthor" runat="server"></asp:Label></p>
		<p class="Valuelabel Block"><label>Company:</label> <asp:Label ID="pluginViewCompany" runat="server"></asp:Label></p>
		<p class="Valuelabel Block"><label>Copyright:</label> <asp:Label ID="pluginViewCopyright" runat="server"></asp:Label></p>
		<p class="Valuelabel Block"><label>HomePage:</label> <asp:HyperLink ID="pluginViewHomepage" Target="_blank" runat="server"></asp:HyperLink></p>
		<p class="Valuelabel Block"><label>Version:</label> <asp:Label ID="pluginViewVersion" runat="server"></asp:Label></p>
		<p class="Valuelabel"><label>Description:</label> <br /><asp:Label ID="pluginViewDescription" runat="server"></asp:Label></p>
		<div>
			<asp:Button id="lkbCancel" runat="server" CssClass="buttonSubmit" Text="Cancel" onclick="lkbCancel_Click" />
		</div>
	</st:AdvancedPanel>

	<st:AdvancedPanel id="Edit" runat="server" LinkStyle="Image" DisplayHeader="True" HeaderCssClass="CollapsibleTitle"
		HeaderText="Plugin Settings" Collapsible="False" Visible="false" BodyCssClass="Edit">
		<p class="Valuelabel Block"><label>Name:</label> <asp:Label ID="pluginEditName" runat="server"></asp:Label></p>
		<fieldset class="clear" title="Plugin Settings">
			<legend>Plugin Settings</legend>
			<asp:PlaceHolder ID="pluginSetting" runat="server"></asp:PlaceHolder>
		</fieldset>
		<div>
			<asp:Button id="lkbPost" runat="server" CssClass="buttonSubmit" Text="Post" OnClick="lkbPost_Click" />
			<asp:Button id="lkbEditCancel" runat="server" CssClass="buttonSubmit" Text="Cancel" onclick="lkbEditCancel_Click" />
		</div>
	</st:AdvancedPanel>
</asp:Content>