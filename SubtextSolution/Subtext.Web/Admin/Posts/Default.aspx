<%@ Page Language="C#" MasterPageFile="Posts.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Subtext.Web.Admin.Posts.Default" Title="Subtext Admin - Posts" %>
<%@ Register TagPrefix="st" Src="~/Admin/UserControls/EntriesList.ascx" TagName="EntriesList" %>

<asp:Content ID="content" ContentPlaceHolderID="postsContent" runat="server">
    <st:MessagePanel id="Messages" runat="server" />
    <st:EntriesList id="entries" runat="server" EntryType="BlogPost" HeaderText="Posts" />
</asp:Content>
