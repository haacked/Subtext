<%@ Page Language="C#" EnableTheming="false" Title="Subtext Admin - Edit Articles" Codebehind="EditArticles.aspx.cs" AutoEventWireup="True" Inherits="Subtext.Web.Admin.Pages.EditArticles" %>

<asp:Content ContentPlaceHolderID="actionsHeading" runat="server">
    <h2>Actions</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListHeading" runat="server">
    <h2>Categories</h2>
</asp:Content>

<asp:Content ContentPlaceHolderID="categoryListLinks" runat="server">
    <st:CategoryLinks ID="categoryLinks" runat="server" CategoryType="StoryCollection" />
</asp:Content>

<asp:Content ContentPlaceHolderID="pageContent" runat="server">
    <st:EntryEditor id="Editor" CategoryType="StoryCollection" EntryType="Story" ResultsTitle="Articles" runat="server" />
</asp:Content>