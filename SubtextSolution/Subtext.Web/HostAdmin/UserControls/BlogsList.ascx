<%@ Control Language="C#" EnableTheming="false"  AutoEventWireup="True" Codebehind="BlogsEditor.ascx.cs" Inherits="Subtext.Web.HostAdmin.UserControls.BlogsList" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<st:MessagePanel id="messagePanel" runat="server"></st:MessagePanel>
<st:AdvancedPanel id="pnlResults" runat="server">

<table class="log">
	<tr>
		<td colspan="6" class="pre-header">
			<asp:CheckBox id="chkShowInactive" AutoPostBack="True" Text="Show Inactive Blogs" Runat="server" oncheckedchanged="OnActiveChanged" />
			<asp:Button ID="addNewBlogButton" CssClass="button" runat="server" OnClick="OnCreateNewBlogClick" Text="Create New Blog" />
		</td>
	</tr>

	<st:RepeaterWithEmptyDataTemplate id="rprBlogsList" Runat="server" OnItemCommand="OnItemCommand">
		<HeaderTemplate>
			<tr class="header">
				<th>Title</th>
				<th>Host</th>
				<th>Subfolder</th>
				<th>Active</th>
				<th colspan="2">Action</th>
			</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td>
					<a href="<%# DataBinder.Eval(Container.DataItem, "RootUrl") %>Default.aspx"><%# DataBinder.Eval(Container.DataItem, "Title") %></a>
				</td>
				<td>
					<strong>
						<%# DataBinder.Eval(Container.DataItem, "Host") %>
					</strong>
				</td>
				<td>
					<strong>
						<%# DataBinder.Eval(Container.DataItem, "Subfolder") %>
					</strong>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
				</td>
				<td>
					<a href='EditBlog.aspx?blog-id=<%# DataBinder.Eval(Container.DataItem, "Id") %>' title='Edit <%# HttpUtility.HtmlEncode((string)DataBinder.Eval(Container.DataItem, "Title")) %>'>Edit</a>
				</td>
				<td>
					<asp:LinkButton id="lnkDelete" CausesValidation="False" CommandName="ToggleActive" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text='<%# ToggleActiveString((bool)DataBinder.Eval(Container.DataItem, "IsActive")) %>' runat="server" />
				</td>
			</tr>
		</ItemTemplate>
		<AlternatingItemTemplate>
			<tr class="alt">
				<td>
					<a href="<%# DataBinder.Eval(Container.DataItem, "RootUrl") %>Default.aspx"><%# DataBinder.Eval(Container.DataItem, "Title") %></a>
				</td>
				<td>
					<strong>
						<%# DataBinder.Eval(Container.DataItem, "Host") %>
					</strong>
				</td>
				<td>
					<strong>
						<%# DataBinder.Eval(Container.DataItem, "Subfolder") %>
					</strong>
				</td>
				<td>
					<%# DataBinder.Eval(Container.DataItem, "IsActive") %>
				</td>
				<td>
					<a href='EditBlog.aspx?blog-id=<%# DataBinder.Eval(Container.DataItem, "Id") %>' title='Edit <%# HttpUtility.HtmlEncode((string)DataBinder.Eval(Container.DataItem, "Title")) %>'>Edit</a>
				</td>
				<td>
					<asp:LinkButton id="lnkDeleteAlt" CausesValidation="False" CommandName="ToggleActive" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' Text='<%# ToggleActiveString((bool)DataBinder.Eval(Container.DataItem, "IsActive")) %>' runat="server" />
				</td>
			</tr>
		</AlternatingItemTemplate>
		<EmptyDataTemplate>
			<tr><td colspan="6" align="center">No entries found</td></tr>
		</EmptyDataTemplate>
		<FooterTemplate>
			
		</FooterTemplate>
	</st:RepeaterWithEmptyDataTemplate>
</table>

	<st:PagingControl id="resultsPager" runat="server" 
			PrefixText="<div>Goto page</div>" 
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
			UrlFormat="Default.aspx?pg={0}" 
			CssClass="Pager" />
</st:AdvancedPanel>