<%@ Page Language="C#" MasterPageFile="Articles.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Subtext.Web.Admin.Articles.Edit" Title="Untitled Page" %>
<%@ Register TagPrefix="st" TagName="EntryEditor" Src="~/Admin/UserControls/EntryEditor.ascx" %>

<asp:Content ID="content" ContentPlaceHolderID="postsContent" runat="server">
    <st:EntryEditor id="Editor" CategoryType="StoryCollection" EntryType="Story" ResultsTitle="Articles" runat="server" />
</asp:Content>
