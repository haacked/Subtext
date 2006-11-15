<%@ Page Title="Subtext - Host Admin - Error Log" MasterPageFile="~/HostAdmin/HostAdminTemplate.Master" language="c#" Codebehind="ErrorLog.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.HostAdmin.Pages.ErrorLog" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Controls" Assembly="Subtext.Web.Controls" %>
<%@ Register TagPrefix="st" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>

<asp:Content id="sectionTitle" ContentPlaceHolderID="MPSectionTitle" runat="server">Subtext - Host Admin - Error Log</asp:Content>
<asp:Content id="sidebar" ContentPlaceHolderID="MPSideBar" runat="server"></asp:Content>
<asp:Content id="blogList" ContentPlaceHolderID="MPContent" runat="server">
	<st:AdvancedPanel id="Log" runat="server" Collapsible="False" HeaderText="Error Log" HeaderCssClass="CollapsibleHeader"
		DisplayHeader="true">
		
		<st:RepeaterWithEmptyDataTemplate id="LogPage" runat="server">
			<HeaderTemplate>
				<table id="Listing" class="log highlightTable" cellspacing="0" cellpadding="0" border="0">
				<tr>
					<th>Date</th>
					<th>Level</th>
					<th class="logger">Logger</th>
					<th>Message</th>
					<th>Exception</th>
					<th>Url</th>
				</tr>
			</HeaderTemplate>
			<ItemTemplate>
				<tr>
					<td class="date">
						<%# DataBinder.Eval(Container.DataItem, "Date") %>
					</td>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Level") %>
					</td>				
					<td>
						<%# FormatLogger(DataBinder.Eval(Container.DataItem, "Logger"))%>
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
			<AlternatingItemTemplate>
				<tr class="Alt">
					<td class="date">
						<%# DataBinder.Eval(Container.DataItem, "Date") %>
					</td>
					<td>
						<%# DataBinder.Eval(Container.DataItem, "Level") %>
					</td>
					<td>
						<%# FormatLogger(DataBinder.Eval(Container.DataItem, "Logger"))%>
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
			</AlternatingItemTemplate>
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
		<br class="clear" />
	</st:AdvancedPanel>
</asp:Content>