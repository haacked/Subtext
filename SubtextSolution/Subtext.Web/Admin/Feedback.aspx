<%@ Page language="c#" Title="Subtext Admin - Feedback" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Feedback.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Feedback" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<%@ Register TagPrefix="sub" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
	Folders
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
				<table id="feedback" class="Listing" style="<%= CheckHiddenStyle() %>">
					<tr>
						<th>Title</th>
						<th>Posted By</th>
						<th width="100">Date</th>
						<th width="50"><input type="checkbox" onclick="ToggleCheckAll(this);" /></th>
					</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td>
						<b><%# GetTitle(Container.DataItem) %></b>
					</td>
					<td>
						<strong><%# GetAuthor(Container.DataItem) %></strong> <%# GetAuthorInfo(Container.DataItem) %>
					</td>
					<td nowrap="nowrap">
						<%# DataBinder.Eval(Container.DataItem, "DateCreated", "{0:M/d/yy h:mmt}") %>
					</td>
					<td>
						<asp:CheckBox id="chkDelete" Runat="Server"></asp:CheckBox>
						<input type="hidden" id="FeedbackId" name="FeedbackId" value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" />
					</td>
				</tr>
				<tr class="body">
					<td colspan="5">
						<%# GetBody(Container.DataItem) %>
					</td>
				</tr>
			</ItemTemplate>
			<AlternatingItemTemplate>
				<tr class="Alt">
					<td>
						<b><%# GetTitle(Container.DataItem) %></b>
					</td>
					<td>
						<strong><%# GetAuthor(Container.DataItem) %></strong> <%# GetAuthorInfo(Container.DataItem) %>
					</td>
					<td nowrap="nowrap">
						<%# DataBinder.Eval(Container.DataItem, "DateCreated", "{0:M/d/yy h:mmt}") %>
					</td>
					<td>
						<asp:CheckBox id="chkDeleteAlt" Runat="Server"></asp:CheckBox>
						<input type="hidden" id="FeedbackIdAlt" name="FeedbackIdAlt" value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" />
					</td>
				</tr>
				<tr class="body Alt">
					<td colspan="4">
						<%# GetBody(Container.DataItem) %>
					</td>
				</tr>				
			</AlternatingItemTemplate>
			<FooterTemplate>
			</table>
		</FooterTemplate>
		</ASP:Repeater>
		<sub:PagingControl id="resultsPager" runat="server" 
			PrefixText="<div>Goto page</div>" 
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
			UrlFormat="Feedback.aspx?pg={0}" 
			CssClass="Pager" />
		<asp:Button id="btnDelete" runat="server" CssClass="buttonSubmit" style="float:right" Text="Delete" onclick="OnDeleteClick" ToolTip="Move To Trash" />
		<asp:Button id="btnDestroy" runat="server" CssClass="buttonSubmit" style="float:right" Text="Destroy" onclick="OnDestroyClick" Visible="false" OnClientClick="return confirm('This will delete these comments permanently. Continue?');" ToolTip="Delete Forever" />
		<asp:Button id="btnConfirmSpam" runat="server" CssClass="buttonSubmit" style="float:right" Text="Spam" onclick="OnConfirmSpam" ToolTip="Confirm Spam Moves Item To Trash" />
		<asp:Button id="btnApprove" runat="server" CssClass="buttonSubmit" style="float:right" Text="Approve" onclick="OnApproveClick" ToolTip="Approve" />
		<br class="clear" />
	</st:AdvancedPanel>
</asp:Content>