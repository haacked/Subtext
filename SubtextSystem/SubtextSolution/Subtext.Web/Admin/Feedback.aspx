<%@ Page language="c#" Codebehind="Feedback.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.Feedback" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<ANW:Page runat="server" id="PageContainer" TabSectionID="Feedback">
	<ANW:MessagePanel id="Messages" runat="server" />
	<ANW:AdvancedPanel id="Results" runat="server" Collapsible="False" HeaderText="Comments" HeaderCssClass="CollapsibleHeader" DisplayHeader="true">
		<ASP:Repeater id="rprSelectionList" runat="server">
			<HeaderTemplate>
				<table id="Listing" class="Listing" cellSpacing="0" cellPadding="0" border="0" style="<%= CheckHiddenStyle() %>">
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
						<%# DataBinder.Eval(Container.DataItem, "Author") %>
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
						<%# DataBinder.Eval(Container.DataItem, "Author") %>
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
		<ANW:Pager id="ResultsPager" runat="server" UseSpacer="False" PrefixText="<div>Goto page</div>" LinkFormatActive='<a href="{0}" class="Current">{1}</a>' UrlFormat="Feedback.aspx?pg={0}" CssClass="Pager" />
		<asp:Button id="btnDelete" runat="server" CssClass="buttonSubmit" style="float:right" Text="Delete" />
		<br class="Clear">
	</ANW:AdvancedPanel>
</ANW:Page>
