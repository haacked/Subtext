<%@ Page language="c#" Codebehind="Statistics.aspx.cs" AutoEventWireup="false" Inherits="Subtext.Web.Admin.Pages.Statistics" %>
<%@ Register TagPrefix="ANW" Namespace="Subtext.Web.Admin.WebUI" Assembly="Subtext.Web" %>
<ANW:Page id="PageContainer" TabSectionID="Stats" runat="server">
	<ANW:AdvancedPanel id="Results" runat="server" Collapsible="False" HeaderText="Statistics" HeaderCssClass="CollapsibleHeader"
		DisplayHeader="true" BodyCssClass="Edit">
		<p>
			<a href = "Referrers.aspx">Referrers</a>: See who is linking to you.
		</p>
		<p>
			<a href = "StatsView.aspx">Check Page Views</a>: Get an overview of page views.
		</p>
		<br class="Clear">
	</ANW:AdvancedPanel>
</ANW:Page>
