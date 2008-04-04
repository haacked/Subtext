<%@ Page Language="C#" MasterPageFile="~/HostAdmin/HostAdminTemplate.Master" AutoEventWireup="true" CodeBehind="EditBlog.aspx.cs" Inherits="Subtext.Web.HostAdmin.EditBlog" Title="Untitled Page" %>
<%@ Register TagPrefix="st" TagName="BlogEditor" Src="~/HostAdmin/UserControls/BlogEditor.ascx" %>

<asp:Content ID="titleContent" ContentPlaceHolderID="MPSectionTitle" runat="server">Host Admin - Edit Blog</asp:Content>

<asp:Content ID="mainContent" ContentPlaceHolderID="MPContent" runat="server">
	<st:BlogEditor id="blogEditor" runat="server" OnSaveComplete="Redirect" />
</asp:Content>
