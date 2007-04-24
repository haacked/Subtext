<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Error" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="Error.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Error" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
</asp:Content>

<asp:Content ID="entryEditor" ContentPlaceHolderID="pageContent" runat="server">
    <asp:Panel id="ErrorPanel" runat="server" CssClass="errorPage">
		<h2>Problem!</h2>
		<p>There was an error processing your request.</p>
		<p>Please click&nbsp;<asp:HyperLink id="HomeLink" runat="server">Here</asp:HyperLink>&nbsp;to return.</p>
		<div>
			<strong>The specific error message encountered was as follows:</strong>
			<p><asp:Label id="ErrorMessageLabel" runat="server" /></p>
		</div>
	</asp:Panel>
</asp:Content>