<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - Stats View" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="StatsView.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.StatsView" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="statsViewContent" ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server" ErrorIconUrl="~/images/icons/ico_critical.gif" ErrorCssClass="ErrorPanel" MessageIconUrl="~/images/icons/ico_info.gif" MessageCssClass="MessagePanel" />
	Coming Soon
	<st:AdvancedPanel id="Results" runat="server" Collapsible="False" HeaderText="View Stats" HeaderCssClass="CollapsibleHeader" DisplayHeader="true" Visible = "False">
		<ASP:Repeater id="rprSelectionList" runat="server" >
			<HeaderTemplate>
				<table id="Listing" class="listing highlightTable" cellspacing="0" cellpadding="0" border="0" style="<%= CheckHiddenStyle() %>">
					<tr>
						<th>Page Title</th>
						<th>View Count</th>
						<th>Date</th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td nowrap>
						<b><%# GetPageTitle(Container.DataItem) %></b>
					</td>
					<td nowrap>
						<%# DataBinder.Eval(Container.DataItem, "ViewCount") %>
					</td>
					<td nowrap>
						<%# DataBinder.Eval(Container.DataItem, "ViewDate", "{0:M/d/yy h:mmt}") %>
					</td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="alt">
					<td nowrap>
						<b><%# GetPageTitle(Container.DataItem) %></b>
					</td>
					<td nowrap>
						<%# DataBinder.Eval(Container.DataItem, "ViewCount") %>
					</td>
					<td nowrap>
						<%# DataBinder.Eval(Container.DataItem, "ViewDate", "{0:M/d/yy h:mmt}") %>
					</td>
				</tr>				
			</AlternatingItemTemplate>
			<FooterTemplate>
			</table>
		</FooterTemplate>
		</ASP:Repeater>
		<st:PagingControl id="resultsPager" runat="server" 
			PrefixText="<div>Goto page</div>" 
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
			UrlFormat="StatsView.aspx?pg={0}" 
			CssClass="Pager" />
		<br class="clear" />	
	</st:AdvancedPanel>
</asp:Content>