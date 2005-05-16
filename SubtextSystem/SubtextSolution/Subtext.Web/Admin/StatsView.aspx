<%@ Page language="c#" Codebehind="StatsView.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.StatsView" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<ANW:Page id="PageContainer" TabSectionID="Stats" runat="server">

	<ANW:MessagePanel id=Messages runat="server" ErrorIconUrl="~/admin/resources/ico_critical.gif" ErrorCssClass="ErrorPanel" MessageIconUrl="~/admin/resources/ico_info.gif" MessageCssClass="MessagePanel" />
	Coming Soon
	<ANW:AdvancedPanel id="Results" runat="server" Collapsible="False" HeaderText="View Stats" HeaderCssClass="CollapsibleHeader" DisplayHeader="true" Visible = "False">
		<ASP:Repeater id="rprSelectionList" runat="server" >
			<HeaderTemplate>
				<table id="Listing" class="Listing" cellSpacing="0" cellPadding="0" border="0" style="<%= CheckHiddenStyle() %>">
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
				<tr class="Alt">
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
		<ANW:Pager id="ResultsPager" runat="server" UseSpacer="False" PrefixText="<div>Goto page</div>" LinkFormatActive='<a href="{0}" class="Current">{1}</a>' UrlFormat="StatsView.aspx?pg={0}" CssClass="Pager" />
		<br class="Clear">	
	</ANW:AdvancedPanel>

	
	
</ANW:Page>
