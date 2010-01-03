<%@ Page Language="C#" MasterPageFile="~/aspx/Admin/Posts/Posts.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Subtext.Web.Admin.Posts.Default" Title="Subtext Admin - Posts" %>

<asp:Content ContentPlaceHolderID="postsContent" runat="server">
    <st:MessagePanel id="Messages" runat="server" />
    <st:EntriesList id="entries" runat="server" EntryType="BlogPost" HeaderText="Posts" />
</asp:Content>
