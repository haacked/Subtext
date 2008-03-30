<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - Stats" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Statistics.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Statistics" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="statsContent" ContentPlaceHolderID="pageContent" runat="server">
	<st:AdvancedPanel id="Results" runat="server" Collapsible="False" HeaderText="Statistics" HeaderCssClass="CollapsibleHeader"
		DisplayHeader="true" BodyCssClass="Edit">
		<p>
			<a href = "Referrers.aspx">Referrers</a>: See who is linking to you.
		</p>
		<p>
			<a href = "StatsView.aspx">Check Page Views</a>: Get an overview of page views.
		</p>
		<p>
			<a href = "ErrorLog.aspx">Error Log</a>: Configure and view error log
		</p>
		<br class="clear" />
	</st:AdvancedPanel>
</asp:Content>
