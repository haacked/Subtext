<%@ Page Language="C#" MasterPageFile="Posts.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Subtext.Web.Admin.Posts.Edit" Title="Subtext Admin - Edit Posts" %>
<%@ Register TagPrefix="st" TagName="EntryEditor" Src="~/Admin/UserControls/EntryEditor.ascx" %>

<asp:Content ID="content" ContentPlaceHolderID="postsContent" runat="server">
    <st:EntryEditor id="Editor" CategoryType="PostCollection" EntryType="BlogPost" ResultsTitle="Posts" runat="server" />
</asp:Content>
