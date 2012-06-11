<%@ Page Language="C#" MasterPageFile="Articles.Master" CodeBehind="Default.aspx.cs" Inherits="Subtext.Web.Admin.Articles.Default" Title="Subtext Admin - Articles" %>

<asp:Content ContentPlaceHolderID="postsContent" runat="server">
    <st:MessagePanel id="Messages" runat="server" />
    <st:EntriesList id="entries" runat="server" EntryType="Story" HeaderText="Articles" />
</asp:Content>
