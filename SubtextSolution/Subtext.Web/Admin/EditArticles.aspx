<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Edit Articles" MasterPageFile="~/Admin/WebUI/AdminPageTemplate.Master" Codebehind="EditArticles.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.EditArticles" %>
<%@ Register TagPrefix="st" TagName="EntryEditor" Src="~/Admin/UserControls/EntryEditor.ascx" %>
<%@ Register TagPrefix="st" TagName="CategoryLinks" Src="~/Admin/UserControls/CategoryLinkList.ascx" %>

<asp:Content ID="actions" ContentPlaceHolderID="actionsHeading" runat="server">
    Actions
</asp:Content>

<asp:Content ID="categoryListTitle" ContentPlaceHolderID="categoryListHeading" runat="server">
    Categories
</asp:Content>

<asp:Content ID="categoriesLinkListing" ContentPlaceHolderID="categoryListLinks" runat="server">
    <st:CategoryLinks ID="categoryLinks" runat="server" CategoryType="StoryCollection" />
</asp:Content>

<asp:Content ID="entryEditor" ContentPlaceHolderID="pageContent" runat="server">
    <st:EntryEditor id="Editor" CategoryType="StoryCollection" EntryType="Story" ResultsTitle="Articles" runat="server" />
</asp:Content>