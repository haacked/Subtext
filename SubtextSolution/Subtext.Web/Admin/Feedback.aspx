<%@ Page language="c#" Title="Subtext Admin - Feedback" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Feedback.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Feedback" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="entryEditor" ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server" />
	<st:AdvancedPanel id="Results" runat="server" Collapsible="False" HeaderText="Comments" HeaderCssClass="CollapsibleHeader" DisplayHeader="true">
		<ASP:Repeater id="rprSelectionList" runat="server">
			<HeaderTemplate>
				<table id="Listing" class="Listing highlightTable" cellSpacing="0" cellPadding="0" border="0" style="<%= CheckHiddenStyle() %>">
					<tr>
						<th>Title</th>
						<th width="*">Posted By</th>
						<th width="100">Date</th>
						<th width="50"><input type="checkbox" onclick="ToggleCheckAll(this);" /></th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td>
						<b><%# DataBinder.Eval(Container.DataItem, "Title") %></b>						
					</td>
					<td nowrap>
						<%# GetAuthor(Container.DataItem) %>
					</td>
					<td nowrap>
						<%# DataBinder.Eval(Container.DataItem, "DateCreated", "{0:M/d/yy h:mmt}") %>
					</td>
					<td>
						<asp:CheckBox id="chkDelete" Runat="Server"></asp:CheckBox>
						<input type="hidden" id="EntryID" name="EntryID" value='<%# DataBinder.Eval(Container.DataItem, "EntryID") %>' runat="server" />
					</td>
				</tr>
				<tr>
					<td colspan="4">
						<%# GetBody(Container.DataItem) %>
					</td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="Alt">
					<td>
						<b><%# DataBinder.Eval(Container.DataItem, "Title") %></b>
					</td>
					<td nowrap>
						<%# GetAuthor(Container.DataItem) %>
					</td>
					<td nowrap>
						<%# DataBinder.Eval(Container.DataItem, "DateCreated") %>
					</td>
					<td>
						<asp:CheckBox id="chkDeleteAlt" Runat="Server"></asp:CheckBox>
						<input type="hidden" id="EntryIDAlt" name="EntryIDAlt" value='<%# DataBinder.Eval(Container.DataItem, "EntryID") %>' runat="server" />
					</td>
				</tr>
				<tr class="Alt">
					<td colspan="4">
						<%# GetBody(Container.DataItem) %>
					</td>
				</tr>				
			</AlternatingItemTemplate>
			<FooterTemplate>
			</table>
		</FooterTemplate>
		</ASP:Repeater>
		<st:Pager id="ResultsPager" runat="server" UseSpacer="False" PrefixText="<div>Goto page</div>" LinkFormatActive='<a href="{0}" class="Current">{1}</a>' UrlFormat="Feedback.aspx?pg={0}" CssClass="Pager" />
		<asp:Button id="btnDelete" runat="server" CssClass="buttonSubmit" style="float:right" Text="Delete" onclick="btnDelete_Click" />
		<br class="clear">
	</st:AdvancedPanel>
</asp:Content>