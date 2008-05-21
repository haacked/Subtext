<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Confirmation Dialog" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master"  Codebehind="Confirm.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.Confirm" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Actions</h2>
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server" />

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server" />

<asp:Content ID="confirmContent" ContentPlaceHolderID="pageContent" runat="server">
	<st:MessagePanel id="Messages" runat="server" MessageCssClass="MessagePanel" MessageIconUrl="~/images/icons/ico_info.gif" ErrorCssClass="ErrorPanel" ErrorIconUrl="~/images/icons/ico_critical.gif"/>
	<st:AdvancedPanel id="HeaderSection" runat="server" DisplayHeader="true" CssClass="Dialog" HeaderCssClass="DialogTitle" BodyCssClass="DialogBody" HeaderText="Confirm Action" LinkText="[toggle list]"> 
		<ASP:Label id="lblOutput" runat="server" />
		<div style="margin-top: 12px;">
			<ASP:Button id="lkbContinue" runat="server" text="Continue" visible="false" CssClass="buttonSubmit" onclick="lkbContinue_Click" />
			<ASP:Button id="lkbYes" runat="server" Text="Yes" CssClass="buttonSubmit" onclick="Yes_Click" />
			<ASP:Button id="lkbNo" runat="server" Text="No" CssClass="buttonSubmit" onclick="No_Click" />
			<br/>&nbsp;
		</div>
	</st:AdvancedPanel>
</asp:Content>
