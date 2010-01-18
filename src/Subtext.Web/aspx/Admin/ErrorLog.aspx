<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Error Log" Codebehind="ErrorLog.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.ErrorLog" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Actions</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="entryEditor" ContentPlaceHolderID="pageContent" runat="server">
	<h2>Error Log</h2>
    <div class="section">	
		<st:RepeaterWithEmptyDataTemplate id="LogPage" runat="server">
			<HeaderTemplate>
				<table id="Listing" class="listing highlightTable" cellSpacing="0" cellPadding="0" border="0">
				<tr>
					<th>Date</th>
					<th>Logger</th>
					<th>Level</th>
					<th>Message</th>
					<th>Exception</th>
					<th>Url</th>
				</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Date") %>
					</td>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Logger") %>
					</td>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Level") %>
					</td>				
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Message") %>
					</td>				
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Exception") %>
					</td>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Url") %>
					</td>
				</tr>
			</ItemTemplate>
			<EmptyDataTemplate>
				<tr>
					<td colspan="6">No log entries to show.</td>
				</tr>
			</EmptyDataTemplate>
			<FooterTemplate>
				</table>
			</FooterTemplate>
		</st:RepeaterWithEmptyDataTemplate>
	
		<st:PagingControl id="resultsPager" runat="server" 
			PrefixText="<div>Goto page</div>" 
			LinkFormatActive='<a href="{0}" class="Current">{1}</a>' 
			UrlFormat="ErrorLog.aspx?pg={0}" 
			CssClass="Pager" />
		<asp:Button id="btnExportToExcel" runat="Server" CssClass="buttonSubmit" Text="Export" title="Export to Excel" style="float:right;"></asp:Button> 
		<asp:Button id="btnClearLog" runat="server" CssClass="buttonSubmit" Text="Clear" title="Clear the Log" style="float:right;"/>
    </div>
</asp:Content>