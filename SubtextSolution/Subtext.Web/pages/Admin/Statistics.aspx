<%@ Page Language="C#" EnableTheming="false"  Title="Subtext Admin - Stats" Codebehind="Statistics.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Statistics" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Stats</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="statsContent" ContentPlaceHolderID="pageContent" runat="server">
	<h2>Statistics</h2>
	<dl>
		<dt><a href="Referrers.aspx" title="Referrers">Referrers</a>:</dt>
		<dd>See who is linking to you</dd>
	    <dt><a href="ErrorLog.aspx" title="Error Log">Error Log</a>:</dt>
	    <dd>Configure and view error log</dd>
	</dl>
</asp:Content>
