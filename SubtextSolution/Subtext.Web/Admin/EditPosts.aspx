<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Edit Posts" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" AutoEventWireup="true" CodeBehind="EditPosts.aspx.cs" Inherits="Subtext.Web.Admin.WebUI.EditPosts" %>
<%@ Register TagPrefix="st" TagName="CategoryLinks" Src="~/Admin/UserControls/CategoryLinkList.ascx" %>
<%@ Register TagPrefix="st" TagName="EntryEditor" Src="~/Admin/UserControls/EntryEditor.ascx" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
    Actions
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
    Categories
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
    <st:CategoryLinks ID="categoryLinks" runat="server" CategoryType="PostCollection" />
</asp:Content>

<asp:Content ID="entryEditor" ContentPlaceHolderID="pageContent" runat="server">
    <st:EntryEditor id="Editor" CategoryType="PostCollection" EntryType="BlogPost" ResultsTitle="Posts" runat="server" />
</asp:Content>
