<%@ Page Language="C#" MasterPageFile="Articles.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Subtext.Web.Admin.Articles.Default" Title="Untitled Page" %>
<%@ Register TagPrefix="st" Src="~/Admin/UserControls/EntriesList.ascx" TagName="EntriesList" %>

<asp:Content ID="content" ContentPlaceHolderID="postsContent" runat="server">
    <st:MessagePanel id="Messages" runat="server" />
    <st:EntriesList id="entries" runat="server" EntryType="Story" HeaderText="Articles" />
</asp:Content>
